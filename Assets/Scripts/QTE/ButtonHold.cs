using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHold : QTEButton
{
    bool held;
    int press;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator HoldEvent(int count)
    {
        yield return new WaitForSeconds(duration);
        if(press == count && held)
        {
            Succeed();
        }
    }

    void OnMouseDown()
    {
        Held();
    }

    void OnMouseUp()
    {
        held = false;
    }

    public void Held()
    {
        held = true;
        press++;
        StartCoroutine(HoldEvent(press));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
