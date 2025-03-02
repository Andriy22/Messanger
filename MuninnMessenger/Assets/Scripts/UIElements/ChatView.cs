using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatView : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollView;
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _downButton;

    [Space]
    [SerializeField] private AvatarView _chatAvatar;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;

    private float _scrollDelta = 0;
    private float _prevPosition = 0;
    private int _currentMessageIndex = 0;
    private Dictionary<long, MessageView> _messages = new();
    private Chat _chat;

    private void OnEnable()
    {
        _scrollView.onValueChanged.AddListener(OnScroll);
    }

    private void OnDisable()
    {
        _scrollView.onValueChanged.RemoveListener(OnScroll);
    }

    private void OnScroll(Vector2 position)
    {
        _scrollDelta = _prevPosition - position.y;
        _prevPosition = position.y;

        // var searchDirection = _scrollDelta > 0 ? 1 : -1;
        var minDistance = float.MaxValue;
        // for(int i = Mathf.Clamp(_currentMessageIndex - searchDirection, 0, _messages.Count - 1); i >= 0 && i < _messages.Count; i += searchDirection)
        // {
        //     var message = _messages.ElementAt(i).Value;
        //     // var rect = message.transform as RectTransform;

        //     // if(_scrollView.GetSnapPosition(rect).y > _scrollView.content.localPosition.y + _scrollView.viewport.rect.height)
        //     // {
        //     //     _currentMessageIndex = i;
        //     //     break;
        //     // }

        //     if(Mathf.Abs(message.transform.position.y - _scrollView.transform.position.y) < minDistance)
        //     {
        //         minDistance = Mathf.Abs(message.transform.localPosition.y - _scrollView.content.localPosition.y);
        //         _currentMessageIndex = i;
        //         continue;
        //     }

        //     break;
        // }

        for (int i = 0; i < _messages.Count; i++)
        {
            var message = _messages.ElementAt(i).Value;
            var rect = message.transform as RectTransform;

            float distance = Mathf.Abs(message.transform.position.y - _scrollView.transform.position.y);
            if (distance < minDistance)
            {
                _currentMessageIndex = i;
                minDistance = distance;
                continue;
            }

            break;
        }

        Debug.Log(_currentMessageIndex);
    }

    private void Update()
    {
        _downButton.localScale = Vector3.MoveTowards(_downButton.localScale, (_scrollView.content.localPosition.y + 100) < _scrollView.content.sizeDelta.y
            ? Vector3.one : Vector3.zero, 32f * Time.deltaTime);
    }

    public void Render(Chat chat)
    {
        _chat = chat;

        foreach (Transform item in _parent)
        {
            Destroy(item.gameObject);
        }

        _messages = new Dictionary<long, MessageView>();

        _title.text = chat.Title;
        _description.text = chat.Participants.Count > 2 ? $"{chat.Participants.Count} participants" : "online";

        foreach (var item in chat.Messages)
        {
            _messages[item.Id] = MessageFactory.Create(item, chat.Participants.Count > 2);
            _messages[item.Id].transform.SetParent(_parent, false);
        }

        ScrollDown();
        _chatAvatar.Load(chat.AvatarUrl);
    }

    public void ScrollDown(bool blink = false)
    {
        if (_messages.Count == 0)
        {
            return;
        }

        ScrollTo(_messages.Last().Key, blink);
    }

    public void ScrollTo(long id, bool blink = true)
    {
        Canvas.ForceUpdateCanvases();

        _scrollView.DOKill();
        var sequence = DOTween.Sequence();
        sequence.Append(_scrollView.content.DOLocalMove(_scrollView.GetSnapPosition(_messages[id].transform as RectTransform), 0.25f));

        if (blink)
        {
            sequence.AppendCallback(() => _messages[id].Blink());
        }

        sequence.SetTarget(_scrollView);
    }

    public void BackButtonClick()
    {
        ChatManager.OpenChatList();
    }

    public void HandleNewMessage(Message message)
    {
        if (message.ChatId != _chat.Id)
        {
            return;
        }

        var messageView = MessageFactory.Create(message);
        messageView.transform.SetParent(_parent, false);
        messageView.Appear();

        _messages[message.Id] = messageView;

        if (gameObject.activeInHierarchy)
        {
            ScrollDown(true);
        }
        else
        {
            _scrollView.content.localPosition = _scrollView.GetSnapPosition(_messages[_currentMessageIndex].transform as RectTransform);
        }
    }

    private void Awake()
    {
        ChatManager.OnMessageReceived += HandleNewMessage;
    }

    private void OnDestroy()
    {
        ChatManager.OnMessageReceived -= HandleNewMessage;
    }
}
