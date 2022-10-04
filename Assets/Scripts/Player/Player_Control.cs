using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player_Control : MonoBehaviour
{
    public int x, y = 0;
    Vector3 velocity = Vector3.zero;
    Vector3 mouseToWorld;
    public float speed = 5;
    //public Rigidbody rb;
    public NavMeshAgent movementControl;
    PlayerInput controls;
    Part[] parts = new Part[3];
    string[] partIndexes = { "body", "arm", "leg" };
    Bar_Control[] partBars = new Bar_Control[3];
    public Bar_Control body, arm, leg;
    // Start is called before the first frame update
    void Start()
    {
        movementControl = GetComponent<NavMeshAgent>();
        movementControl.updatePosition = false;
        movementControl.speed = speed;
        partBars[0] = body;
        partBars[1] = arm;
        partBars[2] = leg;
        MoveEnable();
        //if(rb==null)
        //    rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        controls = new PlayerInput();
        controls.Player.move.performed += ctx => ClickToMove();
        MoveEnable();
    }

    public void MoveEnable()
    {
        controls.Player.Enable();
    }

    public void MoveDisable()
    {
        controls.Player.Disable();
    }

    public void AddPart(Part part)
    {
        parts[Array.IndexOf(partIndexes, part.name)] = part;
        //partBars[Array.IndexOf(partIndexes, part.name)].SetValue(); //Changes ui bars, add later
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
        return parts[Array.IndexOf(partIndexes, type)] != null;
    }


    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.collider.tag != "Player")
                Move(hit.point);
        }
    }

    public void Move(Vector3 pos)
    {
        mouseToWorld = pos;
        movementControl.SetDestination(pos);
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
