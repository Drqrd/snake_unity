using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFace
{
    public Mesh mesh;
    public int resolution;
    public float size, distBetween;
    public Vector3 position, axisA, axisB;

    public QuadFace(Mesh mesh, int resolution, float size, Vector3 position)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.size = size;
        this.position = position;

        // Creates perpendicular line to localUp
        axisA = new Vector3(position.y, position.z, position.x);
        // Same but the other way
        axisB = Vector3.Cross(position, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        int tri = 0;
        // Loop over every vertex
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                // converts the face of the mesh to a percentage
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                // LocalUp starts in the center, moves based on percent along axisA and axisB
                Vector3 pointOnUnitCube = position + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                vertices[i] = pointOnUnitCube * size;

                // Draw two triangles forming a quad if not the last vertices
                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[tri + 0] = i;
                    triangles[tri + 1] = i + resolution + 1;
                    triangles[tri + 2] = i + resolution;

                    triangles[tri + 3] = i;
                    triangles[tri + 4] = i + 1;
                    triangles[tri + 5] = i + resolution + 1;
                    tri += 6;
                }
            }
        }

        mesh.Clear();
        // Increases number of max vertices for better resolution (from UInt16)
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        // Set mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
