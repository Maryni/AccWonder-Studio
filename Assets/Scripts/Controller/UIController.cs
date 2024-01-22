using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPanel;

    public void ChangeStateHero() => heroPanel.SetActive(!heroPanel.activeSelf);
    public void ChangeStateSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);
    public void ChangeStateRestart() => restartPanel.SetActive(!restartPanel.activeSelf);
}
