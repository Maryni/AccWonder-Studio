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
        foreach(var item in saveController.SupportHeroes)
        {
            heroController.UpdateUI(item);
        }

        heroController.SetList(saveController.SupportHeroes);
        idleGame.SetStats(saveController.RegionDatas.FirstOrDefault(x => !x.IsComplete).Stats);
        //Debug.Log($"DPS = {heroController.GetAllDamage().ToString("E3")}");
    }
}
