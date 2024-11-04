using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    [SerializeField]
    private GameObject Room, menuPopup, levelName, Maptile, canvas, player, LevelPopup_Contents, LevelUI;
    public int[,] RoomArray;
    public static string currentLevel;
    public int n;
    public List <GameObject> Rooms = new List <GameObject>();
    private List <GameObject> Maptiles = new List<GameObject>();
    
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Menu"){
            if(SceneManager.GetActiveScene().name == "Create_Level")
                GenerateBasicMap();
            else
            {
                GenerateMap();
                GenerateMiniMap();
            }   
        }
    }
    public void GenerateBasicMap() // If no levels are loaded, generates a basic default map
    {
        foreach(GameObject R in Rooms)
        {
            Destroy(R);
        }
        Rooms = new List<GameObject>();

        for(int i=0; i<n; i+=1)
            for(int j=0; j<n; j+=1)
            {
                GameObject instance = Instantiate(Room,new Vector3(i*2, j*1.7f, 0), Quaternion.Euler(0, -90, 0));
                instance.GetComponent<RoomManager>().i = i+1;
                instance.GetComponent<RoomManager>().j = j+1;
                instance.GetComponent<RoomManager>().stat = 0;
                Rooms.Add(instance);
            }
    }
    public void GenerateMap() // Generates a map based on the chosen level
    {
        foreach(GameObject R in Rooms)
        {
            Destroy(R);
        }
        loadLevel(currentLevel);
        for(int i=0; i<n; i+=1)
            for(int j=0; j<n; j+=1)
            {
                GameObject instance = Instantiate(Room,new Vector3(i*2, j*1.7f, 0), Quaternion.Euler(0, -90, 0));
                instance.GetComponent<RoomManager>().i = i+1;
                instance.GetComponent<RoomManager>().j = j+1;
                instance.GetComponent<RoomManager>().stat = RoomArray[i,j];
                if(RoomArray[i,j]==2)
                {  
                    instance.GetComponent<RoomManager>().activateSoldier();
                }
                if(RoomArray[i,j]==1)
                {  
                    instance.GetComponent<RoomManager>().activateWumpus();
                }
                Rooms.Add(instance);
            }         
        PlayerController.GameMap = new int[n+2,n+2];       
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                PlayerController.GameMap[i+1,j+1] = RoomArray[i,j];
            }
        }
    }
    public void GenerateMiniMap() // Generates a Minimap of the entire game in a corner for navigation
    {
        for(int i=0; i<n; i+=1)
            for(int j=0; j<n; j+=1)
            {
                GameObject instance = Instantiate(Maptile, new Vector3(0, 0, 0), Quaternion.identity);
                instance.transform.SetParent(canvas.transform);
                instance.GetComponent<RectTransform>().position = new Vector3(25+i*40, 25+j*40, 0);

                Maptiles.Add(instance);                
            }
    }   
    private void updateMiniMap() // Updates the Minimap when player changes location
    {
        var player_x = player.GetComponent<PlayerController>().Player_x;
        var player_y = player.GetComponent<PlayerController>().Player_y;
        foreach(GameObject mt in Maptiles)
        {
            mt.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 10);
            
        }
        Maptiles[(player_x-1)*n+(player_y-1)].gameObject.GetComponent<Image>().color = new Color32(103, 65, 61, 50);
    }   

    public void LevelPopUp() // Loads the levels from file when the level popup is opened
    {
        if(File.Exists(Application.persistentDataPath + "/LevelData.txt"))
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/LevelData.txt");
            string line;
            while((line = sr.ReadLine())!=null)
            {
                if(line=="Level")
                {
                    line = sr.ReadLine();
                    GameObject instance = Instantiate(LevelUI, new Vector3(0, 0, 0), Quaternion.identity);
                    LevelUI.GetComponent<LevelButton>().setButtonName(line);
                    instance.transform.SetParent(LevelPopup_Contents.transform);
                }
            }
            sr.Close();
        }
    }
    public void selectLevel(GameObject LevelButton) // Gets the name of the chosen level in the level popup menu
    {
        currentLevel = LevelButton.GetComponent<TMP_Text>().text;
    }

    public void DestroyLevelWhenClosed() // Destroys all the loaded levels when the level popup is closed
    {
        var levels = GameObject.FindGameObjectsWithTag("LevelUI");
        foreach(var level in levels)
        {
            Destroy(level);
        }
    }    

    public void saveLevel() // Saves a customized level made by the player inside a file
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/LevelData.txt", true);
        sw.WriteLine("Level");
        sw.WriteLine(levelName.GetComponent<TMP_InputField>().text);
        sw.WriteLine(n);
        foreach(GameObject R in Rooms)
        {
            sw.WriteLine(R.GetComponent<RoomManager>().stat);
        }
        sw.Close();
    }

    public void loadLevel(string levelName) // Loads a certain level from a file
    {
        if(File.Exists(Application.persistentDataPath + "/LevelData.txt"))
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/LevelData.txt");
            string line;
            while((line = sr.ReadLine())!=null)
            {
                if(line==levelName)
                {
                    line = sr.ReadLine();
                    n = int.Parse(line);
                    RoomArray = new int[n,n];
                    for(int i = 0; i < n; i++)
                        for(int j = 0; j < n; j++)
                        {
                            RoomArray[i,j] = int.Parse(sr.ReadLine());
                        }
                }
            }
            sr.Close();
        }
        else
        {
            GenerateBasicMap();
        }
    }

    ////////////////////////////////////// UI Navigation section //////////////////////////////////////
    public void pressPlay()
    {
        SceneManager.LoadScene("Generate_Level");
    }
    public void pressCreateLevel()
    {
        SceneManager.LoadScene("Create_Level");
    }
    public void pressExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void goToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Retry()
    {
        SceneManager.LoadScene("Generate_Level");
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Generate_Level")
            updateMiniMap();
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            menuPopup.SetActive(!menuPopup.activeInHierarchy);
        }
    }
}
