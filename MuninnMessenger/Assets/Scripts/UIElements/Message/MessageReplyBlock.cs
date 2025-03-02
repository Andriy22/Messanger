using TMPro;
using UnityEngine;

public class MessageReplyBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _message;

    private long _messageId;

    public void Render(Message message)
    {
        _messageId = message.Id;
        _message.text = message.Text;
        _username.text = message.Author.Name;
    }

    public void OnClick()
    {
        ChatManager.GoToMessage(_messageId);
    }
}