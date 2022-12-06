using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SurvivalManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _healthDepletionRate = 1f;
    private float _currentHealth;
    public float HealthPercent => _currentHealth / _maxHealth;

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

    public static UnityAction OnPlayerDied;

    public TextMeshProUGUI healthValueUI;

    private void Start()
    {
        _currentHunger = _maxHunger;
        _currentThirst = _maxThirst;
        _currentStamina = _maxStamina;
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReplenishHealth(20f);
        }

        healthValueUI.text = ((int)_currentHealth).ToString();

        _currentHunger -= _hungerDepletionRate * Time.deltaTime;
        _currentThirst -= _thirstDepletionRate * Time.deltaTime;

        if (_currentHealth <= 0f)
        {
            OnPlayerDied?.Invoke();
        }

        if (_currentHunger <= 0f)
        {
            DepleteHealthOverTime();
            _currentHunger = 0f;
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

    public void ReplenishHunger(float hungerAmount)
    {
        _currentHunger += hungerAmount;

        if (_currentHunger > _maxHunger) _currentHunger = _maxHunger;
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

        if (_currentHealth < 0f) _currentHealth = 0f;
    }

    public void ReplenishHealth(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > 100f) _currentHealth = 100f;
    }
}
