using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

/// <summary>
/// 提示管理器 提供 所有相关提示的公共方法给外部 提高开发效率
/// </summary>
public class TipMgr : BaseManager<TipMgr> {

    /// <summary>
    /// 显示一键提示面板
    /// </summary>
    /// <param name="info"></param>
    
    private TipMgr()
    {
        
    }
    
	public void ShowOneBtnTip(string info)
    {
        UIManager.Instance.ShowPanel<OneBtnTipPanel>("OneBtnTipPanel", E_UI_Layer.System, (panel) =>
        {
            panel.InitInfo(info);
        });
    }
}
