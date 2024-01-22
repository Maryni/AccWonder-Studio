using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private SaveController saveController;
    [SerializeField] private HeroContentController heroController;

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

        Debug.Log($"DPS = {heroController.GetAllDamage().ToString("E")}");
    }
}
