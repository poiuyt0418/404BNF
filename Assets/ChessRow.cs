using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRow : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
