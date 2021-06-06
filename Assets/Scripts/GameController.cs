using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{ 
    QuadFace background;
    QuadFace[] gameCells;
    QuadFace playerHead;
    List<QuadFace> playerBody;
    List<QuadFace> gameItem;

    // Global movement for access outside move function
    Vector3 playerMovement = Vector3.up * Settings.Cells.cellSize * 2;
    KeyCode invalidMove = Settings.Movement.down;

    // GameObjects
    GameObject head;
    GameObject[] cells;
    List<GameObject> body;
    List<GameObject> item;
    GameObject mainMenu;
    GameObject pauseMenu;
    GameObject scoreBoard;

    // Time bools
    float second = 0f;
    float itemTime = 0f;
    bool pauseGame = true;

    // Vars when item eaten
    int triggeredItem;
    int score;
    float gameSpeed = Settings.initialGameSpeed;

    // Initialize all meshes and such
    private void Start()
    {
        GenerateBackground();
        GenerateCells();
        GeneratePlayer();
        GenerateItem();
        GenerateMenus();
    }

    private void Update()
    {
        // Pause game
        if (pauseGame == true) { Time.timeScale = 0; }
        else if (pauseGame == false) { Time.timeScale = 1; }

        second += Time.deltaTime;
        itemTime += Time.deltaTime;
        if (second >= gameSpeed)
        {
            second %= gameSpeed;
            UpdatePlayer();
        }
        if (itemTime >= gameSpeed * Settings.Items.spawnRate)
        {
            itemTime %= gameSpeed * Settings.Items.spawnRate;
            UpdateItem();
        }
    }

    void GenerateBackground()
    {   // Create game object
        GameObject obj = new GameObject("Field");

        // Attach mesh filter and material
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        obj.AddComponent<MeshRenderer>().sharedMaterial = Settings.Background.material;
        
        // Generate new QuadFace and construct its mesh
        background = new QuadFace(meshFilter.mesh, Settings.Background.resolution, Settings.Background.size, Settings.position);
        background.ConstructMesh();
    }

    void GenerateCells()
    {
        Material mat;

        // Allocate arrays
        gameCells = new QuadFace[Settings.Cells.cellsResolution];
        MeshFilter[] meshFilters = new MeshFilter[Settings.Cells.cellsResolution];

        // Create game object
        GameObject obj = new GameObject("Cells");

        int pos = 0;
        cells = new GameObject[Settings.Cells.cellsResolution];
        // Attach mesh filter and material
        for (int i = 0; i < Settings.Cells.cells; i++)
        {
            for (int j = 0; j < Settings.Cells.cells; j++)
            {
                // Create cell gameobject, make it child of Cells empty gameobject
                cells[pos] = new GameObject("Cell - " + (pos + 1));
                cells[pos].transform.parent = obj.transform;

                // Attach mesh filter and material
                meshFilters[pos] = cells[pos].AddComponent<MeshFilter>();
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
                cells[pos].AddComponent<MeshRenderer>().sharedMaterial = mat;

                // Generate QuadFaces and construct meshes
                gameCells[pos] = new QuadFace(meshFilters[pos].sharedMesh, Settings.Cells.resolution, Settings.Cells.cellSize, Settings.position);
                gameCells[pos].ConstructMesh();

                // Change position based on cell number
                cells[pos].transform.localPosition =  CalculateCellOffset(i, j) + Vector3.forward / 1.01f;
                
                pos += 1;
            }
        }

        // center on screen
        obj.transform.position += (Settings.Cells.cells * Settings.Cells.cellSize)*(Vector3.up + Vector3.right) - new Vector3(Settings.Cells.cellSize, Settings.Cells.cellSize, 0f);
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

        head.AddComponent<MeshRenderer>().sharedMaterial = Settings.Player.headMaterial;

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

            body[i].AddComponent<MeshRenderer>().sharedMaterial = Settings.Player.bodyMaterial;

            bFilters.Add(body[i].AddComponent<MeshFilter>());
            bFilters[i].mesh = new Mesh();

            playerBody.Add(new QuadFace(bFilters[i].mesh, Settings.Player.resolution, Settings.Player.size, Settings.position));
            playerBody[i].ConstructMesh();

            body[i].transform.localPosition += Vector3.back * .02f;
        }
    }
    
    void GenerateItem()
    {
        List<MeshFilter> iFilters = new List<MeshFilter>();
        gameItem = new List<QuadFace>();
        
        item = new List<GameObject>();
        GameObject obj = new GameObject("Items");

        for (int i = 0; i < Settings.Items.initialNumber; i++)
        {
            item.Add(new GameObject("Item"));
            item[i].transform.parent = obj.transform;

            item[i].AddComponent<MeshRenderer>().sharedMaterial = Settings.Items.material;

            iFilters.Add(item[i].AddComponent<MeshFilter>());
            iFilters[i].mesh = new Mesh();

            gameItem.Add(new QuadFace(iFilters[i].mesh, Settings.Items.resolution, Settings.Items.size, Settings.position));
            gameItem[i].ConstructMesh();

            item[i].transform.localPosition += Vector3.back * .02f;
            RandomizeLocation(item[i]);
        }
    }

    // All manual scaling just about
    void GenerateMenus()
    {
        // Make event system
        GameObject eventSystem = new GameObject("Event System");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();

        GenerateMainMenu();
        GeneratePauseMenu();
        GenerateSettings();
        GenerateScoreBoard();
    }

    // Main Menu stuff
    void GenerateMainMenu()
    {
        // Lots of setup
        mainMenu = new GameObject("Main Menu");
        mainMenu.transform.position += Vector3.back;
        Canvas mCanvas = mainMenu.AddComponent<Canvas>();
        mainMenu.AddComponent<CanvasScaler>();
        mainMenu.AddComponent<GraphicRaycaster>();
        mCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        mCanvas.worldCamera = Camera.main;
        mCanvas.planeDistance = 1;

        // Position on Canvas, from center
        float mmX = -mainMenu.GetComponent<RectTransform>().sizeDelta.x / 2f + Settings.Menu.spacing;
        float mmY = mainMenu.GetComponent<RectTransform>().sizeDelta.y / 2f - (Settings.Menu.spacing * 8f);

        // Add text for title, modify font size and position
        string text = "Snake";
        GameObject title = GenerateText("Title", text, Settings.Menu.fontSizeTitle, mainMenu);
        title.GetComponent<Text>().GetComponent<RectTransform>().localPosition = new Vector2(mmX, mmY);

        // Title spacing from menu
        mmY = mmY - (title.GetComponent<Text>().fontSize + Settings.Menu.spacing * 13.0f);

        text = "Play";
        GameObject play = GenerateText("PlayButton", text, Settings.Menu.fontSizeLarge, mainMenu);
        play.GetComponent<Text>().GetComponent<RectTransform>().localPosition = new Vector2(mmX, mmY);

        // Spacing from play
        mmY = mmY - (play.GetComponent<Text>().fontSize + Settings.Menu.spacing / 1.5f);

        text = "Settings";
        GameObject settings = GenerateText("SettingsButton", text, Settings.Menu.fontSizeMedium, mainMenu);
        settings.GetComponent<Text>().GetComponent<RectTransform>().localPosition = new Vector2(mmX, mmY);

        // Spacing from settings
        mmY = mmY - (settings.GetComponent<Text>().fontSize + Settings.Menu.spacing / 3f);

        text = "Quit";
        GameObject quit = GenerateText("QuitButton", text, Settings.Menu.fontSizeMedium, mainMenu);
        quit.GetComponent<Text>().GetComponent<RectTransform>().localPosition = new Vector2(mmX, mmY);
    }

    // Pause Menu stuff
    void GeneratePauseMenu()
    {
        pauseMenu = new GameObject("Pause Menu");
    }

    // Settings menu to modify Settings class
    void GenerateSettings()
    {

    }

    // Score Board stuff
    void GenerateScoreBoard()
    {
        // More setup same as all other canvases
        scoreBoard = new GameObject("Score Board");
        scoreBoard.transform.position += Vector3.back;
        Canvas sCanvas = scoreBoard.AddComponent<Canvas>();
        scoreBoard.AddComponent<CanvasScaler>();
        scoreBoard.AddComponent<GraphicRaycaster>();
        sCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        sCanvas.worldCamera = Camera.main;
        sCanvas.planeDistance = 1;

        // Position on Canvas, from center
        float mmX = mainMenu.GetComponent<RectTransform>().sizeDelta.x / 2f - Settings.Menu.spacing;
        float mmY = mainMenu.GetComponent<RectTransform>().sizeDelta.y / 2f - (Settings.Menu.spacing * 8f);

        string text = "Score";
        GameObject score = GenerateText(text, text, Settings.Menu.fontSizeTitle, scoreBoard);
        score.GetComponent<Text>().GetComponent<RectTransform>().localPosition = new Vector2(mmX, mmY);
    }

    void UpdatePlayer()
    {
        // Temp storage for positions
        Vector3 prevPosition = head.transform.localPosition;
        Vector3 tempPosition;

        // Move player
        playerMovement = Move();

        // Check if move will cause game over
        if (CollisionCheck(body) || OutOfBounds())
        {
            GameOver();
        }

        // if collides with item
        if (CollisionCheck(item))
        {
            // Destroy item and remove it from list
            Destroy(item[triggeredItem]);
            item.RemoveAt(triggeredItem);

            // Add to body
            UpdateBody();

            // Update scores
            UpdateScores();

            // Change gamespeed
            gameSpeed -= Settings.difficulty;
        }

        if (pauseGame == false)
        {
            // Update head position
            head.transform.localPosition += playerMovement;

            // Update segments in body
            for (int i = 0; i < playerBody.Count; i++)
            {

                tempPosition = body[i].transform.localPosition;
                body[i].transform.localPosition = prevPosition;
                prevPosition = tempPosition;
            }
        }
    }

    // Spawn new item
    void UpdateItem()
    {
        GameObject parent = GameObject.Find("Items");
        item.Add(new GameObject("Item"));

        int ind = item.Count - 1;
        item[ind].transform.parent = parent.transform;

        item[ind].AddComponent<MeshRenderer>().sharedMaterial = Settings.Items.material;
        item[ind].AddComponent<MeshFilter>().mesh = new Mesh();

        QuadFace face = new QuadFace(item[ind].GetComponent<MeshFilter>().mesh, Settings.Items.resolution, Settings.Items.size, Settings.position);
        face.ConstructMesh();

        item[ind].transform.localPosition += Vector3.back * .02f;
        RandomizeLocation(item[ind]);
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

    void UpdateBody()
    {
        body.Add(new GameObject("Body"));

        int ind = body.Count - 1;

        body[ind].AddComponent<MeshRenderer>().sharedMaterial = Settings.Player.bodyMaterial;

        body[ind].AddComponent<MeshFilter>().mesh = new Mesh();

        playerBody.Add(new QuadFace(body[ind].GetComponent<MeshFilter>().mesh, Settings.Player.resolution, Settings.Player.size, Settings.position));
        playerBody[ind].ConstructMesh();

        body[ind].transform.parent = GameObject.Find("Player").transform;
        body[ind].transform.localPosition = body[ind - 1].transform.localPosition;
    }

    void UpdateScores()
    {
        score += 1;
    }
    
    // Display GAME OVER, allow to reset game
    void GameOver()
    {
        pauseGame = true;
        Debug.Log("Game Ended");
    }

    void RandomizeLocation(GameObject obj)
    {
        Vector3 position = obj.transform.localPosition;
        // Offset that centers the parent obj of cells[]
        Vector3 offset = (Settings.Cells.cells * Settings.Cells.cellSize) * (Vector3.up + Vector3.right) - new Vector3(Settings.Cells.cellSize, Settings.Cells.cellSize, 0f);

        int randInt = Random.Range(1, Settings.Cells.cellsResolution) - 1;
        Vector3 randomLocation = cells[randInt].transform.localPosition;
        randomLocation.z = head.transform.localPosition.z;

        List<Vector3> invalidSpawn = new List<Vector3>();

        invalidSpawn.Add(head.transform.localPosition);
        for (int i = 0; i < body.Count; i++) { invalidSpawn.Add(body[i].transform.localPosition); }
        for (int i = 0; i < item.Count; i++) { invalidSpawn.Add(item[i].transform.localPosition); }


        // Check if random location is located on an existing item or snake
        for (int i = 0; i < invalidSpawn.Count; i++)
        {
            if (invalidSpawn[i] == randomLocation + offset)
            {
                randInt = Random.Range(1, Settings.Cells.cellsResolution) - 1;
                randomLocation = cells[randInt].transform.localPosition;
                randomLocation.z = head.transform.localPosition.z;
                i = 0;
            }
        }

        obj.transform.localPosition = randomLocation + offset;
    }

    bool CollisionCheck(List<GameObject> list)
    {
        Vector2 headPos = head.transform.localPosition + playerMovement;
        float err = Settings.Cells.cellSize / 2f;
        for (int i = 0; i < list.Count; i++)
        {
            Vector2 itemPos = list[i].transform.localPosition;

            if (headPos.x <= itemPos.x + err && headPos.x >= itemPos.x - err && headPos.y <= itemPos.y + err && headPos.y >= itemPos.y - err)
            {
                triggeredItem = i;
                return true;
            }
        }
        return false;
    }

    bool OutOfBounds()
    {
        //headPosition
        Vector2 headPos = new Vector2(head.transform.localPosition.x + playerMovement.x, head.transform.localPosition.y + playerMovement.y);

        // Out of cells
        float outStep = Settings.Cells.cellSize * Settings.Cells.cells;

        if (headPos.x <= -outStep || headPos.x >= outStep || headPos.y <= -outStep || headPos.y >= outStep)
        {
            return true;
        }

        return false;
    }

    GameObject GenerateText(string textName, string text, int fontSize, GameObject parent)
    {
        GameObject obj = new GameObject(textName);
        obj.transform.SetParent(parent.transform);

        Text textObj = obj.AddComponent<Text>();
        textObj.font = Settings.Menu.font;
        textObj.fontSize = fontSize;
        textObj.text = text;

        RectTransform parentTransform = parent.GetComponent<RectTransform>();
        RectTransform rec = textObj.GetComponent<RectTransform>();

        rec.localScale = new Vector3(1f, 1f, 1f);
        rec.pivot = new Vector2(0f, 1f);
        rec.sizeDelta = new Vector2(parentTransform.sizeDelta.x / 4.5f, fontSize + Settings.Menu.spacing * 3);

        obj.AddComponent<TextMesh>();

        return obj;
    }
}