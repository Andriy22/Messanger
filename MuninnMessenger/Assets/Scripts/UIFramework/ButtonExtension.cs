using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 _scaleTo = new Vector3(0.9f, 1.1f, 1);
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_button != null && _button.interactable == false)
        {
            return;
        }

        transform.DOKill();
        transform.DOScale(_scaleTo, 0.15f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_button != null && _button.interactable == false)
        {
            return;
        }

        transform.DOKill();
        transform.DOScale(1, 0.15f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_button != null && _button.interactable == false)
        {
            return;
        }
        
        transform.DOKill();
        transform.DOScale(Vector3.Lerp(Vector3.one, _scaleTo, 0.5f), 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_button != null && _button.interactable == false)
        {
            return;
        }

        transform.DOKill();
        transform.DOScale(1, 0.15f);
    }
}
