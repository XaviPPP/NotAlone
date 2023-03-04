using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[HideMonoScript]
public class SurvivalManager : MonoBehaviour
{
    [Title("Stats")]
    [Indent][SerializeField] private StatsClass stats;

    [Title("UI")]
    [Indent][SerializeField] private UITextClass textItems;
    [Indent][SerializeField] private UIObjectsClass objects;
    
    [Title("Audio")]
    [Indent][SerializeField] private AudioSource audioSource;
    [Indent][SerializeField] private AudioClip lowHealthLoopClip;
    [Indent][SerializeField] private AudioClip deathClip;
    private bool fadeIn;
    private bool fadeOut;

    [Title("Camera")]
    [Indent][SerializeField] private Camera cam;

    private void Start()
    {
        stats._currentHunger = stats._maxHunger;
        stats._currentThirst = stats._maxThirst;
        stats._currentStamina = stats._maxStamina;
        stats._currentHealth = stats._maxHealth;

        fadeIn = true;
        fadeOut = true;

        stats.isDead = false;
        stats.isStarving = false;
    }

    private void Update()
    {
        textItems.healthValueUI.text = ((int)stats._currentHealth).ToString();
        textItems.hungerValueUI.text = ((int)stats._currentHunger).ToString();
        textItems.thirstValueUI.text = ((int)stats._currentThirst).ToString();
        textItems.staminaValueUI.text = ((int)stats._currentStamina).ToString();
        

        if (!stats.isDead && (stats._currentHealth <= 15f && stats._currentHealth > 1f))
        {
            if (fadeIn)
            {
                AudioManager.instance.PlayLowHealthLoopClip(lowHealthLoopClip);

                fadeIn = false;
                fadeOut = true;
            }
            VignetteController.instance.ShowLowHealthVignette();
        }
        else if (!stats.isDead && stats._currentHealth > 15f && fadeOut)
        {
            AudioManager.instance.StopLowHealthLoopClip();
            fadeOut = false;
            fadeIn = true;
        }

        DepleteHunger(stats._hungerDepletionRate * Time.deltaTime);
        DepleteThirst(stats._thirstDepletionRate * Time.deltaTime);



        if (stats._currentHealth < 1f && stats.isStarving)
        {
            DeathManager.instance.PlayerDied(DeathReasons.STARVING);
        }

        if (stats._currentHunger <= 0f)
        {
            stats.isStarving = true;
            DepleteHealthOverTime();
        } else
        {
            stats.isStarving = false;
        }

        /*if (_currentStamina > _staminaToRun)
        {
            PlayerMovement.canSprint = true;
        } else if (PlayerMovement.isSprinting && _currentStamina > 0)
        {
            PlayerMovement.canSprint = true;
        }
        else
        {
            PlayerMovement.canSprint = false;
        }

        if (PlayerMovement.isSprinting)
        {
            _currentStamina -= _staminaDepletionRate * Time.deltaTime;
            _currentStaminaDelayCounter = 0;
        }

        if (PlayerMovement.jumped)
        {
            _currentStamina -= _staminaToJump;
            _currentStaminaDelayCounter = 0;
            PlayerMovement.jumped = false;
        }

        if (!PlayerMovement.isSprinting && _currentStamina < _maxStamina)
        {
            if (_currentStaminaDelayCounter < _staminaRechargeDelay)
            {
                _currentStaminaDelayCounter += Time.deltaTime;
            }

            if (_currentStaminaDelayCounter >= _staminaRechargeDelay)
            {
                _currentStamina += _staminaRechargeRate * Time.deltaTime;
                if (_currentStamina > _maxStamina)
                {
                    _currentStamina = _maxStamina;
                }
            }
        }*/
    }

    

    private void LoadDeathUI()
    {
        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
    }

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

    public void DepleteOneHealth()
    {
        stats._currentHealth -= 1f;

        if (stats._currentHealth <= 0f) stats._currentHealth = 0f;
    }

    public void ReplenishHealth(float amount)
    {
        stats._currentHealth += amount;

        if (stats._currentHealth > 100f) stats._currentHealth = 100f;
    }

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
    private class UITextClass
    {
        public TextMeshProUGUI healthValueUI;
        public TextMeshProUGUI hungerValueUI;
        public TextMeshProUGUI thirstValueUI;
        public TextMeshProUGUI staminaValueUI;
    }

    [Serializable]
    private class UIObjectsClass
    {
        public GameObject canvasDeath;
        public GameObject canvasMenu;
        public GameObject canvasUI;
    }
}
