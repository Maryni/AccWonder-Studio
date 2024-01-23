using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private TMP_Text damageAutoText;
    [SerializeField] private TMP_Text damageSelfText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text textMoney;
    [SerializeField] private TMP_Text textTime;

    public void ChangeStateHero() => heroPanel.SetActive(!heroPanel.activeSelf);
    public void ChangeStateSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);
    public void ChangeStateRestart() => restartPanel.SetActive(!restartPanel.activeSelf);
    public void UpdateUIMoney(float value) => textMoney.text = value.ToString("F0");
    public void UpdateLevel(int value) => levelText.text = value.ToString();
    public void UpdateDamageAuto(float value) => damageAutoText.text = IdleGame.GetValue(value.ToString());
    public void UpdateDamageSelf(float value) => damageSelfText.text = IdleGame.GetValue(value.ToString());
}
