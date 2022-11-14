using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRow : MonoBehaviour
{
    public bool blackFirst;
    public int col;
    public GameObject blackTile, whiteTile;
    public GameObject[] tiles;
    public Color[] tileColors;
    // Start is called before the first frame update
    public void SpawnTiles()
    {
        Vector3 tileSize = blackTile.GetComponent<Renderer>().bounds.size;
        tiles = new GameObject[col];
        tileColors = new Color[col];
        for (int i = 0; i < col; i++)
        {
            GameObject tile;
            if (blackFirst)
            {
                tile = blackTile;
            }
            else
            {
                tile = whiteTile;
            }
            tiles[i] = Instantiate(tile, transform.position + Vector3.right * tileSize.x * i, Quaternion.identity, transform);
            tileColors[i] = tile.GetComponent<Renderer>().sharedMaterial.color;
            blackFirst = !blackFirst;
        }
        //a = transform.Find("A").gameObject;
        //b = transform.Find("B").gameObject;
        //c = transform.Find("C").gameObject;
        //d = transform.Find("D").gameObject;
        //e = transform.Find("E").gameObject;
        //f = transform.Find("F").gameObject;
        //g = transform.Find("G").gameObject;
        //h = transform.Find("H").gameObject;
    }

    public void ResetTileColor()
    {
        for(int i = 0; i < col;i++)
        {
            tiles[i].GetComponent<Renderer>().material.color = tileColors[i];
        }
    }

    public GameObject Get(int value)
    {
        if(value > col-1 || col < 0)
        {
            return null;
        }
        //switch(value)
        //{
        //    case 0:
        //        return a;
        //    case 1:
        //        return b;
        //    case 2:
        //        return c;
        //    case 3: 
        //        return d;
        //    case 4:
        //        return e;
        //    case 5:
        //        return f;
        //    case 6:
        //        return g;
        //    case 7: 
        //        return h;
        //    default:
        //        return null;
        //}
        return tiles[value];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
