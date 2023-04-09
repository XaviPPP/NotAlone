using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSpot : Interactable
{

    public string cantSleepPrompt = "Não podes dormir agora";

    protected override void Interact(GameObject player)
    {
        if (Enviro.EnviroManager.instance.Time.hours >= 20f || Enviro.EnviroManager.instance.Time.hours < 6f)
        {
            StartCoroutine(Sleep(player));
        }   
    }

    private IEnumerator Sleep(GameObject player)
    {
        UiManager.instance.EnableSleep(true);

        yield return new WaitForSeconds(3f);

        SurvivalManager survivalManager = player.GetComponent<SurvivalManager>();

        survivalManager.DepleteHunger(35f);
        survivalManager.DepleteThirst(60f);

        UiManager.instance.EnableSleep(false);

        Enviro.EnviroManager.instance.Time.hours += 10;
    }
}
