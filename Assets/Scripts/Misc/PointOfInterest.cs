using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SphereCollider))]
public class PointOfInterest : MonoBehaviour
{
    [Title("Properties")]
    [SerializeField] private float timeToLook = 5f;
    [SerializeField] private float fov = 30f;
    [SerializeField] private float fovSwitchTime = 0.5f;
    [SerializeField, Multiline] private string subtitleText;

    bool wasAlreadyTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wasAlreadyTriggered)
        {
            //Debug.Log("Entered");
            LookAtInterestPoint(other.gameObject);
            wasAlreadyTriggered = true;
        }
    }

    void LookAtInterestPoint(GameObject player)
    {
        CinemachineVirtualCamera cam = player.GetComponentInChildren<CinemachineVirtualCamera>();

        StartCoroutine(LookAtPoint(cam));
    }

    IEnumerator LookAtPoint(CinemachineVirtualCamera cam)
    {
        //ScriptController.instance.EnableMouseLook(false);
        ScriptController.instance.EnableInventoryController(false);
        ScriptController.instance.EnablePauseController(false);
        UiManager.instance.EnableUI(false);
        UiManager.instance.ShowSubtitle(subtitleText);

        float originalFov = cam.m_Lens.FieldOfView;

        cam.LookAt = gameObject.transform;
        StartCoroutine(SmoothFOVChange(cam, fov, fovSwitchTime));


        yield return new WaitForSeconds(timeToLook);


        cam.LookAt = null;
        StartCoroutine(SmoothFOVChange(cam, originalFov, fovSwitchTime));


        yield return new WaitForSeconds(.5f);

        UiManager.instance.EnableSubtitle(false);
        UiManager.instance.EnableUI(true);
        //ScriptController.instance.EnableMouseLook(true);
        ScriptController.instance.EnableInventoryController(true);
        ScriptController.instance.EnablePauseController(true);

        GetComponent<Flashback>().PlayFlashback();
    }

    IEnumerator SmoothFOVChange(CinemachineVirtualCamera cam, float targetFOV, float duration)
    {
        float initialFOV = cam.m_Lens.FieldOfView; // get the initial FOV
        float timeElapsed = 0f; // initialize the time elapsed variable

        while (timeElapsed < duration)
        { // while the time elapsed is less than the duration
            timeElapsed += Time.deltaTime; // add the time since the last frame to the time elapsed
            float t = Mathf.Clamp01(timeElapsed / duration); // calculate the lerp t value between 0 and 1

            // use Mathf.Lerp to smoothly interpolate the FOV between the initial and target FOV
            cam.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            yield return null; // wait for the next frame
        }

        cam.m_Lens.FieldOfView = targetFOV; // set the FOV to the target value when the duration is reached
    }
}
