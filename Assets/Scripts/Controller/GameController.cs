using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private SaveController saveController;
    [SerializeField] private HeroContentController heroController;
    [SerializeField] private IdleGame idleGame;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        heroController.SetList(saveController.SupportHeroes);
        heroController.UpdateHeroUI();

        idleGame.SetRegionData(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete));
        idleGame.SetBackground(saveController.RegionDatas.FirstOrDefault(x=>!x.IsComplete).SpriteBackground);
        idleGame.SetStats(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete).Stats);
        idleGame.onUpdate += ()=> saveController.UpdateValues(idleGame.PlayerData, idleGame.RegionData);
        idleGame.onUpdate += UpdateRegionData;
        heroController.onUpdate += () => saveController.UpdateValues(heroes: heroController.SupportHeroes);
        //Debug.Log($"DPS = {heroController.GetAllDamage().ToString("E3")}");
    }

    private void UpdateRegionData()
    {
        idleGame.SetRegionData(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete));
        idleGame.SetBackground(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete).SpriteBackground);
        idleGame.SetStats(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete).Stats);
    }
}
