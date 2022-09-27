using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_Button : MonoBehaviour
{
    public float duration;
    public Vector2 targetPos;
    // Start is called before the first frame update
    protected void Succeed()
    {
        Cursor.visible = true;
        Destroy(gameObject);
    }
}
