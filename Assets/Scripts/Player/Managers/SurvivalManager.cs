using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[HideMonoScript]
public class SurvivalManager : MonoBehaviour
{
    [Title("Stats")]
    [Indent][SerializeField] private StatsClass stats;

    [Title("UI")]
    [Indent][SerializeField] private UISlidersClass sliders;
    [Indent][SerializeField] private UIObjectsClass objects;
    
    [Title("Audio")]
    [Indent][SerializeField] private AudioSource audioSource;
    [Indent][SerializeField] private AudioClip lowHealthLoopClip;
    [Indent][SerializeField] private AudioClip deathClip;
    private bool fadeIn;
    private bool fadeOut;

    //[Title("Camera")]
    //[Indent][SerializeField] private Camera cam;

    private PlayerMovement playerMovement;

    private GameObject lastAttacker;

    private void Start()
    {
        InitializeStats();

        fadeIn = true;
        fadeOut = true;

        playerMovement = GetComponent<PlayerMovement>();
        lastAttacker = null;
    }

    private void Update()
    {
        UpdateStats();
       

        if (!stats.isDead && stats._currentHealth <= 15f)
        {
            if (fadeIn)
            {
                AudioManager.instance.PlayLowHealthLoopClip(lowHealthLoopClip);

                fadeIn = false;
                fadeOut = true;
            }
            VignetteController.instance.ShowLowHealthVignette(stats._currentHealth);
        }
        else if (!stats.isDead && stats._currentHealth > 15f)
        {
            if (fadeOut)
            {
                AudioManager.instance.StopLowHealthLoopClip();
                
                fadeIn = true;
                fadeOut = false;
            }
            
            VignetteController.instance.HideVignette();
        }


        DepleteHunger(stats._hungerDepletionRate * Time.deltaTime);
        DepleteThirst(stats._thirstDepletionRate * Time.deltaTime);

        HandleDeath();
        

        if (stats._currentHunger <= 0f)
        {
            stats.isStarving = true;
            DepleteHealthOverTime();
        } else
        {
            stats.isStarving = false;
        }

        

        if (playerMovement.isRunning)
        {
            stats._currentStamina -= stats._staminaDepletionRate * Time.deltaTime;
            stats._currentStaminaDelayCounter = 0;
        }

        if (playerMovement.jumped)
        {
            stats._currentStamina -= stats._staminaToJump;
            stats._currentStaminaDelayCounter = 0;
            playerMovement.jumped = false;
        }

        if (!playerMovement.isRunning && stats._currentStamina < stats._maxStamina)
        {
            if (stats._currentStaminaDelayCounter < stats._staminaRechargeDelay && playerMovement.isGrounded)
            {
                stats._currentStaminaDelayCounter += Time.deltaTime;
            }

            if (stats._currentStaminaDelayCounter >= stats._staminaRechargeDelay)
            {
                stats._currentStamina += stats._staminaRechargeRate * Time.deltaTime;
                if (stats._currentStamina > stats._maxStamina)
                {
                    stats._currentStamina = stats._maxStamina;
                }
            }
        }
    }

    
    
    

    

    #region Getters
    
    public float GetCurrentStamina()
    {
        return stats._currentStamina;
    }

    public float GetStaminaToJump()
    {
        return stats._staminaToJump;
    }

    public float GetStaminaToRun()
    {
        return stats._staminaToRun;
    }

    public float GetCurrentHealth()
    {
        return stats._currentHealth;
    }
    
    #endregion
    
    #region Utilities

    public void DepleteHunger(float amount)
    {
        stats._currentHunger -= amount;

        if (stats._currentHunger <= 0f) stats._currentHunger = 0f;
    }

    public void ReplenishHunger(float hungerAmount)
    {
        stats._currentHunger += hungerAmount;

        if (stats._currentHunger > stats._maxHunger) stats._currentHunger = stats._maxHunger;
    }

    public void DepleteThirst(float amount)
    {
        stats._currentThirst -= amount;

        if (stats._currentThirst <= 0f) stats._currentThirst = 0f;
    }

