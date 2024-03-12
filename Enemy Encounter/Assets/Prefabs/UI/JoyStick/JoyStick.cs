using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnStickInputValueUpdated(Vector2 DistanceMovedInput);
    public delegate void OnStickTaped();
    public event OnStickTaped onStickTaped;
    /*
    It's saying, "Hey, when someone updates the joystick, this is the format (delegate)(A contract) they need to follow. They should give me a Vector2 value."
    So, every time the joystick is moved, it follows this format (delegate) and gives back a Vector2 value, which represents the direction the joystick was moved in.
    */
    public event OnStickInputValueUpdated OnStickValueUpdated;
    [SerializeField] RectTransform ThumbStickTrans;
    [SerializeField] RectTransform BackgroundTrans;
    [SerializeField] RectTransform CenterTrans;

    //detect the tab to switch the weapon
    bool bWasDragging;
   
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 TouchPos = eventData.position; //The finger position(x,y)
        Vector2 centerPos = BackgroundTrans.position; //The center of the Background white cercle(x,y)
        Vector2 localOffset = Vector2.ClampMagnitude(TouchPos - centerPos, BackgroundTrans.sizeDelta.x / 2); //localOffset represents the distance moved from the center(x,y)
        Vector2 DistanceMovedInput = localOffset / (BackgroundTrans.sizeDelta.x / 2);
        ThumbStickTrans.position = centerPos + localOffset; //Move the stick 
        OnStickValueUpdated?.Invoke(DistanceMovedInput); //Trigger the event and passing DistanceMovedInput as the argument
        bWasDragging = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BackgroundTrans.position = eventData.position;
        ThumbStickTrans.position = eventData.position;
        bWasDragging = false;
    } //Move the whole block to where i placed my finger

    public void OnPointerUp(PointerEventData eventData)
    {
        BackgroundTrans.position = CenterTrans.position;
        ThumbStickTrans.position = BackgroundTrans.position; //Get the block position back to it's initial position
        OnStickValueUpdated?.Invoke(Vector2.zero); //Trigger the event and passing ... as the argument
        //we are tabing the screen to switch the weapon(not dragging)
        if (!bWasDragging)
        {
            onStickTaped?.Invoke();
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
