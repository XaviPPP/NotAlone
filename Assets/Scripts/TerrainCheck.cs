using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TerrainCheck : MonoBehaviour
{
    public Transform playerTransform;
    public Terrain t;
    public int posX;
    public int posZ;
    public float[] textureValues;
    public LayerMask groundMask;
    public LayerMask metalMask;
    public LayerMask woodMask;
    public LayerMask rockMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;


    public AudioSource source;
    public AudioClip[] dirtClips;
    public AudioClip[] grassClips;
    public AudioClip[] gravelClips;
    public AudioClip[] mudClips;
    public AudioClip[] snowClips;
    public AudioClip[] leavesClips;
    public AudioClip[] metalClips;
    public AudioClip[] woodClips;
    public AudioClip[] rockClips;

    public AudioClip[] dirtRunClips;
    public AudioClip[] grassRunClips;
    public AudioClip[] graveRunlClips;
    public AudioClip[] mudRunClips;
    public AudioClip[] snowRunClips;
    public AudioClip[] leavesRunClips;
    public AudioClip[] metalRunClips;
    public AudioClip[] woodRunClips;
    public AudioClip[] rockRunClips;
    AudioClip previousClip;

    void Start()
    {
        t = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
    }
    void Update()
    {
        //GetTerrainTexture();
    }
    public void GetTerrainTexture()
    {
        ConvertPosition(playerTransform.position);
        CheckTexture();
    }
    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;
        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    public void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];
        textureValues[4] = aMap[0, 0, 4];
        textureValues[5] = aMap[0, 0, 5];
    }

    public void PlayFootstep()
    {
        GetTerrainTexture();
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            if (textureValues[0] > 0)
            {
                source.PlayOneShot(GetClip(dirtClips), textureValues[0]);
            }
            if (textureValues[1] > 0)
            {
                source.PlayOneShot(GetClip(grassClips), textureValues[1]);
            }
            if (textureValues[2] > 0)
            {
                source.PlayOneShot(GetClip(gravelClips), textureValues[2]);
            }
            if (textureValues[3] > 0)
            {
                source.PlayOneShot(GetClip(mudClips), textureValues[3]);
            }
            if (textureValues[4] > 0)
            {
                source.PlayOneShot(GetClip(snowClips), textureValues[4]);
            }
            if (textureValues[5] > 0)
            {
                source.PlayOneShot(GetClip(leavesClips), textureValues[5]);
            }
        }
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, metalMask))
        {
            source.PlayOneShot(GetClip(metalClips));
        } 
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, woodMask))
        {
            source.PlayOneShot(GetClip(woodClips));
        } 
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, rockMask))
        {
            source.PlayOneShot(GetClip(rockClips));
        }
    }
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }
}