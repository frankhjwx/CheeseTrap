using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConnectivityJudger
{
    public static bool isConnected(Vector2 pos1, Vector2 pos2, Texture2D texture){
        bool[,] visited = new bool[960,540]; 
        int[] dx = {0, 0, 1, -1};
        int[] dy = {1, -1, 0, 0};
        int l,r;
        List<Vector2> visit_list = new List<Vector2>();
        float time1 = Time.time;

        pos1.x = (int)pos1.x * 50;
        pos1.y = (int)pos1.y * 50;
        pos2.x = (int)pos2.x * 50;
        pos2.y = (int)pos2.y * 50;

        visit_list.Add(pos1);
        if (texture.GetPixel((int)pos1.x, (int)pos1.y).r != 0)
            return false;

        visited[(int)pos1.x, (int)pos1.y] = true;
        l = 0;
        r = 0;
        while (l <= r){
            Vector2 currentPoint = visit_list[l];
            for (int i=0; i<3; i++){
                if ((int)currentPoint.x + dx[i] >= 0 && (int)currentPoint.x + dx[i] < 960 &&
                    (int)currentPoint.y + dy[i] >= 0 && (int)currentPoint.y + dy[i] < 540 &&
                    !visited[(int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]] &&
                    texture.GetPixel((int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]).r == 0){
                        visited[(int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]] = true;
                        visit_list.Add(new Vector2((int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]));
                        if ((int)currentPoint.x + dx[i] == (int)pos2.x && (int)currentPoint.y + dy[i] == (int)pos2.y)
                            return true;
                        r++;
                    }
            }
            l++;
        }
        return false;
    }
}