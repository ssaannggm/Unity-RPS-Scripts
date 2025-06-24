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
            result = "���º�!";
        else if ((myChoice == "����" && opponentChoice == "��") ||
                 (myChoice == "����" && opponentChoice == "����") ||
                 (myChoice == "��" && opponentChoice == "����"))
        {
            result = "�¸�!";
            GameManager.Instance.recordManager.AddWin();
        }
        else
        {
            result = "�й�...";
            GameManager.Instance.recordManager.AddLose();
        }

        GameManager.Instance.uiManager.SetResultText(result);
        GameManager.Instance.uiManager.SetWinLoseText(
            GameManager.Instance.recordManager.GetWinCount(),
            GameManager.Instance.recordManager.GetLoseCount()
        );
    }
}