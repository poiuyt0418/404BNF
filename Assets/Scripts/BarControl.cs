using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarControl : MonoBehaviour
{
    PlayerControl player;
    [SerializeField]
    Slider slider;
    [SerializeField]
    string partType;
    void Start()
    {
        if (player == null)
            player = UnityEngine.Object.FindObjectsOfType<PlayerControl>()[0];
    }

    public void SetValue()
    {
        slider.value = player.PartDurability(partType);
    }
}
