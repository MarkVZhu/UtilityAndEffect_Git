using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
using TMPro;

public class RankInputPanel : BasePanel {

	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start () {
		//TODO:Score
		GetControl<TextMeshProUGUI>("ScoreText").text = "Score : " + 1;
		
		RankMgr.Instance.updatedPlayerList = new List<PlayerData>();
		
		if(RankMgr.Instance.isOnlineRank)
		{
		    RankMgr.Instance.LoadPlayerDataOnline((pl) =>
		    {
		        RankMgr.Instance.updatedPlayerList = pl;
		        Debug.Log("加载线上Rank, Count: " + pl.Count);	        
		    }
		    );
		}
		else
		{
		    RankMgr.Instance.updatedPlayerList = RankMgr.Instance.LoadPlayerDataLocal();
		}
	}

	private void Drag(BaseEventData data)
	{
		//拖拽逻辑
	}

	private void PointerDown(BaseEventData data)
	{
		//PointerDown逻辑
	}

	// Update is called once per frame
	void Update () {
		
	}

	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}

	protected override void OnClick(string btnName)
	{
		base.OnClick(btnName);
		
		switch(btnName)
		{
			case "btnSubmit":
				Debug.Log("btnSubmit被点击");
				string name = GetControl<TMP_InputField>("NameInputField").text;
				
				PlayerData currentPd = new PlayerData();
				currentPd.playerName = name;
				currentPd.rankNum = 0;
				//TODO:Score
				currentPd.score = 50;//GameManager.Instance.GetScore();
				//Random.Range(1000,2000); //StaticDataCenter.Instance.currentScore;
				
				RankMgr.Instance.updatedPlayerList = RankMgr.Instance.InsertPlayerData(currentPd, RankMgr.Instance.updatedPlayerList);
				
				if(!RankMgr.Instance.isOnlineRank)
				{
					RankMgr.Instance.WritePlayerDataLocal(RankMgr.Instance.updatedPlayerList);
				}
				else
				{
				    List<PlayerData> playersData = RankMgr.Instance.LoadPlayerDataLocal();
					RankMgr.Instance.WritePlayerDataOnline(currentPd);
					Debug.Log("写入玩家数据至线上排名文件");
				}
				UIManager.Instance.HidePanel("RankInputPanel");
				UIManager.Instance.ShowPanel<RankShowPanel>("RankShowPanel");
				break;
			case "btnCancel":
				Debug.Log("btnCancel被点击");
				UIManager.Instance.HidePanel("RankInputPanel");
				UIManager.Instance.ShowPanel<RankShowPanel>("RankShowPanel");
				break;
		}
	}

	protected override void OnValueChanged(string toggleName, bool value)
	{
		//在这来根据名字判断 到底是那一个单选框或者多选框状态变化了 当前状态就是传入的value
	}


	public void InitInfo()
	{
		Debug.Log("初始化数据");
	}

	//点击开始按钮的处理(可以放到switch里)
	public void ClickStart()
	{
	}

	//点击开始按钮的处理
	public void ClickQuit()
	{
		Debug.Log("Quit Game");
	}
}
