using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[HideMonoScript]
public class DeathManager : MonoBehaviour
{
    private Animator animator;

    [Title("Audio")]
    [Indent][SerializeField] private AudioClip deathClip;
    [Indent][SerializeField] private AudioClip bodyFallingClip;

    [Title("Camera")]
    [Indent][SerializeField] private Camera mainCamera;

    [Title("Character")]
    [Indent][SerializeField] private Transform headBone;

    [Title("UI")]
    [Indent] public UIItems items;

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
        ScriptController.instance.EnableMouseLook(false);
        ScriptController.instance.EnablePauseController(false);

        items.itemsUI.SetActive(false);
        items.canvasDeath.SetActive(true);
    }

    private void FallDeath()
    {
        ScriptController.instance.EnablePauseController(false);
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