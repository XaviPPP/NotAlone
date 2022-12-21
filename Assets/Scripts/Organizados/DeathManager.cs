using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    private Animator animator;

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
            AudioManager.instance.PlayDeathClip(deathClip);
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
        AudioManager.instance.PlayDeathClip(bodyFallingClip);
    }
}