using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text statusText;

    public TMP_InputField nicknameInput;

    void Start()
    {
        statusText.text = "Photon ������ ���� ��...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "���� �Ϸ�! ���� ���� ��ư�� �����ּ���.";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_StartGame()
    {
        //  �г��� ���� �߰�
        string inputName = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(inputName))
        {
            statusText.text = "�г����� �Է����ּ���!";
            return; //  �Է� ������ ���� ���� ����
        }
        PhotonNetwork.NickName = inputName;

        statusText.text = "�� ���� �õ� ��...";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "�� ���� �� �� �� ���� ��...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "�� ���� �Ϸ�! ��� ��ٸ��� ��...";
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}

