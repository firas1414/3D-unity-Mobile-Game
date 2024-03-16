using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] float aimRange = 1000;
    [SerializeField] LayerMask aimMask;



    public GameObject GetAimTarget(out Vector3 aimDir){ // This method is responsible for determining the object that the aim is pointing to.
        /*
        ""out"" is a keyword used in method parameters to indicate that the corresponding argument is passed by reference and is intended to be modified by the method.
        */
        Vector3 aimStart = muzzle.position;
        aimDir=GetAimDir();
        if(Physics.Raycast(aimStart, aimDir, out RaycastHit hitInfo, aimRange, aimMask))
        {
            return hitInfo.collider.gameObject; //return the object that the line collided with
        }
        return null;
    }

        private void OnDrawGizmos(){
            Gizmos.DrawLine(muzzle.position, muzzle.position + GetAimDir() * aimRange);
        }

        Vector3 GetAimDir(){ // This method calculates the aim direction.
            Vector3 muzzleDir = muzzle.forward; // .forward gives you the direction the object is facing.
            Debug.Log("Muzzle Direction: " + muzzleDir);
            return new Vector3(muzzleDir.x, 0f, muzzleDir.z).normalized; 
            /*
            This line creates a new Vector3 with the same X and Z components as muzzleDir but sets the Y component to 0.
            This effectively removes any vertical component from the direction,
            keeping it parallel to the ground (assuming Y is the vertical axis).
             */
        }

}
