using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] dirtWalkClips;
    [SerializeField]
    private AudioClip[] dirtRunClips;

    [SerializeField]
    private AudioClip[] grassWalkClips;
    [SerializeField]
    private AudioClip[] grassRunClips;

    [SerializeField]
    private AudioClip[] gravelWalkClips;
    [SerializeField]
    private AudioClip[] gravelRunClips;

    [SerializeField]
    private AudioClip[] mudWalkClips;
    [SerializeField]
    private AudioClip[] mudRunClips;

    [SerializeField]
    private AudioClip[] leavesWalkClips;
    [SerializeField]
    private AudioClip[] leavesRunClips;

    [SerializeField]
    private AudioClip[] metalWalkClips;
    [SerializeField]
    private AudioClip[] metalRunClips;

    [SerializeField]
    private AudioClip[] woodWalkClips;
    [SerializeField]
    private AudioClip[] woodRunClips;

    [SerializeField]
    private AudioClip[] rockWalkClips;
    [SerializeField]
    private AudioClip[] rockRunClips;
    [SerializeField]
    private AudioSource audioSource;
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

    private AudioClip GetRandomClip()
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
}
