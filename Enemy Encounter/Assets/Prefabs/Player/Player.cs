using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick moveStick; //Created an instance
    [SerializeField] JoyStick aimStick;
    [SerializeField] CharacterController characterController;
    [SerializeField] CameraController cameraController; // aalech khalitha SerializeField ?
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float  turnSpeed = 30f;
    
    Vector2 DistanceMovedInput;
    Vector2 aimInput;
    Camera mainCam;


    // Start is called before the first frame update
    void Start()
    {
     moveStick.OnStickValueUpdated += GetDistanceMoved; //subscribe the GetDistanceMoved function to the OnStickValueUpdated Event
     aimStick.OnStickValueUpdated +=aimStickUpdated;
     mainCam = Camera.main;
     /*
     When you do joystick_1.event_name += function_name, you are subscribing function_name to the event_name.
     This means that when event_name is raised or triggered, function_name will be called.
     "Hey, whenever the OnStickValueUpdated event happens in the joystick_1 object, call the GetDistanceMoved function."
     */
    }

    //turn the 2D direction to 3D direction
    Vector3 StickInputToWorldDirection (Vector2 inputValue) {
        Vector3 x_axis = mainCam.transform.right;
        Vector3 z_axis = Vector3.Cross(x_axis, Vector3.up);
        return x_axis * inputValue.x + z_axis * inputValue.y ; //convert the input to the world direction
        
    }

    void aimStickUpdated(Vector2 inputValue){
        aimInput = inputValue;
     }
    void GetDistanceMoved(Vector2 inputValue)
    {
        DistanceMovedInput = inputValue;
    }


    // Update is called once per frame
    void Update()
    {
        PerformMoveAndAim();
        UpdateCamera();
    }

// make this function just to clear the code
    private void PerformMoveAndAim(){
        Vector3 MoveDir= StickInputToWorldDirection( DistanceMovedInput) ; //control the move direction
        characterController.Move(MoveDir* Time.deltaTime * moveSpeed); //Move the character
        UpdateAim(MoveDir);

    }

    private void  UpdateAim(Vector3 currentMoveDir){
        Vector3 AimDir= currentMoveDir ; //control the aim direction
        //ckech if the player is trying to aim (if we are not aiming aim is where we are moving)
        if(aimInput.magnitude != 0){
            AimDir= StickInputToWorldDirection(aimInput);

        }
        RotationTowards(AimDir);
    }

    private void UpdateCamera(){
        // don't update camera direction while aiming (if the player move and don't aim)
        if (DistanceMovedInput.magnitude != 0 && aimInput.magnitude == 0 && cameraController != null) 
        {
            
            cameraController.AddYawInput(DistanceMovedInput.x);
            

        }
    }

    private void RotationTowards(Vector3 AimDir){
        if(AimDir.magnitude != 0){
            
            // we want some animation when the player move from looking up to looking down instantly (rotaion progress => lerp to the rotaion instead of turn to it lerp=turn from one rotation to an other with alpha)
            // we want the player to aim in a direction while moving to another direction when he is attacked by the enemeies
            //when you aim the direction of the player is controlled by the aim not the move (aim independent from the move)
            float turnLerpAlpha = turnSpeed* Time.deltaTime; //calculating alpha
            transform.rotation=Quaternion.Lerp( transform.rotation,Quaternion.LookRotation(AimDir,Vector3.up), turnLerpAlpha);//player aim the direction (vector3.up to direct the head of the player to the direction)
        }
    }
    
        
}