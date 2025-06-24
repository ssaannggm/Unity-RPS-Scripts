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
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        recordManager.Initialize(reset: true);
        uiManager.Initialize();
        logicManager.Initialize();
        networkManager.Initialize();
    }
}