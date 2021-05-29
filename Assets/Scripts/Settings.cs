using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Vector3 position = Vector3.back;
    public static float gameSpeed = 1.5f;
    public class Background
    {
        public static int resolution = 2;
        public static float size = 10;
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
    }

    public class Movement
    {
        public static KeyCode up = KeyCode.W;
        public static KeyCode left = KeyCode.A;
        public static KeyCode down = KeyCode.S;
        public static KeyCode right = KeyCode.D;
    }
}
