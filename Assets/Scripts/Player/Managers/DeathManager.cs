using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

[HideMonoScript]
public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;

    private Animator animator;

    [Title("Player")]
    [Indent][SerializeField] private GameObject player;
    [Indent][SerializeField] private Transform headBone;
    [Indent][SerializeField] private CinemachineVirtualCamera cam;

    [Title("Audio")]
    [Indent][SerializeField] private AudioClip deathClip;
    [Indent][SerializeField] private AudioClip bodyFallingClip;

    [Title("UI")]
    [Indent] public UIItems items;

    private int isDeadHash;
    private bool playDeathSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = player.GetComponent<Animator>();

        isDeadHash = Animator.StringToHash("isDead");

        playDeathSound = true;
    }
    public void PlayerDied(int causeOfDeath)
    {
        switch (causeOfDeath)
        {
            case DeathReasons.GENERIC:
                Death();
                break;
            case DeathReasons.STARVING:
                Death();
                break;
            case DeathReasons.FALL:
                FallDeath();
                break;
            case DeathReasons.ENEMY:
                
                break;
        }
    }

    private void Death()
    {
        cam.transform.SetParent(headBone);
        animator.SetBool(isDeadHash, true);
        if (playDeathSound)
        {
            AudioManager.instance.PlayDeathClip(deathClip);
            playDeathSound = false;
        }
        ScriptController.instance.EnableMouseLook(false);
        ScriptController.instance.EnablePauseController(false);

        items.UI.SetActive(false);
        items.canvasDeath.SetActive(true);
    }

    private void FallDeath()
    {
        ScriptController.instance.EnablePauseController(false);
        ScriptController.instance.EnableInventoryController(false);
        items.UI.SetActive(false);
        //Debug.Log("Current health: " + player.GetComponent<SurvivalManager>().GetCurrentHealth());
    }

    public void PlayBodyFallingSound() {
        AudioManager.instance.PlayDeathClip(bodyFallingClip);
    }

    //UI class
    [System.Serializable]
    public class UIItems
    {
        public GameObject UI;
        public GameObject canvasDeath;
    }
}