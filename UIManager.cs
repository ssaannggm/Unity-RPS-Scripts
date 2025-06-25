using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text resultText;
    public TMP_Text winLoseText;
    public TMP_Text myNameText;
    public TMP_Text opponentNameText;
    public Button[] choiceButtons;
    public Button resetButton;

    public void Initialize()
    {
        myNameText.text = $"�� �̸� : {PhotonNetwork.NickName}";
        opponentNameText.text = "��� �̸� : ��� ��...";
        resetButton.interactable = false;
    }

    public void SetOpponentName(string name)
    {
        opponentNameText.text = $"��� �̸� : {name}";
    }

    public void OnClick_Choose(string choice)
    {
        GameManager.Instance.logicManager.SetMyChoice(choice);
    }

    public void OnClick_Reset()
    {
        GameManager.Instance.logicManager.ResetChoices();
    }

    public void SetResultText(string result)
    {
        resultText.text = result;
        resultText.transform.localScale = Vector3.zero;
        resultText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        resetButton.interactable = true;
    }

    public void SetWinLoseText(int win, int lose)
    {
        winLoseText.text = $"��: {win} | ��: {lose}";
    }

    public void ResetUI()
    {
        resultText.text = "";
        resultText.transform.localScale = Vector3.one;
        resetButton.interactable = false;

        foreach (var btn in choiceButtons)
            btn.interactable = true;
    }

    public void UpdateButtonState(string selectedChoice)
    {
        foreach (var btn in choiceButtons)
            btn.interactable = btn.name.Contains(selectedChoice);

        resetButton.interactable = false; // ���ø� ���� �� ��Ȱ��ȭ
    }
}
