using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterNode : MonoBehaviour
{
    //关卡标题
    private Image _title;
    //下一章
    private Button _next;
    //前一章
    private Button _prev;
    //返回主菜单
    private Button _back;
    //背景
    private Image _bg;

    private bool _inited = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void init()
    {
        _title = transform.Find("ChapterTitle").gameObject.GetComponent<Image>();
        _next = transform.Find("ButtonList/Next").gameObject.GetComponent<Button>();
        _prev = transform.Find("ButtonList/Prev").gameObject.GetComponent<Button>();
        _back = transform.Find("ButtonList/Back").gameObject.GetComponent<Button>();
        _bg = gameObject.GetComponent<Image>();
        _inited = true;
    }

    //加载数据
    public void LoadData(Chapter chapter, UserChapter userChapter)
    {
        if (!_inited)
        {
            init();
        }

        //设置BG
        Texture2D t = Resources.Load<Texture2D>("Textures/UI/StageChoose/" + chapter.ChapterBg);
        _bg.sprite = Sprite.Create(t,new Rect(0f,0f,t.width,t.height),new Vector2(0.5f,0.5f));
        t = Resources.Load<Texture2D>("Textures/UI/StageChoose/" + chapter.ChapterTitle);
        _title.sprite = Sprite.Create(t,new Rect(0f,0f,t.width,t.height),new Vector2(0.5f,0.5f));
        //TODO 设置按钮调用
    }
}
