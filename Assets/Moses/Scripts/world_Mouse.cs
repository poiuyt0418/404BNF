using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world_Mouse : MonoBehaviour
{
    public Vector3 mouseToWorld;
    Plane plane = new Plane(Vector3.up, 0);
    public player_Control player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = Object.FindObjectsOfType<player_Control>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            mouseToWorld = ray.GetPoint(distance);
            mouseToWorld = new Vector3(Mathf.Round(mouseToWorld.x),Mathf.Round(mouseToWorld.y),Mathf.Round(mouseToWorld.z));
        }
        if(Input.GetMouseButtonDown(0))
        {
            player.Move(mouseToWorld);
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(mouseToWorld, new Vector3(1, 1, 1));
    }
}
