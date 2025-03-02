using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MessageFactory : MonoBehaviour
{
    private static MessageFactory _instance;

    [SerializeField] private MessageView _inMessagePrefab;
    [SerializeField] private MessageView _outMessagePrefab;

    [Space]
    [SerializeField] private MessageImageBlock _imageBlockPrefab;
    [SerializeField] private MessageReplyBlock _replyBlockPrefab;
    [SerializeField] private ImageScaler _imagePrefab;

    public IEnumerator Load()
    {
        _instance = this;
        yield break;
    }

    public static MessageView Create(Message message, bool showAvatar = true)
    {
        return _instance.CreateInternal(message, showAvatar);
    }

    private MessageView CreateInternal(Message message, bool showAvatar = true)
    {
        var instance = Instantiate(message.Author == ConnectionService.User
            ? _outMessagePrefab
            : _inMessagePrefab);

        instance.Render(message, showAvatar && message.Author != ConnectionService.User);

        if(message.Attachments != null && message.Attachments.Count > 0)
        {
            var messageImageBlock = Instantiate(_imageBlockPrefab);
            instance.Prepend(messageImageBlock.transform as RectTransform);

            foreach (var attachment in message.Attachments)
            {
                var block = Instantiate(_imagePrefab);
                block.LoadAsync(attachment.Url);
                messageImageBlock.Append(block);
            }
        }

        if(message.ReplyTo != null)
        {
            var messageReplyBlock = Instantiate(_replyBlockPrefab);
            instance.Prepend(messageReplyBlock.transform as RectTransform);
            messageReplyBlock.Render(Message.GetMessage(message.ReplyTo.Value));
        }

        return instance;
    }
}