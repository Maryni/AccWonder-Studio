using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
   [SerializeField] private IdleGame idleGame;
   [SerializeField] private SaveController saveController;
   
   public void EnableSkill(int id)
   {
      switch (id)
      {
         case 0:
            StartCoroutine(CallFuncAndOnComplete(
            ()=>EnableDamage(),
            ()=> DisableDamage(),
            5f
            ));
            break;
         case 1: 
            StartCoroutine(CallFuncAndOnComplete(
               ()=>EnableCrit(),
               ()=> DisableDamage(),
               5f
            ));
            break;
         case 2: 
            StartCoroutine(CallFuncAndOnComplete(
               ()=>AddGold(),
               ()=> Debug.Log("gold skill"),
               5f
            ));
            break;
         case 3: 
            StartCoroutine(CallFuncAndOnComplete(
               ()=>AddBlood(),
               ()=> Debug.Log("blood skill"),
               5f
            ));
            break;
      }
   }

   private IEnumerator CallFuncAndOnComplete(Action onStart, Action onComplete, float duration)
   {
      onStart.Invoke();
      yield return new WaitForSeconds(duration);
      onComplete.Invoke();
   }

   private void AddBlood() => saveController.PlayerData.Blood *= 2;
   private void AddGold() => saveController.PlayerData.Gold *= 2;
   private void EnableCrit() => saveController.SupportHeroes.ForEach(x => x.CritChance = 100);//add save prev critChance
   private void EnableDamage() => idleGame.damageAuto *= 2;
   private void DisableDamage() => idleGame.damageAuto /= 2;
}
