using TMPro;
using UnityEngine;

public class ChatListItemView : MonoBehaviour
{
    [SerializeField] private AvatarView _avatar;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _lastMessage;
    [SerializeField] private TMP_Text _sentTime;

    [Space]
    [SerializeField] private GameObject _unreadIndicator;
    [SerializeField] private TMP_Text _unreadCount;

    private int _unreadCountValue;
    private Chat _chat;

    public Chat Chat => _chat;

    public void Render(Chat chat)
    {
        _chat = chat;
        _title.text = chat.Title;
        _lastMessage.text = chat.Messages.Count > 0 ? chat.Messages[^1].Text : "";
        _sentTime.text = chat.LastUpdatedAt.ToString("HH:mm");

        _unreadCountValue = 0;

        for (int i = chat.Messages.Count - 1; i >= 0; i--)
        {
            if (chat.Messages[i].ReadBy.Contains(ConnectionService.User.Id) == true || chat.Messages[i].Author.Id == ConnectionService.User.Id)
            {
                break;
            }

            _unreadCountValue++;
        }

        UpdateUnreadCount();

        _avatar.Load(chat.AvatarUrl);
    }

    private void UpdateUnreadCount()
    {
        _unreadIndicator.SetActive(_unreadCountValue > 0);
        
        if (_unreadCountValue > 0)
        {
            _unreadCount.text = _unreadCountValue.ToString();
        }
    }

    private void OnEnable()
    {
        ChatManager.OnMessageReceived += OnMessageReceived;
    }

    private void OnDisable()
    {
        ChatManager.OnMessageReceived -= OnMessageReceived;
    }

    private void OnMessageReceived(Message message)
    {
        if (message.ChatId != _chat.Id)
        {
            return;
        }
        
        _lastMessage.text = message.Text;
        _sentTime.text = Utils.GetTimeString(message.Sent);
        _unreadCountValue++;
        UpdateUnreadCount();
    }

    public void OnClick()
    {
        ChatManager.OpenChat(_chat.Id);
    }
}