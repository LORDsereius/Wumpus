using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public GameObject text;
    void Start()
    {
/*
        if(File.Exists(Application.persistentDataPath + "/LevelData.txt"))
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/LevelData.txt");
            string line;
            while((line = sr.ReadLine())!=null)
            {
                if(line=="Level")
                {
                    line = sr.ReadLine();
                    text.GetComponent<TMP_Text>().text = line;
                    LevelButtonName = line;                    
                }
            }
            sr.Close();
        }        
*/
    }
    public void setButtonName(string line)
    {
        text.GetComponent<TMP_Text>().text = line;
    }
    void Update()
    {
        
    }
}
