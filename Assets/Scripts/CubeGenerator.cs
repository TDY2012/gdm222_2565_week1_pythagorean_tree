using UnityEngine;

public static class CubeGenerator
{
    public static GameObject Generate( float size, Material material )
    {
        GameObject cubeInstance = new GameObject();

        MeshRenderer meshRenderer = cubeInstance.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        MeshFilter meshFilter = cubeInstance.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(-size/2, 0, size/2),
            new Vector3(-size/2, 0, -size/2),
            new Vector3(size/2, 0, -size/2),
            new Vector3(size/2, 0, size/2),
            new Vector3(-size/2, size, size/2),
            new Vector3(-size/2, size, -size/2),
            new Vector3(size/2, size, -size/2),
            new Vector3(size/2, size, size/2),
        };
        mesh.vertices = vertices;

        int[] tris = new int[36]
        {
            0, 1, 2,
            2, 3, 0,
            4, 5, 0,
            5, 1, 0,
            5, 6, 1,
            6, 2, 1,
            6, 7, 2,
            7, 3, 2,
            7, 4, 3,
            4, 0, 3,
            6, 5, 4,
            4, 7, 6
        };
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        return cubeInstance;
    }
}