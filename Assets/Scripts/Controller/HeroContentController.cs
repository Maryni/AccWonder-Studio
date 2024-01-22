using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class HeroContentController : MonoBehaviour
{
    public List<HeroStats> HeroStats;

    public HeroStats GetHeroStatsByIndex(int index) => HeroStats[index];
    public float GetDamage(SupportHeroes stats) => (stats.Damage * (stats.Level * stats.DamageMod));
    public float GetUpgradeCost(SupportHeroes stats) => (stats.Cost * (stats.Level * stats.CostMod));

    public void UpdateUI(SupportHeroes stats)
    {
        var needStatsToChange = HeroStats.FirstOrDefault(x => x.Id == stats.Id);

        needStatsToChange.Level = stats.Level;
        needStatsToChange.Name = stats.Name;
        needStatsToChange.DPS = GetDamage(stats);
        needStatsToChange.UpgradeCost = GetUpgradeCost(stats);
        needStatsToChange.MaxLevel = stats.MaxLevel;

        needStatsToChange.textName.text = needStatsToChange.Name;
        needStatsToChange.textLevel.text = needStatsToChange.Level.ToString();
        needStatsToChange.textDPS.text = needStatsToChange.DPS.ToString();
        needStatsToChange.textUpgradeCost.text = needStatsToChange.UpgradeCost.ToString();
        needStatsToChange.textMaxLevel.text = needStatsToChange.MaxLevel.ToString();
    }
}

[System.Serializable]
public class HeroStats
{
    public int Id;
    public string Name;
    public float DPS;
    public int Level;
    public int MaxLevel;
    public float UpgradeCost;
    public Image Image;
    public TMP_Text textName;
    public TMP_Text textDPS;
    public TMP_Text textLevel;
    public TMP_Text textMaxLevel;
    public TMP_Text textUpgradeCost;
}
