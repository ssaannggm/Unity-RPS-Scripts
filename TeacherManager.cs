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
        Debug.Log($"IsTeacher: {IsTeacher()}");
        teacherPanel.SetActive(IsTeacher());

        if (IsTeacher())
        {
            Debug.Log("Teacher mode ����");
            var recordManager = RecordManager.Instance;
            Debug.Log("RecordManager.Instance: " + (recordManager == null ? "NULL" : "NOT NULL"));

            if (recordManager != null)
            {
                var allData = recordManager.GetAllRecords();
                Debug.Log($"�� �÷��̾� ��� ��: {allData.Count}");
            }

            exportButton.onClick.AddListener(ExportToCSV);
            LoadAllPlayerRecords();
        }
    }


    bool IsTeacher()
    {
        var nick = PhotonNetwork.NickName.ToLower();
        return nick.Contains("teacher") || nick.Contains("����");
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
        var recordManager = RecordManager.Instance;
        if (recordManager == null)
        {
            Debug.LogWarning("RecordManager �ν��Ͻ��� ã�� �� �����ϴ�.");
            return;
        }

        var allData = recordManager.GetAllRecords();

        if (allData == null || allData.Count == 0)
        {
            Debug.Log("��ϵ� �÷��̾� �����Ͱ� �����ϴ�.");
            ClearPlayerRecordsUI();
            return;
        }

        ClearPlayerRecordsUI();

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

            var textComponent = entry.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
                textComponent.text = $"{record.nickname} - ��: {record.win} / ��: {record.lose}";

            playerRecords.Add(record);
        }

        SortAndAnimate();
    }

    void ClearPlayerRecordsUI()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    void SortAndAnimate()
    {
        playerRecords.Sort((a, b) => b.win.CompareTo(a.win));

        for (int i = 0; i < playerRecords.Count; i++)
        {
            var rt = playerRecords[i].uiObject.GetComponent<RectTransform>();
            rt.DOAnchorPos(new Vector2(0, -i * 100), 0.5f).SetEase(Ease.OutQuint);

            var text = playerRecords[i].uiObject.GetComponentInChildren<TMP_Text>();
            text.text = $"{i + 1}�� | {playerRecords[i].nickname} - ��: {playerRecords[i].win} / ��: {playerRecords[i].lose}";
        }
    }

    public void ExportToCSV()
    {
        var recordManager = RecordManager.Instance;
        if (recordManager == null)
        {
            Debug.LogWarning("RecordManager �ν��Ͻ��� ã�� �� �����ϴ�.");
            return;
        }

        var allData = recordManager.GetAllRecords();
        var path = Path.Combine(Application.persistentDataPath, "student_records.csv");

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("�г���,��,��");
            foreach (var kvp in allData)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value.win},{kvp.Value.lose}");
            }
        }

        Debug.Log($"CSV ���� �Ϸ�: {path}");
    }

    public void OnClick_ResetAllRecords()
    {
        var recordManager = RecordManager.Instance;
        if (recordManager == null)
        {
            Debug.LogWarning("RecordManager �ν��Ͻ��� ã�� �� �����ϴ�.");
            return;
        }

        recordManager.ResetAllRecords();
        LoadAllPlayerRecords();
    }
}