    public void ReplenishThirst(float thirstAmount)
    {
        stats._currentThirst += thirstAmount;

        if (stats._currentThirst > stats._maxThirst) stats._currentThirst = stats._maxThirst;
    }

    public void DepleteHealthOverTime()
    {
        stats._currentHealth -= stats._healthDepletionRate * Time.deltaTime;

        if (stats._currentHealth <= 0f) stats._currentHealth = 0f;
    }

    public void DepleteHealth(float amount)
    {
        stats._currentHealth -= amount;

        if (stats._currentHealth <= 0f) stats._currentHealth = 0f;
    }

    public void ReplenishHealth(float amount)
    {
        stats._currentHealth += amount;

        if (stats._currentHealth > 100f) stats._currentHealth = 100f;
    }

    public bool CanRun()
    {
        if (stats._currentStamina > stats._staminaToRun)
            return true;
        else if (playerMovement.isRunning && stats._currentStamina > 0)
            return true;

        return false;
    }

    public bool CanJump()
    {
        return stats._currentStamina > stats._staminaToJump;
    }

    private void LoadDeathUI()
    {
        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    #region Main Functions

    private void InitializeStats()
    {
        stats._currentHunger = stats._maxHunger;
        stats._currentThirst = stats._maxThirst;
        stats._currentStamina = stats._maxStamina;
        stats._currentHealth = stats._maxHealth;

        stats.isDead = false;
        stats.isStarving = false;
    }

    private void UpdateStats()
    {
        sliders.healthSlider.value = (int)Mathf.Ceil(stats._currentHealth);
        sliders.hungerSlider.value = (int)Mathf.Ceil(stats._currentHunger);
        sliders.thirstSlider.value = (int)Mathf.Ceil(stats._currentThirst);
        sliders.staminaSlider.value = (int)Mathf.Ceil(stats._currentStamina);
    }

    private void HandleDeath()
    {
        if (stats._currentHealth <= 0f)
        {
            if (stats.isStarving)
            {
                DeathManager.instance.PlayerDied(DeathReasons.STARVING);
                return;
            } 

            if (lastAttacker != null)
            {
                DeathManager.instance.PlayerDied(DeathReasons.ENEMY);
                return;
            }

            DeathManager.instance.PlayerDied(DeathReasons.GENERIC);
        }
    }

    #endregion

    #region Internal Classes

    [Serializable]
    private class StatsClass
    {
        [Header("Health")]
        public float _maxHealth = 100f;
        public float _healthDepletionRate = 1f;
        [HideInInspector] public float _currentHealth;
        public float HealthPercent => _currentHealth / _maxHealth;

        [HideInInspector]
        public bool isDead;

        [Header("Hunger")]
        public float _maxHunger = 100f;
        public float _hungerDepletionRate = 0.1f;
        [HideInInspector] public float _currentHunger;
        public float HungerPercent => _currentHunger / _maxHunger;

        [Header("Thirst")]
        public float _maxThirst = 100f;
        public float _thirstDepletionRate = 0.3f;
        [HideInInspector] public float _currentThirst;
        public float ThirstPercent => _currentHunger / _maxHunger;

        [Header("Stamina")]
        public float _maxStamina = 100f;
        public float _staminaDepletionRate = 7.5f;
        public float _staminaRechargeRate = 15f;
        public float _staminaRechargeDelay = 1f;
        public float _staminaToRun = 10f;
        public float _staminaToJump = 15f;
        [HideInInspector] public float _currentStamina;
        [HideInInspector] public float _currentStaminaDelayCounter;
        public float StaminaPercent => _currentStamina / _maxStamina;

        [HideInInspector]
        public bool isStarving;
    }

    [Serializable]
    private class UISlidersClass
    {
        public Slider healthSlider;
        public Slider hungerSlider;
        public Slider thirstSlider;
        public Slider staminaSlider;
    }

    [Serializable]
    private class UIObjectsClass
    {
        public GameObject canvasDeath;
        public GameObject canvasMenu;
        public GameObject canvasUI;
    }

    #endregion

}
