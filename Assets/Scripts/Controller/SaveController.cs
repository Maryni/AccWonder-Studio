using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public enum HeroRarityType
{
    Common,
    Uncommon,
    Rare,
    UltraRare,
    Epic,
    Legendary,
    Legendary2,
    Legendary3,
    Legendary4,
    Legenrady5,
    Legenrady6,
    Legenrady7,
    Legenrady8,
    Legenrady9,
    Legenrady10
}

public class SaveController : MonoBehaviour
{
    public PlayerData PlayerData;
    public List<RegionData> RegionDatas;
    public List<SupportHeroes> SupportHeroes;

    public bool LoadSuccess = false;

    private void Awake()
    {
        LoadLevelData();
    }

    private void OnDestroy()
    {
        SaveLevelData();
    }

    private void LoadLevelData()
    {
        string jsonLevels = PlayerPrefs.GetString("Regions", "");
        var serializableLevels = JsonUtility.FromJson<List<RegionData>>(jsonLevels);

        if (serializableLevels.Count > 0)
        {
            LoadSuccess = true;
            RegionDatas = serializableLevels;
        }

        string jsonPlayerData = PlayerPrefs.GetString("Player", "");
        var serializablePlayerData = JsonUtility.FromJson<PlayerData>(jsonPlayerData);

        if (serializablePlayerData != null)
        {
            LoadSuccess = true;
            PlayerData = serializablePlayerData;
        }

        string jsonHeroes = PlayerPrefs.GetString("HeroesData", "");
        var serializableHeroes = JsonUtility.FromJson<List<SupportHeroes>>(jsonHeroes);

        if (serializableHeroes.Count > 0)
        {
            LoadSuccess = true;
            SupportHeroes = serializableHeroes;
        }
    }

    private void SaveLevelData()
    {
        string jsonRegions = JsonUtility.ToJson(RegionDatas);
        string jsonPlayer = JsonUtility.ToJson(PlayerData);
        string jsonHeroes = JsonUtility.ToJson(SupportHeroes);
        PlayerPrefs.SetString("Regions", jsonRegions);
        PlayerPrefs.SetString("Player", jsonPlayer);
        PlayerPrefs.SetString("HeroesData", jsonHeroes);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class SupportHeroes
{
    public HeroRarityType RarityType;
    public int Id;
    public string Name;
    public int Level;
    public int MaxLevel;
    public float Damage;
    public float DamageMod;
    public float CritChance;
    public int CritMod;
    public Image SpriteEnemy;
    public float Cost;
    public float CostMod;
    public float CostRarity;
    public bool IsUnlocked;
}

[System.Serializable]
public class PlayerData
{
    public BigInteger Gold;
    public int Souls;
    public int Blood;
    public int LastLevelComplete;
}

[System.Serializable]
public class RegionData
{
    public int RegionId;
    public Image SpriteBackground;
    public int CountMobs;
    public int DefaultCountMobs;
    public bool IsComplete;
    public Stats Stats;
}

[System.Serializable]
public struct Stats
{
    public float Hp;
    public float HpMod;
    public BigInteger HpValue; // changes in-game only
    public float Gold;
    public float GoldMod;
    public BigInteger GoldValue; // changes in-game only
    public List<Sprite> SpritesEnemy;
    public List<string> Names;
}
