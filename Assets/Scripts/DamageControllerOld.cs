using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageControllerOld : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip[] extremeDamageClips;
    [SerializeField] private AudioClip boneCrack;

    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject blackBarsUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject[] itemsUI;

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
        if (playerMovement.velocity.y < -15f)
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

            Debug.Log(finalDamage);

            if (finalDamage >= 100f)
            {
                for (int i = 0; i < itemsUI.Length; i++)
                {
                    itemsUI[i].SetActive(false);
                }
                canvasMenu.GetComponent<PauseMenu>().enabled = false;
                blackBarsUI.SetActive(true);
            }
        }

        if (playerMovement.groundedPlayer && isGoingToTakeFallDamage)
        {
            //Debug.Log("Damage done to player: " + finalDamage);
            survivalManager.DepleteHealth(finalDamage);
            if (survivalManager.GetCurrentHealth() <= 0f)
            {
                //audioSource.PlayOneShot(GetRandomExtremeDamageClip());
                deathUI.SetActive(true);
                deathUI.GetComponent<Animator>().enabled = false;
                deathUI.GetComponent<Image>().color = new Color(0, 0, 0, 255);
            } 
            else
            {
                audioSource.PlayOneShot(GetRandomDamageClip());
                audioSource.PlayOneShot(boneCrack);
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
