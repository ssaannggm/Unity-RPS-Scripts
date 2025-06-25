using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UIManager uiManager;
    public LogicManager logicManager;
    public NetworkManager networkManager;
    public RecordManager recordManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (recordManager == null)
        {
            recordManager = RecordManager.Instance;
        }
    }


    void Start()
    {
        if (recordManager == null)
        {
            Debug.LogError("RecordManager �ν��Ͻ��� �������� �ʽ��ϴ�! �ݵ�� �κ� ���� ��ġ�ؾ� �մϴ�.");
            return;
        }

        recordManager.Initialize();
        uiManager?.Initialize();
        logicManager?.Initialize();
        networkManager?.Initialize();
    }
}