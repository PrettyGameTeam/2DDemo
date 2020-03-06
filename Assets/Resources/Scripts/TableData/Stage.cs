
using System;
using LitJson;

public class Stage : TableBase
{
    public int StageId { get; set; }
    public string StageName { get; set; }
    public int ChapterId { get; set; }
    public int MaxStar { get; set; }
    public int Order { get; set; }
    public string PrefabName { get; set; }
    
    public string StageAudio { get; set; }

    public int NextStageId = 0;

    public Stage NextStage = null;

    public Chapter Chapter = null;
    
    public void parseJson(JsonData jd)
    {
        StageId = Int32.Parse(jd["StageId"].ToString());
        StageName = jd["StageName"].ToString();
        ChapterId = Int32.Parse(jd["ChapterId"].ToString());
        MaxStar = Int32.Parse(jd["MaxStar"].ToString());
        Order = Int32.Parse(jd["Order"].ToString());
        PrefabName = jd["PrefabName"].ToString();
        StageAudio = jd["StageAudio"].ToString();
        NextStageId = Int32.Parse(jd["NextStageId"].ToString());
    }
}