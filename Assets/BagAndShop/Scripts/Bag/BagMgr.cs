using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class BagMgr : BaseManager<BagMgr>
{
    //当前拖动着的格子
    private ItemCell nowSelItem;
    //当前鼠标进入的格子
    private ItemCell nowInItem;

    //当前选中装备的 图片信息
    private Image nowSelItemImg;

    //是否拖动中
    private bool isDraging = false;

    private BagMgr()
    {
        
    }
    
    public void Init()
    {
        //TODO:修改EventCenter
        EventCenter.Instance.AddEventListener<ItemCell>(E_EventType.E_ItemCellBeginDrag, BeginDragItemCell);
        EventCenter.Instance.AddEventListener<BaseEventData>(E_EventType.E_ItemCellDrag, DragItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>(E_EventType.E_ItemCellEndDrag, EndDragItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>(E_EventType.E_ItemCellEnter, EnterItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>(E_EventType.E_ItemCellExit, ExitItemCell);
    }

    /// <summary>
    /// 切换装备
    /// </summary>
    public void ChangeEquip()
    {
        //从背包拖装备
        if( nowSelItem.type == E_Item_Type.Bag )
        {
            //存在进入的格子 并且 格子 不是背包中的格子
            if( nowInItem != null && nowInItem.type != E_Item_Type.Bag)
            {
                //读表
                Item info = GameDataMgr.Instance.GetItemInfo(nowSelItem.itemInfo.id);
                //装备交换
                //1.判断 格子 类型 和装备 类型 是否一致
                if( (int)nowInItem.type == info.equipType)
                {
                    Debug.Log(nowInItem.itemInfo);
                    //如果 装备栏 是空 直接放
                    if( nowInItem.itemInfo == null )
                    {
                        //直接装备 直接从背包中移除 然后更新面板即可
                        GameDataMgr.Instance.playerInfo.nowEquips.Add(nowSelItem.itemInfo);
                        GameDataMgr.Instance.playerInfo.equips.Remove(nowSelItem.itemInfo);
                    }
                    //交换
                    else
                    {
                        //直接装备 直接从背包中移除 然后更新面板即可
                        GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowInItem.itemInfo);
                        GameDataMgr.Instance.playerInfo.nowEquips.Add(nowSelItem.itemInfo);

                        GameDataMgr.Instance.playerInfo.equips.Remove(nowSelItem.itemInfo);
                        GameDataMgr.Instance.playerInfo.equips.Add(nowInItem.itemInfo);
                    }

                    //更新背包
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").ChangeType(E_Bag_Type.Equip);
                    //更新人物
                    UIManager.Instance.GetPanel<RolePanel>("RolePanel").UpdateRolePanel();

                    //保存数据
                    GameDataMgr.Instance.SavePlayerInfo();
                }
            }
        }
        //从装备栏往外拖
        else
        {
            if( nowInItem == null || nowInItem.type != E_Item_Type.Bag)
            {
                GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowSelItem.itemInfo);
                GameDataMgr.Instance.playerInfo.equips.Add(nowSelItem.itemInfo);
                Debug.Log(GameDataMgr.Instance.playerInfo.nowEquips.Count);
                Debug.Log(GameDataMgr.Instance.playerInfo.equips.Count);
                //更新背包
                UIManager.Instance.GetPanel<BagPanel>("BagPanel").ChangeType(E_Bag_Type.Equip);
                //更新人物
                UIManager.Instance.GetPanel<RolePanel>("RolePanel").UpdateRolePanel();

                //保存数据
                GameDataMgr.Instance.SavePlayerInfo();
            }
            else if( nowInItem != null && nowInItem.type == E_Item_Type.Bag )
            {
                //读表
                Item info = GameDataMgr.Instance.GetItemInfo(nowInItem.itemInfo.id);
                if ((int)nowSelItem.type == info.equipType)
                {
                    //直接装备 直接从背包中移除 然后更新面板即可
                    GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowSelItem.itemInfo);
                    GameDataMgr.Instance.playerInfo.nowEquips.Add(nowInItem.itemInfo);

                    GameDataMgr.Instance.playerInfo.equips.Remove(nowInItem.itemInfo);
                    GameDataMgr.Instance.playerInfo.equips.Add(nowSelItem.itemInfo);

                    //更新背包
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").ChangeType(E_Bag_Type.Equip);
                    //更新人物
                    UIManager.Instance.GetPanel<RolePanel>("RolePanel").UpdateRolePanel();

                    //保存数据
                    GameDataMgr.Instance.SavePlayerInfo();
                }
            }
        }
    }

    private void BeginDragItemCell(ItemCell itemCell)
    {
        if (itemCell.itemInfo == null)
            return;

        //一开始拖动 直接隐藏 tips面板
        UIManager.Instance.HidePanel("TipsPanel");
        isDraging = true;
        //记录当前选中的格子
        nowSelItem = itemCell;

        //创建一张图片 用来显示 当前格子的装备图标
        GameObject obj = PoolManager.Instance.GetObj("UI/imgIcon");
        nowSelItemImg = obj.GetComponent<Image>();
        nowSelItemImg.sprite = itemCell.imgIcon.sprite;

        //设置父对象 改变缩放大小相关
        nowSelItemImg.transform.SetParent(UIManager.Instance.canvas);
        nowSelItemImg.transform.localScale = Vector3.one;

        //如果异步加载结束 拖动已经结束了 直接放入缓存池
        if(!isDraging)
        {
            PoolManager.Instance.PushObj(nowSelItemImg.gameObject);
            nowSelItemImg = null;
        }
        
    }

    private void DragItemCell(BaseEventData eventData)
    {
        //拖动中 更新这个图片的位置
        //把鼠标位置 转换到 UI相关的位置 让 图片跟随鼠标移动
        if (nowSelItemImg == null)
            return;
        Vector2 localPos;
        //用于坐标转换的api
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.Instance.canvas, //希望得到坐标结果对象的 父对象
            (eventData as PointerEventData).position, //相当于 鼠标位置
            (eventData as PointerEventData).pressEventCamera, //相当于 UI摄像机
            out localPos);
        nowSelItemImg.transform.localPosition = localPos;
    }

    private void EndDragItemCell(ItemCell itemCell)
    {
        isDraging = false;

        //切换装备
        ChangeEquip();

        //结束拖动时 置空 信息
        nowSelItem = null;
        nowInItem = null;

        //结束拖动 移除这个图片
        if (nowSelItemImg == null)
            return;
        PoolManager.Instance.PushObj(nowSelItemImg.gameObject);
        nowSelItemImg = null;
    }

    private void EnterItemCell(ItemCell itemCell)
    {
        Debug.Log("in");
        if (isDraging)
        {
            //拖动中 进入格子  记录进入的信息
            nowInItem = itemCell;
            return;
        }

        //如果是空 别显示 tip面板
        if (itemCell.itemInfo == null)
            return;

        //显示提示面板
        UIManager.Instance.ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //异步加载结束后 去设置位置 设置信息
            //更新信息
            panel.InitInfo(itemCell.itemInfo);
            //更新位置
            panel.transform.position = itemCell.imgBK.transform.position;
            //如果面板异步加载结束 发现 已经开始拖动了 直接隐藏 tip面板
            if(isDraging)
            {
                UIManager.Instance.HidePanel("TipsPanel");
            }
        });
    }

    private void ExitItemCell(ItemCell itemCell)
    {
        Debug.Log("out");
        if (isDraging)
        {
            //拖动中 离开格子 清空记录的信息
            nowInItem = null;
            return;
        }

        if (itemCell.itemInfo == null)
            return;
        //隐藏提示面板
        UIManager.Instance.HidePanel("TipsPanel");
    }

}
