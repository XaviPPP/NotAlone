using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugPlayer
{
    [HideScriptField]
    public class DebugPlayer : MonoBehaviour
    {
        [Header("Keys & Values")]
        [SerializeField] private KeyCode mainKey;

        [Space(20)]

        [SerializeField] private KeyCode deathKey;

        [Space]

        [SerializeField] public KeyCode teleportKey;
        [SerializeField] public float height = 300f;

        [Space]

        public StatsClass stats;

        [Space]

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
                    character.transform.position = new Vector3(character.transform.position.x, height, character.transform.position.z);
                    character.GetComponent<PlayerMovement>().enabled = true;

                }
            }
        }

        private void StatsDebug()
        {
            if (Input.GetKey(mainKey))
            {
                if (Input.GetKeyDown(stats.healthKey))
                {
                    survivalManager.DepleteHealth(10f);

                    if (survivalManager.GetCurrentHealth() < 1f)
                    {
                        Debug.Log("Died from debug");
                        GetComponent<DeathManager>().PlayerDied(DeathReasons.STARVING);
                    }
                }
                if (Input.GetKeyDown(stats.hungerKey))
                {
                    survivalManager.DepleteHunger(10f);
                }
                if (Input.GetKeyDown(stats.thirstKey))
                {
                    survivalManager.DepleteThirst(10f);
                }
                if (Input.GetKeyDown(stats.depleteOneHealthKey))
                {
                    survivalManager.DepleteOneHealth();
                }
                if (Input.GetKeyDown(rainWeatherKey))
                {
                    Enviro.EnviroManager.instance.Weather.ChangeWeather("Rain");
                }
                return;
            }

            if (Input.GetKeyDown(stats.healthKey))
            {
                survivalManager.ReplenishHealth(10f);
            }

            if (Input.GetKeyDown(stats.hungerKey))
            {
                survivalManager.ReplenishHunger(10f);
            }

            if (Input.GetKeyDown(stats.thirstKey))
            {
                survivalManager.ReplenishThirst(10f);
            }
        }
    }

    [System.Serializable]
    public class StatsClass
    {
        public KeyCode healthKey;
        public float healthDepleteValue = 10f;

        [Space]

        public KeyCode hungerKey;
        public float hungerDepleteValue = 10f;

        [Space]

        public KeyCode thirstKey;
        public float thirstDepleteValue = 10f;

        [Space]

        public KeyCode depleteOneHealthKey;
    }
}
