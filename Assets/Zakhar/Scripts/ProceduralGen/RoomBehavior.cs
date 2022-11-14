using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    public GameObject[] walls; //0 - North, 1 - South, 2 - East, 3 - West
    public GameObject[] doors;
    public bool[] Status;
    public Vector2 RoomSize;

    public void UpdateRoom(bool[] status)
    {
        //status = Status;
        for (int i = 0; i < status.Length; i++)
        {
            //doors[i].SetActive(status[i]);
            //walls[i].SetActive(!status[i]);
        }
    }
}
