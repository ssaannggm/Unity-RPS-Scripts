using UnityEngine;

public class LogicManager : MonoBehaviour
{
    private string myChoice = "";
    private string opponentChoice = "";

    public void Initialize()
    {
        ResetChoices();
    }

    public void SetMyChoice(string choice)
    {
        if (!string.IsNullOrEmpty(myChoice)) return;

        myChoice = choice;

        GameManager.Instance.uiManager.UpdateButtonState(choice);
        GameManager.Instance.networkManager.SendChoiceToOpponent(choice);

        TryCheckResult();
    }

    public void SetOpponentChoice(string choice)
    {
        opponentChoice = choice;
        TryCheckResult();
    }

    public void ResetChoices()
    {
        myChoice = "";
        opponentChoice = "";
        GameManager.Instance.uiManager.ResetUI();
    }

    private void TryCheckResult()
    {
        if (string.IsNullOrEmpty(myChoice) || string.IsNullOrEmpty(opponentChoice))
            return;

        var result = GetMatchResult();

        GameManager.Instance.uiManager.SetResultText(result);
        GameManager.Instance.recordManager.AddResult(result);
        GameManager.Instance.uiManager.SetWinLoseText(
            GameManager.Instance.recordManager.GetWinCount(),
            GameManager.Instance.recordManager.GetLoseCount()
        );
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
