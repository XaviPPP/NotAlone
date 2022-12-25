using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private SurvivalManager survivalManager;
    private float finalFallDamage;

    [SerializeField] private float damageMultiplier = 3.5f;

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
        //Debug.Log("Health after deplete: " + survivalManager.GetCurrentHealth());
    }

    public void ResetAccumulatedFallDamage()
    {
        finalFallDamage = 0f;
    }
}
