using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : QTEButton
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnMouseDown()
    {
        MusicControlScript.Instance.PlayQTE();
        Succeed();
    }
}
