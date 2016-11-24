using UnityEngine;
using System.Collections;

// Attach this script to Main Camera to show refraction water if you can not get water effect in editor mode.
public class ShowWaterInEditor : MonoBehaviour
{
#if UNITY_EDITOR
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest);
    }
#endif
}
