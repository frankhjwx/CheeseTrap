using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetScreenShot : MonoBehaviour
{
    RenderTexture rt;
    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        rt = src;
        Graphics.Blit(src, dest);
    }
    
    
    public void GenerateObj(){
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.ARGB32, true); 
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        GameObject screenShotObject = new GameObject();
        DontDestroyOnLoad(screenShotObject);
        screenShotObject.transform.localPosition = new Vector3(-5000, 0, 0);
        screenShotObject.name = "SCREENSHOT";
        screenShotObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
