using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class DamageController : MonoBehaviour
{
    private SurvivalManager survivalManager;
    private float finalFallDamage;

    [Title("Damage")]
    [Indent][SerializeField] private float damageMultiplier = 3.5f;

    private void Start()
    {
        survivalManager = GetComponent<SurvivalManager>();

        finalFallDamage = 0f;
    }

    public void GetAccumulatedFallDamage(float velocity)
    {
        float damage = Mathf.Abs(velocity * damageMultiplier) - 50f;
        finalFallDamage = damage;
        //Debug.Log("Damage: " + damage);
        //Debug.Log("Final damage: " + finalFallDamage);
    }

    public float GetFinalFallDamage()
    {
        return finalFallDamage;
    }

    public void ApplyAccumulatedFallDamage()
    {   
        survivalManager.DepleteHealth(finalFallDamage);
        VignetteController.instance.ShowDamageVignette();
        AudioManager.instance.PlayRandomDamageClip();
        //Debug.Log("Health after deplete: " + survivalManager.GetCurrentHealth());
    }

    public void ApplyDamage(float damage)
    {
        survivalManager.DepleteHealth(damage);
        VignetteController.instance.ShowDamageVignette();
        AudioManager.instance.PlayRandomDamageClip();
    }

    public void ResetAccumulatedFallDamage()
    {
        finalFallDamage = 0f;
    }
}
