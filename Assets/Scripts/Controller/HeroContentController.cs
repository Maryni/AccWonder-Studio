using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class HeroContentController : MonoBehaviour
{
    public List<HeroStats> HeroStats;
    public List<SupportHeroes> SupportHeroes;

    public HeroStats GetHeroStatsByIndex(int index) => HeroStats[index];
    public float GetDamage(SupportHeroes stats) => IdleGame.GetGeometryProgressionValue(stats.Damage,stats.DamageMod, stats.Level);
    public float GetUpgradeCost(SupportHeroes stats) => IdleGame.GetGeometryProgressionValue(stats.Cost,stats.CostMod, stats.Level);
    
    public void SetList(List<SupportHeroes> stats) => SupportHeroes = stats;

    public float GetAllDamage()
    {
        float result = 0f;
        foreach(var item in SupportHeroes)
        {
            var dps = GetDamage(item);
            var crit = item.CritChance;
            var random = Random.Range(0f, 100f);
            if(crit >= random)
            {
                dps *= item.CritMod;
            }
            result += dps;
        }
        return result;
    }

    public void UpdateUI(SupportHeroes stats)
    {
        var needStatsToChange = HeroStats.FirstOrDefault(x => x.Id == stats.Id);

        needStatsToChange.Name = stats.Name;
        needStatsToChange.Level = stats.Level;
        needStatsToChange.DPS = GetDamage(stats);
        needStatsToChange.MaxLevel = stats.MaxLevel;
        needStatsToChange.UpgradeCost = GetUpgradeCost(stats);

        QuickUpdate();
    }

    private void QuickUpdate()
    {
        foreach(var item in HeroStats)
        {
            item.textName.text = item.Name;
            item.textDPS.text = item.DPS.ToString();
            item.textLevel.text = item.Level.ToString();
            item.textMaxLevel.text = item.MaxLevel.ToString();
            item.textUpgradeCost.text = item.UpgradeCost.ToString();
        }
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
