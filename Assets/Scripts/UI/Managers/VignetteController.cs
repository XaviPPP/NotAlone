using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[HideMonoScript]
public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;

    [Title("Objects")]
    [Indent][SerializeField] private GameObject vignette;
    [Indent][SerializeField] private GameObject player;

    [Title("Properties")]
    [Indent]
    [SerializeField]
    [Range(0f, 1f)]
    private float minAlpha = 0.3f;

    [Indent]
    [SerializeField]
    [Range(0f, 1f)] 
    private float maxAlpha = 0.8f;

    [Indent][SerializeField] private float minHealth = 5f;
    [Indent][SerializeField] private float maxHealth = 15f;

    private GameObject vignetteChild;

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

    private void Start()
    {
        vignetteChild = vignette.transform.GetChild(0).gameObject;
    }

    public void ShowLowHealthVignette(float health)
    {
        vignette.SetActive(true);

        // Calculate the alpha value based on the player's health
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, (health - minHealth) / (maxHealth - minHealth));

        // Update the alpha value of the vignette color
        StartCoroutine(FadeInVignette(alpha, 0.5f));

        //Color vignetteColor = vignetteImage.color;
        //vignetteColor.a = alpha;
        //vignetteImage.color = vignetteColor;
    }

    public void HideVignette()
    {
        StartCoroutine(FadeOutVignette(0f, 0.5f));
    }

    IEnumerator FadeInVignette(float targetAlpha, float fadeTime)
    {
        Image image = vignetteChild.GetComponent<Image>();
        Color color = image.color;

        while (image.color.a < targetAlpha)
        {
            color.a += Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }

    IEnumerator FadeOutVignette(float targetAlpha, float fadeTime)
    {
        Image image = vignetteChild.GetComponent<Image>();
        Color color = image.color;

        while (image.color.a > targetAlpha)
        {
            color.a -= Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }
}
