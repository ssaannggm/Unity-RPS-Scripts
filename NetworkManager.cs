using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public void Initialize()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                GameManager.Instance.uiManager.SetOpponentName(player.NickName);
                break;
            }
        }
    }

    public void SendChoiceToOpponent(string choice)
    {
        photonView.RPC("ReceiveChoice", RpcTarget.Others, choice);
    }

    [PunRPC]
    public void ReceiveChoice(string choice)
    {
        GameManager.Instance.logicManager.SetOpponentChoice(choice);
    }
}