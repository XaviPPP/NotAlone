using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip[] extremeDamageClips;

    bool isGoingToTakeFallDamage;

    float currentDamage;
    float finalDamage = 0.0f;
    float damageMultiplier = 3.5f;

    private SurvivalManager survivalManager;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        survivalManager = GetComponent<SurvivalManager>();

        isGoingToTakeFallDamage = false;
    }

    void Update()
    {
        if (playerMovement.velocity.y < -10f)
        {
            isGoingToTakeFallDamage = true;
        }

        if (isGoingToTakeFallDamage)
        {
            currentDamage = Mathf.Abs(playerMovement.velocity.y * damageMultiplier) - 30f;

            if (finalDamage < currentDamage)
            {
                finalDamage = currentDamage;
            }

            //Debug.Log(finalDamage);
        }

        if (playerMovement.groundedPlayer && isGoingToTakeFallDamage)
        {
            //Debug.Log("Damage done to player: " + finalDamage);
            survivalManager.DepleteHealth(finalDamage);
            if (survivalManager.GetCurrentHealth() <= 0f)
            {
                audioSource.PlayOneShot(GetRandomExtremeDamageClip());
            } 
            else
            {
                audioSource.PlayOneShot(GetRandomDamageClip());
            }
            currentDamage = 0f;
            finalDamage = 0f;
            isGoingToTakeFallDamage = false;
        }
    }

    private AudioClip GetRandomDamageClip()
    {
        return damageClips[UnityEngine.Random.Range(0, damageClips.Length)];
    }

    private AudioClip GetRandomExtremeDamageClip()
    {
        return extremeDamageClips[UnityEngine.Random.Range(0, extremeDamageClips.Length)];
    }
}
