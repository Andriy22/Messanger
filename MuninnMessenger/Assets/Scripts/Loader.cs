using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private Theme _theme;
    [SerializeField] private MessageFactory _messageFactory;
    [SerializeField] private ChatManager _chatManager;
    [SerializeField] private ChatListView _chatListView;
    [SerializeField] private GameObject _mainCanvas;

    private IEnumerator Start()
    {
        _theme.SetCurrent();
        
        yield return _messageFactory.Load();
        yield return _chatManager.Load();
        yield return _chatListView.Load();

        _mainCanvas.SetActive(true);
    }
}