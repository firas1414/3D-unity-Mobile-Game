using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



// RESPONSIBLE FOR KNOWING WHAT ABILITY IS ADDED, AND THEN INSTANCIATING THAT ABILITY UI AND POPULATING THE ICON, AND WHEN THE ABILITY IS CASTED IT WILL SHOW THE COOLDOWN
// THIS CLASS SHOULD BE ATTACHED TO THE AbilityDock Prefab
public class AbilityDock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AbilityComponent abilityComponent;
    [SerializeField] RectTransform Root;
    [SerializeField] VerticalLayoutGroup LayoutGrp;
    [SerializeField] AbilityUI AbilityUIPrefab;

    [SerializeField] float ScaleRange = 200f;

    [SerializeField] float highlightSize = 1.5f;
    [SerializeField] float ScaleSpeed = 20f;

    Vector3 GoalScale = Vector3.one;

    List<AbilityUI> abilityUIs = new List<AbilityUI>();

    PointerEventData touchData;
    AbilityUI hightlightedAbility; // REPRESENTS THE AbilityUi THAT BELONGS TO THE ABILITY THE PLAYER CLICKED ON
    private void Awake()
    {
        abilityComponent.onNewAbilityAdded += AddAbility;
    }

    private void AddAbility(Ability newAbility) // WHEN A NEW ABILITY IS ADDED, THIS FUNCTION IS CALLED TO ADD THE UI OF THAT NEW ABILITY
    {
        AbilityUI newAbilityUI = Instantiate(AbilityUIPrefab, Root);
        newAbilityUI.Init(newAbility); // LET THE AbilityUI KNOW WHOS THE NEW ADDED ABILITY
        abilityUIs.Add(newAbilityUI);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(touchData!=null)
        {
            GetUIUnderPointer(touchData, out hightlightedAbility); // RETURNS TRUE IF FOUND AN ABILITY UNDER THE TOUCH POSITION, hightlightedAbility is THAT ABILITY
            ArrangeScale(touchData);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, GoalScale, Time.deltaTime * ScaleSpeed);
    }

    private void ArrangeScale(PointerEventData touchData)
    {
        if (ScaleRange == 0) return;

        float touchVerticalPos = touchData.position.y;
        foreach(AbilityUI abilityUI in abilityUIs)
        {
            float abilityUIVerticalPos = abilityUI.transform.position.y;
            float distance = Mathf.Abs(touchVerticalPos - abilityUIVerticalPos); // Calculates the distance between the vertical position of the touch (touchVerticalPos) and the vertical position of the current AbilityUI element (abilityUIVerticalPos)

            if(distance > ScaleRange) // IF WE'RE FAR FROM THE RANGE, DON'T DO ANY SCALING
            {
                abilityUI.SetScaleAmt(0); // DON'T DO ANY SCALING
                continue;
            }
            // CALCULATES A SCALE AMOUNT BASED ON THE DISTANCE. THE CLOSER THE ABILITY IS TO THE TOUCH POSITION, THE LARGER ITS SCALE AMOUNT WILL BE
            float scaleAmt = (ScaleRange - distance) / ScaleRange;
            abilityUI.SetScaleAmt(scaleAmt);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchData = eventData;
        GoalScale = Vector3.one * highlightSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(hightlightedAbility)
        {
            hightlightedAbility.ActivateAbility();
        }
        touchData = null;
        ResetScale();
        GoalScale = Vector3.one;
    }

    private void ResetScale()
    {
        foreach(AbilityUI abilityUI in abilityUIs)
        {
            abilityUI.SetScaleAmt(0);
        }
    }

    bool GetUIUnderPointer(PointerEventData eventData, out AbilityUI abilityUI) // RETURNS TRUE & AbilityUi IF FOUND AN AbilityUi UNDER THE TOUCH POSITION
    {
        List<RaycastResult> findAbility = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, findAbility); // PERFORMS A RAYCAST FROM THE TOUCH POSITION AND STORES INFORMATION ABOUT ALL THE GAMEOBJECTS HIT BY THE RAYCAST IN THE "findAbility" LIST

        abilityUI = null;
        foreach(RaycastResult result in findAbility)
        {
            abilityUI = result.gameObject.GetComponentInParent<AbilityUI>();
            // "result.gameObject" GETS THE GAMEOBJECT THAT WAS HIT BY THE RAYCAST STORED IN RESULT, THEN CHECKS IF THAT GameObject HAS A COMPONENT CALLED AbilityUI ATTACHED TO, IT IF YES ASSIGN IT TO THE AbilityUi Variable
            if (abilityUI != null)
                return true;
        }

        return false;
    }
}
