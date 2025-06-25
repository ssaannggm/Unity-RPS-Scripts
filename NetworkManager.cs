using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private UIManager ui;
    private LogicManager logic;

    public void Initialize()
    {
        ui = GameManager.Instance.uiManager;
        logic = GameManager.Instance.logicManager;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                ui.SetOpponentName(player.NickName);
                break;
            }
        }
    }
    public void SendResetSignal()
    {
        photonView.RPC("ReceiveResetSignal", RpcTarget.Others);
    }
    public void SendResultToOpponent(string result)
    {
        photonView.RPC("ReceiveResultText", RpcTarget.Others, result);
    }

    [PunRPC]
    public void ReceiveResetSignal()
    {
        GameManager.Instance.logicManager.ReceiveResetRequestFromOpponent();
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
