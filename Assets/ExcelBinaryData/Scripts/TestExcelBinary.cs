using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExcelBinary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BinaryDataMgr.Instance.InitData();


        TowerInfoContainer data = BinaryDataMgr.Instance.GetTable<TowerInfoContainer>();
        print(data.dataDic[5].name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
