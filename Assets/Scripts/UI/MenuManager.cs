using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {

            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
