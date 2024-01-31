using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject menuScene;
    [SerializeField] private GameObject gameScene;
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text damageAutoText;
    [SerializeField] private TMP_Text damageSelfText;
    [SerializeField] private TMP_Text damageSelfUpgradeText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text textGold;
    [SerializeField] private TMP_Text textBlood;
    [SerializeField] private TMP_Text textSouls;
    [SerializeField] private TMP_Text textSoulsReset;
    [SerializeField] private TMP_Text textNameEnemy;

    public void ChangeStateMenuScene() => menuScene.SetActive(!menuScene.activeSelf);
    public void ChangeStatePlayScene() => gameScene.SetActive(!gameScene.activeSelf);
    public void ChangeStateHero() => heroPanel.SetActive(!heroPanel.activeSelf);
    public void ChangeStateSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);
    public void ChangeStateRestart() => restartPanel.SetActive(!restartPanel.activeSelf);
    public void UpdateLevel(int value) => levelText.text = value.ToString();
    public void UpdateEnemyName(string value) => textNameEnemy.text = value.ToString();
    public void UpdateUIGold(string value) => textGold.text = IdleGame.GetValue(value);
    public void UpdateUIBlood(string value) => textBlood.text = IdleGame.GetValue(value);
    public void UpdateUISouls(string value) => textSouls.text = IdleGame.GetValue(value);
    public void UpdateUISoulsReset(string value) => textSoulsReset.text = IdleGame.GetValue(value);
    public void UpdateDamageAuto(string value) => damageAutoText.text = IdleGame.GetValue(value);
    public void UpdateDamageSelf(string value) => damageSelfText.text = IdleGame.GetValue(value);
    public void UpdateSelfDamageUpgrade(string value) => damageSelfUpgradeText.text = IdleGame.GetValue(value);
    public void UpgradeUIBackground(Sprite sprite) => background.sprite = sprite;
    public void Exit() => Application.Quit();
}
