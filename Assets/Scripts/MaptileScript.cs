using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaptileScript : MonoBehaviour
{
    public int i, j;
    private GameObject player;
    void Start()
    {
        player = gameObject.transform.Find("PlayerCapsule").gameObject;
    }

    void Update()
    {
        var player_x = player.GetComponent<PlayerController>().Player_x;
        var player_y = player.GetComponent<PlayerController>().Player_y;
        if(i==player_x && j==player_y)
        {
            this.gameObject.GetComponent<Image>().color = new Color(103, 65, 61);
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
}
