using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleChess : MonoBehaviour
{
    [SerializeField]
    ChessRow[] rows;
    ChessRow r1, r2, r3, r4, r5, r6, r7, r8;
    // Start is called before the first frame update
    void Start()
    {
        r1 = rows[0];
        r2 = rows[1];
        r3 = rows[2];
        r4 = rows[3];
        r5 = rows[4];
        r6 = rows[5];
        r7 = rows[6];
        r8 = rows[7];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
