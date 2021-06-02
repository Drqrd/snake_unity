using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Vector3 position = Vector3.back;
    public static float initialGameSpeed = 1f;
    public static float difficulty = .1f;
    
    public class Background
    {
        public static int resolution = 2;
        public static float size = 10;
        public static Material material = (Material)Resources.Load("Materials/Background", typeof(Material));
    }

    public class Cells
    {
        public static int resolution = 2;
        public static float cellSize = .2f;
        public static int cells = 25;
        public static int cellsResolution = cells * cells;
    }

    public class Player
    {
        public static int resolution = 2;
        public static float ratio = 1f;
        public static float size = Cells.cellSize * ratio;
        public static int initialLength = 3;
        public static Material bodyMaterial = (Material)Resources.Load("Materials/PlayerBody", typeof(Material));
        public static Material headMaterial = (Material)Resources.Load("Materials/PlayerHead", typeof(Material));
    }

    public class Items
    {
        public static int resolution = 2;
        public static float ratio = .5f;
        public static float size = Cells.cellSize * ratio;
        public static int initialNumber = 4;
        public static int spawnRate;
        public static Material material = (Material)Resources.Load("Materials/Item", typeof(Material));
    }

    public class Movement
    {
        public static KeyCode up = KeyCode.W;
        public static KeyCode left = KeyCode.A;
        public static KeyCode down = KeyCode.S;
        public static KeyCode right = KeyCode.D;
    }

    public class Menu
    {
        public static int fontSizeTitle = 48;
        public static int fontSizeLarge = 40;
        public static int fontSizeMedium = 32;
        public static int fontSizeSmall = 24;
        public static int spacing = 4;
        public static Font font = (Font)Resources.Load("Fonts/Roboto/Roboto-Regular", typeof(Font));
        public static Material material = (Material)Resources.Load("Materials/Text", typeof(Material));
        public static Sprite largeButton = (Sprite)Resources.Load("Sprites/LargeEmptyButton", typeof(Sprite));
        public static Sprite button = (Sprite)Resources.Load("Sprites/EmptyButton", typeof(Sprite));
        public static Sprite smallButton = (Sprite)Resources.Load("Sprites/SmallEmptyButton", typeof(Sprite));
    }

    public class ScoreBoard
    { }
}
