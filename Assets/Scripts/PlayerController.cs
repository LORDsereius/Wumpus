using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private AudioSource BackgroundMusic, SoldierAlert, VictorySound;
    public GameObject Player, Audio, E_Button, GameOverPopup, GameOverText, Rose;
    public int Player_x=1, Player_y=1, Protection=2;
    [SerializeField]
    private float enteringElevatorOffset=10;
    public GameManager gameManager;
    public static int[,] GameMap = new int[6,6]{{0,0,0,0,0,0},
                                        {0,0,0,1,0,0},
                                        {0,0,0,0,0,0},
                                        {0,2,0,1,0,0},
                                        {0,0,0,0,1,0},
                                        {0,0,0,0,0,0}};

    [SerializeField]
    private Volume volume;

    [SerializeField]
    private float playerSpeed = 10;
    private bool canGoUp, canGoDown, canGoRight, canGoLeft;
    private GameObject CurrentRoom;
    private Bloom bloom;
    private FilmGrain filmGrain;
    private WhiteBalance whiteBalance;
    private Vignette vignette;
    void Start()
    {
        Cursor.visible = true;

        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<FilmGrain>(out filmGrain);
        volume.profile.TryGet<WhiteBalance>(out whiteBalance);
        volume.profile.TryGet<Vignette>(out vignette);
    }

    private bool SoldiersAround()
    {
        if(GameMap[Player_x-1,Player_y]==2 || GameMap[Player_x+1,Player_y]==2 || GameMap[Player_x,Player_y-1]==2 || GameMap[Player_x,Player_y+1]==2)
            return true;
        return false;
    }
    private bool WumpusAround()
    {
        if(GameMap[Player_x-1,Player_y]==1 || GameMap[Player_x+1,Player_y]==1 || GameMap[Player_x,Player_y-1]==1 || GameMap[Player_x,Player_y+1]==1)
            return true;
        return false;
    }
    private bool gameOver()
    {
        if (GameMap[Player_x,Player_y]==1 || GameMap[Player_x,Player_y]==2)
            return true;
        else 
            return false;
    }
    private void OnTriggerExit(Collider other)
    {
        canGoDown = false;
        canGoLeft = false;
        canGoRight = false;
        canGoUp = false;
        E_Button.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if(!this.enabled)
            return;
        if (collision.gameObject.tag == "Room")
        {
            CurrentRoom = collision.gameObject;
            if (WumpusAround())
            {
                StartCoroutine(3f.Tweeng( (p)=>bloom.dirtIntensity.value=p, bloom.dirtIntensity.value, 100f));
                StartCoroutine(1f.Tweeng( (p)=>filmGrain.intensity.value=p, filmGrain.intensity.value, 1f));
                StartCoroutine(1f.Tweeng( (p)=>whiteBalance.temperature.value=p, whiteBalance.temperature.value, -70f));
            }
            else
            {
                StartCoroutine(1f.Tweeng( (p)=>bloom.dirtIntensity.value=p, bloom.dirtIntensity.value, 0f));
                StartCoroutine(1f.Tweeng( (p)=>filmGrain.intensity.value=p, filmGrain.intensity.value, 0f));
                StartCoroutine(1f.Tweeng( (p)=>whiteBalance.temperature.value=p, whiteBalance.temperature.value, 0f));
            }

            if (SoldiersAround())
            {
                StartCoroutine(3f.Tweeng( (p)=>SoldierAlert.volume=p, SoldierAlert.volume, 0.05f));
            }
            else
            {
                StartCoroutine(1f.Tweeng( (p)=>SoldierAlert.volume=p, SoldierAlert.volume, 0f));
            }

            if(GameMap[Player_x,Player_y]==1)
            {
                StartCoroutine(3f.Tweeng( (p)=>bloom.dirtIntensity.value=p, bloom.dirtIntensity.value, 200f));
                StartCoroutine(1f.Tweeng( (p)=>filmGrain.intensity.value=p, filmGrain.intensity.value, 1f));
                StartCoroutine(1f.Tweeng( (p)=>whiteBalance.temperature.value=p, whiteBalance.temperature.value, -100f));         
            }

            if (gameOver())
            {
                GameObject wumpus = GameObject.FindGameObjectWithTag("Wumpus");
                if(Protection==1 && GameMap[Player_x,Player_y]==1)
                {
                    Debug.Log("Win!");
                    VictorySound.Play();
                    BackgroundMusic.Stop();
                    SoldierAlert.Stop();
                    StartCoroutine(1f.Tweeng( (p)=>bloom.dirtIntensity.value=p, bloom.dirtIntensity.value, 0f));
                    StartCoroutine(1f.Tweeng( (p)=>filmGrain.intensity.value=p, filmGrain.intensity.value, 0f));
                    StartCoroutine(1f.Tweeng( (p)=>whiteBalance.temperature.value=p, whiteBalance.temperature.value, 0f));
                    wumpus.GetComponent<WumpusController>().StartWinAnimation();
                    Vector2 target = new Vector2(0.5f, 0f);
                    StartCoroutine(20f.Tweeng( (p)=>vignette.center.value=p, vignette.center.value, target));
                    GameOverText.GetComponent<TMP_Text>().text = "Win!";
                }
                else
                {
                    Debug.Log("Game Over!");
                    wumpus.GetComponent<WumpusController>().StartGameOverAnimation();
                    Vector2 target = new Vector2(0.5f, -1.5f);
                    StartCoroutine(20f.Tweeng( (p)=>vignette.center.value=p, vignette.center.value, target));
                    GameOverText.GetComponent<TMP_Text>().text = "Game Over";
                }
                StartCoroutine("ActivateGameOverPopup");
                StartCoroutine(3f.Tweeng( (p)=>BackgroundMusic.pitch=p, BackgroundMusic.pitch, 2f));
            }
            else
            {
                if(Protection==1)
                {
                    Rose.SetActive(false);
                    Protection = 0;
                }
                StartCoroutine(2f.Tweeng( (p)=>BackgroundMusic.pitch=p, BackgroundMusic.pitch, 1f));
            }
        }
        if (collision.gameObject.tag == "Right_Door" && Player_x<gameManager.n)
        {
            canGoRight = true;
            E_Button.gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "Left_Door" && Player_x>1)
        {
            canGoLeft = true;
            E_Button.gameObject.SetActive(true);     
        }
        if (collision.gameObject.tag == "Up_Door" && Player_y<gameManager.n)
        {
            canGoUp = true;
            E_Button.gameObject.SetActive(true);            
        }
        if (collision.gameObject.tag == "Down_Door" && Player_y>1)
        {
            canGoDown = true;
            E_Button.gameObject.SetActive(true);
        }

    }

    private IEnumerator ActivateGameOverPopup()
    {
        this.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(15);
        GameOverPopup.SetActive(true);
        GameOverPopup.GetComponent<Animator>().Play("GameOverFadeIn");
    }


    private void GoUp()
    {
        Player_y += 1;

        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y+1.75f, Player.transform.position.z);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-enteringElevatorOffset);
        this.GetComponent<ThirdPersonController>().enabled = true;
        this.GetComponent<CharacterController>().enabled = true;                
        Camera.main.GetComponent<Camera_Effect>().setCameraPosition();        
        E_Button.gameObject.SetActive(false);
    }
    private void GoDown()
    {
        Player_y -= 1;

        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y-1.75f, Player.transform.position.z);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-enteringElevatorOffset);
        this.GetComponent<ThirdPersonController>().enabled = true;
        this.GetComponent<CharacterController>().enabled = true;   
        Camera.main.GetComponent<Camera_Effect>().setCameraPosition();
        E_Button.gameObject.SetActive(false);              
    }
    private void GoRight()
    {
        Player_x += 1;
        Player.transform.position = new Vector3(Player.transform.position.x+2, Player.transform.position.y, Player.transform.position.z);
        this.transform.position = new Vector3(this.transform.position.x-1.6f, this.transform.position.y, this.transform.position.z);
        this.GetComponent<ThirdPersonController>().enabled = true;
        this.GetComponent<CharacterController>().enabled = true;                
        Camera.main.GetComponent<Camera_Effect>().setCameraPosition();        
        E_Button.gameObject.SetActive(false);         
    }
    private void GoLeft()
    {
        Player_x -= 1;
        Player.transform.position = new Vector3(Player.transform.position.x-2, Player.transform.position.y, Player.transform.position.z);
        this.transform.position = new Vector3(this.transform.position.x+1.6f, this.transform.position.y, this.transform.position.z);
        this.GetComponent<ThirdPersonController>().enabled = true;
        this.GetComponent<CharacterController>().enabled = true;                  
        Camera.main.GetComponent<Camera_Effect>().setCameraPosition();
        E_Button.gameObject.SetActive(false);         
    }

    private void EnterElevator()
    {
        Vector3 target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + enteringElevatorOffset);
        StartCoroutine(1.75f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
    }

    private void EnterRight_1()
    {
        Vector3 target = new Vector3(this.transform.position.x - 0.3f, this.transform.position.y, this.transform.position.z);
        StartCoroutine(0.5f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
        Invoke("EnterRight_2", 0.5f);
    }
    private void EnterRight_2()
    {
        Vector3 target = new Vector3(this.transform.position.x + 0.6f, this.transform.position.y, this.transform.position.z);
        StartCoroutine(1f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
    }

    private void EnterLeft_1()
    {
        Vector3 target = new Vector3(this.transform.position.x + 0.3f, this.transform.position.y, this.transform.position.z);
        StartCoroutine(0.5f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
        Invoke("EnterLeft_2", 0.5f);
    }
    private void EnterLeft_2()
    {
        Vector3 target = new Vector3(this.transform.position.x - 0.6f, this.transform.position.y, this.transform.position.z);
        StartCoroutine(1f.Tweeng( (p)=>this.transform.position=p, this.transform.position, target));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(canGoDown)
            {
                canGoDown = false;
                this.GetComponent<ThirdPersonController>().enabled = false;
                this.GetComponent<CharacterController>().enabled = false;
                Invoke("EnterElevator", 0.2f);
                CurrentRoom.GetComponent<RoomManager>().Play_GoDown();
                Invoke("GoDown", 2);
            }

            if(canGoLeft)
            {
                canGoLeft = false;
                this.GetComponent<ThirdPersonController>().enabled = false;
                this.GetComponent<CharacterController>().enabled = false;
                Invoke("EnterLeft_1", 0.2f);
                CurrentRoom.GetComponent<RoomManager>().Play_GoLeft();
                Invoke("GoLeft", 2);
            }

            if(canGoRight)
            {
                canGoRight = false;
                this.GetComponent<ThirdPersonController>().enabled = false;
                this.GetComponent<CharacterController>().enabled = false;
                Invoke("EnterRight_1", 0.2f);
                CurrentRoom.GetComponent<RoomManager>().Play_GoRight();
                Invoke("GoRight", 2);
            }

            if(canGoUp)
            {
                canGoUp = false;
                this.GetComponent<ThirdPersonController>().enabled = false;
                this.GetComponent<CharacterController>().enabled = false;
                Invoke("EnterElevator", 0.2f);
                CurrentRoom.GetComponent<RoomManager>().Play_GoUp();
                Invoke("GoUp", 2);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Protection -= 1;
            if(Protection==1)
            {
                Rose.SetActive(true);
            }
            Debug.Log("protection activated: "+Protection);
        }
    }
}
