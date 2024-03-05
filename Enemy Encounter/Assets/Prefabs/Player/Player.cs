using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick moveStick;
    [SerializeField] CharacterController characterController;
    [SerializeField] float moveSpeed = 40f;
    Vector2 moveInput;
    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
     moveStick.OnStickValueUpdated += moveStickmoveStick; //subscribe the moveStickmoveStick function to the OnStickValueUpdated Event
     mainCam = Camera.main;
     /*
     When you do moveStick.event_name += function_name, you are subscribing function_name to the event_name.
     This means that when event_name is raised or triggered, function_name will be called.
     "Hey, whenever the OnStickValueUpdated event happens in the moveStick object, call the moveStickmoveStick function."
     */
    }
    void moveStickmoveStick(Vector2 inputValue)
    {
        moveInput = inputValue;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 x_axis = mainCam.transform.right;
        Vector3 z_axis = Vector3.Cross(x_axis, Vector3.up);
        characterController.Move((x_axis * moveInput.x + z_axis * moveInput.y) * Time.deltaTime * moveSpeed);
    }
}
