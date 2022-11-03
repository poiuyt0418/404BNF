using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField]
    Transform cameraPos;
    [SerializeField]
    float cameraY = 12;
    bool lockOn, ended;
    Vector3 oldCameraPos;
    Quaternion oldCameraRot;
    CameraControl camControl;
    // Start is called before the first frame update
    void Start()
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
            //Camera.main.transform.position = oldCameraPos;
            //Camera.main.transform.rotation = oldCameraRot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lockOn)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraPos.rotation, Time.deltaTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos.position, Time.deltaTime);
        } else if(camControl.enabled == false && ended)
        {
            if(Camera.main.transform.position.y != cameraY || Camera.main.transform.rotation != oldCameraRot)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, oldCameraRot, Time.deltaTime*5);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, oldCameraPos.y, Camera.main.transform.position.z), Time.deltaTime*5);
                if (Camera.main.transform.position.y > cameraY - .01)
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
