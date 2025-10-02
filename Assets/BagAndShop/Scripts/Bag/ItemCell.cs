using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

/// <summary>
/// 格子类型枚举
/// </summary>
public enum E_Item_Type
{
    Bag = 0,
    Head,
    Neck,
    Weapon,
    Cloth,
    Trousers,
    Shoes,
}

/// <summary>
/// 道具格子对象
/// </summary>
public class ItemCell : BasePanel {

    private ItemInfo _itemInfo;

    public E_Item_Type type = E_Item_Type.Bag;

    public Image imgBK;

    public Image imgIcon;

    private bool isOpenDrag = false;

    public ItemInfo itemInfo
    {
        get
        {
            return _itemInfo;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        imgIcon = GetControl<Image>("imgIcon");
        imgBK = GetControl<Image>("imgBK");
        //一开始就把图标隐藏了 应该是初始化了信息后 有了图标 才显示
        imgIcon.gameObject.SetActive(false);
        
        //监听鼠标移入 和鼠标移除的事件 来进行处理

        EventTrigger trigger = imgBK.gameObject.AddComponent<EventTrigger>();
        //申明一个 鼠标进入的事件类对象
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(EnterItemCell);

        //申明一个 鼠标移除的事件类对象
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(ExitItemCell);

        trigger.triggers.Add(enter);
        trigger.triggers.Add(exit);

    }

    /// <summary>
    /// 开启检测鼠标拖动相关的事件
    /// </summary>
    private void OpenDragEvent()
    {

        if (isOpenDrag)
            return;

        isOpenDrag = true;
        EventTrigger trigger = GetControl<Image>("imgBK").GetComponent<EventTrigger>();

        //EventTrigger中的 拖动相关知识点
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragItemCell);

        trigger.triggers.Add(beginDrag);

        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener(DragItemCell);

        trigger.triggers.Add(drag);

        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragItemCell);

        trigger.triggers.Add(endDrag);
    }

    private void BeginDragItemCell(BaseEventData data)
    {
        Debug.Log("开始拖动");
        EventCenter.Instance.EventTrigger<ItemCell>(E_EventType.E_ItemCellBeginDrag, this);
    }

    private void DragItemCell(BaseEventData data)
    {
        Debug.Log("拖动中");
        EventCenter.Instance.EventTrigger<BaseEventData>(E_EventType.E_ItemCellDrag, data);
    }

    private void EndDragItemCell(BaseEventData data)
    {
        Debug.Log("结束拖动");
        EventCenter.Instance.EventTrigger<ItemCell>(E_EventType.E_ItemCellEndDrag, this);
    }

    private void EnterItemCell(BaseEventData data)
    {
        EventCenter.Instance.EventTrigger<ItemCell>(E_EventType.E_ItemCellEnter, this);
    }

    private void ExitItemCell(BaseEventData data)
    {
        EventCenter.Instance.EventTrigger<ItemCell>(E_EventType.E_ItemCellExit, this);
    }

    /// <summary>
    /// 根据道具信息 初始化 格子信息
    /// </summary>
	public void InitInfo(ItemInfo info)
    {
        this._itemInfo = info;
        //如果是空 隐藏图标
        if( info == null )
        {
            imgIcon.gameObject.SetActive(false);
            return;
        }
        //只要设置了信息 应该激活图片
        imgIcon.gameObject.SetActive(true);
        //根据道具信息的数据 来更新格子对象
        Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
        //使用我们的道具表中的数据
        //图标
        //通过 道具ID得到的 道具表中的 数据信息  就可以得到对应的道具ID 用的图标是什么 加载它来用 即可
        imgIcon.sprite = ResMgr.Instance.Load<Sprite>("Icon/"+ itemData.icon);
        //数量
        if( type == E_Item_Type.Bag )
            GetControl<Text>("txtNum").text = info.num.ToString();

        if (itemData.type == (int)E_Bag_Type.Equip)
            OpenDragEvent();
    }
}
