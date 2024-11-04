using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateLevelCameraMovement : MonoBehaviour
{
    public float dragSpeed = 1;
 
 
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.up * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.down * dragSpeed * Time.deltaTime);
        }
    }
}
