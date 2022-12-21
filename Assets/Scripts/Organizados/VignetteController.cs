using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;

    [SerializeField] private GameObject vignette;

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
    }

    public void ShowVignetteForDamage()
    {

    }

    public void ShowVignette()
    {
        vignette.SetActive(true);
        StartCoroutine(FadeInVignette(2f));
    }

    IEnumerator FadeInVignette(float fadeTime)
    {
        Image image = vignette.GetComponent<Image>();
        Color color = image.color;

        while (image.color.a < 230f)
        {
            color.a += Time.deltaTime / fadeTime;
            image.color = color;
            yield return null;
        }
    }
}
