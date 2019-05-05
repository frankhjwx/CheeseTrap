using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MapStarDisplay : MonoBehaviour
{
    public GameObject oneStar;
    public GameObject twoStar;
    public GameObject threeStar;

    public int StarCount
    {
        get => starCount;
        set
        {
            if (value <= 0) SetStar(1);
            else if (value >= 4) SetStar(3);
            else SetStar(value);
            starCount = value;
        }
    }
    private int starCount = 3;
        
    // Start is called before the first frame update
    void Start()
    {
    }

    private void SetStar(int count)
    {
        switch (count)
        {
            case 1:
                oneStar.SetActive(true);
                twoStar.SetActive(false);
                threeStar.SetActive(false);
                break;
            case 2:
                oneStar.SetActive(false);
                twoStar.SetActive(true);
                threeStar.SetActive(false);
                break;
            case 3:
                oneStar.SetActive(false);
                twoStar.SetActive(false);
                threeStar.SetActive(true);
                break;
        }
    }
}
