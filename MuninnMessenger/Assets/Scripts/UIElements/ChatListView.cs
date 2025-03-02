using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChatListView : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private ChatListItemView _chatListItemPrefab;

    private Dictionary<long, ChatListItemView> _chatListItems = new ();

    public IEnumerator Load()
    {
        Render();
        yield break;
    }

    private void Render()
    {
        foreach (Transform item in _parent)
        {
            Destroy(item.gameObject);
        }

        foreach (var chat in ChatManager.Chats)
        {
            var chatListItem = Instantiate(_chatListItemPrefab, _parent);
            chatListItem.Render(chat);

            _chatListItems[chat.Id] = chatListItem;
            Reorder();
        }
    }

    private void Reorder()
    {
        var orderedChats = _chatListItems.Values.OrderBy(x => x.Chat.LastUpdatedAt);

        foreach (var item in orderedChats)
        {
            item.transform.SetAsFirstSibling();
        }
    }

    private void Awake()
    {
        ChatManager.OnMessageReceived += OnMessageReceived;
        ChatManager.OnChatAdd += OnChatAdd;
    }

    private void OnDestroy()
    {
        ChatManager.OnMessageReceived -= OnMessageReceived;
        ChatManager.OnChatAdd -= OnChatAdd;
    }

    private void OnChatAdd(Chat chat)
    {
        if(_chatListItems.ContainsKey(chat.Id) == false)
        {
            var chatListItem = Instantiate(_chatListItemPrefab, _parent);
            chatListItem.Render(chat);
            chatListItem.transform.SetAsFirstSibling();
            _chatListItems[chat.Id] = chatListItem;
        }
    }

    private void OnMessageReceived(Message message)
    {
        if(_chatListItems.ContainsKey(message.ChatId) == false)
        {
            var chatListItem = Instantiate(_chatListItemPrefab, _parent);
            _chatListItems[message.ChatId] = chatListItem;
        }

        _chatListItems[message.ChatId].Render(ChatManager.GetChat(message.ChatId));
        _chatListItems[message.ChatId].transform.SetAsFirstSibling();
    }
}