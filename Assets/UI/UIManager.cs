using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject HUDPanel;
    [SerializeField] GameObject PausePanel;

    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(!deathPanel.activeSelf);
    }

    public void ToggleHUDPanel()
    {
        HUDPanel.SetActive(!HUDPanel.activeSelf);
    }

    public void TogglePausePanel(bool val = false)
    {
        PausePanel.SetActive(val);
    }
}
