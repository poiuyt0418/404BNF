using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_Button : MonoBehaviour
{
    public float duration;
    public Vector2 targetPos;
    protected GameObject indicator;
    // Start is called before the first frame update
    public void Succeed()
    {
        Cursor.visible = true;
        Destroy(indicator);
        Destroy(gameObject);
    }
}
