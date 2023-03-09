using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManagerProxy : MonoBehaviour
{
    public void CallDeathManagerBodySound()
    {
        DeathManager.instance.PlayBodyFallingSound();
    }
}
