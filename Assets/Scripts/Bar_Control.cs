using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar_Control : MonoBehaviour
{
    Player_Control player;
    [SerializeField]
    Slider slider;
    [SerializeField]
    string partType;
    void Start()
    {
        if (player == null)
            player = UnityEngine.Object.FindObjectsOfType<Player_Control>()[0];
    }

    public void SetValue()
    {
        slider.value = player.PartDurability(partType);
    }
}
