using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEButton : MonoBehaviour
{
    public float duration;
    public Vector2 targetPos;
    public float lifetime, endTime;
    protected Image tarImage;
    //protected GameObject indicator;
    // Start is called before the first frame update
    public void Succeed()
    {
        Cursor.visible = true;
        //Destroy(indicator);
        Destroy(gameObject);
    }

    void Update()
    {
        if(endTime > Time.time)
        {
            Color color = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = (endTime - Time.time) / lifetime;
            if(tarImage != null)
                tarImage.color = color;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
    }
}
