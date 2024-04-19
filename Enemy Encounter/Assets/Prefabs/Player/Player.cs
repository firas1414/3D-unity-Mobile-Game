using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick moveStick; //Created an instance of JoyStick class
    [SerializeField] JoyStick aimStick;
    [SerializeField] CharacterController characterController;
    [SerializeField] CameraController cameraController; 
    [SerializeField] float animTurnSpeed;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] MovementComponent  movementComponent;
    [Header("Inventory")]
    [SerializeField] InventoryComponent inventoryComponent;
    
    Vector2 moveStickUpdated; // MoveStick value
    Vector2 aimInput;
    Camera mainCam;
    Animator animator ;


    // Start is called before the first frame update
    void Start()
    {
     moveStick.OnStickValueUpdated += GetmoveStickUpdated; // Each time the stick value gets updated, the GetmoveStickUpdated function will be called
     aimStick.OnStickValueUpdated +=aimStickUpdated; // Each time the stick value gets updated, the aimStickUpdated function will be called
     aimStick.onStickTaped += StartSwitchWeapon; // Each time the aim stick gets tapped, the StartSwitchWeapon function will be called
     mainCam = Camera.main;
     animator=GetComponent<Animator>();
    
     /*
     When you do joystick_1.event_name += function_name, you are subscribing function_name to the event_name.
     This means that when event_name is raised or triggered, function_name will be called.
     "Hey, whenever the OnStickValueUpdated event happens in the joystick_1 instance, call the GetmoveStickUpdated function."
     */
    }


    void GetmoveStickUpdated(Vector2 inputValue)
    {
        moveStickUpdated = inputValue;
    }

    Vector3 StickInputToWorldDirection (Vector2 inputValue) {
        Vector3 X_camera = mainCam.transform.right;
        Vector3 Forward_camera = Vector3.Cross(X_camera, Vector3.up);
        return X_camera * inputValue.x + Forward_camera * inputValue.y ; // Get Move Direction based on Camera direction (x,0,z)
        
    }

    void aimStickUpdated(Vector2 inputValue){
        aimInput = inputValue;
        //switch to attacking animations
        if(aimInput.magnitude > 0){
            animator.SetBool("attacking",true);

        }else{
            animator.SetBool("attacking",false);
        }
     }


    // Update is called once per frame
    void Update()
    {
        PerformMoveAndAim();
        UpdateCamera();

    }

// make this function just to clear the code
    private void PerformMoveAndAim(){
        Vector3 MoveDir= StickInputToWorldDirection(moveStickUpdated) ; // Get the move direction
        characterController.Move(MoveDir* Time.deltaTime * moveSpeed); // Move
        UpdateAim(MoveDir); // Aim
        
        // change animation based on MoveDirection (MoveDir)
        //when we aim we use the aim direction to set the animation not the move direction
        //how much we are moving forward and right (back= forward -1) (calculatated via DOT product) 
        float forward = Vector3.Dot(MoveDir,transform.forward); // How much we are moving forward(z_axis)
        float right = Vector3.Dot(MoveDir,transform.right); // How much we are moving right(x_axis)
        animator.SetFloat("forwardSpeed",forward);
        animator.SetFloat("rightSpeed",right);
        characterController.Move(Vector3.down * Time.deltaTime * 10f); // Force the player to all be on the ground

    }

    private void  UpdateAim(Vector3 currentMoveDir){
        Vector3 AimDir= currentMoveDir ; // Get the aim direction(which is the same as move direction if the player is not aiming)
        if(aimInput.magnitude != 0){ // Check if the player is aimnig: true->Aim diretion = AimStick Direction      False->Aim direction = MoveStick Direction
            AimDir = StickInputToWorldDirection(aimInput);
        }
        RotationTowards(AimDir);
    }

    private void UpdateCamera(){ // Turn the camera with the player
        // don't update camera direction while aiming (if the player move and don't aim)
        if (moveStickUpdated.magnitude != 0 && aimInput.magnitude == 0 && cameraController != null) 
        {
            cameraController.AddYawInput(moveStickUpdated.x);
        }
    }

    private void RotationTowards(Vector3 AimDir)
    {
        float currentTrunSpeed = movementComponent.RotationTowards(AimDir);
        animTurnSpeed = Mathf.Lerp(animTurnSpeed, currentTrunSpeed, Time.deltaTime * animTurnSpeed);

        animator.SetFloat("turnSpeed", animTurnSpeed);
    }
    
    public void AttackPoint(){
        inventoryComponent.GetActiveWeapon().Attack();
    }


    void StartSwitchWeapon(){
        animator.SetTrigger("switchWeapon");
    }
    public void SwitchWeapon(){
        inventoryComponent.NextWeapon();
    }
    
        
}