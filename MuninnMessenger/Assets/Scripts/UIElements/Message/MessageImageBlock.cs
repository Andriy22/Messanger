using UnityEngine;

public class MessageImageBlock : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    public void Append(ImageScaler image)
    {
        image.transform.SetParent(_parent, false);
    }
}
