using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchLogic : MonoBehaviour
{
    static MatchLogic Instance;

    public GameObject WireCanvas;
    public GameObject trigger;
    public int MaxWires = 4;
    private int connected = 0;
    public Text testText;
    public Door door;
    public PlayerControl player;
    void Start()
    {
        Instance = this;
    }

    void UpdateConnected()
    {
        testText.text = connected + "/" + MaxWires;
        if(connected == MaxWires)
        {
            WireCanvas.SetActive(false);
            trigger.SetActive(false);
            player.MoveEnable();
            door.Activate();
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
