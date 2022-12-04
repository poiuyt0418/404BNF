using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControl : MonoBehaviour
{
    public int x, y = 0;
    Vector3 velocity = Vector3.zero;
    public Vector3 mouseToWorld;
    public float speed = 5;
    //public Rigidbody rb;
    public NavMeshAgent movementControl;
    public PlayerInput controls;
    [SerializeField]
    Part[] parts = new Part[3];
    public string[] partIndexes = { "body", "arm", "leg" };
    BarControl[] partBars = new BarControl[3];
    public BarControl body, arm, leg;
    public int movementQueued = 0;
    // Start is called before the first frame update
    void Start()
    {
        movementControl = GetComponent<NavMeshAgent>();
        movementControl.updatePosition = false;
        movementControl.speed = speed;
        partBars[0] = body;
        partBars[1] = arm;
        partBars[2] = leg;
        //if(rb==null)
        //    rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        controls = new PlayerInput();
        controls.Player.move.performed += ctx => ClickToMove();
        controls.System.exit.performed += ctx => Exit();
        controls.System.Enable();
        MoveEnable();
        GetComponent<StartDialogue>().enabled = true;
    }

    void Exit()
    {
        DataManager.WriteFile();
        Application.Quit();
    }

    public void MoveEnable()
    {
        controls.Player.Enable();
    }

    public void MoveDisable()
    {
        controls.Player.Disable();
    }

    public bool MoveEnabled()
    {
        return controls.Player.enabled;
    }

    public void AddPart(Part part)
    {
        parts[Array.IndexOf(partIndexes, part.name)] = part;
        partBars[Array.IndexOf(partIndexes, part.name)].SetValue(); //Changes ui bars, add later
        movementControl.speed += part.speed;
    }

    public float PartDurability(string type)
    {
        if (parts[Array.IndexOf(partIndexes, type)] != null)
        {
            return parts[Array.IndexOf(partIndexes, type)].dur;
        } else
        {
            return 0;
        }
    }

    public bool CheckPart(string type)
    {
        return parts[Array.IndexOf(partIndexes, type)] != null && parts[Array.IndexOf(partIndexes, type)].name != "";
    }

    public void DestroyPart(int index)
    {
        movementControl.speed -= parts[index].speed;
        parts[index] = null;
    }

    public Part[] GetPartByUsage(string usage)
    {
        return Array.FindAll(parts, element => element != null && element.usage == usage);
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if (Time.timeScale != 0 && CheckCameraBounds() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.collider.tag != "Player")
            {
                Move(hit.point);
            }
        }
    }

    bool CheckCameraBounds()
    {
        Vector3 bounds = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        return bounds.x >= 0 && bounds.x <= 1 && bounds.y >= 0 && bounds.y <= 1;
    }

    public void Move(Vector3 pos)
    {
        mouseToWorld = pos;
        if(movementControl != null)
        {
            movementControl.SetDestination(pos);
            StartCoroutine(WaitForPath());
        }
    }

    IEnumerator WaitForPath()
    {
        yield return movementControl.pathPending;
        movementQueued++;
        int currMovement = movementQueued;
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            if (movementQueued > currMovement || movementControl.remainingDistance <= 0.01f)
            {
                movementQueued--;
                yield break;
            }
            else if(movementQueued < currMovement)
            {
                currMovement = movementQueued;
            }
            foreach (Part element in GetPartByUsage("step"))
            {
                if(element.name != "")
                {
                    element.dur -= .1f * movementControl.velocity.magnitude;
                    UpdateBar(element);
                }
            }
        }
    }

    public void UpdateBar(Part part)
    {
        if(Array.IndexOf(partIndexes, part.name) < 0)
        {
            return;
        }
        partBars[Array.IndexOf(partIndexes, part.name)].SetValue();
        if (part.dur < 0)
        {
            DestroyPart(Array.IndexOf(partIndexes, part.name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, movementControl.nextPosition, ref velocity, .1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(mouseToWorld, new Vector3(1, 1, 1));
    }
}
