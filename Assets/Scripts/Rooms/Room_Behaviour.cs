using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Behaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;      // 0 = Up - 1 = Down - 2 = Right - 3 = Left
    [SerializeField] private GameObject[] doors;      // 0 = Up - 1 = Down - 2 = Right - 3 = Left

    public void UpdateRoom(bool[] status) 
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
