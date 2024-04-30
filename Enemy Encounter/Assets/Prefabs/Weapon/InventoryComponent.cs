using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour, IPurchaseListener
{
    [SerializeField] Weapon[] initWeaponsPrefabs;

    [SerializeField] Transform defaultWeaponSlot;
    [SerializeField] Transform[] weaponSlots; // THIS LIST REPRESENTS THE SLOTS THAT WEAPONS GO INTO

    List<Weapon> weapons; // THIS LIST REPRESENTS THE WEAPONS THAT THE PLAYER CAN EQUIP
    int currentWeaponIndex = -1;

    private void Start()
    {
        InitializeWeapons();
    }

    private void InitializeWeapons()
    {
        weapons = new List<Weapon>();
        foreach (Weapon weapon in initWeaponsPrefabs)
        {
            GiveNewWeapon(weapon);
        }

        NextWeapon();
    }

    private void GiveNewWeapon(Weapon weapon) // RESPONSIBLE FOR DISPLAYING WEAPONS IN IT'S GAMEPLAY SLOT WHEN THE WEAPON IS PURCHASED
    {
        Transform weaponSlot = defaultWeaponSlot;
        foreach (Transform slot in weaponSlots)
        {
            if (slot.gameObject.tag == weapon.GetAttachSlotTag()) // IF WE FOUND THE SLOT WHERE THE NEW PURCHASED WEAPON SHOULD GO, THEN PUT IT THERE
            {
                weaponSlot = slot;
            }
        }
        Weapon newWeapon = Instantiate(weapon, weaponSlot); // newWeapon is referencig to a Prefab([SerializeField]), soo that it will be visible in the GamePlay
        newWeapon.Init(gameObject);
        weapons.Add(newWeapon);
    }

    public void NextWeapon()
    {
        int nextWeaponIndex = currentWeaponIndex + 1;
        if(nextWeaponIndex >= weapons.Count)
        {
            nextWeaponIndex = 0;
        }

        EquipWeapon(nextWeaponIndex);
    }

    internal Weapon GetActiveWeapon()
    {
        if(HasWeapon())
        {
            return weapons[currentWeaponIndex];
        }
        return null;
    }

    private void EquipWeapon(int weaponIndex)
    {
        if(weaponIndex < 0 || weaponIndex >= weapons.Count)
            return;

        if(currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count) // IF THE PLAYER ALREADY HAVE AN EQUIPED WEAPON, THEN UNEQUIP IT
        {
            weapons[currentWeaponIndex].UnEquip();
        }

        weapons[weaponIndex].Equip();
        currentWeaponIndex = weaponIndex;
    }

    public bool HandlePurchase(Object newPurchase)
    {
        GameObject itemAsGameObject = newPurchase as GameObject;
        if (itemAsGameObject == null) return false;

        Weapon itemAsWeapon = itemAsGameObject.GetComponent<Weapon>();
        if (itemAsWeapon == null) return false;

        bool hasWeapon = true;
        if(weapons.Count == 0)
        {
            hasWeapon = false;
        }

        GiveNewWeapon(itemAsWeapon);
        if(!hasWeapon)
        {
            EquipWeapon(0);
        }
        return true;
    }

    public bool HasWeapon()
    {
        return weapons.Count != 0;
    }
}
