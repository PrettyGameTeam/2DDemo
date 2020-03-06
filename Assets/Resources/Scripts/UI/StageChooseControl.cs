using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChooseControl : MonoBehaviour
{
    private StageNode[] _stageNodes;
    private Chapter _chapter;
    
    // Start is called before the first frame update
    void Start()
    {
        //查找Bg
        _stageNodes = new StageNode[25];
        GameObject stageList = GameObject.Find("Canvas/StageList");
        for (int i = 0; i < 25; i++)
        {
            _stageNodes[i] = stageList.transform.Find("Stage" + i).gameObject.GetComponent<StageNode>();
        }
        var userData = UserDataManager.GetInstance().GetUserData();
        LoadChapter(userData.Chapters[userData.Chapters.Count - 1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //加载策划配置
        ConfigManager.GetInstance().LoadConfig();
        UserDataManager.GetInstance().LoadUserData(); 
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PlayStage,OnPlayStage);
    }

    public void LoadChapter(UserChapter userChapter)
    {
        //加载标题
        ChapterNode cn = GameObject.Find("Canvas/Bg").GetComponent<ChapterNode>();
        ConfigManager cm = ConfigManager.GetInstance();
        _chapter = cm.GetChapter(userChapter.ChapterId);
        cn.LoadData(_chapter,userChapter);

        Chapter chapter = ConfigManager.GetInstance().GetChapter(userChapter.ChapterId);
        //加载关卡按钮
        for (int i = 0; i < _stageNodes.Length; i++)
        {
            if (i >= chapter.Stages.Count)
            {
                _stageNodes[i].LoadData(null,null);
            }
            else if (i >= userChapter.Stages.Count)
            {
                _stageNodes[i].LoadData(ConfigManager.GetInstance().GetStage(chapter.Stages[i].StageId),null);
            }
            else
            {
                Debug.Log("chapter.Stages[i].StageId=" + chapter.Stages[i].StageId);
                Debug.Log("userChapter.Stages[i]=" + userChapter.Stages[i]);
                _stageNodes[i].LoadData(ConfigManager.GetInstance().GetStage(chapter.Stages[i].StageId),userChapter.Stages[i]);
            }
        }
    }

    private void OnPlayStage(UEvent evt)
    {
        Debug.Log("OnPlayStage");
        //查找到点击的关卡
        UserDataManager.GetInstance().GetUserData().CurrentStage = (int) evt.eventParams;
        
        // s.PrefabName  
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }
}
