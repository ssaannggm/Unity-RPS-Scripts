using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UIManager uiManager;
    public LogicManager logicManager;
    public NetworkManager networkManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        logicManager.SetDependencies(uiManager, networkManager);
        logicManager.Initialize();

        uiManager.SetLogicManager(logicManager);
        uiManager.Initialize();

        networkManager.Initialize();
    }
}
