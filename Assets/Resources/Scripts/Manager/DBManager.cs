using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;


public class DBManager
{

    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
    public static readonly string UserDataPath =  
    #if UNITY_ANDROID   //安卓  
        Application.persistentDataPath + "/";
    #elif UNITY_IPHONE  //iPhone  
        Application.dataPath + "/Raw/";  
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
        Application.dataPath + "/Resources/UserData/";  
    #else  
        string.Empty;  
    #endif    

    public static List<Chapter> LoadChapterConfig() {
        WWW t_WWW = new WWW(Application.streamingAssetsPath + "/chapter.json");
        while ( !t_WWW.isDone )
        {

        }
        JsonData jd = JsonMapper.ToObject(t_WWW.text);
        //第一步 读取章节表
        // JsonReader js = new JsonReader(new StreamReader(Application.streamingAssetsPath + "/chapter.json"));
        //第二步：将json文本转换成对象
        
        // JsonData jd = JsonMapper.ToObject(js);
        Debug.Log("LoadChapterConfig jd.ToJson()=" + jd.ToJson());
        List<Chapter> chapters = new List<Chapter>();
        for (int i = 0; i < jd.Count; i++)
        {
            Chapter c = new Chapter();
            chapters.Add(c);
            c.parseJson(jd[i]);
        }
        return chapters;
    }
    
    public static List<Stage> LoadStageConfig() {
        WWW t_WWW = new WWW(Application.streamingAssetsPath + "/stage.json");
        while ( !t_WWW.isDone )
        {

        }
        JsonData jd = JsonMapper.ToObject(t_WWW.text);
        //第一步 读取章节表
        // JsonReader js = new JsonReader(new StreamReader(Application.streamingAssetsPath + "/stage.json"));
        //第二步：将json文本转换成对象
        // JsonData jd = JsonMapper.ToObject(js);
        Debug.Log("LoadStageConfig jd.ToJson()=" + jd.ToJson());
        List<Stage> stages = new List<Stage>();

        for (int i = 0; i < jd.Count; i++)
        {
            Stage c = new Stage();
            stages.Add(c);
            c.parseJson(jd[i]);
        }
        return stages;
    }

    //读取本地存档
    public static UserData ReadUserData()
    {
        string path = UserDataPath + "data.json";
        if (!System.IO.File.Exists(path))
        {
            Debug.Log("ReadUserData null");
            return null;
        }
        Debug.Log("ReadUserData path=" + path);
        JsonReader js = new JsonReader(new StreamReader(path));
        //第二步：将json文本转换成对象
        JsonData jd = JsonMapper.ToObject(js);
        Debug.Log("ReadUserData jd.ToJson()=" + jd.ToJson());
        UserData userData = new UserData();
        userData.parseJson(jd);
        return userData;
    }

    //写入本地存档
    public static void WriteUserData()
    {
        UserData userData = UserDataManager.GetInstance().GetUserData();
        string json = JsonMapper.ToJson(userData);
        
        string path = UserDataPath + "data.json";
        StreamWriter sw = new StreamWriter(path);
        sw.Write(json);
        sw.Close();
    }
}