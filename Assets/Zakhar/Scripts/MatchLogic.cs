using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchLogic : MonoBehaviour
{
    static MatchLogic Instance;

    public GameObject WireCanvas;

    public int MaxWires = 4;
    public GameObject TaskCompleteUI;
    private int connected = 0;
    public Text testText;

    void Start()
    {
        Instance = this;
    }

    void UpdateConnected()
    {
        testText.text = connected + "/" + MaxWires;
        if(connected == MaxWires)
        {
            //Door door.Activate();
            WireCanvas.SetActive(false);
        }
    }

    public static void AddPoint()
    {
        AddPoints(1);
    }

    public static void AddPoints(int points)
    {
        Instance.connected += points;
        Instance.UpdateConnected();
    }
}
