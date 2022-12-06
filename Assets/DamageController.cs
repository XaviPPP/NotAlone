using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageClips;

    bool isGoingToTakeFallDamage;

    float currentDamage;
    float finalDamage = 0.0f;
    float damageMultiplier = 3.5f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

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
            GetComponent<SurvivalManager>().DepleteHealth(finalDamage);
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
            currentDamage = 0f;
            finalDamage = 0f;
            isGoingToTakeFallDamage = false;
        }
    }

    private AudioClip GetRandomClip()
    {
        return damageClips[UnityEngine.Random.Range(0, damageClips.Length)];
    }
}
