using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using LitJson;
using System;
using UnityEngine.Events;

namespace MarkFramework
{
    public class PlayerData
    {
        public int rankNum;
        public string playerName;
        public int score;
    }

    public class RankMgr: SingletonMono<RankMgr>
    {
        public bool isOnlineRank;
        public string serverUrl = "http://124.221.195.228/api.php";
        private const string FileName = "PlayerData"; // JSON file name without extension
        [Tooltip("服务器文件最大容量为100")]
        public int rankRange = 50; // 默认排名范围为50
        public List<PlayerData> updatedPlayerList;

        public List<PlayerData> LoadPlayerDataLocal()
        {
            // 使用 JsonMgr 读取数据
            List<PlayerData> playersList = JsonMgr.Instance.LoadData<List<PlayerData>>(FileName, JsonType.LitJson);

            // 如果文件为空或不存在，返回空列表
            if (playersList == null || playersList.Count == 0)
            {
                Debug.LogWarning("No player data found, returning an empty list.");
                return new List<PlayerData>();
            }

            // 按 rankNum 排序后返回
            playersList.Sort((a, b) => a.rankNum.CompareTo(b.rankNum));
            return playersList;
        }
        
         // 从服务器加载玩家数据
        public void LoadPlayerDataOnline(UnityAction<List<PlayerData>> callback)
        {
            StartCoroutine(LoadPlayerDataCoroutine(callback));
        }
        
        // 协程：从服务器加载玩家数据
        private IEnumerator LoadPlayerDataCoroutine(UnityAction<List<PlayerData>> callback)
        {
            // 创建 GET 请求
            using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
            {
                // 发送请求
                yield return request.SendWebRequest();
                
                // 打印请求结果
                Debug.Log("Request result: " + request.result);
                Debug.Log("Response code: " + request.responseCode);
                Debug.Log("Response text: " + request.downloadHandler.text);

                // 检查请求结果
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error loading player data: " + request.error);
                }
                else
                {
                    // 解析服务器返回的 JSON 数据
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Raw JSON response: " + jsonResponse);
                    List<PlayerData> playersList = JsonMapper.ToObject<List<PlayerData>>(jsonResponse);

                    // 打印加载的数据
                    foreach(PlayerData p in playersList){
                        Debug.Log("Online Player data loaded: " + p.playerName + ", Rank: " + p.rankNum + ", Score: " + p.score + "\n");
                    }
                    callback(playersList);
                }
            }
        }
        
        //本地写入直接把全部playersList按顺序写入
        public void WritePlayerDataLocal(List<PlayerData> playersList)
        {
            // 使用 JsonMgr 保存数据
            JsonMgr.Instance.SaveData(playersList, FileName, JsonType.LitJson);
            Debug.Log("Player data successfully saved using JsonMgr.");
        }
        
        // 将玩家数据上传到服务器 上传单个PlayerData
        public void WritePlayerDataOnline(PlayerData playerData)
        {
            StartCoroutine(WritePlayerDataCoroutine(playerData));
        }
        // 协程：将玩家数据上传到服务器
        private IEnumerator WritePlayerDataCoroutine(PlayerData playerData)
        {
            // 将玩家数据转换为 JSON 字符串
            string jsonData = JsonMgr.Instance.SerializeData(playerData, JsonType.LitJson);
            

            // 创建 POST 请求
            using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
            {
                // 设置请求头和请求体
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                // 发送请求
                yield return request.SendWebRequest();

                // 检查请求结果
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error writing player data: " + request.error);
                }
                else
                {
                    Debug.Log("Player data uploaded successfully!");
                }
            }
        }

        public List<PlayerData> InsertPlayerData(PlayerData playerData, List<PlayerData> playersList)
        {
            // 比较 playerData 的 score，确定其排名
            int rank = 1;
            foreach (var player in playersList)
            {
                if (playerData.score < player.score)
                {
                    rank++;
                }
                else
                {
                    break;
                }
            }

            playerData.rankNum = rank;

            // 插入新玩家
            playersList.Insert(rank - 1, playerData);

            // 更新所有玩家的 rankNum
            for (int i = 0; i < playersList.Count; i++)
            {
                playersList[i].rankNum = i + 1;
            }

            // 移除超过 rankRange 范围的玩家
            if (playersList.Count > rankRange)
            {
                playersList.RemoveRange(rankRange, playersList.Count - rankRange);
            }

            return playersList;
        }
    }
}
