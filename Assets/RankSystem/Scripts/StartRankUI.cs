using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class StartRankUI : MonoBehaviour
{
    public PlayerData p1= new PlayerData();
    public PlayerData p2= new PlayerData();
    
    // Start is called before the first frame update
    void Start()
    {
        // p1.playerName = "test Write1";
        // p1.rankNum = 2;
        // p1.score = 223;
        
        // List<PlayerData> plist = new List<PlayerData>();
        // plist.Add(p1);
        // plist.Add(p2);
        
        // RankMgr.Instance.WritePlayerDataOnline(p1);
        
        //RankMgr.Instance.LoadPlayerDataOnline();
        UIManager.Instance.ShowPanel<RankInputPanel>("RankInputPanel");
    }

}
