using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    ChessRow[] rows;
    [SerializeField]
    ChessTilePiece[] tilePieces;
    [SerializeField]
    ChessPiece black, white;
    public ChessPiece attached;
    public int boardWidth, boardHeight;
    public float tileWidth, tileHeight;
    public Vector3 boardCenter;
    enum ChessEvent { none,run,die };
    Dictionary<Vector2,ChessTile> tiles = new Dictionary<Vector2, ChessTile>();
    class ChessTile
    {
        public ChessEvent tileEvent = 0;
        public ChessPiece piece;
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
        attached = null;
        cg.alpha = 0f;
        cg.interactable = false;
    }

    public void Release()
    {
        ChessManager.Release();
    }

    public void EnterBoard()
    {
        ChessPiece piece = null;
        foreach (ChessTilePiece p in tilePieces)
        {
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
            tiles.Add(p.tileVector, tile);
        }
        cg.alpha = 1f;
        cg.interactable = true;
    }

    Vector3 ConvertTileToPos(Vector2 tileVector, ChessPiece piece)
    {
        return new Vector3(boardCenter.x - (tileWidth * boardWidth - tileWidth) / 2 + tileVector.x * tileWidth, boardCenter.y + piece.transform.localScale.y, boardCenter.z - (tileHeight * boardHeight - tileHeight) / 2 + tileVector.y * tileHeight);
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
            }
            else
            {
                Debug.Log("Another piece is there");
                return;
            }
            attached.Release();
            attached.transform.position = ConvertTileToPos(new Vector2(posX,posZ),attached);
            attached = null;
        }
    }

    public void Attach(ChessPiece p)
    {
        attached = p;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
