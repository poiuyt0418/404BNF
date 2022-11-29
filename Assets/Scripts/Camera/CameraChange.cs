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
    protected float frame;
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
            player = other.transform;
            other.GetComponent<PlayerControl>().MoveDisable();
            oldCameraPos = Camera.main.transform.position;
            oldCameraRot = Camera.main.transform.rotation;
            oldCameraPos.y = cameraY;
            lockOn = true;
            camControl.enabled = false;
            ended = true;
            frame = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            lockOn = false;
            player = other.transform;
            frame = 0;
            //Camera.main.transform.position = oldCameraPos;
            //Camera.main.transform.rotation = oldCameraRot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lockOn)
        {
            if(frame+speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraPos.rotation, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos.position, frame / 180f * speed);
                frame++;
            }
            else
            {
                player.GetComponent<PlayerControl>().MoveEnable();
            }
        } else if(camControl.enabled == false && ended)
        {
            if(frame+speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, oldCameraRot, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x, oldCameraPos.y, player.position.z), frame / 180f * speed);
                frame++;
            } else
            {
                camControl.enabled = true;
                ended = false;
            }
        }
    }
}
