using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowControl : MonoBehaviour
{
    [SerializeField]
    PlayerControl player;
    [SerializeField]
    float distanceToStop;
    [SerializeField]
    Sprite on, off;
    [SerializeField]
    List<Transform> guides;
    Stack<Transform> stack = new Stack<Transform>();
    Transform currentTransform;
    bool stop = true;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform guide in guides)
        {
            stack.Push(guide);
        }
        if (player == null)
            player = UnityEngine.Object.FindObjectsOfType<PlayerControl>()[0];
    }

    Vector3 CalcDirection(Transform other)
    {
        return player.transform.position - new Vector3(other.position.x, player.transform.position.y, other.position.z);
    }

    void SwapGraphics()
    {
        if(stop)
        {
            GetComponent<Image>().sprite = off;
        }
        else
        {
            GetComponent<Image>().sprite = on;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTransform == null)
        {
            if(stack.Count > 0)
            {
                currentTransform = stack.Pop();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        Vector3 direction = Vector3.zero;
        if (currentTransform != null)
        {
            direction = CalcDirection(currentTransform);
        }
        if (direction.magnitude >= distanceToStop)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(direction.x, transform.position.y, direction.z), 1f, 0.0f));
            transform.position = new Vector3(currentTransform.position.x, transform.position.y, currentTransform.position.z);
            GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Clamp(GetComponent<RectTransform>().localPosition.x, -150, 150), Mathf.Clamp(GetComponent<RectTransform>().localPosition.y, -200, 200), 0);
            if (stop)
            {
                stop = false;
                SwapGraphics();
            }
        }
        else
        {
            if(!stop)
            {
                stop = true;
                SwapGraphics();
            }
        }
    }
}
