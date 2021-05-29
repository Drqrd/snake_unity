using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
    QuadFace background;
    QuadFace[] cells;
    QuadFace playerHead;
    List<QuadFace> playerBody;
    List<QuadFace> item;

    // Global movement for access outside move function
    Vector3 playerMovement = Vector3.up * Settings.Cells.cellSize * 2;
    KeyCode invalidMove = Settings.Movement.down;

    GameObject head;
    List<GameObject> body;

    // for checking if an item is eaten
    bool ate;

    // time bools
    float second = 0f;

    // Initialize all meshes and such
    private void Start()
    {
        GenerateBackground();
        GenerateCells();
        GeneratePlayer();

        //MainMenu();
    }

    private void Update()
    {
        second += Time.deltaTime;
        if (second >= 1f)
        { 
            second %= 1f;
            UpdatePlayer();
        }
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
        // List init for body filters
        List<MeshFilter> bFilters = new List<MeshFilter>();
        playerBody = new List<QuadFace>();

        //obj to hold all parts of player
        GameObject obj = new GameObject("Player");

        head = new GameObject("Head");
        head.transform.parent = obj.transform;

        head.AddComponent<MeshRenderer>().sharedMaterial = (Material)Resources.Load("Materials/PlayerHead", typeof(Material));

        MeshFilter hFilter = head.AddComponent<MeshFilter>();
        hFilter.mesh = new Mesh();

        // Create Head
        playerHead = new QuadFace(hFilter.mesh, Settings.Player.resolution, Settings.Player.size, Settings.position);
        playerHead.ConstructMesh();

        head.transform.localPosition += Vector3.back *.02f;

        body = new List<GameObject>();
        // Init body
        for (int i = 0; i < Settings.Player.initialLength; i++)
        {
            
            body.Add(new GameObject("Body"));
            body[i].transform.parent = obj.transform;

            body[i].AddComponent<MeshRenderer>().sharedMaterial = (Material)Resources.Load("Materials/PlayerBody", typeof(Material));

            bFilters.Add(body[i].AddComponent<MeshFilter>());
            bFilters[i].mesh = new Mesh();

            playerBody.Add(new QuadFace(bFilters[i].mesh, Settings.Player.resolution, Settings.Player.size, Settings.position));
            playerBody[i].ConstructMesh();

            body[i].transform.localPosition += Vector3.back * .02f;
        }
    }

    void MainMenu()
    {

    }
    
    void UpdatePlayer()
    {
        // Temp storage for positions
        Vector3 prevPosition = head.transform.localPosition;
        Vector3 tempPosition;

        // Move player
        playerMovement = Move();
        head.transform.localPosition += playerMovement;
        
        // Update segments in body
        for (int i = 0; i < playerBody.Count; i++)
        {

            tempPosition = body[i].transform.localPosition;
            body[i].transform.localPosition = prevPosition;
            prevPosition = tempPosition;
        }

        if (ate == true)
        {

        }
    }

    void UpdateItem()
    {

    }

    // Movement controller (check if key is pressed & does not equal previous move, if no key pressed move in same direction)
    Vector3 Move()
    {
        if (Input.GetKey(Settings.Movement.up) && Settings.Movement.up != invalidMove) 
        { 
            invalidMove = Settings.Movement.down; 
            return Vector3.up * Settings.Cells.cellSize * 2;
        }
        else if (Input.GetKey(Settings.Movement.down) && Settings.Movement.down != invalidMove)
        {
            invalidMove = Settings.Movement.up;
            return Vector3.down * Settings.Cells.cellSize * 2;
        }
        else if (Input.GetKey(Settings.Movement.left) && Settings.Movement.left != invalidMove)
        {
            invalidMove = Settings.Movement.right;
            return Vector3.left * Settings.Cells.cellSize * 2;
        }
        else if (Input.GetKey(Settings.Movement.right) && Settings.Movement.right != invalidMove)
        {
            invalidMove = Settings.Movement.left;
            return Vector3.right * Settings.Cells.cellSize * 2;
        }
        else { return playerMovement; }
    }
}
