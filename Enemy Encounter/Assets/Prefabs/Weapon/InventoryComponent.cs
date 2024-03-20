using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
   [SerializeField] Weapon[] initWeaponsPrefabs; // An array of Weapon objects that represent the initial weapons the player starts with in the game.
   [SerializeField] Transform[] weaponSlots;  // An array of transforms representing the locations where weapons can be equipped.
   int currentWeaponIndex = -1; // An integer indicating the index of the currently equipped weapon (-1 means no weapon is equipped).
   List<Weapon> weapons; // A list that stores references to all the weapons the player has.

   private void Start(){
       InitializeWeapons();
   }

   // This will be executed at the start of the game
private void InitializeWeapons(){
       
       weapons = new List<Weapon>(); //Initialize empty list of weapons we can have
       foreach(Weapon weapon in initWeaponsPrefabs) {
          Transform weaponSlot = null;
          foreach(Transform slot in weaponSlots){
            // if there's no changes we still use the default slot
            if (slot.gameObject.tag == weapon.GetAttachSlotTag()){
                weaponSlot=slot;
            }
          } 
          Weapon newWeapon = Instantiate(weapon,weaponSlot); 
          /*
          'weaponSlot' is a GameObject. In Unity, 
          the Transform component is attached to GameObjects to represent their position, rotation, and scale.
          When you access the Transform component of a GameObject, you're essentially accessing the information about its position and orientation in the game world.
          */
          newWeapon.Init(gameObject);
          weapons.Add(newWeapon);
       }

       NextWeapon(); // Equip first weapon in the bag
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


    internal Weapon GetActiveWeapon()
    {
    return weapons[currentWeaponIndex];
    }


private void EquipWeapon(int WeaponIndex){
    if(WeaponIndex < 0 || WeaponIndex >= weapons.Count){
        return;
    }
    
    if(currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count){
        weapons[currentWeaponIndex].UnEquip();

    }

    weapons[WeaponIndex].Equip();
    currentWeaponIndex=WeaponIndex;

}


}
