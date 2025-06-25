using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        string inputName = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            statusText.text = "�г����� �Է����ּ���!";
            return;
        }

        PhotonNetwork.NickName = inputName;

        // �г��ӿ� ���� ���� ��: ����� �� �ε�
        string lowerName = inputName.ToLower();
        if (lowerName.Contains("teacher") || lowerName.Contains("����"))
        {
            statusText.text = "����� ���� ���� ��...";
            PhotonNetwork.Disconnect(); // ���� ���� ���� ��
            StartCoroutine(LoadTeacherSceneAfterDisconnect());
            return;
        }

        // �Ϲ� �÷��̾�� ���� ��Ĵ�� �� ����
        statusText.text = "�� ���� �õ� ��...";
        PhotonNetwork.JoinRandomRoom();
    }

    private System.Collections.IEnumerator LoadTeacherSceneAfterDisconnect()
    {
        // ���� ������ ���� ������ ���
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene("TeacherScene");
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
