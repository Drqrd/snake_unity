using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
    QuadFace background;
    QuadFace[] cells;
    QuadFace playerHead;
    QuadFace item;

    // Initialize all meshes and such
    private void Start()
    {
        GenerateBackground();
        GenerateCells();
        GeneratePlayer();
    }

    // Move player and spawn items, update scores
    private void Update()
    {

    }

    void GenerateBackground()
    {   // Create game object
        GameObject obj = new GameObject("Field");

        // Attach mesh filter and material
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        obj.AddComponent<MeshRenderer>().sharedMaterial = (Material)Resources.Load("Materials/Background", typeof(Material));
        
        // Generate new QuadFace and construct its mesh
        background = new QuadFace(meshFilter.mesh, Settings.Background.resolution, Settings.Background.size, Settings.position);
        background.ConstructMesh();
    }

    void GenerateCells()
    {
        Material mat;

        // Allocate arrays
        cells = new QuadFace[Settings.Cells.cellsResolution];
        MeshFilter[] meshFilters = new MeshFilter[Settings.Cells.cellsResolution];

        // Create game object
        GameObject obj = new GameObject("Cells");

        int pos = 0;
        // Attach mesh filter and material
        for (int i = 0; i < Settings.Cells.cells; i++)
        {
            for (int j = 0; j < Settings.Cells.cells; j++)
            {
                // Create cell gameobject, make it child of Cells empty gameobject
                GameObject child = new GameObject("Cell - " + (pos + 1));
                child.transform.parent = obj.transform;

                // Attach mesh filter and material
                meshFilters[pos] = child.AddComponent<MeshFilter>();
                meshFilters[pos].sharedMesh = new Mesh();

                // Checkerboard colors
                if (i % 2 == 0)
                {
                    if (j % 2 == 0) { mat = (Material)Resources.Load("Materials/Cell1", typeof(Material)); }
                    else { mat = (Material)Resources.Load("Materials/Cell2", typeof(Material)); }
                }
                else
                {
                    if (j % 2 != 0) { mat = (Material)Resources.Load("Materials/Cell1", typeof(Material)); }
                    else { mat = (Material)Resources.Load("Materials/Cell2", typeof(Material)); }
                }
                child.AddComponent<MeshRenderer>().sharedMaterial = mat;

                // Generate QuadFaces and construct meshes
                cells[pos] = new QuadFace(meshFilters[pos].sharedMesh, Settings.Cells.resolution, Settings.Cells.cellSize, Settings.position);
                cells[pos].ConstructMesh();

                // Change position based on cell number
                child.transform.localPosition =  CalculateCellOffset(i, j) + Vector3.forward / 1.01f;
                
                pos += 1;
            }
        }

        // center on screen
        obj.transform.position += (Settings.Cells.cells * Settings.Cells.cellSize)*(Vector3.up + Vector3.right);
        obj.transform.position -= new Vector3(0.2f, 0.2f, 0f);
    }

    Vector3 CalculateCellOffset(int i, int j)
    {
        Vector3 position = Settings.position;
        // Creates perpendicular line to localUp
        Vector3 axisA = new Vector3(position.y, position.z, position.x);
        // Same but the other way
        Vector3 axisB = Vector3.Cross(position, axisA);

        Vector2 percent = new Vector2(i, j) * Settings.Cells.cellSize;

        // returns the value adjusted by where it is located in relation to the cellOrigin
        return position + (percent.x - (1 / Settings.Cells.cells)) * 2 * axisA + (percent.y - (1 / Settings.Cells.cells)) * 2 * axisB;
    }

    /* 
     * Two approaches:
     * Variable sized list to contain player squares
     * Spawn an initial snake, add square in direction travelling and delete tail
    */
    void GeneratePlayer()
    {

        MeshFilter[] bFilters = new MeshFilter[Settings.Player.initialLength - 1];

        GameObject obj = new GameObject("Player");

        GameObject head = new GameObject("Head");
        head.transform.parent = obj.transform;

        head.AddComponent<MeshRenderer>().sharedMaterial = (Material)Resources.Load("Materials/PlayerHead", typeof(Material));

        MeshFilter hFilter = head.AddComponent<MeshFilter>();
        hFilter.mesh = new Mesh();

        playerHead = new QuadFace(hFilter.mesh, Settings.Player.resolution, Settings.Player.size, Settings.position);
        playerHead.ConstructMesh();

        for (int i = 0; i < Settings.Player.initialLength; i++)
        {

        }
    }

    void UpdatePlayer()
    {

    }

    void UpdateItem()
    {

    }
}
