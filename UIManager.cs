using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text winLoseText;
    public TMP_Text myNameText;
    public TMP_Text opponentNameText;
    public Button[] choiceButtons;
    public Button resetButton;

    public void Initialize()
    {
        myNameText.text = $"내 이름 : {Photon.Pun.PhotonNetwork.NickName}";
        opponentNameText.text = "상대 이름 : 대기 중...";
        resetButton.interactable = false; // 시작 시 비활성화
    }

    public void SetOpponentName(string name)
    {
        opponentNameText.text = $"상대 이름 : {name}";
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
        resultText.transform.localScale = Vector3.zero;
        resultText.text = result;
        resultText.transform.DOScale(Vector3.one, 0.5f).SetEase(DG.Tweening.Ease.OutBack);
        resetButton.interactable = true; // 결과가 나왔을 때만 리셋 버튼 활성화
    }

    public void SetWinLoseText(int win, int lose)
    {
        winLoseText.text = $"승: {win} | 패: {lose}";
    }

    public void ResetUI()
    {
        resultText.text = "";
        resultText.transform.localScale = Vector3.one;
        resetButton.interactable = false; // 리셋되면 다시 비활성화

        foreach (Button btn in choiceButtons)
            btn.interactable = true;
    }

    public void UpdateButtonState(string selectedChoice)
    {
        foreach (Button btn in choiceButtons)
        {
            btn.interactable = btn.name.Contains(selectedChoice);
        }
        resetButton.interactable = false; // 선택 시점엔 리셋 비활성화
    }
}