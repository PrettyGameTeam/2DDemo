


using System;
using System.Collections.Generic;
using LitJson;

public class UserChapter
{
    public int ChapterId { get; set; } //章节ID

    public List<UserStage> Stages { get; set; } //关卡列表

    public void parseJson(JsonData jd)
    {
        ChapterId = Int32.Parse(jd["ChapterId"].ToString());
        Stages = new List<UserStage>(); 
        foreach (JsonData stage in jd["Stages"])
        {
            UserStage s = new UserStage();
            s.parseJson(stage);   
            Stages.Add(s);
        }
    }
}