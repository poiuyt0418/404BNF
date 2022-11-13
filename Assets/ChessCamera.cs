using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessCamera : CameraChange
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Awake()
    {
        base.Awake();
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
            ChessManager.Instance.AddBoard(transform.parent.GetComponent<ChessBoard>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            StartCoroutine(ChessManager.Instance.board.DoEvents());
            StartCoroutine(ExitBoard(other.transform));
        }
    }

    IEnumerator ExitBoard(Transform other)
    {
        while(ChessManager.Instance.board.executing)
        {
            yield return new WaitForSeconds(.1f);
        }
        lockOn = false;
        player = other;
    }

    public bool Entered()
    {
        return lockOn;
    }
}
