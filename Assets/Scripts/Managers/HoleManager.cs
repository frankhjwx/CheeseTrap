using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    const int texWidth = 960;
    const int texHeight = 540;
    public List<Hole> holes;
    public int[] areas;
    // playerNum should be fetched from GameManager later
    private int playerNum = 2000;
    // holeTexture has the size 960x540
    // holeTexture is used to calculate holes and masks
    private Texture2D holeTexture;  

    // only for debug
    // private GameObject testTextureDisplay;

    // SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        holes = new List<Hole>();
        areas = new int[playerNum + 1];
        HoleTextureInitialize();


        UpdateHoleTexture(new Vector2(960, 540), 50, 1);
        UpdateHoleTexture(new Vector2(1280, 540), 60, 2);
        Debug.Log(areas[1]);
        Debug.Log(areas[2]);
        // UpdateHoleTexture(new Vector2(960, 480), 50, 255);
        // Debug.Log(holeTexture.GetPixel(25, 25));
        // Sprite pic = Sprite.Create(holeTexture, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f, 0.5f));
        // sr.sprite = pic;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHoleTexture();
    }

    private void HoleTextureInitialize(){
        holeTexture = new Texture2D(texWidth, texHeight);
        Color[] colors = new Color[texWidth*texHeight];
        for (int i=0; i<texWidth*texHeight; i++){
            colors[i] = Color.black;
        }
        holeTexture.SetPixels(0, 0, texWidth, texHeight, colors);
        holeTexture.Apply();

        // testTextureDisplay = new GameObject();
        // testTextureDisplay.name = "TestTexture";

        // sr = testTextureDisplay.AddComponent<SpriteRenderer>();
        // Sprite pic = Sprite.Create(holeTexture, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f,0.5f));
        // sr.sprite = pic;
        
    }

    public int CreateHole(Vector2 position, float radius, int playerID){
        Hole hole = new Hole();
        hole.position = position;
        hole.radius = radius;
        holes.Add(hole);
        UpdateHoleTexture(position, radius, playerID);
        
        // return value is the hole ID
        return holes.Count;
    }

    public void UpdateHole(int id, Vector2 position, float radius, int playerID){
        holes[id].position = position;
        holes[id].radius = radius;
        UpdateHoleTexture(position, radius, playerID);
        return;
    }

    private void UpdateHoleTexture(Vector2 position, float radius, int playerID){
        position.x /= 2;
        position.y /= 2;
        radius /= 2;
        int Left, Right, Top, Bottom;
        Left = (int)(position.x - radius);
        Right = (int)(position.x + radius);
        Top = (int)(position.y + radius);
        Bottom = (int)(position.y - radius);

        if (Left < 0)
            Left = 0;
        if (Right > texWidth)
            Right = texWidth;
        if (Bottom < 0)
            Bottom = 0;
        if (Top > texHeight)
            Top = texHeight;

        int colorIndex = 0;
        for (int x = Left; x <= Right; x++){
            for (int y = Bottom; y <= Top; y++){
                if ((x - position.x)*(x - position.x) + (y - position.y)*(y - position.y) <= radius*radius && holeTexture.GetPixel(x, y).r == 0){
                    holeTexture.SetPixel(x, y, new Color32((byte)playerID, 0, 0, 255));
                    areas[playerID]++;
                }

                colorIndex++;
            }
        }
        holeTexture.Apply();
    }

    public void DisplayHoleTexture(){
        GameObject map = GameObject.Find("Map");
        map.GetComponent<SpriteRenderer>().material.SetTexture("_Mask", holeTexture);
    }
}
