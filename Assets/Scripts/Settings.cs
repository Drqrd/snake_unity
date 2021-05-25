using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Vector3 position = Vector3.back;
    public class Background
    {
        public static int resolution = 2;
        public static float size = 10;
    }

    public class Cells
    {
        public static int resolution = 2;
        public static float size = Background.size - 0.1f;
        public static float cellSize = 1;
        public static int cells = 10;
        public static int cellsResolution = cells * cells;
    }

    public class Player
    {

    }
}
