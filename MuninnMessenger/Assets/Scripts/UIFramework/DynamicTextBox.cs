using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform _objectToScale;
    [SerializeField] private float _maxWidth = 500f; // Maximum width before wrapping
    [SerializeField] private float _padding = 10f;   // Padding around the text
    
    private TextMeshProUGUI _text;

    public string Text
    {
        get => _text.text;
        set => SetText(value);
    }

    public TextMeshProUGUI TMPText => _text;

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
        CheckLinks();

        AdjustSize();
        AdjustSize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink (_text, eventData.position, eventData.pressEventCamera);
	
    	if (linkIndex == -1)
        {
            return;
        }

        var linkInfo = _text.textInfo.linkInfo[linkIndex];
		var selectedLink = linkInfo.GetLinkID();
		
        if (selectedLink != "") 
        {
			Debug.LogFormat ("Open link {0}", selectedLink);
			Application.OpenURL (selectedLink);        
		}
    }

    private string ShortLink(string link) 
    {
		return $"<#7f7fe5><u><link=\"{link}\">{link}</link></u></color>";
	}        

	// Check links in text
	private void CheckLinks() 
    {
		var regx = new Regex ("((http://|https://|www\\.)([A-Z0-9.-:]{1,})\\.[0-9A-Z?;~&#=\\-_\\./]{2,})" , RegexOptions.IgnoreCase | RegexOptions.Singleline); 
		var matches = regx.Matches (_text.text); 
		foreach (Match match in matches)
        {
            _text.text = _text.text.Replace (match.Value, ShortLink(match.Value));
        }
    }
}