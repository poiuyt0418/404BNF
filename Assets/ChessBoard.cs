using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    TextMeshProUGUI buttonText;
    [SerializeField]
    Door door;
    [SerializeField]
    ChessRow[] rows;
    [SerializeField]
    ChessTilePiece[] tilePieces;
    [SerializeField]
    ChessPiece black, white;
    [HideInInspector]
    public ChessPiece attached;
    [HideInInspector]
    public int boardWidth, boardHeight, coroutines;
    [HideInInspector]
    public float tileWidth, tileHeight;
    [HideInInspector]
    public bool executing;
    [HideInInspector]
    public Vector3 boardCenter;
    int pieceCount;
    bool resetButton, solved;
    enum ChessEvent { none,run,die };
    Stack<ChessTile> chessEvents = new Stack<ChessTile>();
    Dictionary<Vector2,ChessTile> tiles = new Dictionary<Vector2, ChessTile>();

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
        boardCenter = transform.position;
        boardWidth = rows.Length;
        boardHeight = rows[0].transform.childCount;
        tileWidth = Mathf.Abs((rows[0].transform.Find("A").transform.position.x - rows[boardWidth - 1].transform.Find(((char)(65 + boardHeight - 1)).ToString()).transform.position.x) / (boardWidth - 1));
        tileHeight = Mathf.Abs((rows[0].transform.Find("A").transform.position.z - rows[boardWidth - 1].transform.Find(((char)(65 + boardHeight - 1)).ToString()).transform.position.z) / (boardHeight - 1));
    }

    public void Reset()
    {
        tiles = new Dictionary<Vector2, ChessTile>();
        chessEvents = new Stack<ChessTile>();
        attached = null;
        executing = false;
        cg.alpha = 0f;
        cg.interactable = false;
        buttonText.text = "Reset";
        coroutines = 0;
        resetButton = false;
    }

    public void Release()
    {
        if(solved)
        {
            cg.alpha = 0f;
            cg.interactable = false;
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
            }
            else if(p.color.ToLower() == "white")
            {
                piece = Instantiate(white, ConvertTileToPos(p.tileVector, white), Quaternion.identity);
            }
            ChessTile tile = new ChessTile();
            tile.piece = piece;
            tile.tileEvent = p.tileEvent;
            tile.pos = p.tileVector;
            if(tile.tileEvent == ChessEvent.die)
            {
                rows[(int)p.tileVector.y].Get((int)p.tileVector.x).GetComponent<Renderer>().material.color = new Color(1,0,0);
            }
            else if (tile.tileEvent == ChessEvent.run)
            {
                rows[(int)p.tileVector.y].Get((int)p.tileVector.x).GetComponent<Renderer>().material.color = new Color(0, 1, 0);
            }
            tiles.Add(p.tileVector, tile);
        }
        cg.alpha = 1f;
        cg.interactable = true;
    }

    Vector3 ConvertTileToPos(Vector2 tileVector, ChessPiece piece)
    {
        return new Vector3(boardCenter.x - (tileWidth * boardWidth - tileWidth) / 2 + tileVector.x * tileWidth, boardCenter.y + piece.transform.localScale.y, boardCenter.z - (tileHeight * boardHeight - tileHeight) / 2 + tileVector.y * tileHeight);
    }

    public IEnumerator DoEvents()
    {
        resetButton = true;
        executing = true;
        pieceCount = 0;
        Debug.Log("a");
        if(attached)
        {
            cg.alpha = 0f;
            cg.interactable = false;
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
            cg.alpha = 0f;
            cg.interactable = false;
            solved = true;
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
            int posX = (int)((attached.transform.position.x - boardCenter.x) / tileWidth + (tileWidth * boardWidth) / 2);
            int posZ = (int)((attached.transform.position.z - boardCenter.z) / tileHeight + (tileHeight * boardHeight) / 2);
            Vector2 tileVector = new Vector2(posX, posZ);
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
        Destroy(piece.gameObject);
        coroutines--;
    }

    public void Attach(ChessPiece p)
    {
        attached = p;
        buttonText.text = "Drop";
        cg.alpha = 1f;
        cg.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
