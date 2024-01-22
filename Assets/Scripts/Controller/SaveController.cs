using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<LevelData> LevelDatas;
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
        string jsonLevels = PlayerPrefs.GetString("Levels", "");
        var serializableLevels = JsonUtility.FromJson<List<LevelData>>(jsonLevels);

        if (serializableLevels.Count > 0)
        {
            LoadSuccess = true;

            LevelDatas = serializableLevels;
        }

        string jsonPlayerData = PlayerPrefs.GetString("PlayerData", "");
        var serializablePlayerData = JsonUtility.FromJson<PlayerData>(jsonPlayerData);

        if (serializablePlayerData != null)
        {
            LoadSuccess = true;

            PlayerData = serializablePlayerData;
        }

        string jsonHeroes = PlayerPrefs.GetString("Levels", "");
        var serializableHeroes = JsonUtility.FromJson<List<SupportHeroes>>(jsonHeroes);

        if (serializableHeroes.Count > 0)
        {
            LoadSuccess = true;

            SupportHeroes = serializableHeroes;
        }
    }

    private void SaveLevelData()
    {
        string jsonLevels = JsonUtility.ToJson(LevelDatas);
        PlayerPrefs.SetString("Levels", jsonLevels);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public struct SupportHeroes
{
    public HeroRarityType RarityType;
    public int Id;
    public string Name;
    public int Level;
    public int MaxLevel;
    public float Damage;
    public float DamageMod;
    public float CritChance;
    public Sprite SpriteEnemy;
    public float Cost;
    public float CostMod;
    public float CostRarity;
    public bool IsUnlocked;
}

[System.Serializable]
public class PlayerData
{
    public int Money;
    public int Souls;
    public int Blood;
    public int LastLevelComplete;

}

[System.Serializable]
public class LevelData
{
    public int LevelId;
    public Sprite SpriteBackground;
    public bool IsComplete;
    public Stats Stats;
}

[System.Serializable]
public struct Stats
{
    public float Hp;
    public float Gold;
    public Sprite SpriteEnemy;
    public string Name;
}
