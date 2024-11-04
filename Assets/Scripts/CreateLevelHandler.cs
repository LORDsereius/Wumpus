using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateLevelHandler : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject GameManager, Wumpus, Soldier, WumpusDisable;
    private GameObject follower;
    private int followerValue=0;

    public void changeCursor_Wumpus()
    {
        Destroy(follower);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        follower = Instantiate(Wumpus, mousePosition, Quaternion.Euler(0,180,0));
        followerValue = 1;
    }
    public void changeCursor_Soldier()
    {
        if(follower!=null)
        Destroy(follower);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        follower = Instantiate(Soldier, mousePosition, Quaternion.Euler(-90,180,0));
        follower.transform.localScale = new Vector3(1000, 1000, 1000);
        followerValue = 2;
    }
    public void set_N() // Gets the number of n for n*n rooms from user input
    {
        WumpusDisable.SetActive(false);
        GameManager.GetComponent<GameManager>().n = (int)slider.value;
    }
    void Update()
    {
        if(follower!=null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            follower.transform.position = mousePosition;
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(follower!=null)
            {                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    foreach(GameObject R in GameManager.GetComponent<GameManager>().Rooms)
                    {
                        if(hit.collider.gameObject==R)
                        {
                            int index = GameManager.GetComponent<GameManager>().Rooms.IndexOf(R);
                            
                            if(R.GetComponent<RoomManager>().stat==1)
                                WumpusDisable.SetActive(false);
                            R.GetComponent<RoomManager>().stat = followerValue;
                            if(followerValue==1)
                            {
                                R.GetComponent<RoomManager>().activateWumpus();
                                WumpusDisable.SetActive(true);
                            }
                            else if(followerValue==2)
                            {
                                R.GetComponent<RoomManager>().activateSoldier();
                            }
                            Destroy(follower);
                            followerValue = 0;
                        }
                    }
                }
                else
                {
                    Destroy(follower);
                    followerValue = 0;
                }
            }
            
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    foreach(GameObject R in GameManager.GetComponent<GameManager>().Rooms)
                    {
                        if(hit.collider.gameObject==R)
                        {
                            int index = GameManager.GetComponent<GameManager>().Rooms.IndexOf(R);
                            followerValue = R.GetComponent<RoomManager>().stat;
                            if(followerValue==1)
                            {
                                R.GetComponent<RoomManager>().deactivateThreat();
                                R.GetComponent<RoomManager>().stat = 0;
                                changeCursor_Wumpus();
                                WumpusDisable.SetActive(false);
                            }
                            else if(followerValue==2)
                            {
                                R.GetComponent<RoomManager>().deactivateThreat();
                                R.GetComponent<RoomManager>().stat = 0;
                                changeCursor_Soldier();
                            }
                        }
                    }
                }                
            }
        }
    }
}
