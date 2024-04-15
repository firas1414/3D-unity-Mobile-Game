using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {
        /*// Create a Node
        BTTask_Wait waitTask = new BTTask_Wait(2f);

        Sequencer Root = new Sequencer();
        Root.AddChild(waitTask);*/
        BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(this, "Target");
        rootNode = moveToTarget;


    }
}
