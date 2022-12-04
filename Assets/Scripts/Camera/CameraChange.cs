using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            other.GetComponent<NavMeshAgent>().ResetPath();
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
    void FixedUpdate()
    {
        if (lockOn)
        {
            Quaternion oldRot = Camera.main.transform.rotation;
            Vector3 oldPos = Camera.main.transform.position;
            if (frame * speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(oldRot, cameraPos.rotation, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(oldPos, cameraPos.position, frame / 180f * speed);
                frame++;
            }
            else
            {
                player.GetComponent<PlayerControl>().MoveEnable();
            }
        }
        else if (camControl.enabled == false && ended)
        {
            Quaternion oldRot = Camera.main.transform.rotation;
            Vector3 oldPos = Camera.main.transform.position;
            if (frame * speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(oldRot, oldCameraRot, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(oldPos, new Vector3(player.position.x, oldCameraPos.y, player.position.z), frame / 180f * speed);
                frame++;
            }
            else
            {
                player.GetComponent<PlayerControl>().MoveEnable();
                camControl.enabled = true;
                ended = false;
            }
        }
    }
}
