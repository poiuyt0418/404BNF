using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    float distanceFromCenter, cameraSpeed, cameraY = 12;
    [SerializeField]
    Transform player;
    public bool changing;
    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 SetDestination(Vector3 destination)
    {
        Vector3 playerPos = Camera.main.WorldToViewportPoint(player.transform.position);
        destination.x = Mathf.Clamp(destination.x, playerPos.x - distanceFromCenter, playerPos.x + distanceFromCenter);
        destination.y = Mathf.Clamp(destination.y, playerPos.y - distanceFromCenter, playerPos.y + distanceFromCenter);
        destination = Camera.main.ViewportToWorldPoint(destination);
        destination.y = cameraY;
        return Vector3.Slerp(transform.position, new Vector3(destination.x, destination.y, destination.z), Time.deltaTime * player.gameObject.GetComponent<PlayerControl>().movementControl.speed * cameraSpeed / 2);
    }

    //Vector3 CheckScreenBounds()
    //{
    //    Vector3 playerPos = Camera.main.WorldToViewportPoint(player.transform.position);
    //    Vector3 destinationPos = Camera.main.WorldToViewportPoint(destination);
    //    if ((destinationPos.x > .5 && playerPos.x <= .5 - distanceFromCenter) || (destinationPos.x < .5 && playerPos.x >= .5 + distanceFromCenter))
    //    {
    //        destination.x = transform.position.x;
    //    }
    //    if ((destinationPos.y > .5 && playerPos.y <= .5 - distanceFromCenter) || (destinationPos.y < .5 && playerPos.y >= .5 + distanceFromCenter))
    //    {
    //        destination.z = transform.position.z;
    //    }
    //    return Vector3.Slerp(transform.position, new Vector3(destination.x, transform.position.y, destination.z), Time.deltaTime * 10 * player.GetComponent<PlayerControl>().speed);
    //}

    bool CheckCameraBounds()
    {
        Vector3 bounds = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        return bounds.x >= 0 && bounds.x <= 1 && bounds.y >= 0 && bounds.y <= 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCameraBounds() && player.GetComponent<PlayerControl>().MoveEnabled())
        {
            transform.position = SetDestination(Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
        }
        else
        {
            transform.position = SetDestination(Vector3.one / 2);
        }
    }
}