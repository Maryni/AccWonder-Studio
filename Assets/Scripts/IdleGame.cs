using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IdleGame : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private UIController uiController;
    [SerializeField] private HeroContentController heroController;
    [SerializeField] private SaveController saveController;
    [SerializeField] private Image mainImage;
    [SerializeField] private Slider sliderHp;
    [SerializeField] private List<Sprite> sprites;
    [Space, SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    [SerializeField] private BigInteger damageAuto;
    [SerializeField] private BigInteger damageSelf;
    [SerializeField] private Stats currentEnemy;

    public PlayerData PlayerData;

    #endregion Inspector variables

    #region private variables

    private BigInteger gold;
    private int blood;
    private int souls;
    private float timer;
    private int index = 1;
    private int upgradeIndex = 1;
    private int indexLevel = 0;
    private Coroutine timerCoroutine;

    #endregion private variables

    private void Start()
    {
        PlayerData = saveController.PlayerData;
        indexLevel = saveController.PlayerData.LastLevelComplete;
        //Debug.Log(GetValue("123456"));
        //int number = 123456;

        // Виведіть число в форматі "E0"
        //Debug.Log(number.ToString("E3"));
    }

    #region public functions

    public void DealDamageByClick()
    {
        IsEnemyDead();
        currentEnemy.HpValue -= damageSelf;
    }

    public void UpgradeSelfDamage() //rewrite
    {
        if (gold > index * upgradeIndex)
        {
            gold -= index * upgradeIndex;
        }
        uiController.UpdateDamageSelf(BigInteger.Add(damageSelf,1).ToString());
    }

    #endregion public functions

    #region private functions

    private IEnumerator Timer(float value)
    {
        timer = value;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            value -= Time.deltaTime;
            timer = value;
            GetAutoDamage();
            DealDamageByTime();

            uiController.UpdateUIGold(gold.ToString());
        }
        StartIdle();
    }

    //private float GetRandom() => Random.Range(minRandom + (minRandom / index), maxRandom * index);

    private void SetRandomSprite() => mainImage.sprite = sprites[Random.Range(0, sprites.Count)];

    private void StartTimer(float value) => timerCoroutine = StartCoroutine(Timer(value));

    private void GetAutoDamage() => damageAuto = heroController.GetAllDamage();

    private void SetHpToSlider()
    {
        if (sliderHp.maxValue == 0 || sliderHp.maxValue <= 1)
        {
            sliderHp.maxValue = float.Parse(currentEnemy.HpValue.ToString());
            sliderHp.minValue = 0;
        }

        sliderHp.value = float.Parse(currentEnemy.HpValue.ToString());
    }

    private void IsEnemyDead()
    {
        if (currentEnemy.Hp <= 0)
        {
            gold += (int)GetGeometryProgressionValue(currentEnemy.Gold, currentEnemy.GoldMod, indexLevel);
            index++;
            indexLevel++;
            PlayerData.LastLevelComplete++;
            StopTimerCoroutine();
            uiController.UpdateUIGold(gold.ToString("E3"));
            uiController.UpdateLevel(indexLevel);
            StartIdle();
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
        currentEnemy.HpValue -= damageAuto;
    }

    #endregion private functions

    public void StartIdle()
    {
        ReloadStats();
        UpdatePlayerData();
        //SetRandomSprite(); //need sprite from designer(artist)
        StartTimer(30f);
        sliderHp.maxValue = 1;
    }

    public void SetStats(Stats stats)
    {
        var statsNew = stats;

        statsNew.HpValue = (BigInteger)GetGeometryProgressionValue(stats.Hp, stats.HpMod, indexLevel);
        statsNew.GoldValue = (BigInteger)GetGeometryProgressionValue(stats.Gold, stats.GoldMod, indexLevel);
        sprites = stats.SpritesEnemy;

        currentEnemy = statsNew;
    }

    private void ReloadStats()
    {
        currentEnemy.HpValue = (BigInteger)GetGeometryProgressionValue(currentEnemy.Hp, currentEnemy.HpMod, indexLevel);
        currentEnemy.GoldValue = (BigInteger)GetGeometryProgressionValue(currentEnemy.Gold, currentEnemy.GoldMod, indexLevel);
        sprites = currentEnemy.SpritesEnemy;
    }

    private void UpdatePlayerData()
    {
        PlayerData.Gold = gold;
        PlayerData.Blood = blood;
        PlayerData.Souls = souls;
        saveController.PlayerData = PlayerData;
    }

    public static string GetValue(string value)
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
            var newValue = digit.ToString("F0") + "+" + countOfDigitsAfter1000;

            var intValue = BigInteger.Parse(value);
            //Debug.Log($" value = {intValue.ToString("E+0")}");
            return newValue;
        }
    }

    public static string ReturnValue(string valueOld)
    {
        if (float.TryParse(valueOld, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value))
        {
            return value.ToString();
        }
        else
        {
            return "><";
        }
    }

    public static float GetGeometryProgressionValue(float baseValue, float firstMod, float increment, float secondMod = 0) => secondMod > 0 ? baseValue * Mathf.Pow(firstMod, (increment * secondMod) - 1) : baseValue * Mathf.Pow(firstMod, (increment) - 1);

}
