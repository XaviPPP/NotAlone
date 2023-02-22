using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[HideMonoScript]
public class JumpSounds : MonoBehaviour
{
    [Title("Audio")]
    [Indent][SerializeField] private AudioSource audioSource;

    [Indent][SerializeField] private ClipsClass clips;

    [Title("Ground")]
    [Indent][SerializeField] private Transform groundCheck;

    [Title("Masks")]
    [Indent][SerializeField] private MasksClass masks;
    
    private float groundDistance = 0.4f;

    private TerrainDetector terrainDetector;

    // Start is called before the first frame update
    void Start()
    {
        terrainDetector = new TerrainDetector();
    }

    /* Update is called once per frame
    void Update()
    {
        if (playerMovement.isJumping && playJumpSound)
        {
            PlayJumpSound();
            //Debug.Log("called jump sound");
            playJumpSound = false;
            playLandSound = true;
        }

        if (!playerMovement.isJumping && playLandSound)
        {
            PlayLandSound();
            //Debug.Log("called land sound");
            playLandSound = false;
            playJumpSound = true;
        }
    }*/

    private void PlayJumpSound()
    {
        AudioClip clip = GetRandomJumpClip();
        audioSource.PlayOneShot(clip);
    }

    private void PlayLandSound()
    {
        AudioClip clip = GetRandomLandClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomJumpClip()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return clips.dirtJumpClips[UnityEngine.Random.Range(0, clips.dirtJumpClips.Length)];
                case 1:
                    return clips.grassJumpClips[UnityEngine.Random.Range(0, clips.grassJumpClips.Length)];
                case 2:
                    return clips.gravelJumpClips[UnityEngine.Random.Range(0, clips.gravelJumpClips.Length)];
                case 3:
                    return clips.mudJumpClips[UnityEngine.Random.Range(0, clips.mudJumpClips.Length)];
                case 4:
                    return clips.leavesJumpClips[UnityEngine.Random.Range(0, clips.leavesJumpClips.Length)];
                case 5:
                    return clips.grassJumpClips[UnityEngine.Random.Range(0, clips.grassJumpClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objMetalMask))
        {
            return clips.metalJumpClips[UnityEngine.Random.Range(0, clips.metalJumpClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objWoodMask))
        {
            return clips.woodJumpClips[UnityEngine.Random.Range(0, clips.woodJumpClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objRockMask))
        {
            return clips.rockJumpClips[UnityEngine.Random.Range(0, clips.rockJumpClips.Length)];
        }
        else { return null; }
    }

    private AudioClip GetRandomLandClip()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return clips.dirtLandClips[UnityEngine.Random.Range(0, clips.dirtLandClips.Length)];
                case 1:
                    return clips.grassLandClips[UnityEngine.Random.Range(0, clips.grassLandClips.Length)];
                case 2:
                    return clips.gravelLandClips[UnityEngine.Random.Range(0, clips.gravelLandClips.Length)];
                case 3:
                    return clips.mudLandClips[UnityEngine.Random.Range(0, clips.mudLandClips.Length)];
                case 4:
                    return clips.leavesLandClips[UnityEngine.Random.Range(0, clips.leavesLandClips.Length)];
                case 5:
                    return clips.grassLandClips[UnityEngine.Random.Range(0, clips.grassLandClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objMetalMask))
        {
            return clips.metalLandClips[UnityEngine.Random.Range(0, clips.metalLandClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objWoodMask))
        {
            return clips.woodLandClips[UnityEngine.Random.Range(0, clips.woodLandClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, masks.objRockMask))
        {
            return clips.rockLandClips[UnityEngine.Random.Range(0, clips.rockLandClips.Length)];
        }
        else { return null; }
    }

    [System.Serializable]
    private class ClipsClass
    {
        public AudioClip[] dirtJumpClips;
        public AudioClip[] dirtLandClips;

        public AudioClip[] grassJumpClips;
        public AudioClip[] grassLandClips;

        public AudioClip[] gravelJumpClips;
        public AudioClip[] gravelLandClips;

        public AudioClip[] mudJumpClips;
        public AudioClip[] mudLandClips;

        public AudioClip[] leavesJumpClips;
        public AudioClip[] leavesLandClips;

        public AudioClip[] metalJumpClips;
        public AudioClip[] metalLandClips;

        public AudioClip[] woodJumpClips;
        public AudioClip[] woodLandClips;

        public AudioClip[] rockJumpClips;
        public AudioClip[] rockLandClips;
    }

    [System.Serializable]
    private class MasksClass
    {
        public LayerMask groundMask;
        public LayerMask objMetalMask;
        public LayerMask objWoodMask;
        public LayerMask objRockMask;
    }
}
