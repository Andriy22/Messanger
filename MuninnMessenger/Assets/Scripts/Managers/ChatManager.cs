using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    private static ChatManager _instance;

    [SerializeField] private Transform _parent;
    [SerializeField] private ChatListView _chatList;

    [Space]
    [SerializeField] private ChatView _chatViewPrefab;

    private Dictionary<long, Chat> _chats = new();
    private Dictionary<Chat, ChatView> _chatViews = new();

    public static IReadOnlyList<Chat> Chats => _instance._chats.Values.ToList();
    public static event Action<Message> OnMessageReceived;
    public static event Action<Chat> OnChatAdd;

    public static Chat GetChat(long chatId) => _instance._chats[chatId];

    public IEnumerator Load()
    {
        _instance = this;

        foreach (var chat in Mocks.CHATS)
        {
            _chats[chat.Id] = chat;
        }

        yield break;
    }

    public static void GoToMessage(long messageId)
    {
        OpenChat(Message.GetMessage(messageId).ChatId);
        var chat = _instance._chats[Message.GetMessage(messageId).ChatId];
        var chatView = _instance._chatViews[chat];

        foreach (var item in _instance._chatViews)
        {
            item.Value.gameObject.SetActive(item.Key == chat);
        }

        chatView.ScrollTo(messageId);
    }

    public static void OpenChat(long chatId)
    {
        if (_instance._chats.TryGetValue(chatId, out var chat) == false)
        {
            return;
        }

        if (_instance._chatViews.TryGetValue(chat, out var chatView) == false)
        {
            chatView = Instantiate(_instance._chatViewPrefab, _instance._parent);
            _instance._chatViews[chat] = chatView;

            chatView.Render(chat);
        }

        foreach (var item in _instance._chatViews)
        {
            item.Value.gameObject.SetActive(item.Key == chat);
        }

        _instance._chatList.gameObject.SetActive(false);
    }

    public static void OpenChatList()
    {
        foreach (var item in _instance._chatViews)
        {
            item.Value.gameObject.SetActive(false);
        }

        _instance._chatList.gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    [ContextMenu("Simulate Message")]
    public void SimulateMessage()
    {
        OnMessageReceived?.Invoke(Mocks.GetNewMessage());
    }

    [ContextMenu("Simulate ChatAdd")]
    public void SimulateChatAdd()
    {
        var chat = Mocks.GetNewChat();
        _chats[chat.Id] = chat;

        OnChatAdd?.Invoke(chat);
    }
#endif
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ChatManager))]
public class ChatManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var chatManager = target as ChatManager;
        base.OnInspectorGUI();

        UnityEditor.EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Simulate Message"))
        {
            chatManager.SimulateMessage();
        }

        if (GUILayout.Button("+10", GUILayout.Width(50)))
        {
            for (int i = 0; i < 10; i++)
            {
                chatManager.SimulateMessage();
            }
        }

        if (GUILayout.Button("+100", GUILayout.Width(50)))
        {
            for (int i = 0; i < 100; i++)
            {
                chatManager.SimulateMessage();
            }
        }

        UnityEditor.EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Simulate ChatAdd"))
        {
            chatManager.SimulateChatAdd();
        }
    }
}
#endif