using UnityEngine;
using UnityEngine.UI;

[HideScriptField]
public class DeathManager : MonoBehaviour
{
    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip bodyFallingClip;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Character")]
    [SerializeField] private Transform headBone;

    [Header("UI")]
    public UIItems items;

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
        mainCamera.transform.SetParent(headBone);
        animator.SetBool(isDeadHash, true);
        if (playDeathSound)
        {
            AudioManager.instance.PlayDeathClip(deathClip);
            playDeathSound = false;
        }
        mainCamera.GetComponent<MouseLook>().enabled = false;
        items.menu.GetComponent<PauseMenu>().enabled = false;

        items.itemsUI.SetActive(false);
        items.canvasDeath.SetActive(true);
    }

    private void FallDeath()
    {
        items.menu.GetComponent<PauseMenu>().enabled = false;
        items.itemsUI.SetActive(false);
        items.blackBarsUI.SetActive(true);
        //Debug.Log("Current health: " + survivalManager.GetCurrentHealth());
    }

    public void PlayBodyFallingSound() {
        AudioManager.instance.PlayDeathClip(bodyFallingClip);
    }

    //UI class
    [System.Serializable]
    public class UIItems
    {
        public GameObject menu;
        public GameObject itemsUI;
        public GameObject blackBarsUI;
        public GameObject canvasDeath;
    }
}