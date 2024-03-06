using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followPlayerTrans;
    [SerializeField] float TurnSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = followPlayerTrans.position;
        /*
        This line sets the position of the current object (The camera) to match the position of the followPlayerTrans transform.
        This effectively makes the camera follow the player's position.
        */
    }
    public void AddYawInput(float amt)
    {
        transform.Rotate(Vector3.up, amt * Time.deltaTime * TurnSpeed);
    }

}
