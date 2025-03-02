using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageScaler : MonoBehaviour
{
    [SerializeField] private InternetImage _internetImage;
    [SerializeField] private Image _scaledImage;

    public async void LoadAsync(string url, float x = 0.5f, float y = 0.5f, float scale = 1f)
    {
        await _internetImage.LoadAsync(url);
        ScaleChildImage(x, y, scale);
    }

    [ContextMenu("Test")]
    public void Test()
    {
        ScaleChildImage();
    }

    public void ScaleChildImage(float x = 0.5f, float y = 0.5f, float scale = 1f)
    {
        if (transform.childCount == 0)
        {
            Debug.LogWarning("No child _scaledImage to scale.");
            return;
        }

        var parentRect = transform as RectTransform;
        var childRect = transform.GetChild(0).transform as RectTransform;

        if (parentRect == null || childRect == null)
        {
            Debug.LogWarning("RectTransform component missing.");
            return;
        }

        float childAspect = _scaledImage != null && _scaledImage.sprite != null
            ? (float)_scaledImage.sprite.texture.width / (float)_scaledImage.sprite.texture.height
            : childRect.sizeDelta.x / childRect.sizeDelta.y;
            
        float parentAspect = parentRect.sizeDelta.x / parentRect.sizeDelta.y;

        Vector2 vector2 = parentAspect > childAspect
            ? new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.x / childAspect)
            : new Vector2(parentRect.sizeDelta.y * childAspect, parentRect.sizeDelta.y);
        childRect.sizeDelta = vector2;

        Debug.Log($"{vector2.x}, {vector2.y}");

        childRect.anchorMax = new Vector2(x, y);
        childRect.anchorMin = new Vector2(x, y);
        childRect.anchoredPosition = Vector2.zero;

        childRect.localScale = new Vector3(scale, scale, 1);
    }
}