using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_QTEManager : MonoBehaviour
{
    private QTE_ScriptableObject qte;
    public GameObject canvas;
    public GameObject qteImage;
    Vector2[] coords = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void TriggerQTE(QTE_ScriptableObject triggered)
    {
        qte = triggered;
    }

    void runQTE()
    {
        int events = qte.numPress;
        int duration = qte.qteDuration;
        Shape shape = qte.qteScreenSpace;
        GameObject background = Instantiate(qte.backgroundImage) as GameObject;
        background.transform.SetParent(canvas.transform, false);
        background.transform.position = new Vector3(shape.Offset().x + (shape.Min().x + shape.Max().x)/2, shape.Offset().y + (shape.Min().y + shape.Max().y) / 2, 0);
        for (int i = 0; i < events; i++)
        {
            coords[i] = new Vector2(Random.Range(shape.Offset().x + shape.Min().x, shape.Offset().x + shape.Max().x),Random.Range(shape.Offset().y + shape.Min().y, shape.Offset().y + shape.Max().y));
            GameObject button = Instantiate(qteImage) as GameObject;
            button.transform.SetParent(canvas.transform, false);
            button.transform.position = new Vector3(coords[i].x, coords[i].y,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(qte != null)
        {
            //run qte function, move it to a new script?
            coords = new Vector2[qte.numPress];
            runQTE();
            Debug.Log("qte was triggered");
            qte = null;
        }
    }
}
