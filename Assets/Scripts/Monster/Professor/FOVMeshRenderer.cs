using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMeshRenderer : MonoBehaviour
{
    public float fieldOfView = 60f;
    public float viewDistance = 10f;
    public int segments = 30;
    public float yOffset = 1.5f;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateFOVMesh();
    }

    void CreateFOVMesh()
    {
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = new Vector3(0, yOffset, 0); // 중심점

        float angleStep = fieldOfView / segments;
        float startAngle = -fieldOfView / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 dir = rot * Vector3.forward;
            vertices[i + 1] = vertices[0] + dir * viewDistance;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0; // 중심점
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void UpdateFOV(float fieldOfView, float viewDistance, float yOffset)
    {
        this.fieldOfView = fieldOfView;
        this.viewDistance = viewDistance;
        this.yOffset = yOffset;
        CreateFOVMesh();
    }

}
