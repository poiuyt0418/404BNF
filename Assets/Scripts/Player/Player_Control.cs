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
    NavMeshAgent movementControl;
    PlayerInput controls;
    // Start is called before the first frame update
    void Start()
    {
        movementControl = GetComponent<NavMeshAgent>();
        movementControl.updatePosition = false;
        movementControl.speed = speed;
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
