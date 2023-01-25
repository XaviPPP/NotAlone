using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugPlayer
{
    public class DebugPlayer : MonoBehaviour
    {
        [Header("Keys")]
        [SerializeField] private KeyCode mainKey;

        [Space(20)]

        [SerializeField] private KeyCode deathKey;
        [SerializeField] private KeyCode teleportKey;

        [Space(20)]

        [SerializeField] private KeyCode healthKey;
        [SerializeField] private KeyCode hungerKey;
        [SerializeField] private KeyCode thirstKey;
        [SerializeField] private KeyCode depleteOneHealthKey;

        [Space(20)]

        [SerializeField] private KeyCode rainWeatherKey;

        [Header("Character")]
        [SerializeField] private GameObject character;
        private SurvivalManager survivalManager;

        private void Start()
        {
            survivalManager = GetComponent<SurvivalManager>();
        }

        // Update is called once per frame
        void Update()
        {
            DeathDebug();
            StatsDebug();
        }

        private void DeathDebug()
        {
            if (Input.GetKey(mainKey))
            {
                if (Input.GetKeyDown(deathKey))
                {
                    GetComponent<DeathManager>().PlayerDied(DeathReasons.STARVING);
                }

                if (Input.GetKeyDown(teleportKey))
                {
                    character.GetComponent<PlayerMovement>().enabled = false;
                    character.transform.position = new Vector3(character.transform.position.x, 300f, character.transform.position.z);
                    character.GetComponent<PlayerMovement>().enabled = true;

                }
            }
        }

        private void StatsDebug()
        {
            if (Input.GetKey(mainKey))
            {
                if (Input.GetKeyDown(healthKey))
                {
                    survivalManager.DepleteHealth(10f);

                    if (survivalManager.GetCurrentHealth() < 1f)
                    {
                        Debug.Log("Died from debug");
                        GetComponent<DeathManager>().PlayerDied(DeathReasons.STARVING);
                    }
                }
                if (Input.GetKeyDown(hungerKey))
                {
                    survivalManager.DepleteHunger(10f);
                }
                if (Input.GetKeyDown(thirstKey))
                {
                    survivalManager.DepleteThirst(10f);
                }
                if (Input.GetKeyDown(depleteOneHealthKey))
                {
                    survivalManager.DepleteOneHealth();
                }
                if (Input.GetKeyDown(rainWeatherKey))
                {
                    Enviro.EnviroManager.instance.Weather.ChangeWeather("Rain");
                }
                return;
            }

            if (Input.GetKeyDown(healthKey))
            {
                survivalManager.ReplenishHealth(10f);
            }

            if (Input.GetKeyDown(hungerKey))
            {
                survivalManager.ReplenishHunger(10f);
            }

            if (Input.GetKeyDown(thirstKey))
            {
                survivalManager.ReplenishThirst(10f);
            }
        }
    }
}
