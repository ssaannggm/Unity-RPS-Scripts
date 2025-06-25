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

    private LogicManager logic;

    public void Initialize()
    {
        myNameText.text = $"내 이름 : {PhotonNetwork.NickName}";
        opponentNameText.text = "상대 이름 : 대기 중...";
        resetButton.interactable = false;

        foreach (var btn in choiceButtons)
        {
            string choice = btn.name;
            btn.onClick.AddListener(() => OnClick_Choose(choice));
        }

        resetButton.onClick.AddListener(OnClick_Reset);
        resetButton.gameObject.SetActive(false); // 처음엔 비활성화
    }

    public void SetLogicManager(LogicManager logicManager)
    {
        this.logic = logicManager;
    }

    private void OnClick_Choose(string choice)
    {
        logic.SetMyChoice(choice);
    }

    private void OnClick_Reset()
    {
        logic.RequestReset(); // 결과 계산한 사람만 눌 수 있음
    }

    public void SetOpponentName(string name)
    {
        opponentNameText.text = $"상대 이름 : {name}";
    }

    public void SetResultText(string result, bool showResetButton)
    {
        resultText.text = result;
        resultText.transform.localScale = Vector3.zero;
        resultText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        resetButton.interactable = showResetButton;
        resetButton.gameObject.SetActive(showResetButton); // 승자만 보이게
    }

    public void ResetUI()
    {
        resultText.text = "";
        resultText.transform.localScale = Vector3.one;
        resetButton.interactable = false;
        resetButton.gameObject.SetActive(false);

        foreach (var btn in choiceButtons)
            btn.interactable = true;
    }

    public void UpdateButtonState(string selectedChoice)
    {
        foreach (var btn in choiceButtons)
            btn.interactable = btn.name.Contains(selectedChoice);

        resetButton.interactable = false;
    }

    public void SetWinLoseText(int win, int lose)
    {
        winLoseText.text = $"승: {win} | 패: {lose}";
    }
}
