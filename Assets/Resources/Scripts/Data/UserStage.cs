

using System;
using LitJson;

public class UserStage
{
    public int StageId { get; set; } //关卡ID
    public int Star { get; set; }    //星级
    public bool Completed { get; set; } //是否已完成
    
    public void parseJson(JsonData jd)
    {
        StageId = Int32.Parse(jd["StageId"].ToString());
        Star = Int32.Parse(jd["Star"].ToString());
        Completed = Boolean.Parse(jd["Completed"].ToString());
    }
}