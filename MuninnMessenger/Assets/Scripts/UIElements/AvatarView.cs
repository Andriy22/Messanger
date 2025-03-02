using UnityEngine;

public class AvatarView : MonoBehaviour
{
    [SerializeField] private ImageScaler _image;

    public void Load(string url) => _image.LoadAsync(url);
}