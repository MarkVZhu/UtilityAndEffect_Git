using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameDataMgr.Instance.Init();
        BagMgr.Instance.Init();

        Debug.Log(GameDataMgr.Instance.GetItemInfo(1).name);

        //显示主面板
        UIManager.Instance.ShowPanel<BagMainPanel>("BagMainPanel", E_UI_Layer.Bot);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
