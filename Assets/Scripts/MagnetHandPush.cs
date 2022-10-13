using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetHandPush : MonoBehaviour
{
    Rigidbody rb;
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerControl>().CheckPart("arm"))
        {
            rb.mass = 2;
            door.Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
