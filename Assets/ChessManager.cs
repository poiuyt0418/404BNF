using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager
{
    private static ChessManager instance;
    public ChessBoard board;
    public GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
    
    public static ChessManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChessManager();
            }
            return instance;
        }
    }

    public void AddBoard(ChessBoard bd)
    {
        board = bd;
        board.EnterBoard();
    }

    public void RemoveBoard()
    {
        board.Reset();
        board.ResetTileColor();
        board = null;
        Release();
    }

    public bool GameEnabled()
    {
        return board != null;
    }

    public void Release()
    {
        if(board != null && board.attached == null)
        {
            foreach(ChessPiece piece in GetPieces())
            {
                GameObject.Destroy(piece.gameObject);
            }
        }
    }

    public ChessPiece[] GetPieces()
    {
        return GameObject.FindObjectsOfType<ChessPiece>();
    }

    public void GameEnd()
    {
        board = null;
    }
}
