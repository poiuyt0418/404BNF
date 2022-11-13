using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRow : MonoBehaviour
{
    public int col;
    public GameObject a, b, c, d, e, f, g, h;
    // Start is called before the first frame update
    void Start()
    {
        a = transform.Find("A").gameObject;
        b = transform.Find("B").gameObject;
        c = transform.Find("C").gameObject;
        d = transform.Find("D").gameObject;
        e = transform.Find("E").gameObject;
        f = transform.Find("F").gameObject;
        g = transform.Find("G").gameObject;
        h = transform.Find("H").gameObject;
    }

    public GameObject Get(int value)
    {
        if(value > col-1)
        {
            return null;
        }
        switch(value)
        {
            case 0:
                return a;
            case 1:
                return b;
            case 2:
                return c;
            case 3: 
                return d;
            case 4:
                return e;
            case 5:
                return f;
            case 6:
                return g;
            case 7: 
                return h;
            default:
                return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
