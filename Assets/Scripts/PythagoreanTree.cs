using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PythagoreanTree : MonoBehaviour
{
    public float size;
    public float angle;
    public int iteration;
    public int maxIteration;

    private Material standardMaterial;
    private Vector3 localScale1, localScale2;
    private Quaternion localRotation1, localRotation2;
    private Vector3 localPosition1, localPosition2;

    private GameObject rootCube;
    private List<GameObject> nonRootCubeList;

    void Awake()
    {
        Assert.IsTrue(iteration <= maxIteration);
        standardMaterial = new Material(Shader.Find("Unlit/Color"));
        UpdateLocalTransformValue();
        nonRootCubeList = new List<GameObject>();
    }

    void Start()
    {
        rootCube = CubeGenerator.Generate(1, standardMaterial);
        rootCube.transform.localScale = new Vector3(size, size, size);
        Generate(maxIteration, rootCube.transform);
    }

    void UpdateLocalTransformValue()
    {
        float sin1 = Mathf.Sin(angle/2);
        float sin2 = Mathf.Sin((Mathf.PI - angle)/2);
        float cos1 = Mathf.Cos(angle/2);
        float cos2 = Mathf.Cos((Mathf.PI - angle)/2);

        localScale1 = new Vector3(sin1, sin1, 1);
        localScale2 = new Vector3(sin2, sin2, 1);

        localRotation1 = Quaternion.Euler(0, 0, -Mathf.Rad2Deg*((Mathf.PI - angle)/2));
        localRotation2 =  Quaternion.Euler(0, 0, Mathf.Rad2Deg*(angle/2));

        localPosition1 = new Vector3(0.5f - 0.5f*cos2*cos2, 1 + 0.5f*cos2*sin2, 0);
        localPosition2 = new Vector3( - 0.5f + 0.5f*cos1*cos1, 1 + 0.5f*cos1*sin1, 0);
    }

    void Generate(int iteration, Transform parent)
    {
        if(iteration > 0)
        {
            GameObject cubeInstance1 = CubeGenerator.Generate(1, standardMaterial);
            GameObject cubeInstance2 = CubeGenerator.Generate(1, standardMaterial);
            nonRootCubeList.Add(cubeInstance1);
            nonRootCubeList.Add(cubeInstance2);

            cubeInstance1.transform.SetParent(parent);
            cubeInstance2.transform.SetParent(parent);

            SetCubeLocalTransform(cubeInstance1, localScale1, localRotation1, localPosition1);
            SetCubeLocalTransform(cubeInstance2, localScale2, localRotation2, localPosition2);

            Generate(iteration-1, cubeInstance1.transform);
            Generate(iteration-1, cubeInstance2.transform);
        }
    }

    void SetCubeLocalTransform(GameObject cubeInstance, Vector3 localScale, Quaternion localRotation, Vector3 localPosition)
    {
        cubeInstance.transform.localScale = localScale;
        cubeInstance.transform.localRotation = localRotation;
        cubeInstance.transform.localPosition = localPosition;
    }

    void UpdateNonRootCubeVisibility(int hiddenIteration, int iteration, ref int index)
    {
        if(iteration > 0)
        {
            if( maxIteration - iteration < hiddenIteration )
            {
                nonRootCubeList[index].SetActive(true);
                nonRootCubeList[index+1].SetActive(true);
            }
            else
            {
                nonRootCubeList[index].SetActive(false);
                nonRootCubeList[index+1].SetActive(false);
            }

            index = index + 2;

            UpdateNonRootCubeVisibility(hiddenIteration, iteration-1, ref index);
            UpdateNonRootCubeVisibility(hiddenIteration, iteration-1, ref index);
        }
    }

    public void ChangeSize(float value)
    {
        size = value;
        rootCube.transform.localScale = new Vector3(size, size, size);
    }

    public void ChangeAngle(float value)
    {
        angle = Mathf.Deg2Rad*value;
        UpdateLocalTransformValue();
        for(int i=0; i<nonRootCubeList.Count; i+=2)
        {
            SetCubeLocalTransform( nonRootCubeList[i], localScale1, localRotation1, localPosition1);
            SetCubeLocalTransform( nonRootCubeList[i+1], localScale2, localRotation2, localPosition2);
        }
    }

    public void ChangeIteration(float value)
    {
        iteration = (int)value;
        int index = 0;
        UpdateNonRootCubeVisibility(iteration, maxIteration, ref index);
    }
}
