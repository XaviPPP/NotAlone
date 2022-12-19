using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeathAnimation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<SurvivalManager>().DepleteHealth(100f);
            }
        }
    }
}
