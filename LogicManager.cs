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
            return "���º�!";

        if ((myChoice == "����" && opponentChoice == "��") ||
            (myChoice == "����" && opponentChoice == "����") ||
            (myChoice == "��" && opponentChoice == "����"))
            return "�¸�!";

        return "�й�...";
    }
}
