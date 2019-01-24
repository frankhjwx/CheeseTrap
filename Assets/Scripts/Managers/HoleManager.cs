using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    const int texWidth = 960;
    const int texHeight = 540;
    public List<Hole> holes;
    public int[] areas;

    public GameObject holeColliderPrefab;
    // playerNum should be fetched from GameManager later
    private int playerNum = 2;
    // holeTexture has the size 960x540
    // holeTexture is used to calculate holes and masks
    private Texture2D holeTexture;  
    private Texture2D terrainTexture;

    public int gameLevel;

    // only for debug
    // private GameObject testTextureDisplay;

    // SpriteRenderer sr;

    // Start is called before the first frame update
    public void InitializeLevel(int level)
    {
        holes = new List<Hole>();
        areas = new int[playerNum + 1];
        LoadLevelTerrainTexture(level);
        InitializeHoleTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // judge current status of the player on the terrain
    // -1 -> die
    // 0 -> normal
    // 1 -> ice / cream e.t.c
    public int getTerrainStatus(Vector2 position){
        position.x *= 50;
        position.y *= 50;
        if ((holeTexture.GetPixel((int)position.x, (int)position.y).r != 0 && holeTexture.GetPixel((int)position.x, (int)position.y).r != 1) ||
            holeTexture.GetPixel((int)position.x, (int)position.y).g == 1)
            return -1;
        return (int)Mathf.Round(holeTexture.GetPixel((int)position.x, (int)position.y).g * 255);
    }

    public Texture2D GetHoleTexture(){
        return holeTexture;
    }

    private void InitializeHoleTexture(){
        holeTexture = new Texture2D(texWidth, texHeight);
        Color[] colors = terrainTexture.GetPixels();
        // for (int i=0; i<texWidth*texHeight; i++){
        //     colors[i].r = 0;
        // }
        holeTexture.SetPixels(0, 0, texWidth, texHeight, colors);
        holeTexture.filterMode = FilterMode.Point;
        holeTexture.Apply();
        DisplayHoleTexture();

        // testTextureDisplay = new GameObject();
        // testTextureDisplay.name = "TestTexture";

        // sr = testTextureDisplay.AddComponent<SpriteRenderer>();
        // Sprite pic = Sprite.Create(holeTexture, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f,0.5f));
        // sr.sprite = pic;
        
    }

    void InitializeTerrainTexture(){
        terrainTexture= new Texture2D(texWidth, texHeight);
        Color[] colors = new Color[texWidth*texHeight];
        for (int i=0; i<texWidth*texHeight; i++){
            colors[i] = Color.black;
        }
        terrainTexture.SetPixels(0, 0, texWidth, texHeight, colors);
        terrainTexture.Apply();
    }

    void LoadLevelTerrainTexture(int level){
        // tbd Resources.Load() blahblah
        // read the preset map
        terrainTexture = Resources.Load("Terrains/level" + level.ToString() + "_terrain") as Texture2D;
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

    public void UpdateHole(int holeID, Vector2 position, float radius, int playerID){
        holes[holeID-1].position = position;
        holes[holeID-1].radius = radius;
        UpdateHoleTexture(position, radius, playerID);
        
        return;
    }

    private void UpdateHoleTexture(Vector2 position, float radius, int playerID){
        position.x *= 50;
        position.y *= 50;
        radius *= 50;
        int Left, Right, Top, Bottom;
        Left = (int)(position.x - radius);
        Right = (int)(position.x + radius);
        Top = (int)(position.y + radius);
        Bottom = (int)(position.y - radius);

        if (Left < 0)
            Left = 0;
        if (Right > texWidth)
            Right = texWidth - 1;
        if (Bottom < 0)
            Bottom = 0;
        if (Top > texHeight)
            Top = texHeight - 1;

        int colorIndex = 0;
        for (int x = Left; x <= Right; x++){
            for (int y = Bottom; y <= Top; y++){
                if ((x - position.x)*(x - position.x) + (y - position.y)*(y - position.y) <= radius*radius && holeTexture.GetPixel(x, y).r == 0
                   && holeTexture.GetPixel(x, y).r != 1){
                    holeTexture.SetPixel(x, y, new Color32((byte)playerID, 0, 0, 255));
                    areas[playerID]++;
                }

                colorIndex++;
            }
        }
        holeTexture.Apply();
        DisplayHoleTexture();
    }

    public void DisplayHoleTexture(){
        GameObject map = GameObject.Find("ForeGround");
        map.GetComponent<SpriteRenderer>().material.SetTexture("_Mask", holeTexture);
        GameObject shadow = GameObject.Find("HoleShadow");
        shadow.GetComponent<SpriteRenderer>().material.SetTexture("_Mask", holeTexture);
    }
}
