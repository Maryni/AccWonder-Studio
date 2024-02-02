using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class IdleGame : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private UIController uiController;
    [SerializeField] private HeroContentController heroController;
    [SerializeField] private SaveController saveController;
    [SerializeField] private UnityEngine.UI.Image mainImage;
    [SerializeField] private UnityEngine.UI.Slider sliderHp;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<string> names;
    [SerializeField] private GameObject slice;
    [SerializeField] private GameObject stars;
    [Space, SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    [SerializeField] private float soulsMod;
    [SerializeField] private Stats currentEnemy;

    public RegionData RegionData;
    public PlayerData PlayerData;
    public BigInteger damageAuto;
    public BigInteger damageSelf;
    public Action onUpdate;

    #endregion Inspector variables

    #region private variables

    private BigInteger gold;
    private int blood;
    private int souls;
    private float timer;
    private int index = 1;
    private int upgradeIndex = 1;
    private int indexLevel;
    private Coroutine timerCoroutine;
    private BigInteger upgradeCost;
    private BigInteger maxValue = 1000000000000;
    private int soulsDamageMod;

    #endregion private variables

    private void Start()
    {
        PlayerData = saveController.PlayerData;
        indexLevel = saveController.PlayerData.LastLevelComplete;
        damageSelf = BigInteger.Add(damageSelf, 1);

        //sliderHp.onValueChanged.AddListener(OnSliderValueChanged);
        UpdateUIPanelInfo();
        uiController.UpdateLevel(indexLevel);
        uiController.UpdateDamageAuto(damageAuto.ToString());
        uiController.UpdateDamageSelf(damageSelf.ToString());
    }

    #region public functions

    public void DealDamageByClick()
    {
        IsEnemyDead();
        currentEnemy.HpValue -= damageSelf;
        SetHpToSlider();
        slice.SetActive(true);
        slice.GetComponent<Animator>().SetTrigger("Slice");
    }

    public void UpgradeSelfDamage() 
    {
        if (gold > upgradeCost)
        {
            gold -= upgradeCost;
            damageSelf = BigInteger.Add(damageSelf, 1);
            uiController.UpdateDamageSelf(damageSelf.ToString());
            upgradeIndex++;
        }
        SetSelfUpgradeToUI();
    }

    public void SetSelfUpgradeToUI()
    {
        upgradeCost = index * upgradeIndex;
        uiController.UpdateSelfDamageUpgrade(upgradeCost.ToString());
    }

    public void UpgradeHeroById(int id)
    {
        if(gold > heroController.CostUpgradeHero(id))
        {
            heroController.UpgradeHero(id);
            gold = gold - heroController.CostUpgradeHero(id);
        }

    }

    public void SetRegionData(RegionData data) => RegionData = data;

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
            uiController.UpdateDamageAuto(damageAuto.ToString());

            uiController.UpdateUIGold(gold.ToString());
        }
        StartIdle();
    }

    private void SetRandomSprite() => mainImage.sprite = sprites[Random.Range(0, sprites.Count)];
    private void SetRandomName() => uiController.UpdateEnemyName(names[Random.Range(0, currentEnemy.Names.Count)]);
    private void StartTimer(float value) => timerCoroutine = StartCoroutine(Timer(value));
    private void GetAutoDamage() => damageAuto = heroController.GetAllDamage();

    private void SetHpToSlider()
    {
        float normalizedValue = MapTo01(currentEnemy.HpValue, BigInteger.Zero, maxValue);
        sliderHp.value = normalizedValue;
    }

    public void OnSliderValueChanged(float normalizedValue)
    {
        currentEnemy.HpValue = MapFrom01(normalizedValue, BigInteger.Zero, maxValue);
    }

    private float MapTo01(BigInteger value, BigInteger min, BigInteger max)
    {
        BigInteger range = max - min;
        BigInteger clampedValue = BigInteger.Max(BigInteger.Min(value, max), min);
        BigInteger scaledValue = clampedValue - min;

        return (float)scaledValue/ (float)range;
    }

    private BigInteger MapFrom01(float normalizedValue, BigInteger min, BigInteger max)
    {
        BigInteger range = max - min;
        BigInteger scaledValue = (BigInteger)(normalizedValue * (double)range);

        return scaledValue + min;
    }

    private void IsEnemyDead()
    {
        if (currentEnemy.HpValue <= 0)
        {
            stars.SetActive(true);
            stars.GetComponent<Animator>().SetTrigger("Die");
            gold = BigInteger.Add(gold,(BigInteger)GetGeometryProgressionValue(currentEnemy.Gold, currentEnemy.GoldMod, indexLevel+1));
            index++;
            indexLevel++;
            PlayerData.LastLevelComplete++;

            if(indexLevel > RegionData.CountMobs)
            {
                RegionData.IsComplete = true;
                indexLevel = 0;
            }

            onUpdate?.Invoke();
            StopTimerCoroutine();
            uiController.UpdateUIGold(gold.ToString());
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
        if(damageAuto >1)
        {
            if(souls > 10)
            {
                int tens = (souls / 10) % 10;
                soulsDamageMod = tens;
            }
            currentEnemy.HpValue -= ((damageAuto * soulsDamageMod)/60);
        }
        SetHpToSlider();
        IsEnemyDead();
    }

    #endregion private functions

    public void StartIdle()
    {
        ReloadStats();
        UpdatePlayerData();
        SetRandomSprite();
        SetRandomName();
        StartTimer(30f);
        SetHpToSlider();
    }
    
    
    public void SetStats(Stats stats)
    {
        var statsNew = stats;

        statsNew.HpValue = (BigInteger)GetGeometryProgressionValue(stats.Hp, stats.HpMod, indexLevel+1);
        statsNew.GoldValue = (BigInteger)GetGeometryProgressionValue(stats.Gold, stats.GoldMod, indexLevel+1);
        sprites = stats.SpritesEnemy;
        names = stats.Names;
        maxValue = statsNew.HpValue;

        currentEnemy = statsNew;
        //uiController.UpgradeUIBackground(back)
    }

    public void SetBackground(Sprite sprite) => uiController.UpgradeUIBackground(sprite);

    private void ReloadStats()
    {
        currentEnemy.HpValue = (BigInteger)GetGeometryProgressionValue(currentEnemy.Hp, currentEnemy.HpMod, indexLevel+1);
        currentEnemy.GoldValue = (BigInteger)GetGeometryProgressionValue(currentEnemy.Gold, currentEnemy.GoldMod, indexLevel+1);
        sprites = currentEnemy.SpritesEnemy;
    }

    private void UpdatePlayerData()
    {
        PlayerData.Gold = gold;
        PlayerData.Blood = blood;
        PlayerData.Souls = souls;
        PlayerData.LastLevelComplete = indexLevel;
    }

    private void UpdateUIPanelInfo()
    {
        uiController.UpdateUIBlood(PlayerData.Blood.ToString());
        uiController.UpdateUIGold(PlayerData.Gold.ToString());
        uiController.UpdateUISouls(PlayerData.Souls.ToString());
    }

    public void GetSouls()
    {
        foreach(var item in saveController.RegionDatas)
        {
            item.IsComplete = false;
            item.CountMobs = item.DefaultCountMobs;
        }

        souls += (int)(indexLevel * soulsMod);
        PlayerData.LastLevelComplete = 0;
        indexLevel = 0;
        gold = 0;
        onUpdate?.Invoke();
        UpdatePlayerData();
    }

    public void UpdateSoulsQuestion()
    {
        uiController.UpdateUISoulsReset((indexLevel * soulsMod).ToString());
    }

    public static string GetValue(string value) => BigInteger.Parse(value).ToString("E2");

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
