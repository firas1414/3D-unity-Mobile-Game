using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {
        // Create a Node
        rootNode = new BTTask_Wait(2f);
    }
}
