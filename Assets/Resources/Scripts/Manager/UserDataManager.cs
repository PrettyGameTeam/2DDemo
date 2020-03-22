using System;
using System.Collections.Generic;

public class UserDataManager
{
    private static UserDataManager _userDataManager = new UserDataManager();

    private UserData _userData;

    private UserDataManager()
    {
        
    }
    
    public static UserDataManager GetInstance()
    {
        return _userDataManager;
    }

    //加载玩家数据
    public void LoadUserData()
    {
        //从本地读取玩家存档
        _userData = DBManager.ReadUserData();
        if (_userData == null)
        {
            InitUserData();
        }
    }

    public void OneKeyOpen()
    {
        foreach (var st in ConfigManager.GetInstance().GetAllStages())
        {
            _userData.OpenStage(st);
        }
    }

    private void InitUserData()
    {
        _userData = new UserData();
        //初始化第一章第一关
        _userData.DeviceId = System.Guid.NewGuid().ToString();
        Chapter c = ConfigManager.GetInstance().GetAllChapters()[0];
        List<UserChapter> chapters = new List<UserChapter>();
        UserChapter uc = new UserChapter();
        uc.ChapterId = c.ChapterId;
        uc.Stages = new List<UserStage>();
        chapters.Add(uc);
        Stage s = c.Stages[0];
        UserStage us = new UserStage();
        us.StageId = s.StageId;
        us.Completed = false;
        us.Star = 0;
        uc.Stages.Add(us);
        _userData.Chapters = chapters;
        _userData.CurrentStage = s.StageId;
        DBManager.WriteUserData();
    }

    public UserData GetUserData()
    {
        return _userData;
    }
}