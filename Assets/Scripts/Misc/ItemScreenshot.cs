using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemScreenshot : MonoBehaviour
{
    private Camera camera;

    public string path;

    private void Start()
    {
        string filename = string.Format("Assets/!Cenas Principais/Screenshots/capture_{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
        if (!Directory.Exists("Assets/!Cenas Principais/Screenshots"))
        {
            Directory.CreateDirectory("Assets/!Cenas Principais/Screenshots");
        }
        TakeScreenshot(filename);
    }

    void TakeScreenshot(string fullPath)
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }

        RenderTexture rt = new RenderTexture(256, 256, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        camera.targetTexture = null;

        if (Application.isEditor)
        {
            DestroyImmediate(rt);
        } else
        {
            Destroy(rt);
        }

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
        Debug.Log(fullPath);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
