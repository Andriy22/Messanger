using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColoredPanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ThemeColor _color;

    private void OnEnable()
    {
        UpdateColor();
        Theme.OnThemeChange += UpdateColor;
    }

    private void OnDisable()
    {
        Theme.OnThemeChange -= UpdateColor;
    }

    private void UpdateColor()
    {
        _image.color = Theme.Current[_color];
    }
}
