using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Clicked()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        print("asd");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
