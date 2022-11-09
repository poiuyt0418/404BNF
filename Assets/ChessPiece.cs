using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    Transform player;
    Vector3 relativePosition;
    float relativeRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        player = collision.collider.transform;
        if (player != null && ChessManager.board.attached == null)
        {
            ChessManager.board.Attach(this);
            Vector3 tempOriginalScale = transform.localScale;
            transform.localScale = Vector3.one;
            relativePosition = transform.InverseTransformPoint(player.position);
            transform.localScale = tempOriginalScale;
            relativeRot = Quaternion.LookRotation(relativePosition).eulerAngles.y-180;
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Release()
    {
        if(player != null)
        {
            player = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.position - Quaternion.Euler(0, player.eulerAngles.y - relativeRot, 0) * relativePosition;
        }
    }
}
