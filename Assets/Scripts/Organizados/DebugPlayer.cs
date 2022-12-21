using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugPlayer
{
    public class DebugPlayer : MonoBehaviour
    {
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
            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<DeathManager>().PlayerDied(DeathReasons.STARVING);
                }

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    character.GetComponent<PlayerMovement>().enabled = false;
                    character.transform.position = new Vector3(character.transform.position.x, 300f, character.transform.position.z);
                    character.GetComponent<PlayerMovement>().enabled = true;

                }
            }
        }

        private void StatsDebug()
        {
            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    survivalManager.DepleteHealth(10f);

                    if (survivalManager.GetCurrentHealth() < 1f)
                    {
                        Debug.Log("Died from debug");
                        GetComponent<DeathManager>().PlayerDied(DeathReasons.STARVING);
                    }
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    survivalManager.DepleteHunger(10f);
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    survivalManager.DepleteThirst(10f);
                }
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    survivalManager.DepleteOneHealth();
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                survivalManager.ReplenishHealth(10f);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                survivalManager.ReplenishHunger(10f);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                survivalManager.ReplenishThirst(10f);
            }
        }
    }
}
