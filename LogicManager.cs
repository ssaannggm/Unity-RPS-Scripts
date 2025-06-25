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

        if (result == "승리!")
            winCount++;
        else if (result == "패배...")
            loseCount++;

        // 결과 계산한 쪽만 결과 출력 + 리셋 버튼 표시
        ui.SetResultText(result, true);
        ui.SetWinLoseText(winCount, loseCount);
    }

    public void RequestReset()
    {
        ResetChoices();
        net.SendResetSignal(); // 상대도 강제 리셋됨
    }

    public void ReceiveResetRequestFromOpponent()
    {
        ResetChoices(); // 강제 리셋
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
            return "무승부!";

        if ((myChoice == "가위" && opponentChoice == "보") ||
            (myChoice == "바위" && opponentChoice == "가위") ||
            (myChoice == "보" && opponentChoice == "바위"))
            return "승리!";

        return "패배...";
    }
}
