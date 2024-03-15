using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon is the parent class of the other (riffle and pistol)
public abstract class Weapon : MonoBehaviour
{
    //[SerializeField] float AttackRateMult =1f;
    // SlotTag Name of the weapon(which represents the position of the weapon)
    [SerializeField] string AttachSlotTag;

    //change the animation when change the player
    [SerializeField] AnimatorOverrideController overrideController;

    public abstract void Attack();

    // Return the SlotTag Name of the weapon
    public string GetAttachSlotTag()
    {
        return AttachSlotTag;
    }

    //Define the owner
    public GameObject Owner
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
    
    //assign the owner
    public void Init(GameObject owner)
    {
        Owner = owner;
        //deactivate the weapon in the beginning (be in the backpack not in hands)
        UnEquip();
    }

    //equip and unequip
    public void Equip()
    {
        gameObject.SetActive(true);
        //override the default animation with the equipped weapon animation
        Owner.GetComponent<Animator>().runtimeAnimatorController = overrideController;
        //Owner.GetComponent<Animator>().SetFloat("AttackRateMult", AttackRateMult);
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public void DamageGameObject(GameObject objToDamage, float amt){
        HealthComponent healthComp = objToDamage.GetComponent<HealthComponent>();
        if(healthComp != null ){
              healthComp.changeHealth(-amt);
        }
       
    }
}
