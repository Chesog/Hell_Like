using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {Up,Down,Right,Left}
public class Dungegon_Generator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [Header("SetUp")]
    [SerializeField] private Vector2 mazeSize;
    [SerializeField] private Vector2 offSet;
    [SerializeField] private int starPos = 0;
    [SerializeField] private GameObject room;

    List<Cell> board;


    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DungegonGenerator() 
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                var newRoom = Instantiate(room,new Vector3(i * offSet.x, 0f, - j * offSet.y),Quaternion.identity,transform).GetComponent<Room_Behaviour>();
                newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * mazeSize.x)].status);

                newRoom.name += " " + i + " - " + j;
            }
        }
    }

    private void MazeGenerator() 
    {
        board = new List<Cell>();
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = starPos;
        Stack<int> path = new Stack<int>();

        int k = 0;

        int mazeMaxSize = 1000;

        while (k < mazeMaxSize)
        {
            k++;

            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            // Check Cells Neighbors
            List<int> neightbors = CheckAdjacentCell(currentCell);

            if (neightbors.Count == 0)
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

                int newCell = neightbors[Random.Range(0, neightbors.Count)];

                if (newCell > currentCell)
                {
                    //Down or Right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[(int)Directions.Right] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Left] =  true;
                    }
                    else
                    {
                        board[currentCell].status[(int)Directions.Down] = true;
                        currentCell = newCell;   
                        board[currentCell].status[(int)Directions.Up] =  true;
                    }
                }
                else
                {
                    //Up or Left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[(int)Directions.Left] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Right] = true;
                    }
                    else
                    {
                        board[currentCell].status[(int)Directions.Up] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Down] = true;
                    }
                }
            }
        }

        DungegonGenerator();
    }


    /// <summary>
    /// Check The Adjacent Cells To the Current Cell (Up ,Down , Right , Left)
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private List<int> CheckAdjacentCell(int cell)
    {
        List<int> neightbors = new List<int>();

        // Check Up Neightbor
        if (cell - mazeSize.x >= 0 && !board[Mathf.FloorToInt(cell - mazeSize.x)].visited)
        {
            neightbors.Add(Mathf.FloorToInt(cell - mazeSize.x));
        }

        // Check Down Neightbor
        if (cell + mazeSize.x >= board.Count && !board[Mathf.FloorToInt(cell + mazeSize.x)].visited)
        {
            neightbors.Add(Mathf.FloorToInt(cell + mazeSize.x));
        }

        // Check Right Neightbor
        if ((cell + 1) % mazeSize.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neightbors.Add(Mathf.FloorToInt(cell + 1));
        }

        // Check Left Neightbor
        if (cell % mazeSize.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neightbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neightbors;
    }
}
