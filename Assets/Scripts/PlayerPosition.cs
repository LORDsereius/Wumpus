using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
