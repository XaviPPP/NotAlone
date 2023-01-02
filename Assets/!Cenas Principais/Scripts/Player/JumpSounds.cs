using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JumpSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] dirtJumpClips;
    [SerializeField] private AudioClip[] dirtLandClips;

    [SerializeField] private AudioClip[] grassJumpClips;
    [SerializeField] private AudioClip[] grassLandClips;

    [SerializeField] private AudioClip[] gravelJumpClips;
    [SerializeField] private AudioClip[] gravelLandClips;

    [SerializeField] private AudioClip[] mudJumpClips;
    [SerializeField] private AudioClip[] mudLandClips;

    [SerializeField] private AudioClip[] leavesJumpClips;
    [SerializeField] private AudioClip[] leavesLandClips;

    [SerializeField] private AudioClip[] metalJumpClips;
    [SerializeField] private AudioClip[] metalLandClips;

    [SerializeField] private AudioClip[] woodJumpClips;
    [SerializeField] private AudioClip[] woodLandClips;

    [SerializeField] private AudioClip[] rockJumpClips;
    [SerializeField] private AudioClip[] rockLandClips;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask objMetalMask;
    [SerializeField] private LayerMask objWoodMask;
    [SerializeField] private LayerMask objRockMask;
    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;

    private TerrainDetector terrainDetector;

    private PlayerMovement playerMovement;

    bool playJumpSound;
    bool playLandSound;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        terrainDetector = new TerrainDetector();
        playJumpSound = true;
        playLandSound = true;
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
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return dirtJumpClips[UnityEngine.Random.Range(0, dirtJumpClips.Length)];
                case 1:
                    return grassJumpClips[UnityEngine.Random.Range(0, grassJumpClips.Length)];
                case 2:
                    return gravelJumpClips[UnityEngine.Random.Range(0, gravelJumpClips.Length)];
                case 3:
                    return mudJumpClips[UnityEngine.Random.Range(0, mudJumpClips.Length)];
                case 4:
                    return leavesJumpClips[UnityEngine.Random.Range(0, leavesJumpClips.Length)];
                case 5:
                    return grassJumpClips[UnityEngine.Random.Range(0, grassJumpClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objMetalMask))
        {
            return metalJumpClips[UnityEngine.Random.Range(0, metalJumpClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objWoodMask))
        {
            return woodJumpClips[UnityEngine.Random.Range(0, woodJumpClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objRockMask))
        {
            return rockJumpClips[UnityEngine.Random.Range(0, rockJumpClips.Length)];
        }
        else { return null; }
    }

    private AudioClip GetRandomLandClip()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return dirtLandClips[UnityEngine.Random.Range(0, dirtLandClips.Length)];
                case 1:
                    return grassLandClips[UnityEngine.Random.Range(0, grassLandClips.Length)];
                case 2:
                    return gravelLandClips[UnityEngine.Random.Range(0, gravelLandClips.Length)];
                case 3:
                    return mudLandClips[UnityEngine.Random.Range(0, mudLandClips.Length)];
                case 4:
                    return leavesLandClips[UnityEngine.Random.Range(0, leavesLandClips.Length)];
                case 5:
                    return grassLandClips[UnityEngine.Random.Range(0, grassLandClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objMetalMask))
        {
            return metalLandClips[UnityEngine.Random.Range(0, metalLandClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objWoodMask))
        {
            return woodLandClips[UnityEngine.Random.Range(0, woodLandClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objRockMask))
        {
            return rockLandClips[UnityEngine.Random.Range(0, rockLandClips.Length)];
        }
        else { return null; }
    }
}
