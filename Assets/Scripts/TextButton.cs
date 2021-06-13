using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Text))]
public class TextButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    // add callbacks in the inspector like for buttons
    public UnityEvent onClick = new UnityEvent();

    Text textComp;

    private void Start()
    {
        textComp = GetComponent<Text>();
        if (name == Settings.Names.playButtonName) { onClick.AddListener(IPlay); }
        if (name == Settings.Names.settingsButtonName) { onClick.AddListener(ISettings); }
        if (name == Settings.Names.quitButtonName) { onClick.AddListener(IQuit); }
        if (name == Settings.Names.restartButtonName) { onClick.AddListener(IPlayAgain); }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        // Change color for visual indication
        textComp.material = Settings.Menu.material;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {        
        // Reset color
        textComp.material = null;

        // Load the event associated with button
        onClick.Invoke();
    }

    // Allows game state to update
    void IPlay()
    {
        GameController.pauseGame = false;
        GameObject obj = GameObject.Find("Main Menu");
        for (int i = 1; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Moves to settings menu
    void ISettings()
    {
        SceneManager.LoadScene("Settings");
    }

    // Quits game
    void IQuit()
    {
        Application.Quit();
    }

    // Restarts game
    void IPlayAgain()
    {
        GameController.newGame = true;
    }
}