using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private SurvivalManager survivalManager;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip bodyFallingClip;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private GameObject blackBarsUI;
    [SerializeField] private GameObject deathFade;

    private int isDeadHash;
    private bool playDeathSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        survivalManager = GetComponent<SurvivalManager>();

        isDeadHash = Animator.StringToHash("isDead");

        playDeathSound = true;
    }
    public void PlayerDied(int causeOfDeath)
    {
        switch (causeOfDeath)
        {
            case DeathReasons.STARVING:
                StarvingDeath();
                break;
            case DeathReasons.FALL:
                FallDeath();
                break;
            case DeathReasons.ENEMY:

                break;
        }
    }

    private void StarvingDeath()
    {
        animator.SetBool(isDeadHash, true);
        if (playDeathSound)
        {
            AudioManager.instance.PlayClip(audioSource, deathClip, 1f);
            playDeathSound = false;
        }
        mainCamera.GetComponent<MouseLook>().enabled = false;
        menu.GetComponent<PauseMenu>().enabled = false;

        itemsUI.SetActive(false);
        deathFade.SetActive(true);
    }

    private void FallDeath()
    {
        menu.GetComponent<PauseMenu>().enabled = false;
        itemsUI.SetActive(false);
        blackBarsUI.SetActive(true);
        //Debug.Log("Current health: " + survivalManager.GetCurrentHealth());
    }

    public void PlayBodyFallingSound() {
        AudioManager.instance.PlayClip(audioSource, bodyFallingClip, 1f);
    }
}