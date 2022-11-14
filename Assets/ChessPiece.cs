using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChessPiece : MonoBehaviour
{
    Transform player;
    Vector3 relativePosition;
    float relativeRot;
    public bool dropped = false;
    public Vector2 movement;
    public int forward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        player = collision.collider.transform;
        if (player != null && player.GetComponent<PlayerControl>().CheckPart("arm") && ChessManager.Instance.board.attached == null && !dropped)
        {
            foreach(Part part in player.GetComponent<PlayerControl>().GetPartByUsage("usage"))
            {
                part.dur -= 100 / 5;
                player.GetComponent<PlayerControl>().UpdateBar(part);
            }
            ChessManager.Instance.board.Attach(this);
            Vector3 tempOriginalScale = transform.localScale;
            transform.localScale = Vector3.one;
            relativePosition = transform.InverseTransformPoint(player.position);
            //relativePosition += Vector3.forward * .1f;
            transform.localScale = tempOriginalScale;
            relativeRot = Quaternion.LookRotation(relativePosition).eulerAngles.y-180;
            GetComponent<NavMeshObstacle>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Release()
    {
        if(player != null)
        {
            player = null;
            GetComponent<NavMeshObstacle>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && ChessManager.Instance.board.attached == this)
        {
            transform.position = player.position - Quaternion.Euler(0, player.eulerAngles.y - relativeRot, 0) * relativePosition;
            //transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
