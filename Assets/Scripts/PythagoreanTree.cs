using UnityEngine;

public class PythagoreanTree : MonoBehaviour
{
    public float size;
    public float angle;
    public int iteration;

    private Material standardMaterial;
    private Vector3 localScale1, localScale2;
    private Quaternion localRotation1, localRotation2;
    private Vector3 localPosition1, localPosition2;

    void Awake()
    {
        standardMaterial = new Material(Shader.Find("Unlit/Color"));
        UpdateLocalTransformValue();
    }

    void Start()
    {
        GameObject cubeInstance = CubeGenerator.Generate(1, standardMaterial);
        cubeInstance.transform.localScale = new Vector3(size, size, size);
        Generate(iteration, cubeInstance.transform);
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
}
