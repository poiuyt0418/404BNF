using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_QTEManager : MonoBehaviour
{
    private QTE_ScriptableObject qte;
    public GameObject canvas;
    GameObject currQTE;
    GameObject qteImage;
    GameObject background;
    int duration;
    float timer = 0;
    Stack coords = new Stack();
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
        duration = qte.qteDuration;
        Shape shape = qte.qteScreenSpace;
        qteImage = qte.qteImage;
        background = Instantiate(qte.backgroundImage) as GameObject;
        background.transform.SetParent(canvas.transform, false);
        background.transform.position = new Vector2(shape.Center().x, Screen.height - shape.Center().y);
        for (int i = 0; i < events; i++)
        {
            coords.Push(new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x),Random.Range(Screen.height - shape.StartCorner().y, Screen.height - shape.EndCorner().y)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(qte != null)
        {
            //run qte function, move it to a new script?
            runQTE();
            Debug.Log("qte was triggered");
            qte = null;
        }
        if(duration > 0 && (Time.time > timer || currQTE == null))
        {
            if (currQTE != null)
            {
                Debug.Log("a");
                //durability -x% for each missed qte
                Destroy(currQTE);
            }
            if (coords.Count > 0)
            {
                timer = duration + Time.time;
                Vector2 currButton = (Vector2)coords.Pop();
                currQTE = Instantiate(qteImage) as GameObject;
                currQTE.transform.SetParent(canvas.transform, false);
                currQTE.transform.position = currButton;
            }
            else
            {
                Destroy(background);
                duration = 0;
            }
        }
    }
}
