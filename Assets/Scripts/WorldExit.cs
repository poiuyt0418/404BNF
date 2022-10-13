using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldExit : MonoBehaviour
{
    public TMP_Text objText;
    public PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {
        objText.text = "Requires 2 Hands to exit.";
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        if (player.CheckPart("arm") && player.CheckPart("body"))
        {
            objText.text = "You have escaped.";
        }
        else
        {
            if (player.CheckPart("arm"))
            {
                objText.text = "1 Hand remaining.";
            }
            if (player.CheckPart("body"))
            {
                objText.text = "1 Hand remaining.";
            }
        }
    }
}
