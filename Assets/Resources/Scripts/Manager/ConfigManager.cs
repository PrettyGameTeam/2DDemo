
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager
{
    private static ConfigManager _configManager = new ConfigManager();

    private Dictionary<int, Chapter> _chapterMap;

    private Dictionary<int, Stage> _stageMap;
    
    private ConfigManager()
    {
        
    }

    public static ConfigManager GetInstance()
    {
        return _configManager;
    }

    public void LoadConfig()
    {
        _chapterMap = new Dictionary<int, Chapter>();
        _stageMap = new Dictionary<int, Stage>();
        List<Chapter> chapters = DBManager.LoadChapterConfig();
        List<Stage> stages = DBManager.LoadStageConfig();

        foreach (Chapter c in chapters)
        {
            _chapterMap.Add(c.ChapterId,c);   
        }

        foreach (Stage s in stages)
        {
            _stageMap.Add(s.StageId,s);
            if (_chapterMap.ContainsKey(s.ChapterId))
            {
                var chapter = _chapterMap[s.ChapterId];
                chapter.Stages.Add(s);
            }
            else
            {
                
                Debug.LogWarning("can not find Chapter[" + s.ChapterId + "] when load Stage[" + s.StageId + "]");
            }
        }

        foreach (var kv in _chapterMap)
        {
            var v = kv.Value;
            v.Stages.Sort((a, b) => {
                var o = a.Order - b.Order;
                return o;
            });
            if (v.NextChapterId > 0){
                v.NextChapter = _chapterMap[v.NextChapterId];
                v.NextChapter.PreChapter = v;
            }
        }

        foreach (var kv in _stageMap)
        {
            var v = kv.Value;
            if (v.NextStageId > 0){
                v.NextStage = _stageMap[v.NextStageId];
            }
            v.Chapter = _chapterMap[v.ChapterId];
        }
    }

    public List<Chapter> GetAllChapters()
    {
        List<Chapter> chapters = new List<Chapter>();
        foreach (var kv in _chapterMap)
        {
            chapters.Add(kv.Value);
        }
        chapters.Sort((a, b) => {
            var o = a.Order - b.Order;
            return o;
        });
        return chapters;
    }

    public List<Stage> GetAllStages()
    {
        List<Stage> stages = new List<Stage>();
        foreach (var kv in _stageMap)
        {
            stages.Add(kv.Value);
        }
        stages.Sort((a, b) => {
            var o = a.StageId - b.StageId;
            return o;
        });
        return stages;
    }

    public Chapter GetChapter(int chapterId)
    {
        return _chapterMap[chapterId];
    }
    
    public Stage GetStage(int stageId)
    {
        return _stageMap[stageId];
    }

    public Stage GetLastStageByChapterId(int chapterId)
    {
        Chapter c = _chapterMap[chapterId];
        if (c == null){
            Debug.LogError("Can not found ChapterConfig [chapterId=" + chapterId + "], some thing wrong");
            return null;
        }

        return c.Stages[c.Stages.Count - 1];
    }

    public Stage GetFirstStageByChapterId(int chapterId)
    {
        Chapter c = _chapterMap[chapterId];
        if (c == null){
            Debug.LogError("Can not found ChapterConfig [chapterId=" + chapterId + "], some thing wrong");
            return null;
        }

        return c.Stages[0];
    }

    public Chapter GetFirstChapter()
    {
        var chapters = GetAllChapters();
        return chapters[0];
    }

    public Chapter GetLastChapter()
    {
        var chapters = GetAllChapters();
        return chapters[chapters.Count - 1];
    }

    public Chapter GetNextStage(int chapterId,int stageId)
    {

        var chapters = GetAllChapters();
        return chapters[chapters.Count - 1];
    }



}