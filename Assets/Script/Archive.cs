using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.UIWidgets.widgets;
using UnityEngine;

public class Archive
{
    //private string savePath = string.Empty;
    private static Archive archive = null;
    public bool HasArchive
    {
        get;
    }
    public bool HasRead
    {
        get;
        set;
    } = false;

    private Archive()
    {
        HasArchive = File.Exists(Application.persistentDataPath + @"\default.arc");
    }

    public static Archive GetInstance()
    {
        if (archive == null)
        {
            archive = new Archive();
        }
        return archive;
    }

    public void Del(string archiveName = @"\default.arc")
    {
        File.Delete(Application.persistentDataPath + archiveName);
    }

    public void Save(string archiveName = @"\default.arc")
    {
        JObject jsonObj = new JObject();
        GameObject obj = GameObject.Find("GameController");
        GameManager gameManager = (GameManager)obj.GetComponent("GameManager");
        GameController gameController = (GameController)obj.GetComponent("GameController");
        JArray positonArr = new JArray();
        foreach (ArrayList i in gameController.gemstoneList)
        {
            foreach(Gemstone j in i)
            {
                string x = JsonUtility.ToJson(j);
                JObject o = JObject.Parse(x);
                positonArr.Add(o);
            }
        }
        JArray scoreTextArr= new JArray();
        foreach (UnityEngine.UI.Text i in gameController.scoreText)
        {
            scoreTextArr.Add(i.text);
        }
        jsonObj.Add("positionList", positonArr);
        jsonObj.Add("scoreTextArr", scoreTextArr);
        jsonObj.Add("timeText", gameManager.timeText.text);
        jsonObj.Add("playerScore", gameManager.playerScore.text);
        
        File.WriteAllText(Application.persistentDataPath + archiveName, jsonObj.ToString());
    }

    public JObject Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + @"\default.arc");
        HasRead = true;
        return JObject.Parse(json);
    }

    private void Start()
    {

    }
    private void Update()
    {

    }
}
