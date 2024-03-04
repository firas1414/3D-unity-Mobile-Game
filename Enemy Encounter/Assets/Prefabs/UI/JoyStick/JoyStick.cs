using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnStickInputValueUpdated(Vector2  inputVal);
    public event OnStickInputValueUpdated OnStickValueUpdated;
    [SerializeField] RectTransform ThumbStickTrans;
    [SerializeField] RectTransform BackgroundTrans;
    [SerializeField] RectTransform CenterTrans;

public void OnDrag(PointerEventData eventData)
{
    Debug.Log($"On Drag Fired {eventData.position}");
    Vector2 TouchPos = eventData.position;
    Vector2 centerPos = BackgroundTrans.position;
    Vector2 localOffset = Vector2.ClampMagnitude(TouchPos - centerPos,BackgroundTrans.sizeDelta.x/2);
    Vector2 inputVal = localOffset / BackgroundTrans.sizeDelta.x/4;
    ThumbStickTrans.position = centerPos + localOffset;
    OnStickValueUpdated?.Invoke(inputVal);

}


    public void OnPointerDown(PointerEventData eventData)
    {
        BackgroundTrans.position = eventData.position;
        ThumbStickTrans.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BackgroundTrans.position = CenterTrans.position;
        ThumbStickTrans.position = BackgroundTrans.position;
        OnStickValueUpdated?.Invoke(Vector2.zero);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
