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
        statusText.text = "Photon 서버에 연결 중...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "연결 완료! 게임 시작 버튼을 눌러주세요.";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_StartGame()
    {
        string inputName = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            statusText.text = "닉네임을 입력해주세요!";
            return;
        }

        PhotonNetwork.NickName = inputName;

        // 닉네임에 교사 포함 시: 교사용 씬 로드
        string lowerName = inputName.ToLower();
        if (lowerName.Contains("teacher") || lowerName.Contains("교사"))
        {
            statusText.text = "교사용 모드로 진입 중...";
            PhotonNetwork.Disconnect(); // 포톤 연결 해제 후
            StartCoroutine(LoadTeacherSceneAfterDisconnect());
            return;
        }

        // 일반 플레이어는 기존 방식대로 룸 입장
        statusText.text = "룸 입장 시도 중...";
        PhotonNetwork.JoinRandomRoom();
    }

    private System.Collections.IEnumerator LoadTeacherSceneAfterDisconnect()
    {
        // 포톤 연결이 끊길 때까지 대기
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene("TeacherScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "룸 없음 → 새 룸 생성 중...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "룸 입장 완료! 상대 기다리는 중...";
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
