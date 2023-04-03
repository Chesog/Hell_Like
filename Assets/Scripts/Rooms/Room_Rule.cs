using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Spawn_Type {Cannot_Spawn,Can_Spawn,Has_To_Spawn}

public class Room_Rule : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private GameObject room;
    [SerializeField] private Vector2Int minPos;
    [SerializeField] private Vector2Int maxPos;
    [SerializeField] private bool obligatory;

    public int ProbabilityOfSpawning(int x , int y) 
    {
        if (x >= minPos.x && x <= maxPos.x && y >= minPos.y && y <= maxPos.y)
        {
            return obligatory ? (int)Spawn_Type.Has_To_Spawn : (int)Spawn_Type.Can_Spawn;
        }

        return (int)Spawn_Type.Cannot_Spawn;
    }

    public GameObject GetRoom() 
    {
        return room;
    }

    public Vector2Int GetMinPos() 
    {
        return minPos;
    }
    public Vector2Int GetMaxPos()
    {
        return maxPos;
    }
}
