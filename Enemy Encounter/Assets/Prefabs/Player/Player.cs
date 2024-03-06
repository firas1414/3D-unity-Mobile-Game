using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick joystick_1; //Created an instance
    [SerializeField] CharacterController characterController;
    [SerializeField] CameraController cameraController;
    [SerializeField] float moveSpeed = 20f;
    Vector2 DistanceMovedInput;
    Camera mainCam;


    // Start is called before the first frame update
    void Start()
    {
     joystick_1.OnStickValueUpdated += GetDistanceMoved; //subscribe the GetDistanceMoved function to the OnStickValueUpdated Event
     mainCam = Camera.main;
     /*
     When you do joystick_1.event_name += function_name, you are subscribing function_name to the event_name.
     This means that when event_name is raised or triggered, function_name will be called.
     "Hey, whenever the OnStickValueUpdated event happens in the joystick_1 object, call the GetDistanceMoved function."
     */
    }


    void GetDistanceMoved(Vector2 inputValue)
    {
        DistanceMovedInput = inputValue;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 x_axis = mainCam.transform.right;
        Vector3 z_axis = Vector3.Cross(x_axis, Vector3.up);
        characterController.Move((x_axis * DistanceMovedInput.x + z_axis * DistanceMovedInput.y) * Time.deltaTime * moveSpeed); //Move the character
        if (DistanceMovedInput.magnitude != 0 && cameraController != null)
        {
            cameraController.AddYawInput(DistanceMovedInput.x);
        }
    }
}
