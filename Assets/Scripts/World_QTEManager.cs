using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class World_QTEManager : MonoBehaviour
{
    private QTE_ScriptableObject qte;
    public GameObject canvas;
    public Camera canvasCamera;
    GameObject currQTE;
    string[] indexes = { "press", "drag", "hold" };
    public GameObject[] qteEvents;
    GameObject background;
    string type;
    float duration, holdDuration;
    Vector2 target;
    float timer = 0;
    Stack qteStack = new Stack();
    int screenWidth = 1920, screenHeight = 1080;
    // Start is called before the first frame update
    class QTEInstance
    {
        string name;
        Vector2 position;
        public string type { get { return name; } set { name = value; } }
        public Vector2 coord { get { return position; } set { position = value; } }
    }
    void Start()
    {

    }

    public void TriggerQTE(QTE_ScriptableObject triggered)
    {
        qte = triggered;
    }

    void runQTE()
    {
        int events = qte.amount;
        duration = qte.duration;
        type = qte.type;
        target = qte.direction;
        holdDuration = qte.holdFor;
        Shape shape = qte.space;
        //qteImage = qte.image;
        background = Instantiate(qte.background) as GameObject; 
        background.transform.SetParent(canvas.transform, false);
        //background.transform.position = canvasCamera.ScreenToViewportPoint(new Vector3(shape.Center().x * Screen.width, Screen.height - shape.Center().y * Screen.height));
        //RectTransform backgroundRect = background.GetComponent<RectTransform>();
        //Debug.Log(new Vector3((shape.Center().x-.5f) * screenWidth, (shape.Center().y-.5f) * -screenHeight));
        //background.GetComponent<RectTransform>().localPosition = canvasCamera.ScreenToViewportPoint(new Vector3(shape.Center().x * Screen.width, Screen.height - shape.Center().y * Screen.height));
        background.GetComponent<RectTransform>().localPosition = new Vector3((shape.Center().x - .5f) * screenWidth, (shape.Center().y - .5f) * -screenHeight);
        for (int i = 0; i < events; i++)
        {
            QTEInstance instance = new QTEInstance();
            if(type == "random")
            {
                instance.type = indexes[Random.Range(0,3)];
            } else
            {
                instance.type = type;
            }
            instance.coord = new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x), 1 - Random.Range(shape.StartCorner().y, shape.EndCorner().y));
            //qteStack.Push(canvasCamera.ScreenToViewportPoint(new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x) * Screen.width, Screen.height - Random.Range(shape.StartCorner().y, shape.EndCorner().y) * Screen.height)));
            qteStack.Push(instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (qte != null)
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
                Cursor.visible = true;
                Destroy(currQTE);
            }
            if (qteStack.Count > 0)
            {
                timer = duration + Time.time;
                QTEInstance instance = (QTEInstance)qteStack.Pop();
                Vector2 currPos = instance.coord;
                Vector3 currButton = new Vector3((currPos.x - .5f) * screenWidth, (currPos.y - .5f) * screenHeight, -1);
                currQTE = Instantiate(qteEvents[Array.IndexOf(indexes, instance.type)]) as GameObject;
                currQTE.GetComponent<QTE_Button>().duration = holdDuration;
                currQTE.GetComponent<QTE_Button>().targetPos = (type=="random")?new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)):target;
                currQTE.transform.SetParent(canvas.transform, false);
                currQTE.GetComponent<RectTransform>().localPosition = currButton;
            }
            else
            {
                Destroy(background);
                duration = 0;
            }
        }
    }
}
