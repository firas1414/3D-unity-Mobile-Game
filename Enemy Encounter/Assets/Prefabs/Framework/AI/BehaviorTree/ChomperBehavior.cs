using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {
        /*// Create a Node
        BTTask_Wait waitTask = new BTTask_Wait(2f);

        Sequencer Root = new Sequencer();
        Root.AddChild(waitTask);*/
        Sequencer patrollingSeq = new Sequencer();
        BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(this, "PatrolPoint");
        BTTask_MoveToLoc moveToPatrolPoint = new BTTask_MoveToLoc(this, "PatrolPoint", 3);
        BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(2f);

        patrollingSeq.AddChild(getNextPatrolPoint);
        patrollingSeq.AddChild(moveToPatrolPoint);
        patrollingSeq.AddChild(waitAtPatrolPoint);
        rootNode = patrollingSeq;


    }
}
