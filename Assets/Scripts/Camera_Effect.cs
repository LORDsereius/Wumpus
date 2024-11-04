using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Camera_Effect : MonoBehaviour
{   
    public GameObject Player, PlayerLocation;
    public float x_Offset, y_Offset, z_Offset, speed;
    private bool TweengRunning=false;
    void Start()
    {
        setCameraPosition();
    }

    public void setCameraPosition()
    {
        TweengRunning = true;   
        var target = new Vector3(PlayerLocation.transform.position.x + x_Offset, PlayerLocation.transform.position.y + y_Offset, z_Offset);
        StartCoroutine(2f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
        Invoke("TweengEnded", 1.5f);
    }
    private void TweengEnded()
    {
        TweengRunning = false;
    }
    void Update()
    {
        if(!TweengRunning)
        {
            Vector3 targetDir = Player.transform.position - this.transform.position;
            float step = speed * Time.deltaTime;
    
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
         
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
