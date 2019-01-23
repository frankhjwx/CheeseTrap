using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OccupyAreaCalculator
{
    public static int getEatenArea(int playerID, Texture2D texture){
        int area = 0;
        for (int i=0; i<960; i++)
            for (int j=0; j<540; j++)
                if (Math.Abs(texture.GetPixel(i, j).r * 255 - playerID) < 0.1f) {
                    area += 1;
                }
        return area;
    }

    public static int getConnectedArea(Vector2 position, Texture2D texture){
        bool[,] visited = new bool[960,540];
        int[] dx = {0, 0, 1, -1};
        int[] dy = {1, -1, 0, 0};
        int l,r;
        List<Vector2> visit_list = new List<Vector2>();

        position.x *= 50;
        position.y *= 50;

        visit_list.Add(position);
        if (texture.GetPixel((int)position.x, (int)position.y).r != 0)
            return 0;
        
        visited[(int)position.x, (int)position.y] = true;
        l = 0;
        r = 0;

        while (l <= r){
            Vector2 currentPoint = visit_list[l];
            for (int i=0; i<3; i++){
                if ((int)currentPoint.x + dx[i] >= 0 && (int)currentPoint.x + dx[i] < 960 &&
                    (int)currentPoint.y + dy[i] >= 0 && (int)currentPoint.y + dy[i] < 540 &&
                    !visited[(int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]] &&
                    //map[(int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]]){
                    texture.GetPixel((int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]).r == 0){
                        visited[(int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]] = true;
                        visit_list.Add(new Vector2((int)currentPoint.x + dx[i], (int)currentPoint.y + dy[i]));
                        r++;
                    }
            }
            l++;
        }
        return l;
    }

}