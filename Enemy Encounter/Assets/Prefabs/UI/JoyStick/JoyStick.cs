using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnStickInputValueUpdated(Vector2  inputVal);
/*
It's saying, "Hey, when someone updates the joystick, this is the format (delegate)(A contract) they need to follow. They should give me a Vector2 value."
So, every time the joystick is moved, it follows this format (delegate) and gives back a Vector2 value, which represents the direction the joystick was moved in.
*/
    public event OnStickInputValueUpdated OnStickValueUpdated;
    [SerializeField] RectTransform ThumbStickTrans;
    [SerializeField] RectTransform BackgroundTrans;
    [SerializeField] RectTransform CenterTrans;

public void OnDrag(PointerEventData eventData)
{
    Debug.Log($"On Drag Fired {eventData.position}");
    Vector2 TouchPos = eventData.position;
    Vector2 centerPos = BackgroundTrans.position;
    Vector2 localOffset = Vector2.ClampMagnitude(TouchPos - centerPos,BackgroundTrans.sizeDelta.x/4);
    Vector2 inputVal = localOffset / BackgroundTrans.sizeDelta.x/4;
    ThumbStickTrans.position = centerPos + localOffset;
    OnStickValueUpdated?.Invoke(inputVal); //Trigger the event and passing inputVal as the argument

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
        OnStickValueUpdated?.Invoke(Vector2.zero); //Trigger the event and passing inputVal as the argument
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
