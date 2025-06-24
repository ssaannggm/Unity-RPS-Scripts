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
        GameManager.Instance.networkManager.SendChoiceToOpponent(choice);
        CheckResult();

        GameManager.Instance.uiManager.UpdateButtonState(choice);
    }

    public void SetOpponentChoice(string choice)
    {
        opponentChoice = choice;
        CheckResult();
    }

    public void ResetChoices()
    {
        myChoice = "";
        opponentChoice = "";
        GameManager.Instance.uiManager.ResetUI();
    }

    void CheckResult()
    {
        if (string.IsNullOrEmpty(myChoice) || string.IsNullOrEmpty(opponentChoice)) return;

        string result = "";
        if (myChoice == opponentChoice)
            result = "무승부!";
        else if ((myChoice == "가위" && opponentChoice == "보") ||
                 (myChoice == "바위" && opponentChoice == "가위") ||
                 (myChoice == "보" && opponentChoice == "바위"))
        {
            result = "승리!";
            GameManager.Instance.recordManager.AddWin();
        }
        else
        {
            result = "패배...";
            GameManager.Instance.recordManager.AddLose();
        }

        GameManager.Instance.uiManager.SetResultText(result);
        GameManager.Instance.uiManager.SetWinLoseText(
            GameManager.Instance.recordManager.GetWinCount(),
            GameManager.Instance.recordManager.GetLoseCount()
        );
    }
}