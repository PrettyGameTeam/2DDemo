

using System;
using System.Collections.Generic;
using LitJson;

public class UserData
{
    public string DeviceId { get; set; }//设备ID

    public List<UserChapter> Chapters { get; set; }

    public int CurrentStage;

    public void parseJson(JsonData jd)
    {
        DeviceId = jd["DeviceId"].ToString();
        Chapters = new List<UserChapter>();
        foreach (JsonData chapter in jd["Chapters"])
        {
            UserChapter uc = new UserChapter();
            uc.parseJson(chapter);
            Chapters.Add(uc);
        }

        if (jd["CurrentStage"] != null)
        {
            CurrentStage = Int32.Parse(jd["CurrentStage"].ToString());
        }
        else
        {
            CurrentStage = 0;
        }
    }

    public UserStage GetUserStage(int stageId){
        foreach (var c in Chapters)
        {
            foreach (var s in c.Stages)
            {
                if (s.StageId == stageId){
                    return s;
                }
                
            }
        }
        return null;
    }

    public bool OpenStage(Stage stage){
        //是否有当前章节
        UserChapter uc = null;
        foreach (var c in Chapters)
        {
            if (c.ChapterId == stage.ChapterId)
            {
                uc = c;
                break;
            }
        }

        if (uc != null)
        {
            foreach (var s in uc.Stages)
            {
                if (stage.StageId == s.StageId)
                {
                    return false;
                }
            }
        }
        else 
        {
            UserChapter nuc = new UserChapter();
            nuc.Stages = new List<UserStage>();
            nuc.ChapterId = stage.ChapterId;
        }

        UserStage us = new UserStage();
        us.StageId = stage.StageId;
        us.Star = 0;
        us.Completed = false;
        uc.Stages.Add(us);
        DBManager.WriteUserData();
        return true;
    }
}