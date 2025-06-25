using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyUIController ui;

    private void Start()
    {
        ui.SetStatus("Photon ������ ���� ��...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ui.SetStatus("���� �Ϸ�! �г����� �Է��ϰ� �����ϼ���.");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_StartGame()
    {
        string nickname = ui.GetNickname();
        if (string.IsNullOrEmpty(nickname))
        {
            ui.SetStatus("�г����� �Է����ּ���!");
            return;
        }

        PhotonNetwork.NickName = nickname;
        ui.SetStatus("�� ���� �õ� ��...");
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClick_TeacherMode()
    {
        ui.SetStatus("����� ���� ���� ��...");
        PhotonNetwork.Disconnect(); // �ϴ� ȣ�⸸, �����ص� ����
        SceneManager.LoadScene("TeacherScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ui.SetStatus("�� ���� �� �� �� ���� ��...");
        string roomName = "Room_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        ui.SetStatus("�� ���� �Ϸ�! ��� ��ٸ��� ��...");
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
