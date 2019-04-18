using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Collider2D playerCollider;

    public int a=20;
    public float density = 0.01f;
    public float limit = 90;
    private float die = 0;
    private float normal = 0;
    private float ice = 0;
    float time = 0;
    public HoleManager holeManager;
    Dictionary<string, float> Scaninfo = new Dictionary<string, float>();//vector2为本地坐标，玩家位置为原点\
    public void Awake()
    {
        playerCollider = gameObject.GetComponent<Collider2D>();
        Scaninfo.Add("up", 0);
        Scaninfo.Add("down", 0);
        Scaninfo.Add("right", 0);
        Scaninfo.Add("left", 0);
        Scaninfo.Add("upright", 0);
        Scaninfo.Add("upleft", 0);
        Scaninfo.Add("downright", 0);
        Scaninfo.Add("downleft", 0);
        Scaninfo.Add("no", 200);
    }
    public void Update()
    {
        ScanAround();
        time += Time.deltaTime;
        if (time >1)
        {
            time = 0;
        }
        if(time>0.9)
        {
            RandomDirct();
            Debug.Log(RandomDirct());
        }
    }

    public void ScanAround()
    {
        Vector2 playerposition = transform.position;
        for(int i=3; i<a; i++)
        {
            float add = Mathf.Sqrt(a - Mathf.Abs(i));
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x - i * density, playerposition.y));
            if (terrainfo ==-1)
            {
                Scaninfo["left"] += add;
            }
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density, playerposition.y));
            if (terrainfo == -1)
            {
                Scaninfo["right"] += add;
            }       
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x , playerposition.y - i * density));
            if (terrainfo == -1)
            {
                Scaninfo["down"] += add;
            }       
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x, playerposition.y + i * density));
            if (terrainfo == -1)
            {
                Scaninfo["up"] += add;
            }      
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x+i*density/1.41f, playerposition.x + i * density / 1.41f));
            if (terrainfo == -1)
            {
                Scaninfo["upright"] += add;
            }
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x - i * density / 1.41f, playerposition.x + i * density / 1.41f));
            if (terrainfo == -1)
            {
                Scaninfo["upleft"] += add;
            }
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density / 1.41f, playerposition.x - i * density / 1.41f));
            if (terrainfo == -1)
            {
                Scaninfo["downright"] += add;
            }
            terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x - i * density / 1.41f, playerposition.x - i * density / 1.41f));
            if (terrainfo == -1)
            {
                Scaninfo["downleft"] += add;
            }            
        }
    }
    
    public float GetMin(Dictionary<string,float> Scan)
    {

        Dictionary<string, float> Scaninfo = Scan;
        //Debug.Log("up" + Scaninfo["up"] + "    down" + Scaninfo["down"] + "     left" + Scaninfo["left"] + "    right" + Scaninfo["right"]);
        KeyValuePair<string, float> mkvp = new KeyValuePair<string, float>("anydirect", 100);
        foreach (KeyValuePair<string, float> kvp in Scaninfo)
        {
            if (kvp.Value < mkvp.Value)
            {
                mkvp = kvp;
            }
        }
        return mkvp.Value;
    }
    public string RandomDirct()
    {
        string random = "no";
        while (Scaninfo[random] > GetMin(Scaninfo))
        {
            int n = Random.Range(1, 9);
            switch (n)
            {
                case 1:
                    random = "left";
                    break;
                case 2:
                    random = "right";
                    break;
                case 3:
                    random = "down";
                    break;
                case 4:
                    random = "up";
                    break;
                case 5:
                    random = "upright";
                    break;
                case 6:
                    random = "upleft";
                    break;
                case 7:
                    random = "downright";
                    break;
                case 8:
                    random = "downleft";
                    break;
            }
        }
        return random;
    }
}
