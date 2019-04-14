using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Collider2D playerCollider;
    Dictionary<Vector2, int> scaninfo = new Dictionary<Vector2, int>();//vector2为本地坐标，玩家位置为原点
    public int w=48,h=27;
    public float density = 2;
    private float die = 0;
    private float normal = 0;
    private float ice = 0;
    public HoleManager holeManager;
    enum terrainchoose
    {
        die,normal,ice
    }
    private terrainchoose tc;

    public void Awake()
    {
        playerCollider = gameObject.GetComponent<Collider2D>();
    }
    public void Update()
    {
        ScanAround();
        scaninfo.Clear();
    }

    public void ScanAround()
    {
        die = 0;normal = 0;ice = 0;
        Vector2 playerposition = playerCollider.transform.position;
        for(int i=-w; i<w; i++)
        {
                int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density, playerposition.y + j * density));
                Debug.Log(terrainfo);
                switch (terrainfo)
                {
                    case -1:
                        die++;
                        break;
                    case 0:
                        normal++;
                        break;
                    case 1:
                        ice++;
                        break;
                }
                scaninfo.Add(new Vector2(i, 0), terrainfo);
        }
        for (int i = -w; i < w; i++)
        {
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density, playerposition.y + j * density));
            Debug.Log(terrainfo);
            switch (terrainfo)
            {
                case -1:
                    die++;
                    break;
                case 0:
                    normal++;
                    break;
                case 1:
                    ice++;
                    break;
            }
            scaninfo.Add(new Vector2(i, 0), terrainfo);

        }
    }

    public Vector2 Turning()
    {
        die =die* 0.1f;
        ice =ice* 0.2f;
        float rd= Random.Range(0,die+normal+ice);
        if(rd<=normal)
        {
            tc = terrainchoose.normal;
        }
        if(rd<ice+normal)
        {
            tc = terrainchoose.ice;
        }
        if (rd < die + normal + ice)
        {
            tc = terrainchoose.die;
        }
        switch(tc)
        {
            case terrainchoose.normal:

                break;
            case terrainchoose.ice:

                break;
            case terrainchoose.die:

                break;
        }
        Vector2 turnDirection= new Vector2();
        return turnDirection;
    }
}
