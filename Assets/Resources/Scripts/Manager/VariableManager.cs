using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableManager
{
    private static VariableManager _variableManager = new VariableManager();

    private Dictionary<string, string> _strVariableMap = new Dictionary<string, string>();

    private Dictionary<string, int> _numVariableMap = new Dictionary<string, int>();

    private VariableManager()
    {

    }

    public static VariableManager GetInstance()
    {
        return _variableManager;
    }

    public void SetIntVariable(string key,int v){
        if (_numVariableMap.ContainsKey(key)){
            _numVariableMap[key] = v;
            return;
        }
        _numVariableMap.Add(key,v);
    }

    public int GetIntVariable(string key){
        int val = _numVariableMap.ContainsKey(key) ? _numVariableMap[key] : 0;
        return val;
    }

    public void SetStrVariable(string key,string v){
        if (_strVariableMap.ContainsKey(key)){
            _strVariableMap[key] = v;
            return;
        }
        _strVariableMap.Add(key,v);
    }
    public string GetStrVariable(string key){
        string val = _strVariableMap.ContainsKey(key) ? _strVariableMap[key] : null;
        return val;
    }





}
