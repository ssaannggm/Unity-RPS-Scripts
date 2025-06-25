using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyUIController ui;

    private void Start()
    {
        ui.SetStatus("Photon 서버에 연결 중...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ui.SetStatus("연결 완료! 닉네임을 입력하고 시작하세요.");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_StartGame()
    {
        string nickname = ui.GetNickname();
        if (string.IsNullOrEmpty(nickname))
        {
            ui.SetStatus("닉네임을 입력해주세요!");
            return;
        }

        PhotonNetwork.NickName = nickname;
        ui.SetStatus("룸 입장 시도 중...");
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClick_TeacherMode()
    {
        ui.SetStatus("교사용 모드로 진입 중...");
        PhotonNetwork.Disconnect(); // 일단 호출만, 실패해도 무방
        SceneManager.LoadScene("TeacherScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ui.SetStatus("룸 없음 → 새 룸 생성 중...");
        string roomName = "Room_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        ui.SetStatus("룸 입장 완료! 상대 기다리는 중...");
        TryStartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TryStartGame();
    }

    private void TryStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
