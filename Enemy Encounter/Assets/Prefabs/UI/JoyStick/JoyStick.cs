using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform ThumbStickTrans;
    [SerializeField] RectTransform BackgroundTrans;
    [SerializeField] RectTransform CenterTrans;

public void OnDrag(PointerEventData eventData)
{
    Debug.Log($"On Drag Fired {eventData.position}");
    Vector2 TouchPos = eventData.position;
    Vector2 centerPos = BackgroundTrans.position;
    Vector2 localOffset = Vector2.ClampMagnitude(TouchPos - centerPos,50);
    ThumbStickTrans.position = centerPos + localOffset;
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
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
