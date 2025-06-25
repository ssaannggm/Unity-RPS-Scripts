using UnityEngine;

public class LogicManager : MonoBehaviour
{
    private string myChoice = "";
    private string opponentChoice = "";

    private int winCount = 0;
    private int loseCount = 0;

    private UIManager ui;
    private NetworkManager net;

    public void SetDependencies(UIManager ui, NetworkManager net)
    {
        this.ui = ui;
        this.net = net;
    }

    public void Initialize()
    {
        winCount = 0;
        loseCount = 0;
        ResetChoices();
    }

    public void SetMyChoice(string choice)
    {
        if (!string.IsNullOrEmpty(myChoice)) return;

        myChoice = choice;
        ui.UpdateButtonState(choice);
        net.SendChoiceToOpponent(choice);
        TryCheckResult();
    }

    public void SetOpponentChoice(string choice)
    {
        opponentChoice = choice;
        TryCheckResult();
    }

    private void TryCheckResult()
    {
        if (string.IsNullOrEmpty(myChoice) || string.IsNullOrEmpty(opponentChoice))
            return;

        string result = GetMatchResult();

        if (result == "�¸�!")
            winCount++;
        else if (result == "�й�...")
            loseCount++;

        // ��� ����� �ʸ� ��� ��� + ���� ��ư ǥ��
        ui.SetResultText(result, true);
        ui.SetWinLoseText(winCount, loseCount);
    }

    public void RequestReset()
    {
        ResetChoices();
        net.SendResetSignal(); // ��뵵 ���� ���µ�
    }

    public void ReceiveResetRequestFromOpponent()
    {
        ResetChoices(); // ���� ����
    }

    private void ResetChoices()
    {
        myChoice = "";
        opponentChoice = "";
        ui.ResetUI();
    }

    private string GetMatchResult()
    {
        if (myChoice == opponentChoice)
            return "���º�!";

        if ((myChoice == "����" && opponentChoice == "��") ||
            (myChoice == "����" && opponentChoice == "����") ||
            (myChoice == "��" && opponentChoice == "����"))
            return "�¸�!";

        return "�й�...";
    }
}
