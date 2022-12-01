using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    TextMeshProUGUI buttonText;
    [SerializeField]
    Door door;
    ChessCamera cameraControl;
    [SerializeField]
    GameObject blackTile, whiteTile;
    [SerializeField]
    int boardWidth, boardDepth;
    public List<ChessRow> rows;
    [SerializeField]
    ChessTilePiece[] tilePieces;
    [SerializeField]
    ChessPiece black, white;
    [HideInInspector]
    public ChessPiece attached;
    [HideInInspector]
    public int coroutines;
    [HideInInspector]
    public Vector3 tileSize;
    [HideInInspector]
    public bool executing;
    [HideInInspector]
    public Vector3 boardFirstTile;
    int pieceCount;
    bool resetButton, solved;
    enum ChessEvent { none,run,die };
    Stack<ChessTile> chessEvents = new Stack<ChessTile>();
    Dictionary<Vector2,ChessTile> tiles = new Dictionary<Vector2, ChessTile>();
    PlayerInput controls;

    class ChessTile
    {
        public ChessEvent tileEvent = 0;
        public ChessPiece piece;
        public Vector2 pos;
        public void DoEvent()
        {
            if(tileEvent == ChessEvent.run)
            {
                ChessManager.Instance.board.coroutines++;
                ChessManager.Instance.board.StartCoroutine(ChessManager.Instance.board.MovePiece(piece,pos + piece.forward * piece.movement));
            }
            else if(tileEvent == ChessEvent.die)
            {
                ChessManager.Instance.board.coroutines++;
                ChessManager.Instance.board.StartCoroutine(ChessManager.Instance.board.DestroyPiece(piece));
            }
        }
    }
    [System.Serializable]
    class ChessTilePiece
    {
        public Vector2 tileVector;
        public string color;
        public ChessEvent tileEvent = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject go;
        rows = new List<ChessRow>();
        tileSize = blackTile.GetComponent<Renderer>().bounds.size;
        boardFirstTile = transform.position;
        for(int i = 0; i < boardDepth; i++)
        {
            go = new GameObject();
            ChessRow row = go.AddComponent<ChessRow>();
            row.blackTile = blackTile;
            row.whiteTile = whiteTile;
            row.col = boardWidth;
            if (i % 2 == 1)
            {
                row.blackFirst = true;
            }
            go.name = i.ToString();
            go.transform.position = new Vector3(boardFirstTile.x, boardFirstTile.y, boardFirstTile.z + tileSize.z * i);
            go.transform.SetParent(transform);
            row.SpawnTiles();
            rows.Add(go.GetComponent<ChessRow>());
        }
        go = new GameObject();
        go.name = "CameraControl";
        cameraControl = go.AddComponent<ChessCamera>();
        go.transform.parent = transform;
        cameraControl.SetCollider(boardWidth, boardDepth, tileSize);
        //boardWidth = rows.Length;
        //boardDepth = rows[0].transform.childCount;
        //tileWidth = Mathf.Abs((rows[0].transform.Find("A").transform.position.x - rows[boardWidth - 1].transform.Find(((char)(65 + boardDepth - 1)).ToString()).transform.position.x) / (boardWidth - 1));
        //tileDepth = Mathf.Abs((rows[0].transform.Find("A").transform.position.z - rows[boardWidth - 1].transform.Find(((char)(65 + boardDepth - 1)).ToString()).transform.position.z) / (boardDepth - 1));
    }

    void Awake()
    {
        controls = new PlayerInput();
        controls.Board.MouseClick.performed += ctx => Drop();
    }

    public void Exit()
    {
        cameraControl.RevertCamera();
        ChessManager.Instance.RemoveBoard();
    }

    public void Reset()
    {
        tiles = new Dictionary<Vector2, ChessTile>();
        chessEvents = new Stack<ChessTile>();
        attached = null;
        executing = false;
        ButtonOff();
        buttonText.text = "Reset";
        coroutines = 0;
        resetButton = false;
    }

    public void ResetTileColor()
    {
        foreach (ChessRow row in rows)
        {
            row.ResetTileColor();
        }
    }

    public void Release()
    {
        if (solved)
        {
            ButtonOff();
            return;
        }
        if (attached != null)
        {
            Drop();
        }
        else if(!resetButton)
        {
            if(chessEvents.Count == 0)
            {
                resetButton = true;
                buttonText.text = "Reset";
                //cameraControl.RevertCamera();
                //controls.Board.Disable();
                Release();
                return;
            }
            StartCoroutine(DoEvents());
        }
        else if(resetButton)
        {
            EnterBoard();
        }
    }

    public void EnterBoard()
    {
        if (solved)
            return;
        Reset();
        ChessManager.Instance.Release();
        foreach (ChessTilePiece p in tilePieces)
        {
            ChessPiece piece = null;
            if (p.color.ToLower() == "black")
            {
                piece = Instantiate(black, ConvertTileToPos(p.tileVector, black), Quaternion.identity);
                piece.tileVector = p.tileVector;
            }
            else if(p.color.ToLower() == "white")
            {
                piece = Instantiate(white, ConvertTileToPos(p.tileVector, white), Quaternion.identity);
                piece.tileVector = p.tileVector;
            }
            ChessTile tile = new ChessTile();
            tile.piece = piece;
            tile.tileEvent = p.tileEvent;
            tile.pos = p.tileVector;
            if(tile.tileEvent == ChessEvent.die)
            {
                rows[(int)p.tileVector.y].Get((int)p.tileVector.x).GetComponent<Renderer>().material.color = new Color(0, 1,0);
            }
            else if (tile.tileEvent == ChessEvent.run)
            {
                rows[(int)p.tileVector.y].Get((int)p.tileVector.x).GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            }
            tiles.Add(p.tileVector, tile);
        }
        cg.alpha = 1f;
        cg.interactable = true;
        canvas.SetActive(false);
        canvas.SetActive(true);
        controls.Board.Enable();
    }

    Vector3 ConvertTileToPos(Vector2 tileVector, ChessPiece piece)
    {
        return new Vector3(boardFirstTile.x + tileVector.x * tileSize.x, boardFirstTile.y + piece.transform.localScale.y, boardFirstTile.z + tileVector.y * tileSize.z);
    }

    public IEnumerator DoEvents()
    {
        resetButton = true;
        executing = true;
        pieceCount = 0;
        if(attached)
        {
            ButtonOff();
            Destroy(attached.gameObject);
            attached = null;
            pieceCount = 1;
        }
        while(chessEvents.Count > 0)
        {
            ChessTile e = chessEvents.Pop();
            e.DoEvent();
        }
        while(coroutines>0)
        {
            yield return new WaitForSeconds(.1f);
        }
        executing = false;
        CheckPieces();
    }

    public void CheckPieces()
    {
        foreach(ChessTile chessTile in tiles.Values)
        {
            if(chessTile.piece != null)
            {
                pieceCount++;
            }
        }
        if(pieceCount <= 0)
        {
            ButtonOff();
            solved = true;
            StartCoroutine(cameraControl.ExitBoard(ChessManager.Instance.player.transform));
            StartCoroutine(cameraControl.DeleteCamera());
            door.Activate();
        }
        else
        {
            buttonText.text = "Reset";
        }
    }

    public void Drop()
    {
        if(attached != null)
        {
            int posX = (int)(((attached.transform.position.x - boardFirstTile.x) + .5f) / tileSize.x);
            int posZ = (int)(((attached.transform.position.z - boardFirstTile.z) + .5f) / tileSize.z);
            Vector2 tileVector = new Vector2(posX, posZ);
            Debug.Log(tileVector);
            if (!tiles.ContainsKey(tileVector))
            {
                ChessTile tile = new ChessTile();
                tile.piece = attached;
                tiles.Add(tileVector, tile);
            }
            else if (tiles[tileVector].piece == null)
            {
                tiles[tileVector].piece = attached;
                if(tiles[tileVector].tileEvent == ChessEvent.die || tiles[tileVector].tileEvent == ChessEvent.run)
                {
                    ChessTile chessEvent = tiles[tileVector];
                    chessEvent.piece = attached;
                    chessEvent.pos = tileVector;
                    chessEvents.Push(chessEvent);
                }
            }
            else
            {
                Debug.Log("Another piece is there");
                return;
            }
            attached.dropped = true;
            attached.Release();
            attached.transform.position = ConvertTileToPos(new Vector2(posX,posZ),attached);
            attached = null;
            if(chessEvents.Count > 0)
            {
                buttonText.text = "Execute";
            }
            else
            {
                buttonText.text = "Reset";
            }
                
        }
    }

    IEnumerator MovePiece(ChessPiece piece, Vector2 pos)
    {
        Vector3 newPos = ConvertTileToPos(pos, piece);
        Vector3 oldPos = piece.transform.position;
        int frames = 0;
        int maxFrames = 120;
        while (frames < maxFrames && piece != null)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if(piece == null)
            {
                break;
            }
            frames++;
            float t = frames / (float)maxFrames;
            piece.transform.position = Vector3.Lerp(oldPos, newPos, t);
        }
        if(tiles.ContainsKey(pos))
        {
            tiles[pos].piece = piece;
            tiles[pos].DoEvent();
        }
        yield return new WaitForSeconds(.5f);
        coroutines--;
    }

    IEnumerator DestroyPiece(ChessPiece piece)
    {
        yield return new WaitForSeconds(.5f);
        if (piece == null)
        {
            yield break;
        }
        Destroy(piece.gameObject);
        coroutines--;
    }

    public void ButtonOff()
    {
        cg.alpha = 0f;
        cg.interactable = false;
    }

    public void Attach(ChessPiece p)
    {
        attached = p;
        tiles[p.tileVector].piece = null;
        buttonText.text = "Drop";
        cg.alpha = 1f;
        cg.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
