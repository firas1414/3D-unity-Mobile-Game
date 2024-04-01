using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The PerceptionStimuli class, can be attached to any game object to signify that it can be perceived or sensed by other components in the game.
public class PerceptionStimuli : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SenseComponent.RegisterStimuli(this); // "this" is a stimuli
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy(){
        SenseComponent.UnRegisterStimuli(this);
    }
}
