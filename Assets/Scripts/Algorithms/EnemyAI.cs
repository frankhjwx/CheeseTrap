using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Collider2D playerCollider;

    public int w=20,h=20;
    public float density = 0.01f;
    public float limit = 50;
    private float die = 0;
    private float normal = 0;
    private float ice = 0;
    public HoleManager holeManager;

    public void Awake()
    {
        playerCollider = gameObject.GetComponent<Collider2D>();
    }
    public void Update()
    {
        ScanAround();
    }

    public void ScanAround()
    {
        //Dictionary<Vector2, int> scaninfo = new Dictionary<Vector2, int>();//vector2为本地坐标，玩家位置为原点
        float up = 0; float down = 0; float right = 0; float left = 0;
        Vector2 playerposition = transform.position;
        for(int i=-w; i<-3; i++)
        {
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density, playerposition.y));
            if (terrainfo ==-1)
            {
                float add= w-Mathf.Abs(i);
                left += add;
            }
                //scaninfo.Add(new Vector2(i, 0), terrainfo);
        }
        for (int i =w; i >3; i--)
        {
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x + i * density, playerposition.y));
            if (terrainfo == -1)
            {
                float add = w-Mathf.Abs(i);
                right += add;
                //right += Mathf.Sqrt(add);
            }
        }
        for (int i = -h; i < -3; i++)
        {
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x , playerposition.y + i * density));
            if (terrainfo == -1)
            {
                float add = h-Mathf.Abs(i);
                down += add;
            }
        }
        for (int i = h; i >3; i--)
        {
            int terrainfo = holeManager.getTerrainStatus(new Vector2(playerposition.x, playerposition.y + i * density));
            if (terrainfo == -1)
            {
                float add =h-Mathf.Abs(i);
                up += add;
            }
        }
        //Debug.Log("up"+up+"    down"+down+"     left"+left+"    right"+right);
        bool goup = false;bool goright = false;
        if(up<down&&up<limit)
        {
            goup = true;
        }
        if(right<left&&right<limit)
        {
            goright = true;
        }
        if(goup)
        {
            transform.position += new Vector3(0, 0.1f,0);
        }
        if(!goup)
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }
        if (goright)
        {
            transform.position += new Vector3(0.1f, 0, 0);
        }
        if (!goright)
        {
            transform.position += new Vector3(-0.1f, 0, 0);
        }
    }

}
