using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    public bool HandlePurchase(Object newPurchase);
}

public class CreditComponent : MonoBehaviour, IRewardListener
{
    [SerializeField] int credits;
    [SerializeField] Component[] PurchaseListeners; // THIS IS GONNA TAKE THE InventoryComponent

    List<IPurchaseListener> purchaseListenerInterfaces = new List<IPurchaseListener>();

    private void Start()
    {
        CollectPurchaseListeners();
    }

    private void CollectPurchaseListeners()
    {
        foreach(Component listener in PurchaseListeners) // WE'RE MAKING A foreach TO MAKE THIS FLEXIBLE, SINCE WE HAVE 2 COMPONENTS, AbilityComponent & InventoryComponent
        {
            IPurchaseListener listenerInterface = listener as IPurchaseListener; 
            if(listenerInterface != null)
            {
                purchaseListenerInterfaces.Add(listenerInterface); // ADD THE InventoryComponent's & AbilityComponent interface(IPurchaseListener) TO THE purchaseListenerInterfaces
            }
        }
    }

    private void BroadcastPurchase(Object item)
    {
        foreach(IPurchaseListener purchaseListener in purchaseListenerInterfaces)
        {
            if(purchaseListener.HandlePurchase(item))
            {
                return;
            }
        }
    }

    public int Credit
    {
        get { return credits; }
    }

    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;

    public bool Purchase(int price, Object item)
    {
        if(credits < price) return false;

        credits -= price;
        onCreditChanged?.Invoke(credits);
        BroadcastPurchase(item);

        return true;
    }

    public void Reward(Reward reward)
    {
        credits += reward.creditReward;
        onCreditChanged?.Invoke(credits);
    }
}
