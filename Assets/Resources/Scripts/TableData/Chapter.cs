

using System;
using System.Collections.Generic;
using LitJson;

public class Chapter : TableBase
{
    public int ChapterId { get; set; }
    
    public string ChapterTitle { get; set; }

    public int Order { get; set; }

    public List<Stage> Stages { get; set; }
    
    public string ChapterBg { get; set; }
    
    public string ChapterAudio { get; set; }

    public int NextChapterId { get; set; }

    public Chapter NextChapter { get; set; }

    public Chapter PreChapter { get; set; }

    public void parseJson(JsonData jd)
    {
        ChapterId = Int32.Parse(jd["ChapterId"].ToString());
        ChapterTitle = jd["ChapterTitle"].ToString();
        Order = Int32.Parse(jd["Order"].ToString());
        ChapterBg = jd["ChapterBg"].ToString();
        ChapterAudio = jd["ChapterAudio"].ToString();
        Stages = new List<Stage>();
        NextChapterId = Int32.Parse(jd["NextChapterId"].ToString());
    }

}