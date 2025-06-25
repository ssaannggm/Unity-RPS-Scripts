using UnityEngine;
using TMPro;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_InputField nicknameInput;

    public void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    public string GetNickname()
    {
        return nicknameInput != null ? nicknameInput.text.Trim() : string.Empty;
    }
}
