using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurvivalManager : MonoBehaviour
{
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
    [SerializeField] private float _staminaRechargeRate = 10f;
    [SerializeField] private float _staminaRechargeDelay = 1f;
    private float _currentStamina;
    private float _currentStaminaDelayCounter;
    public float StaminaPercent => _currentStamina / _maxStamina;

    public static UnityAction OnPlayerDied;

    private void Start()
    {
        _currentHunger = _maxHunger;
        _currentThirst = _maxThirst;
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        _currentHunger -= _hungerDepletionRate * Time.deltaTime;
        _currentThirst -= _thirstDepletionRate * Time.deltaTime;

        if (_currentHunger <= 0 || _currentThirst <= 0)
        {
            OnPlayerDied?.Invoke();
            _currentHunger = 0;
            _currentThirst = 0;
        }

        if (PlayerMovement.isSprinting)
        {
            _currentStamina -= _staminaDepletionRate * Time.deltaTime;
            _currentStaminaDelayCounter = 0;
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
        }
    }

    public float getCurrentStamina()
    {
        return _currentStamina;
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
}
