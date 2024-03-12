using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
   // Weapon that the player starts with in the game
   [SerializeField] Weapon[] initWeaponsPrefabs;
   
   // Default place of the weapons (when you can't find the slot for the weapon)
    [SerializeField] Transform defaultWeaponSlot;
   
   // Location of the weapons (slots)
   [SerializeField] Transform[] weaponSlots;

   // create a refrence for the current weapon
   int currentWeaponIndex = -1;

   // List of weapons the player can have
   List<Weapon> weapons;

   private void Start(){
       InitializeWeapons();
   }

   // This will be executed at the start of the game
private void InitializeWeapons(){
       
       weapons = new List<Weapon>(); //Initialize empty list of weapons we can have
       foreach(Weapon weapon in initWeaponsPrefabs) {
          Transform weaponSlot = defaultWeaponSlot;
          foreach(Transform slot in weaponSlots){
            // if there's no changes we still use the default slot
            if (slot.gameObject.tag == weapon.GetAttachSlotTag()){
                weaponSlot=slot;
            }
          } 
          Weapon newWeapon = Instantiate(weapon,weaponSlot);
          newWeapon.Init(gameObject);
          weapons.Add(newWeapon);
       }

       NextWeapon();

   }

//equip the next weapon
public void NextWeapon(){
    int nextWeaponIndex=currentWeaponIndex +1;
    if(nextWeaponIndex >= weapons.Count){
        //return to the first weapon if raise the number of weapons we have(reset)
        nextWeaponIndex=0;
    }

    EquipWeapon(nextWeaponIndex);

    
}

private void EquipWeapon(int WeaponIndex){
    //check index out of bounds or not
    if(WeaponIndex < 0 || WeaponIndex >= weapons.Count){
        return;
    }
    
    //check if the weapon exists (in the bounbs)
    if(currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count){
        weapons[currentWeaponIndex].UnEquip();

    }

    weapons[WeaponIndex].Equip();
    currentWeaponIndex=WeaponIndex;

}


}
