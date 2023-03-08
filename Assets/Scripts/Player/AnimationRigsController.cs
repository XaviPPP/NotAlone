using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigsController : MonoBehaviour
{
    public static AnimationRigsController instance;

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeWeightSmooth(Rig rig, float start, float end)
    {
        StartCoroutine(SmoothRig(rig, start, end));
    }

    IEnumerator SmoothRig(Rig rig, float start, float end)
    {

        float elapsedTime = 0;

        float waitTime = 0.5f;



        while (elapsedTime < waitTime)

        {

            rig.weight = Mathf.Lerp(start, end, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

    }
}
