using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// THIS CLASS REGISTERS THE GAMEOBJECT WHO HAS THIS CLASS ATTACHED TO IT AS A PERCEIVABLE STIMULI
public class PerceptionStimuli : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SenseComp.RegisterStimuli(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        SenseComp.UnRegisterStimuli(this);
    }
}
