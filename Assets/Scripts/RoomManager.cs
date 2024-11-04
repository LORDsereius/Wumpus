using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{

    [SerializeField]
    private GameObject hover, Wumpus, Soldier;
    
    [SerializeField]
    private Animator animator;
    public int i, j, stat;

    public void Play_GoUp()
    {
        animator.Play("GoingUp", -1, 0f);
    }
    public void Play_GoLeft()
    {
        animator.Play("GoingLeft", -1, 0f);
    }
    public void Play_GoDown()
    {
        animator.Play("GoingDown", -1, 0f);
    }
    public void Play_GoRight()
    {
        animator.Play("GoingRight", -1, 0f);
    }

    public void deactivateThreat()
    {
        Wumpus.SetActive(false);
        Soldier.SetActive(false);
    }
    public void activateWumpus()
    {
        Wumpus.SetActive(true);
        Soldier.SetActive(false);
    }

    public void activateSoldier()
    {
        Soldier.SetActive(true);
        Wumpus.SetActive(false);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Create_Level")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject==this.gameObject)
                    hover.gameObject.SetActive(true);

                else
                    hover.gameObject.SetActive(false);
            }
            else
                hover.gameObject.SetActive(false);

        }
    }
}
