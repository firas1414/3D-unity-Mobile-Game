using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon is the parent class of the other (riffle and pistol)
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string AttachSlotTag;
    

    public string GetAttachSlotTag(){
        return AttachSlotTag;
    }
   //Define the owner
   public GameObject Owner {
      get;
      private set;
   }

   //assign the owner
   public void Init(GameObject owner)
   {
     Owner=owner;
   }

   //equip and unequip
   public void Equip(){
      gameObject.SetActive(true);
   }
   
   public void UnEquip(){
      gameObject.SetActive(false);
   }
}
