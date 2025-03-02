using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextBox : MonoBehaviour
{
    [SerializeField] private RectTransform _objectToScale;
    [SerializeField] private float _maxWidth = 500f; // Maximum width before wrapping
    [SerializeField] private float _padding = 10f;   // Padding around the text

    public string Text
    {
        get => _text.text;
        set => SetText(value);
    }

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void AdjustSize()
    {
        // Force update to get correct preferred width and height
        _text.ForceMeshUpdate();

        float preferredWidth = _text.preferredWidth + _padding * 2;
        float preferredHeight = _text.preferredHeight + _padding * 2;

        _objectToScale.sizeDelta = preferredWidth <= _maxWidth 
            ? new Vector2(preferredWidth, preferredHeight) 
            : new Vector2(_maxWidth, preferredHeight);

        _text.textWrappingMode = preferredWidth <= _maxWidth 
            ? TextWrappingModes.NoWrap 
            : TextWrappingModes.Normal;
    }

    public void SetText(string newText)
    {
        _text.text = newText;
        AdjustSize();
        AdjustSize();
    }
}