using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick joystick_1; //Created an instance
    [SerializeField] CharacterController characterController;
    [SerializeField] CameraController cameraController;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float  turnSpeed = 30f;
    
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
        Vector3 MoveDir= x_axis * DistanceMovedInput.x + z_axis * DistanceMovedInput.y ; //where we are moving
        characterController.Move(MoveDir* Time.deltaTime * moveSpeed); //Move the character
        if (DistanceMovedInput.magnitude != 0 )
        {
            //we put it here because we don't want the direction to snap back when we stop moving with the joystock
            // we want some animation when the player move from looking up to looking down instantly (rotaion progress => lerp to the rotaion instead of turn to it lerp=turn from one rotation to an other with alpha)
            float turnLerpAlpha = turnSpeed* Time.deltaTime; //calculating alpha
            transform.rotation=Quaternion.Lerp( transform.rotation,Quaternion.LookRotation(MoveDir,Vector3.up), turnSpeed);//player face the direction (vector3.up to direct the head of the player to the direction)
            if(cameraController != null){
            cameraController.AddYawInput(DistanceMovedInput.x);
            }

             }
           
        }
        
}
