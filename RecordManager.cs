using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class RecordManager : MonoBehaviour
{
    public static RecordManager Instance { get; private set; }

    private int winCount = 0;
    private int loseCount = 0;

    private Dictionary<string, (int win, int lose)> allRecords = new Dictionary<string, (int, int)>();

    private void Awake()
    {
        // 싱글톤 패턴: 이미 존재하면 파괴, 아니면 유지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        winCount = 0;
        loseCount = 0;

        string nick = PhotonNetwork.NickName;
        if (!allRecords.ContainsKey(nick))
            allRecords[nick] = (0, 0);
    }

    public void AddWin()
    {
        string nick = Photon.Pun.PhotonNetwork.NickName;
        Debug.Log($"AddWin 호출 - 닉네임: {nick}");
        if (!allRecords.ContainsKey(nick))
        {
            allRecords[nick] = (0, 0);
            Debug.Log($"닉네임 {nick} 새로운 기록 생성");
        }

        var record = allRecords[nick];
        winCount = record.win + 1;
        loseCount = record.lose;
        allRecords[nick] = (winCount, loseCount);

        Debug.Log($"현재 승리 기록: {winCount}");
    }


    public void AddLose()
    {
        string nick = PhotonNetwork.NickName;
        if (!allRecords.ContainsKey(nick))
            allRecords[nick] = (0, 0);

        var record = allRecords[nick];
        winCount = record.win;
        loseCount = record.lose + 1;
        allRecords[nick] = (winCount, loseCount);

        Debug.Log($"{nick} 패배 기록: {loseCount}패");
    }

    public int GetWinCount()
    {
        string nick = PhotonNetwork.NickName;
        if (allRecords.ContainsKey(nick))
            return allRecords[nick].win;
        return 0;
    }

    public int GetLoseCount()
    {
        string nick = PhotonNetwork.NickName;
        if (allRecords.ContainsKey(nick))
            return allRecords[nick].lose;
        return 0;
    }

    public Dictionary<string, (int win, int lose)> GetAllRecords()
    {
        return new Dictionary<string, (int win, int lose)>(allRecords);
    }

    public void ResetAllRecords()
    {
        allRecords.Clear();
    }

    public void AddResult(string result)
    {
        switch (result)
        {
            case "승리!": AddWin(); break;
            case "패배...": AddLose(); break;
            default: break; // 무승부는 기록 안 함
        }
    }
}
