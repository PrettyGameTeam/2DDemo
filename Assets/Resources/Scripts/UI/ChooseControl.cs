using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseControl : MonoBehaviour
{
    // Start is called before the first frame update
    private StageNode[] _stageNodes;
    private Chapter _chapter;

    private GameObject _tipPanel;

    // public AudioClip BgAudio;

    // private AudioSource audio = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ChooseControl Start");
        _tipPanel = (GameObject)Instantiate(Resources.Load("Prefabs/UI/TipBg"));
        _tipPanel.transform.SetParent(gameObject.transform);
        _tipPanel.transform.position = new Vector2(1080/2,1920/2);
        _tipPanel.SetActive(false);
        //查找Bg
        _stageNodes = new StageNode[25];
        GameObject stageList = GameObject.Find("StageList");
        for (int i = 0; i < 25; i++)
        {
            _stageNodes[i] = stageList.transform.Find("Stage" + i).gameObject.GetComponent<StageNode>();
        }
        var userData = UserDataManager.GetInstance().GetUserData();
        Stage st = ConfigManager.GetInstance().GetStage(userData.CurrentStage);
        UserChapter uc = userData.GetUserChapter(st.ChapterId);
        Debug.Log("userData.CurrentStage=" + userData.CurrentStage + ",st.ChapterId=" + st.ChapterId + ",uc.ChaperId=" + uc.ChapterId);
        LoadChapter(uc);
        // audio = GetComponent<AudioSource>();
        // audio.clip = BgAudio;
        // audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Debug.Log("ChooseControl Awake");
        //加载策划配置 
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PlayStage,OnPlayStage);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.DebugOneKeyClick,OnOneKey);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.NextChapterClick,OnNextChapterClick);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PreChapterClick,OnPreChapterClick);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.TipClose,OnTipClose);
        
        
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.PlayStage,OnPlayStage);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.DebugOneKeyClick,OnOneKey);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.NextChapterClick,OnNextChapterClick);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.PreChapterClick,OnPreChapterClick);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.TipClose,OnTipClose);
        
    }

    public void LoadChapter(UserChapter userChapter)
    {
        //加载标题
        ChapterNode cn = GameObject.Find("Bg").GetComponent<ChapterNode>();
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
        if (evt.eventParams == null){
            showTip("通关上一关卡开启");
            return;
        }
        //查找到点击的关卡
        UserDataManager.GetInstance().GetUserData().CurrentStage = (int) evt.eventParams;
        Debug.Log("CurrentStage=" + UserDataManager.GetInstance().GetUserData().CurrentStage);
        
        // s.PrefabName  
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }

    private void OnOneKey(UEvent evt)
    {
        Debug.Log("OnOneKey");
        //查找到点击的关卡
        UserDataManager.GetInstance().OneKeyOpen();
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.ChapterId);
        LoadChapter(uc);
    }

    private void OnNextChapterClick(UEvent evt)
    {
        Debug.Log("OnNextChapterClick");
        //查找到点击的关卡
        if (_chapter.NextChapter == null){
            showTip("已经是最后一关");
            return;
        }
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.NextChapter.ChapterId);
        if (uc == null){
            showTip("通关本章节开启下一章");
            return;
        }
        _chapter = _chapter.NextChapter;
        LoadChapter(uc);
    }

    private void OnPreChapterClick(UEvent evt)
    {
        Debug.Log("OnPreChapterClick _chapter=" + _chapter.ChapterId);
        //查找到点击的关卡
        if (_chapter.PreChapter == null){
            showTip("已经是第一章");
            return;
        }
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.PreChapter.ChapterId);
        if (uc == null){
            showTip("未知错误");
            return;
        }
        _chapter = _chapter.PreChapter;
        LoadChapter(uc);
    }

    private void OnTipClose(UEvent evt)
    {
        _tipPanel.SetActive(false);
    }

    private void showTip(string msg){
        CommonUIAni c = _tipPanel.GetComponent<CommonUIAni>();

        Text t = _tipPanel.transform.Find("TipMsg").gameObject.GetComponent<Text>();
        t.text = msg;
        c.PlayScale(10f);
        _tipPanel.SetActive(true);
        // c.AutoPlay(10f,10f,1f);
    }
}
