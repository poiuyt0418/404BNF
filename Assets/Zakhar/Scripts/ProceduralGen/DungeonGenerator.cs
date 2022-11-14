using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; //1-north, 2 - south, 3 - east, 4 - west
    }

    [System.Serializable]
    public class Rules
    {
        public GameObject room;
        
        public Vector2Int minPos;
        public Vector2Int maxPos;
        
        // public bool spawnRoom;
        public bool necessary;

        public int SpawnProbability(int x, int y) // 0 - can`t spawn, 1 - spawns, 2 - has to spawn, 3 - spawn room,
        {
            if (x >= minPos.x && x <= maxPos.x && y >= minPos.y && y <= maxPos.y)
            {
                // if (spawnRoom)
                // {
                //     return 3;
                // }
                
                if (necessary)
                {
                    return 2;
                }

                else 
                {
                    return 1;
                }
            }

            return 0;
        }
    }

    public Vector2 dungeonSize;
    public int startPosition = 0;
    public Rules[] rooms;
    //public GameObject[] rooms;
    public Vector2 offset;
    //bool spawn = false;
    List<Cell> board;
    public RoomBehavior roomB {get; }

    void Start()
    {
        MazeGen();
    }

    void GenerateDungeon()
    {       
        for (int i= 0; i < dungeonSize.x; i++) // x size
        {
            for (int j=0; j< dungeonSize.y; j++) //y size
            {                         
                Cell currentCell = board[Mathf.FloorToInt(i+j * dungeonSize.x)];  
                if(currentCell.visited)
                {         
                    int ranRoom = -1; 
                    List<int> aRooms = new List<int>(); //available rooms

                    for (int n= 0; n < rooms.Length; n++)
                    {
                        int prob = rooms[n].SpawnProbability(i,j);
                        // if ((prob == 3) && (spawn == false))
                        // {
                        //     ranRoom = n;
                        //     spawn = true;
                        // }
                        if (prob == 2) // Makes necessary rooms spawn immidiatelly.
                        {
                            ranRoom = n;
                            break;
                        }
                        else if (prob == 1)
                        {
                            aRooms.Add(n); // Adds unnecessary rooms to the list of later spawn.
                        }
                    }

                    if (ranRoom == -1) // refactor this part
                    {
                        if (aRooms.Count > 0)
                        {
                            ranRoom = aRooms[Random.Range(0, aRooms.Count)];
                        }
                        else 
                        {
                            ranRoom = 0;
                        }
                    }
                    //int ranRoom = Random.Range(0, rooms.Length);
                    
                    var newRoom = Instantiate(rooms[ranRoom].room, new Vector3(i*offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehavior>(); // roomB.RoomSize
                    newRoom.UpdateRoom(board [Mathf.FloorToInt(i*dungeonSize.y+j*dungeonSize.x)].status);

                    newRoom.name += " " + i + "-" +  j;
                }
            }
        }
    }
    void MazeGen()
    {
        board = new List<Cell>();
        for (int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                board.Add(new Cell()); // adds new cells
            }
        }

        int currentCell = startPosition;

        Stack<int> path = new Stack<int>();

        int k=0;

        while (k<70)
        {
            k++;

            board[currentCell].visited = true;
            if(currentCell == board.Count - 1)
            {
                break;
            }

            List<int> neighbors = CheckNeighbor(currentCell);

            if(neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if(newCell > currentCell)
                {
                    
                    if (newCell - 1 == currentCell)//South or East.
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;

                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;   
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell) // North or West
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;

                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;   
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbor(int cell)
    {
        List<int> neighbors = new List<int>(); //checks all neighbors. 
        if(cell - dungeonSize.x >= 0 && !board[Mathf.FloorToInt (cell-dungeonSize.x)].visited )//North
        {
            neighbors.Add(Mathf.FloorToInt (cell-dungeonSize.x));
        }

        if (cell + dungeonSize.x <= board.Count && !board[Mathf.FloorToInt(cell+dungeonSize.x)].visited)//South
        {
            neighbors.Add(Mathf.FloorToInt (cell+dungeonSize.x));
        }

        if ((cell+1)% dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)//East PREVIOUSLY WAS  (cell + dungeonSize.y)
        {
            neighbors.Add(Mathf.FloorToInt (cell + 1)); //PREVIOUSLY WAS  (cell + dungeonSize.y)
        }

        if (cell % dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)//West PREVIOUSLY WAS  (cell - dungeonSize.y)
        {
            neighbors.Add(Mathf.FloorToInt (cell - 1)); // PREVIOUSLY WAS  (cell - dungeonSize.y)
        }
        return neighbors;

        // if (cell - dungeonSize.y >= 0 && !board[Mathf.FloorToInt(cell - dungeonSize.y)].visited)//East PREVIOUSLY WAS  (cell + dungeonSize.y)
        // {
        //     neighbors.Add(Mathf.FloorToInt (cell - dungeonSize.y)); //PREVIOUSLY WAS  (cell + dungeonSize.y)
        // }

        // if (cell + dungeonSize.y <= 0 && !board[Mathf.FloorToInt(cell + dungeonSize.y)].visited)//West PREVIOUSLY WAS  (cell - dungeonSize.y)
        // {
        //     neighbors.Add(Mathf.FloorToInt (cell + dungeonSize.y)); // PREVIOUSLY WAS  (cell - dungeonSize.y)
        // }
        // return neighbors;
    }
}
