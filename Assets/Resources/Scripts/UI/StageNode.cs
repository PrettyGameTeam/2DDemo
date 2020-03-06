using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    //关卡标题
    private Text _title;
    //关卡星级
    private Image[] _stars;
    //锁
    private Image _lock;
    //玩家关卡信息
    private UserStage _userStage;

    private Button _button;

    private bool _inited = false;

    // Start is called before the first frame update
    void Start()
    {
    
        Debug.Log(gameObject.name + " start");
        
    }

    private void init()
    {
        _title = transform.Find("Text").gameObject.GetComponent<Text>();
        _lock = transform.Find("Lock").gameObject.GetComponent<Image>();
        _stars = new Image[4];
        for (int i = 0; i < 4; i++)
        {
            _stars[i] = transform.Find("StarLayout/Star" + i).gameObject.GetComponent<Image>();
        }
        //加载按钮点击监听
        _button = GetComponent<Button>();
        _button.onClick.AddListener (OnClick);
        _inited = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //加载数据
    public void LoadData(Stage stage, UserStage userStage)
    {
        if (!_inited)
        {
            init();
        }
        
        if (stage == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _title.text = stage.Order + "";
        int star = -1;
        if (userStage == null)
        {
            _lock.gameObject.SetActive(true);
            _button.enabled = false;
        }
        else
        {
            star = userStage.Star;
            _button.enabled = true;
            _lock.gameObject.SetActive(false);
        }

        Debug.Log("star=" + star + ",_stars.Length=" + _stars.Length);
        for (int i = 0; i < _stars.Length; i++)
        {
            if (i == star)
            {
                _stars[i].gameObject.SetActive(true);    
            }
            else
            {
                _stars[i].gameObject.SetActive(false);    
            }
        }
        _userStage = userStage;
    }

    void OnClick()
    {
        Debug.Log("OnClick stageID = " + _userStage.StageId);
        Stage stage = ConfigManager.GetInstance().GetStage(_userStage.StageId);
        if (stage == null)
        {
            Debug.LogError("can not found Stage[" +  _userStage.StageId  + "] when Click Stage Node");
        }
        else
        {
            UEvent e = new UEvent(EventTypeName.PlayStage,_userStage.StageId);
            ObjectEventDispatcher.dispatcher.dispatchEvent(e,null);    
        }
    }
}
