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
        //  닉네임 설정 추가
        string inputName = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(inputName))
        {
            statusText.text = "닉네임을 입력해주세요!";
            return; //  입력 없으면 게임 시작 막기
        }
        PhotonNetwork.NickName = inputName;

        statusText.text = "룸 입장 시도 중...";
        PhotonNetwork.JoinRandomRoom();
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

