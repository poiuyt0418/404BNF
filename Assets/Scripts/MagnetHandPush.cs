using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagnetHandPush : MonoBehaviour
{
    //Rigidbody rb;
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        
    }

    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerControl>().CheckPart("arm"))
            {
                
                transform.SetParent(collision.gameObject.transform);
                //GetComponent<NavMeshObstacle>().carving = false;
                GetComponent<NavMeshObstacle>().enabled = false;
                door.Activate();
            }
            //rb.mass = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
