using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {

        Selector RootSelector = new Selector();
        Sequencer attackTargetSequencer = new Sequencer();
        BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(this, "Target", 2f);
        attackTargetSequencer.AddChild(moveToTarget);
        BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(this, attackTargetSequencer, "Target",
                                                                                  BlackboardDecorator.RunCondition.KeyExists,
                                                                                  BlackboardDecorator.NotifyRule.RunConditionChange, // We only notify when the run condition is changed, if theres a new target we dont care                                                                                    
                                                                                  BlackboardDecorator.NotifyAbort.both);

        RootSelector.AddChild(attackTargetDecorator);

        Sequencer patrollingSeq = new Sequencer(); // Patrolling Part
        BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(this, "PatrolPoint");
        BTTask_MoveToLoc moveToPatrolPoint = new BTTask_MoveToLoc(this, "PatrolPoint", 3f);
        BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(4f);

        patrollingSeq.AddChild(getNextPatrolPoint);
        patrollingSeq.AddChild(moveToPatrolPoint);
        patrollingSeq.AddChild(waitAtPatrolPoint);
        RootSelector.AddChild(patrollingSeq);
        
        // BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(this, "Target", 2f);
        rootNode = RootSelector;


    }
}
