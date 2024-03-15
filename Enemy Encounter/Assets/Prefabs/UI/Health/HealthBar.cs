using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] Slider healthSlider ;

   // the character above it the health bar will be
   private Transform attatchPoint;

   public void Init(Transform attatchpoint){
     attatchPoint = attatchpoint;
   }
   
   public void SetHealthSliderValue(float health, float delta, float maxHealth){
    healthSlider.value=health/maxHealth;
   }

   public void Update(){
    //figure out the postion of the health bar based on the camera
    Vector3 ownerScreenPoint = Camera.main.WorldToScreenPoint(attatchPoint.position);
    // set the attach Point
    transform.position=ownerScreenPoint;

   }
   
   //To destroy the health bar
   internal void onOwnerDead(){
      Destroy(gameObject);
   }


}
