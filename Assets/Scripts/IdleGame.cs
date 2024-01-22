using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class IdleGame : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private Image mainImage;
    [SerializeField] private Slider sliderHp;
    [SerializeField] private TMP_Text textMoney;
    [SerializeField] private TMP_Text textTime;
    [SerializeField] private TMP_Text textDamageSelf;
    [SerializeField] private TMP_Text textDamageAuto;
    [SerializeField] private TMP_Text textWave;
    [SerializeField] private List<Sprite> sprites;
    [Space, SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    [SerializeField] private float damageAuto;
    [SerializeField] private float damageSelf;
    [SerializeField] private Stats currentEnemy;

    #endregion Inspector variables

    #region private variables

    private float money;
    private float timer;
    private int index = 1;
    private int upgradeIndex = 1;
    private int indexWave = 0;
    private Coroutine timerCoroutine;

    #endregion private variables

    private void Start()
    {
        //Debug.Log(GetValue("123456"));
        //int number = 123456;

        // Виведіть число в форматі "E0"
        //Debug.Log(number.ToString("E3"));
    }

    #region public functions

    public void DealDamageByClick()
    {
        IsEnemyDead();
        currentEnemy.Hp -= damageSelf;
    }

    public void UpgradeSelfDamage() //rewrite
    {
        if (money > index * upgradeIndex)
        {
            money -= index * upgradeIndex;
        }

        damageSelf++;
        UpdateDamageSelf();
    }

    #endregion public functions

    #region private functions

    private void UpdateUI() //rewrite with symbols
    {
        textMoney.text = money.ToString("F0");
    }

    private IEnumerator Timer(float value)
    {
        timer = value;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            value -= Time.deltaTime;
            timer = value;
            DealDamageByTime();

            UpdateTime();
            UpdateUI();
        }
        StartIdle();
    }

    private Stats GetStats() => new Stats() { Hp = GetRandom(), Gold = GetRandom() };

    private float GetRandom() => Random.Range(minRandom + (minRandom / index), maxRandom * index);

    private void SetRandomSprite() => mainImage.sprite = sprites[Random.Range(0, sprites.Count)];

    private void UpdateTime() => textTime.text = timer.ToString("F1");

    private void StartTimer(float value) => timerCoroutine = StartCoroutine(Timer(value));

    private void UpdateDamageSelf() => textDamageSelf.text = damageSelf.ToString("F0"); //rewrite to new Symbols

    private void UpdateDamageAuto() => textDamageAuto.text = damageAuto.ToString("F0"); //rewrite to new Symbols

    private void UpdateWave() => textWave.text = indexWave.ToString("F0");

    private void SetHpToSlider()
    {
        if (sliderHp.maxValue == 0 || sliderHp.maxValue <= 1)
        {
            sliderHp.maxValue = currentEnemy.Hp;
            sliderHp.minValue = 0;
        }

        sliderHp.value = currentEnemy.Hp;
    }

    private void IsEnemyDead()
    {
        if (currentEnemy.Hp <= 0)
        {
            money += currentEnemy.Gold;
            index++;
            indexWave++;
            StopTimerCoroutine();
            UpdateUI();
            StartIdle();
            UpdateWave();
        }
        SetHpToSlider();
    }

    private void StopTimerCoroutine()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private void DealDamageByTime()
    {
        IsEnemyDead();
        currentEnemy.Hp -= damageAuto * Time.deltaTime;
    }

    #endregion private functions

    public void StartIdle()
    {
        currentEnemy = GetStats();
        SetRandomSprite();
        StartTimer(5f);
        sliderHp.maxValue = 1;
    }

    private string GetValue(string value)
    {
        if(float.Parse(value) < 1000 )
        {
            return value;
        }
        else
        {
            var valueFloat = float.Parse(value);
            var countOfDigits = (int)Mathf.Log10(valueFloat) + 1; //all count of digits
            var countOfDigitsAfter1000 = countOfDigits - (int)Mathf.Log10(1000) ; // after 1000

            var digit = valueFloat / Mathf.Pow(10, countOfDigits - 3); //value until 1000

            var newValue = digit.ToString("F0") + "+"+countOfDigitsAfter1000;
            var intValue = int.Parse(value);
            Debug.Log($" value = {intValue.ToString("E+0")}");
            return newValue;
        }
    }
}
