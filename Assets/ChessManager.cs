using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessManager
{
    public static ChessBoard board;
    public static GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
    // Start is called before the first frame update
    public static void AddBoard(ChessBoard bd)
    {
        board = bd;
        board.EnterBoard();
    }

    public static void RemoveBoard()
    {
        board.Reset();
        board = null;
        Release();
    }

    public static bool GameEnabled()
    {
        return board != null;
    }

    public static void Release()
    {
        if(board == null)
        {
            foreach(ChessPiece piece in GameObject.FindObjectsOfType<ChessPiece>())
            {
                GameObject.Destroy(piece.gameObject);
            }
        }
        else
        {
            board.Drop();
        }
    }

    public static void GameEnd()
    {
        board = null;
    }
}
