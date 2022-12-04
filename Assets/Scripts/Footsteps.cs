using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] dirtWalkClips;
    [SerializeField] private AudioClip[] dirtRunClips;

    [SerializeField] private AudioClip[] grassWalkClips;
    [SerializeField] private AudioClip[] grassRunClips;

    [SerializeField] private AudioClip[] gravelWalkClips;
    [SerializeField] private AudioClip[] gravelRunClips;

    [SerializeField] private AudioClip[] mudWalkClips;
    [SerializeField] private AudioClip[] mudRunClips;

    [SerializeField] private AudioClip[] leavesWalkClips;
    [SerializeField] private AudioClip[] leavesRunClips;

    [SerializeField] private AudioClip[] metalWalkClips;
    [SerializeField] private AudioClip[] metalRunClips;

    [SerializeField] private AudioClip[] woodWalkClips;
    [SerializeField] private AudioClip[] woodRunClips;

    [SerializeField] private AudioClip[] rockWalkClips;
    [SerializeField] private AudioClip[] rockRunClips;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask objMetalMask;
    [SerializeField] private LayerMask objWoodMask;
    [SerializeField] private LayerMask objRockMask;
    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;
    private TerrainDetector terrainDetector;

    private void Awake()
    {
        terrainDetector = new TerrainDetector();
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private void RunStep()
    {
        AudioClip clip = GetRandomRunClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return dirtWalkClips[UnityEngine.Random.Range(0, dirtWalkClips.Length)];
                case 1:
                    return grassWalkClips[UnityEngine.Random.Range(0, grassWalkClips.Length)];
                case 2:
                    return gravelWalkClips[UnityEngine.Random.Range(0, gravelWalkClips.Length)];
                case 3:
                    return mudWalkClips[UnityEngine.Random.Range(0, mudWalkClips.Length)];
                case 4:
                    return leavesWalkClips[UnityEngine.Random.Range(0, leavesWalkClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objMetalMask))
        {
            return metalWalkClips[UnityEngine.Random.Range(0, metalWalkClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objWoodMask))
        {
            return woodWalkClips[UnityEngine.Random.Range(0, woodWalkClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objRockMask))
        {
            return rockWalkClips[UnityEngine.Random.Range(0, rockWalkClips.Length)];
        }
        else { return null; }
    }

    private AudioClip GetRandomRunClip()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

            switch (terrainTextureIndex)
            {
                case 0:
                    return dirtRunClips[UnityEngine.Random.Range(0, dirtRunClips.Length)];
                case 1:
                    return grassRunClips[UnityEngine.Random.Range(0, grassRunClips.Length)];
                case 2:
                    return gravelRunClips[UnityEngine.Random.Range(0, gravelRunClips.Length)];
                case 3:
                    return mudRunClips[UnityEngine.Random.Range(0, mudRunClips.Length)];
                case 4:
                    return leavesRunClips[UnityEngine.Random.Range(0, leavesRunClips.Length)];
                default:
                    return null;
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objMetalMask))
        {
            return metalRunClips[UnityEngine.Random.Range(0, metalRunClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objWoodMask))
        {
            return woodRunClips[UnityEngine.Random.Range(0, woodRunClips.Length)];
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, objRockMask))
        {
            return rockRunClips[UnityEngine.Random.Range(0, rockRunClips.Length)];
        }
        else { return null; }
    }
}
