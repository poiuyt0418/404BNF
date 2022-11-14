using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldQTEManager : MonoBehaviour
{
    private QTEScriptableObject qte;
    public GameObject canvas;
    public Camera canvasCamera;
    GameObject currQTE;
    string[] indexes = { "press", "drag", "hold" };
    public GameObject[] qteEvents;
    GameObject background;
    string type;
    float duration, holdDuration, durability;
    int events;
    Vector2 target;
    float timer = 0;
    Stack qteStack = new Stack();
    int screenWidth = 1920, screenHeight = 1080;
    PlayerControl player;
    string partType;
    Part part;
    int[] stars = { 1, 1, 1 };
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
        if (player == null)
            player = UnityEngine.Object.FindObjectsOfType<PlayerControl>()[0];
    }

    public void TriggerQTE(QTEScriptableObject triggered, string part)
    {
        stars[Array.IndexOf(player.partIndexes, part)] = Mathf.Max(stars[Array.IndexOf(player.partIndexes, part)]-1,-1);
        qte = triggered;
        partType = part;
    }

    public int Stars()
    {
        int total = 0;
        foreach (int i in stars)
        {
            total += i;
        }
        return total;
    }

    public IEnumerator Respawn(GameObject go)
    {
        GameObject obj = Instantiate(go);
        obj.SetActive(false);
        while(partType != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        obj.SetActive(true);
    }

    void runQTE()
    {
        qteStack = new Stack();
        events = qte.amount;
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
        //background.GetComponent<RectTransform>().localPosition = new Vector3((shape.Center().x - .5f) * screenWidth, (shape.Center().y - .5f) * -screenHeight);
        background.transform.localScale = Vector3.one * 5000;
        background.layer = 5;
        background.transform.localPosition = new Vector3((shape.Center().x - .5f) * screenWidth, (shape.Center().y - .5f) * -.5f*screenHeight,10f);
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
            durability = 100;
            //qteStack.Push(canvasCamera.ScreenToViewportPoint(new Vector2(Random.Range(shape.StartCorner().x, shape.EndCorner().x) * Screen.width, Screen.height - Random.Range(shape.StartCorner().y, shape.EndCorner().y) * Screen.height)));
            qteStack.Push(instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (qte != null)
        {
            player.MoveDisable();
            player.movementControl.ResetPath();
            //run qte function, move it to a new script?
            runQTE();
            Debug.Log("qte was triggered");
            qte = null;
        }
        if(duration > 0 && (Time.time > timer || currQTE == null))
        {
            if (currQTE != null)
            {
                durability -= 100f / (events);
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
                currQTE.GetComponent<QTEButton>().duration = holdDuration;
                currQTE.GetComponent<QTEButton>().targetPos = (type == "random") ? new Vector2(Random.Range(1f, 2f) * (Random.Range(0, 1) * 2 - 1), Random.Range(1f, 2f) * (Random.Range(0, 1) * 2 - 1)) : target;
                currQTE.transform.SetParent(canvas.transform, false);
                currQTE.GetComponent<RectTransform>().localPosition = currButton;
                currQTE.GetComponent<QTEButton>().lifetime = duration;
                currQTE.GetComponent<QTEButton>().endTime = timer;

            }
            else
            {
                player.MoveEnable();
                Destroy(background);
                part = new Part();
                part.name = partType;
                partType = null;
                part.dur = durability;
                part.SetUsage();
                player.AddPart(part);
                duration = 0;
            }
        }
    }
}
