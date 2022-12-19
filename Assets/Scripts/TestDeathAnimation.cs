using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeathAnimation : MonoBehaviour
{
    [SerializeField] private GameObject character;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<SurvivalManager>().DepleteHealth(100f);
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                character.GetComponent<PlayerMovement>().enabled = false;
                character.transform.position = new Vector3(character.transform.position.x, 300f, character.transform.position.z);
                character.GetComponent<PlayerMovement>().enabled = true;
                
            }
        }
    }
}
