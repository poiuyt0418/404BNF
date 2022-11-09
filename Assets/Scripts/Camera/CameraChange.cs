using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField]
    protected Transform cameraPos;
    [SerializeField]
    protected float cameraY = 12, speed = 3;
    protected bool lockOn, ended;
    protected Vector3 oldCameraPos;
    protected Quaternion oldCameraRot;
    protected CameraControl camControl;
    protected Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Awake()
    {
        camControl = Camera.main.gameObject.GetComponent<CameraControl>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            oldCameraPos = Camera.main.transform.position;
            oldCameraRot = Camera.main.transform.rotation;
            oldCameraPos.y = cameraY;
            lockOn = true;
            camControl.enabled = false;
            ended = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            lockOn = false;
            player = other.transform;
            //Camera.main.transform.position = oldCameraPos;
            //Camera.main.transform.rotation = oldCameraRot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lockOn)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraPos.rotation, Time.deltaTime * speed);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos.position, Time.deltaTime * speed);
        } else if(camControl.enabled == false && ended)
        {
            if(Camera.main.transform.position.y != cameraY || Camera.main.transform.rotation != oldCameraRot)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, oldCameraRot, Time.deltaTime * 3 * speed);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x, oldCameraPos.y, player.position.z), Time.deltaTime * 5 * speed);
                if (Mathf.Abs(Camera.main.transform.position.y - cameraY) < .01)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraY, Camera.main.transform.position.z);
                }
            } else
            {
                camControl.enabled = true;
                ended = false;
            }
        }
    }
}
