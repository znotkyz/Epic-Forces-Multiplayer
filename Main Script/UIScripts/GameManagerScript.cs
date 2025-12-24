using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public bool IsMenuOpened = false;

    public GameObject menuUI;

    public GameObject scoreUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == false)
        {
            scoreUI.SetActive(false);
            menuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsMenuOpened = true;
            AudioListener.pause = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == true)
        {
            scoreUI.SetActive(true);
            menuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            IsMenuOpened = false;
            AudioListener.pause = false;
        }
    }

    public void ExitGame()
    {
        Debug.Log("EXIT GAME!");
        Application.Quit();
    }
}
