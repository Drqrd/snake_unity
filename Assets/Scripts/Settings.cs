using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Vector3 position = Vector3.back;
    public static float initialGameSpeed = 1f;
    public static float difficulty = .1f;
    public static Vector2 resolution = new Vector2(1920, 1080);
    public static string tag = "Destructable";
    
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
        public static Material GObodyMaterial = (Material)Resources.Load("Materials/GOPlayerBody", typeof(Material));
        public static Material GOheadMaterial = (Material)Resources.Load("Materials/GOPlayerHead", typeof(Material));
    }

    public class Audio
    {
        // https://www.fesliyanstudios.com/royalty-free-music/downloads-c/8-bit-music/6
        // 8 Bit Surf (By David Renda) - 60s ver.
        public static AudioClip backgroundMusic = (AudioClip)Resources.Load("Sounds/BGM", typeof(AudioClip));
        public static AudioClip ateItemSound = (AudioClip)Resources.Load("Sounds/AteItem", typeof(AudioClip));
        public static AudioClip buttonPress = (AudioClip)Resources.Load("Sounds/ButtonPress", typeof(AudioClip));
        public static AudioClip gameOver = (AudioClip)Resources.Load("Sounds/GameOver", typeof(AudioClip));
        public static AudioClip onRestart = (AudioClip)Resources.Load("Sounds/OnRestart", typeof(AudioClip));
    }

    public class Items
    {
        public static int resolution = 2;
        public static float ratio = .5f;
        public static float size = Cells.cellSize * ratio;
        public static int initialNumber = 4;
        public static int spawnRate = 5;
        public static int maxItems = 4;
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
        public static float titleScale = 1.0f;
        public static float largeScale = 0.8f;
        public static float mediumScale = 0.6f;
        public static float smallScale = 0.4f;
        public static int spacing = 12;
        public static Font font = (Font)Resources.Load("Fonts/Roboto/Roboto-Bold", typeof(Font));
        public static Material material = (Material)Resources.Load("Materials/Text", typeof(Material));
        public static Sprite largeButton = (Sprite)Resources.Load("Sprites/LargeEmptyButton", typeof(Sprite));
        public static Sprite button = (Sprite)Resources.Load("Sprites/EmptyButton", typeof(Sprite));
        public static Sprite smallButton = (Sprite)Resources.Load("Sprites/SmallEmptyButton", typeof(Sprite));
        public static Color colorNormal;
        public static Color colorHover;
        public static Color colorPressed;
    }

    public class Names
    {
        public static string playButtonName = "PlayButton";
        public static string settingsButtonName = "SettingsButton";
        public static string quitButtonName = "QuitButton";
        public static string restartButtonName = "PlayAgainButton";
    }
}
