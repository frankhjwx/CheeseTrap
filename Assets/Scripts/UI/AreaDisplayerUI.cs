using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaDisplayerUI : MonoBehaviour
{
    public GameObject holeManager;
    private int targetArea1, targetArea2;

    private List<int> randomNums1, randomNums2;

    private int displayNum1, displayNum2;

    private float alpha = 0;
    public GameObject p1Area, p2Area;

    int cnt;
    // Start is called before the first frame update
    void Start()
    {
        randomNums1 = new List<int>();
        randomNums2 = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(){
        targetArea1 = holeManager.GetComponent<HoleManager>().areas[1];
        targetArea2 = holeManager.GetComponent<HoleManager>().areas[2];

        randomNums1.Clear();
        randomNums2.Clear();
        randomNums1.Add(0);
        randomNums1.Add(targetArea1);
        randomNums2.Add(0);
        randomNums2.Add(targetArea2);
        for (int i=0; i<100; i++){
            randomNums1.Add((int)(Random.value * targetArea1));
            randomNums2.Add((int)(Random.value * targetArea2));
        }
        randomNums1.Sort();
        randomNums2.Sort();
        cnt = 0;
        StartCoroutine(displayNumbers());
    }

    IEnumerator displayNumbers(){
        p1Area.GetComponent<Image>().material.SetFloat("_Alpha", 0);
        p2Area.GetComponent<Image>().material.SetFloat("_Alpha", 0);
        yield return new WaitForSeconds(3);
        alpha = 0;
        while (cnt < randomNums1.Count-2) {
            displayNum1 = randomNums1[cnt];
            displayNum2 = randomNums2[cnt];
            cnt++;
            alpha = Mathf.Min(cnt/20.0f, 1f);
            p1Area.GetComponent<Image>().material.SetInt("_Area", Mathf.Min(displayNum1/20, 9999));
            p2Area.GetComponent<Image>().material.SetInt("_Area", Mathf.Min(displayNum2/20, 9999));
            p1Area.GetComponent<Image>().material.SetFloat("_Alpha", alpha);
            p2Area.GetComponent<Image>().material.SetFloat("_Alpha", alpha);
            yield return null;
        }
        alpha = 1;
    }
}
