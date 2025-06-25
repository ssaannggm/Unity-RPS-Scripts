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
            Debug.LogError("RecordManager 인스턴스가 존재하지 않습니다! 반드시 로비 씬에 배치해야 합니다.");
            return;
        }

        recordManager.Initialize();
        uiManager?.Initialize();
        logicManager?.Initialize();
        networkManager?.Initialize();
    }
}