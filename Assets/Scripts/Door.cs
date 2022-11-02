using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    [SerializeField]
    OffMeshLink inLink, outLink;
    [SerializeField]
    Transform cameraPosition, outCameraPosition;
    PlayerControl player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = Object.FindObjectsOfType<PlayerControl>()[0];
    }

    public void Activate()
    {
        door.SetActive(false);
        inLink.activated = true;
        outLink.activated = true;
    }

    public void Deactivate()
    {
        door.SetActive(true);
        inLink.activated = false;
        outLink.activated = false;
    }

    bool Occupied(OffMeshLink link, Transform cameraPos)
    {
        if (link.occupied)
        {
            Vector3 destination = link.endTransform.position;
            destination.y = player.transform.position.y;
            player.movementControl.isStopped = true;
            player.movementControl.Warp(destination);
            //player.transform.position = destination;
            player.transform.rotation = link.endTransform.rotation;
            player.movementControl.isStopped = false;
            //Camera.main.transform.position = cameraPos.position;
            Camera.main.transform.rotation = cameraPos.rotation;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Occupied(inLink,cameraPosition))
        {
            Occupied(outLink,outCameraPosition);
        }
    }
}