using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamInfoUIManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
        {
            Back();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Cover");
    }
}
