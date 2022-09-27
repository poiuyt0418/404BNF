using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_QTEManager : MonoBehaviour
{
    private QTE_ScriptableObject qte;
    public GameObject canvas;
    public Camera canvasCamera;
    GameObject currQTE;
    GameObject qteImage;
    GameObject background;
    int duration;
    float timer = 0;
    Stack coords = new Stack();
    int screenWidth = 1920, screenHeight = 1080;
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
        int events = qte.amount;
        duration = qte.duration;
        Shape shape = qte.space;
        qteImage = qte.image;
        background = Instantiate(qte.background) as GameObject; 
        background.transform.SetParent(canvas.transform, false);
        //background.transform.position = canvasCamera.ScreenToViewportPoint(new Vector3(shape.Center().x * Screen.width, Screen.height - shape.Center().y * Screen.height));
        //RectTransform backgroundRect = background.GetComponent<RectTransform>();
        //Debug.Log(new Vector3((shape.Center().x-.5f) * screenWidth, (shape.Center().y-.5f) * -screenHeight));
        //background.GetComponent<RectTransform>().localPosition = canvasCamera.ScreenToViewportPoint(new Vector3(shape.Center().x * Screen.width, Screen.height - shape.Center().y * Screen.height));
        background.GetComponent<RectTransform>().localPosition = new Vector3((shape.Center().x - .5f) * 1920, (shape.Center().y - .5f) * -1080);
        for (int i = 0; i < events; i++)
        {
            //coords.Push(canvasCamera.ScreenToViewportPoint(new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x) * Screen.width, Screen.height - Random.Range(shape.StartCorner().y, shape.EndCorner().y) * Screen.height)));
            coords.Push(new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x), 1 - Random.Range(shape.StartCorner().y, shape.EndCorner().y)));
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
                Destroy(currQTE);
            }
            if (coords.Count > 0)
            {
                timer = duration + Time.time;
                Vector2 currPos = (Vector2)coords.Pop();
                Vector3 currButton = new Vector3((currPos.x - .5f) * screenWidth, (currPos.y - .5f) * -screenHeight, -1);
                currQTE = Instantiate(qteImage) as GameObject;
                //Debug.Log(currButton);
                currQTE.transform.SetParent(canvas.transform, false);
                //currQTE.transform.position = currButton;
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
