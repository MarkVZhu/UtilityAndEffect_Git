using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

/// <summary>
/// 一键提示面板
/// </summary>
public class OneBtnTipPanel : BasePanel {

	// Use this for initialization
	void Start () {
        GetControl<Button>("btnSure").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("OneBtnTipPanel");
        });
	}
	
	public void InitInfo(string info)
    {
        GetControl<Text>("txtInfo").text = info;
    }
}
