using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration2 : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];   //1-north, 2 - south, 3 - east, 4 - west
    }

    [System.Serializable]
    public class Rules 
    {
        public GameObject room;
        public bool center;
        public bool Hand;
        public bool Body;
        public bool Legs;
        public int SpawnProbability()
        {
            if (center)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

    public Rules[] rooms;
    public int startPosition = 0;
    List<Cell> board;

    [SerializeField]
    private Vector3 Center;
    [SerializeField]
    private Vector3 North;
    [SerializeField]
    private Vector3 South;
    [SerializeField]
    private Vector3 East;
    [SerializeField]
    private Vector3 West;

    //private Transform randomRotation;

    private Vector3 randomPosition; 

    private bool northCheck = false;
    private bool southCheck = false; // TO BE REFACTORED
    private bool westCheck = false;
    private bool eastCheck = false;
    private bool centerCheck = false;

    private Quaternion rotationCheck;

    [SerializeField]
    private int roomsNumber = 5;
    

    void Start()
    {
        GenerateDungeon();
        
    }


    void GenerateDungeon()
    {
        for (int i = 0; i < roomsNumber; i++)
        {

            int ranRoom = -1;
            List<int> aRooms = new List<int>();
            for (int j=0; j < rooms.Length; j++)
            {
                int prob = rooms[j].SpawnProbability();
                //switch(prob) case 4: case 3: ...
                if (prob == 2)
                {
                    if (!centerCheck)
                    {
                        ranRoom = j;
                        randomPosition = Center;
                        centerCheck = true;
                    }
                }

                else if (prob == 1)
                { 
                    aRooms.Add(j);
                }
            }

            if (ranRoom == -1 )
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
            var newRoom = Instantiate(rooms[ranRoom].room, randomPosition, rotationCheck);
            
            newRoom.name += " " + i + "-" + i;// for editor purposes
            
            MazeGen(i);
        }

    }
    
    void MazeGen(int Number)
    {
            switch(Number)
            {
                case 0:
                    if(!northCheck)
                    { 
                        randomPosition = North;
                        rotationCheck = new Quaternion(0, -1f, 0, 1f);
                        northCheck = true;
                    }
                    break;
                    
                case 1:
                    if (!southCheck)
                    {
                        randomPosition = South;
                        rotationCheck = new Quaternion(0, 1.0f, 0, 1f);
                        southCheck = true;
                    }
                    break;

                case 2:
                    if(!eastCheck)
                    {
                        randomPosition = East;
                        rotationCheck = new Quaternion(0, 100000f, 0, 1f);
                        eastCheck = true;
                    }
                    break;

                case 3:
                    if (!westCheck)
                    {   
                        randomPosition = West;
                        rotationCheck = new Quaternion(0, 0f, 0, 1f);
                        westCheck = true;
                    }
                break;
            default:
                    break;
            }
    }
}
