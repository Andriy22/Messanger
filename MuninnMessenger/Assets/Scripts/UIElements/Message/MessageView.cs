using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private GameObject _messageBlock;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private TMP_Text _username;
    [SerializeField] private DynamicTextBox _textBox;
    [SerializeField] private AvatarView _avatar;
    [SerializeField] private List<RectTransform> _copyX = new ();

    public void Render(string text, string avatarUrl, bool showAvatar = true)
    {
        _textBox.Text = text;
        _avatar.gameObject.SetActive(showAvatar);
        _username.gameObject.SetActive(showAvatar);

        _messageBlock.SetActive(string.IsNullOrWhiteSpace(text) == false);

        StartCoroutine(AdjustTexts());
        _avatar.Load(avatarUrl);
    }

    private IEnumerator AdjustTexts()
    {
        yield return new WaitForEndOfFrame();
        foreach (var item in _copyX)
        {
            item.sizeDelta = new Vector2(_parent.sizeDelta.x, item.sizeDelta.y);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AdjustTexts());
    }

    public void Render(Message item, bool showAvatar = true)
    {
        _time.text = Utils.GetTimeString(item.Sent);
        _username.text = item.Author.Name;

        Render(item.Text, item.Author.AvatarUrl, showAvatar);
    }

    public void Append(RectTransform block)
    {
        block.SetParent(_parent, false);
    }

    public void Prepend(RectTransform block)
    {
        block.SetParent(_parent, false);
        block.SetSiblingIndex(1);
    }

    public void Blink()
    {
        _background.DOKill();
        var sequence = DOTween.Sequence();
        sequence.Append(_background.DOColor(new Color(1, 1, 1, 0.5f), 0.25f));
        sequence.Append(_background.DOColor(new Color(1, 1, 1, 0), 0.25f));
        sequence.SetTarget(_background);
    }

    public void Appear()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.666f).SetEase(Ease.OutBack);
    }
}