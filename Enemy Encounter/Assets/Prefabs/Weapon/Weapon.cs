using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon is the parent class of the other (riffle and pistol)
public abstract class Weapon : MonoBehaviour
{

    [SerializeField] string AttachSlotTag; // SlotTag Name of the weapon(which represents the position of the weapon)
    [SerializeField] AnimatorOverrideController overrideController; // change the animation when change the player

    public abstract void Attack();

    public string GetAttachSlotTag() // Return the SlotTag Name of the weapon
    {
        return AttachSlotTag;
    }

    public GameObject Owner // Define the weapon owner
    {
        get;
        private set;
    }
    /*
    get;: This means that other parts of the program can read the value of this property. So, if someone wants to know what GameObject owns this weapon,
    they can ask and get the answer.
    private set;: This means that only this class itself can change the value of Owner. Other parts of the program cannot directly change it. It's like saying,
    Hey, only this class is allowed to decide who the owner is, nobody else can mess with it."
    */
    
    public void Init(GameObject owner) // assign the owner
    {
        Owner = owner;
        UnEquip(); // deactivate the weapon in the beginning (be in the backpack not in hands)
    }

    public void Equip()
    {
        gameObject.SetActive(true);
        //override the default animation with the equipped weapon animation
        Owner.GetComponent<Animator>().runtimeAnimatorController = overrideController;
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public void DamageGameObject(GameObject objToDamage, float amt){
        HealthComponent healthComp = objToDamage.GetComponent<HealthComponent>();
        if(healthComp != null ){
              healthComp.changeHealth(-amt, Owner);
        }
       
    }
}
