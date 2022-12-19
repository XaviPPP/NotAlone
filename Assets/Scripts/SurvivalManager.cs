using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SurvivalManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _healthDepletionRate = 1f;
    private float _currentHealth;
    public float HealthPercent => _currentHealth / _maxHealth;
    private bool isDead;

    [Header("Hunger")]
    [SerializeField] private float _maxHunger = 100f;
    [SerializeField] private float _hungerDepletionRate = 0.1f;
    private float _currentHunger;
    public float HungerPercent => _currentHunger / _maxHunger;

    [Header("Thirst")]
    [SerializeField] private float _maxThirst = 100f;
    [SerializeField] private float _thirstDepletionRate = 0.3f;
    private float _currentThirst;
    public float ThirstPercent => _currentHunger / _maxHunger;

    [Header("Stamina")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaDepletionRate = 7.5f;
    [SerializeField] private float _staminaRechargeRate = 15f;
    [SerializeField] private float _staminaRechargeDelay = 1f;
    [SerializeField] private float _staminaToRun = 10f;
    [SerializeField] private float _staminaToJump = 15f;
    private float _currentStamina;
    private float _currentStaminaDelayCounter;
    public float StaminaPercent => _currentStamina / _maxStamina;

    [Header("UI")]
    public TextMeshProUGUI healthValueUI;
    public TextMeshProUGUI hungerValueUI;
    public TextMeshProUGUI thirstValueUI;
    public TextMeshProUGUI staminaValueUI;
    [SerializeField] private GameObject deathFade;
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject[] itemsUI;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lowHealthLoopClip;
    [SerializeField] private AudioClip deathClip;
    private bool fadeIn;
    private bool fadeOut;
    private bool playDeathSound;

    [Header("Camera")]
    [SerializeField] private Camera camera;

    private Animator animator;

    private void Start()
    {
        _currentHunger = _maxHunger;
        _currentThirst = _maxThirst;
        _currentStamina = _maxStamina;
        _currentHealth = _maxHealth;

        fadeIn = true;
        fadeOut = true;
        playDeathSound = true;

        isDead = false;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        StatsDebug();

        healthValueUI.text = ((int)_currentHealth).ToString();
        hungerValueUI.text = ((int)_currentHunger).ToString();
        thirstValueUI.text = ((int)_currentThirst).ToString();
        staminaValueUI.text = ((int)_currentStamina).ToString();

        if (!isDead && _currentHealth <= 15f && fadeIn)
        {
            StartCoroutine(AudioFader.FadeIn(audioSource, lowHealthLoopClip, 3f));
            fadeIn = false;
            fadeOut = true;
        }
        else if (!isDead && _currentHealth > 15f && fadeOut)
        {
            StartCoroutine(AudioFader.FadeOut(audioSource, 3f));
            fadeOut = false;
            fadeIn = true;
        }

        _currentHunger -= _hungerDepletionRate * Time.deltaTime;
        _currentThirst -= _thirstDepletionRate * Time.deltaTime;

        if (_currentHealth < 1f)
        {
            PlayerDied();
        }

        if (_currentHunger <= 0f)
        {
            DepleteHealthOverTime();
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

    private void PlayerDied()
    {
        isDead = true;
        if (playDeathSound)
        {
            audioSource.PlayOneShot(deathClip);
            playDeathSound = false;
        }
        animator.SetBool("isDead", true);

        for (int i = 0; i < itemsUI.Length; i++)
        {
            itemsUI[i].SetActive(false);
        }

        deathFade.SetActive(true);

        canvasMenu.GetComponent<PauseMenu>().enabled = false;
        camera.GetComponent<MouseLook>().enabled = false;
    }

    private void LoadDeathUI()
    {
        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
    }

    private void StatsDebug()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                DepleteHealth(10f);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                DepleteHunger(10f);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                DepleteThirst(10f);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReplenishHealth(10f);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ReplenishHunger(10f);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ReplenishThirst(10f);
        }
    }

    public float GetCurrentStamina()
    {
        return _currentStamina;
    }

    public float GetStaminaToJump()
    {
        return _staminaToJump;
    }

    public float GetStaminaToRun()
    {
        return _staminaToRun;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void DepleteHunger(float amount)
    {
        _currentHunger -= amount;

        if (_currentHunger <= 0f) _currentHunger = 0f;
    }

    public void ReplenishHunger(float hungerAmount)
    {
        _currentHunger += hungerAmount;

        if (_currentHunger > _maxHunger) _currentHunger = _maxHunger;
    }

    public void DepleteThirst(float amount)
    {
        _currentThirst -= amount;

        if (_currentThirst <= 0f) _currentThirst = 0f;
    }

    public void ReplenishThirst(float thirstAmount)
    {
        _currentThirst += thirstAmount;

        if (_currentThirst > _maxThirst) _currentThirst = _maxThirst;
    }

    public void DepleteHealthOverTime()
    {
        _currentHealth -= _healthDepletionRate * Time.deltaTime;

        if (_currentHealth <= 0f) _currentHealth = 0f;
    }

    public void DepleteHealth(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f) _currentHealth = 0f;
    }

    public void ReplenishHealth(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > 100f) _currentHealth = 100f;
    }
}
