using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// RESPONSIBLE FOR ENABLING AND DISABLING PART OF THE UI BASED ON GAME EVENTS
public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup GameplayControl;
    [SerializeField] CanvasGroup PauseMenu;
    [SerializeField] CanvasGroup Shop;
    [SerializeField] CanvasGroup DeathMenu;
    [SerializeField] CanvasGroup WinMenu;
    [SerializeField] UIAudioPlayer uiAudioPlayer;

    List<CanvasGroup> AllChildren = new List<CanvasGroup>();

    CanvasGroup currentActiveGrp;

    private void Start()
    {
        List<CanvasGroup> children = new List<CanvasGroup>();
        GetComponentsInChildren(true, children); // This method searches for CanvasGroup components in the children of the current object and its descendants.
        // The true parameter indicates that it should include inactive CanvasGroup components as well.
        foreach(CanvasGroup child in children)
        {
            if(child.transform.parent == transform) // WE ONLY NEED THE CHILDRENS OF THE InGameUi Object, SO WE SHOULD TAKE THE CHILDREN'S THAT THEIR PARENT'S IS InGameUi  
            {
                AllChildren.Add(child); // FILL THE AllChildren WITH THESE CHILDRENS'S CanvasGroup(GameplayControl, PauseMenu, DeathMenu, ShopUI, WinMenu)
                SetGroupActive(child, false, false);
            }
        }

        if(AllChildren.Count != 0)
        {
            SetCurrentActiveGrp(AllChildren[0]);
        }

        LevelManager.onLevelFinished += LevelFinished;
    }

    private void LevelFinished()
    {
        SetCurrentActiveGrp(WinMenu);
        GameplayStatics.SetGamePaused(true);
        uiAudioPlayer.PlayWin();
    }

    internal void SwithToGameplayUI()
    {
        SetCurrentActiveGrp(GameplayControl);
        GameplayStatics.SetGamePaused(false);
    }

    private void SetCurrentActiveGrp(CanvasGroup canvasGroup)
    {
        if(currentActiveGrp != null)
        {
            SetGroupActive(currentActiveGrp, false, false);
        }

        currentActiveGrp = canvasGroup;
        SetGroupActive(currentActiveGrp, true, true);
    }

    private void SetGroupActive(CanvasGroup child, bool interactable, bool visible)
    {
        child.interactable = interactable;
        child.blocksRaycasts = interactable;
        child.alpha = visible ? 1 : 0;
    }

    public void SetGameplayControlEnabled(bool enabled)
    {
        SetCanvasGroupEnabled(GameplayControl, enabled);
    }

    public void SwithToPauseMenu()
    {
        SetCurrentActiveGrp(PauseMenu);
        GameplayStatics.SetGamePaused(true);
    }

    internal void SwithToShop()
    {
        SetCurrentActiveGrp(Shop);
        GameplayStatics.SetGamePaused(true);
    }

    private void SetCanvasGroupEnabled(CanvasGroup grp, bool enabled)
    {
        grp.interactable = enabled;
        grp.blocksRaycasts = enabled;
    }

    internal void SwithToDeathMenu()
    {
        SetCurrentActiveGrp(DeathMenu);
        GameplayStatics.SetGamePaused(true);
    }
}
