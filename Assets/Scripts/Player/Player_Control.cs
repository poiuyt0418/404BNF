using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Control : MonoBehaviour
{
    public int x, y = 0;
    Vector3 velocity = Vector3.zero;
    public float speed = 5;
    //public Rigidbody rb;
    NavMeshAgent movementControl;

    // Start is called before the first frame update
    void Start()
    {
        movementControl = GetComponent<NavMeshAgent>();
        movementControl.updatePosition = false;
        movementControl.speed = speed;
        //if(rb==null)
        //    rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 pos)
    {
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
}
