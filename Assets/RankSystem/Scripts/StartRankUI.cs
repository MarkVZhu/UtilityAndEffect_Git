using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class StartRankUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel<RankInputPanel>("RankInputPanel");
    }

}
