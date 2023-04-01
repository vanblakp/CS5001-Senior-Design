using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject HUDPanel;
    [SerializeField] GameObject PausePanel;

    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(!deathPanel.activeSelf);
        if (!deathPanel.activeSelf)
        {
            deathPanel.GetComponent<Animation>().Play("FadeInAnim");
        }
    }

    public void ToggleHUDPanel()
    {
        HUDPanel.SetActive(!HUDPanel.activeSelf);
    }

    public void TogglePausePanel(bool val = false)
    {
        PausePanel.SetActive(val);
    }

    // 0 is paused, 1 is normal
    public void TogglePauseState(int val = 0)
    {
        Time.timeScale = val;
    }

    public void MoveToLevel(int val = 1)
    {
        SceneManager.LoadScene(val);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
