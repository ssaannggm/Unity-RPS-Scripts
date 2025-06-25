using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class TeacherManager : MonoBehaviour
{
    public GameObject teacherPanel;
    public GameObject playerEntryPrefab;
    public Transform contentParent;
    public Button exportButton;

    private List<PlayerRecord> playerRecords = new List<PlayerRecord>();

    void Start()
    {
        teacherPanel.SetActive(IsTeacher());

        if (IsTeacher())
        {
            exportButton.onClick.AddListener(ExportToCSV);
            LoadAllPlayerRecords();
        }
    }

    bool IsTeacher()
    {
        var nick = PhotonNetwork.NickName.ToLower();
        return nick.Contains("teacher") || nick.Contains("교사");
    }

    [System.Serializable]
    public class PlayerRecord
    {
        public string nickname;
        public int win;
        public int lose;
        public GameObject uiObject;
    }

    public void LoadAllPlayerRecords()
    {
        if (GameManager.Instance == null || GameManager.Instance.recordManager == null)
        {
            Debug.LogWarning("GameManager 또는 RecordManager가 할당되어 있지 않습니다.");
            return;
        }

        var allData = GameManager.Instance.recordManager.GetAllRecords();

        if (allData == null || allData.Count == 0)
        {
            Debug.Log("기록된 플레이어 데이터가 없습니다.");
            // UI 초기화 또는 '데이터 없음' 메시지 띄우기 등 처리 가능
            return;
        }

        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        playerRecords.Clear();


        foreach (var kvp in allData)
        {
            var record = new PlayerRecord
            {
                nickname = kvp.Key,
                win = kvp.Value.win,
                lose = kvp.Value.lose
            };

            GameObject entry = Instantiate(playerEntryPrefab, contentParent);
            record.uiObject = entry;

            entry.GetComponentInChildren<TMP_Text>().text =
                $"{record.nickname} - 승: {record.win} / 패: {record.lose}";

            playerRecords.Add(record);
        }

        SortAndAnimate();
    }

    void SortAndAnimate()
    {
        playerRecords.Sort((a, b) => b.win.CompareTo(a.win));

        for (int i = 0; i < playerRecords.Count; i++)
        {
            var rt = playerRecords[i].uiObject.GetComponent<RectTransform>();
            rt.DOAnchorPos(new Vector2(0, -i * 100), 0.5f).SetEase(Ease.OutQuint);

            var text = playerRecords[i].uiObject.GetComponentInChildren<TMP_Text>();
            text.text = $"{i + 1}위 | {playerRecords[i].nickname} - 승: {playerRecords[i].win} / 패: {playerRecords[i].lose}";
        }
    }

    public void ExportToCSV()
    {
        var path = Path.Combine(Application.persistentDataPath, "student_records.csv");
        var allData = GameManager.Instance.recordManager.GetAllRecords();

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("닉네임,승,패");
            foreach (var kvp in allData)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value.win},{kvp.Value.lose}");
            }
        }

        Debug.Log("CSV 저장 완료: " + path);
    }

    public void OnClick_ResetAllRecords()
    {
        GameManager.Instance.recordManager.ResetAllRecords();
        LoadAllPlayerRecords();
    }
}
