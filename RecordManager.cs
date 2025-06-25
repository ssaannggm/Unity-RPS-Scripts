using UnityEngine;
using System.Collections.Generic;

public class RecordManager : MonoBehaviour
{
    private int winCount = 0;
    private int loseCount = 0;

    // 전체 플레이어 전적 (닉네임 기준)
    private Dictionary<string, (int win, int lose)> allRecords = new Dictionary<string, (int, int)>();

    public void Initialize()
    {
        winCount = 0;
        loseCount = 0;

        string nick = Photon.Pun.PhotonNetwork.NickName;
        if (!allRecords.ContainsKey(nick))
            allRecords[nick] = (0, 0);
    }

    public void AddWin()
    {
        string nick = Photon.Pun.PhotonNetwork.NickName;
        if (!allRecords.ContainsKey(nick))
            allRecords[nick] = (0, 0);

        var record = allRecords[nick];
        winCount = record.win + 1;
        loseCount = record.lose;
        allRecords[nick] = (winCount, loseCount);
    }


    public void AddLose()
    {
        string nick = Photon.Pun.PhotonNetwork.NickName;
        if (!allRecords.ContainsKey(nick))
            allRecords[nick] = (0, 0);

        var record = allRecords[nick];
        winCount = record.win;
        loseCount = record.lose + 1;
        allRecords[nick] = (winCount, loseCount);
    }


    public int GetWinCount()
    {
        string nick = Photon.Pun.PhotonNetwork.NickName;
        if (allRecords.ContainsKey(nick))
            return allRecords[nick].win;
        return 0;
    }

    public int GetLoseCount()
    {
        string nick = Photon.Pun.PhotonNetwork.NickName;
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
            default: break; // 무승부는 전적 미반영
        }
    }

}
