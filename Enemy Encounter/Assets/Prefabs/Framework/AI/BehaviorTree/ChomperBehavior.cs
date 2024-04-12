using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {
        // Create a Node
        BTTask_Wait waitTask = new BTTask_Wait(2f);
        BTTask_Log Log = new BTTask_Log("selket");

        Sequencer Root = new Sequencer();
        Root.AddChild(Log);
        Root.AddChild(waitTask);

        rootNode=Root;


    }
}
