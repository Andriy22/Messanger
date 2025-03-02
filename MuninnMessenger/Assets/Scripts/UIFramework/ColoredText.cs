using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ColoredText : MonoBehaviour
{
    [SerializeField] private TMP_Text _image;
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
