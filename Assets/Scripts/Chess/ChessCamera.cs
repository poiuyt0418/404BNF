using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChessCamera : CameraChange
{
    public bool moving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCollider(int width, int depth, Vector3 tileSize)
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        transform.localPosition = new Vector3((width - 1.5f) / 2f * tileSize.x, 0, (depth - 1.5f) / 2f * tileSize.z);
        transform.localScale = new Vector3(tileSize.x * width + tileSize.x, 1, tileSize.z * depth + tileSize.z);
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(0, Mathf.Max(width * tileSize.x, depth * tileSize.z) + 4, 0);
        go.transform.Rotate(90.0f, 0.0f, 0.0f);
        cameraPos = go.transform;
    }

    public override void Awake()
    {
        base.Awake();
        speed = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null && !ended)
        {
            oldCameraPos = Camera.main.transform.position;
            oldCameraRot = Camera.main.transform.rotation;
            oldCameraPos.y = cameraY;
            lockOn = true;
            moving = true;
            camControl.enabled = false;
            ended = true;
            ChessManager.Instance.AddBoard(transform.parent.GetComponent<ChessBoard>());
            frame = 0;
            player = other.transform;
            other.GetComponent<PlayerControl>().MoveDisable();
            other.GetComponent<NavMeshAgent>().ResetPath();
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.GetComponent<PlayerControl>() != null)
        //{
        //    if(lockOn)
        //        StartCoroutine(ChessManager.Instance.board.DoEvents());
        //    StartCoroutine(ExitBoard(other.transform));
        //    frame = 0;
        //    player.GetComponent<PlayerControl>().MoveEnable();
        //}
    }

    public void RevertCamera()
    {
        StartCoroutine(ExitBoard(player.transform));
        lockOn = false;
        frame = 0;
        player.GetComponent<PlayerControl>().MoveEnable();
    }

    public IEnumerator ExitBoard(Transform other)
    {
        if(ChessManager.Instance.board != null)
        {
            while (ChessManager.Instance.board.executing)
            {
                yield return new WaitForSeconds(.1f);
            }
            ChessManager.Instance.board.ResetTileColor();
            ChessManager.Instance.board.ButtonOff();
        }
        lockOn = false;
        player = other;
    }

    public IEnumerator DeleteCamera()
    {
        while(ended || lockOn)
        {
            yield return new WaitForSeconds(.1f);
        }
        player.GetComponent<PlayerControl>().MoveEnable();
        Destroy(gameObject);
    }

    public bool Entered()
    {
        return lockOn;
    }

    void FixedUpdate()
    {
        if (lockOn)
        {
            if (oldRot.w + oldRot.x + oldRot.y + oldRot.z == 0)
            {
                oldRot = Camera.main.transform.rotation;
                oldPos = Camera.main.transform.position;
            }
            if (frame * speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(oldRot, cameraPos.rotation, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(oldPos, cameraPos.position, frame / 180f * speed);
                frame++;
                if(frame * speed > 180)
                {
                    oldRot.Set(0, 0, 0, 0);
                    moving = false;
                }
            }
        }
        else if (camControl.enabled == false && ended)
        {
            if (oldRot.w + oldRot.x + oldRot.y + oldRot.z == 0)
            {
                oldRot = Camera.main.transform.rotation;
                oldPos = Camera.main.transform.position;
            }
            if (frame * speed <= 180)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(oldRot, oldCameraRot, frame / 180f * speed);
                Camera.main.transform.position = Vector3.Lerp(oldPos, new Vector3(player.position.x, oldCameraPos.y, player.position.z), frame / 180f * speed);
                frame++;
            }
            else
            {
                oldRot.Set(0, 0, 0, 0);
                camControl.enabled = true;
                ended = false;
            }
        }
    }
}
