using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree {
    
    protected override void ConstructTree(out BTNode rootNode) {
        // SeePlayer?
        Selector RootSelector = new Selector();
        Sequencer attackTargetSequencer = new Sequencer();
        BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(this, "Target", 2f);

        BTTask_RotateTowardsTarget rotateTowardsTarget = new BTTask_RotateTowardsTarget(this, "Target", 10f);
        //attackTargetSequencer.AddChild(rotateTowardsTarget);
        attackTargetSequencer.AddChild(moveToTarget);
        

        
        BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(this, attackTargetSequencer, "Target",
                                                                                  BlackboardDecorator.RunCondition.KeyExists,
                                                                                  BlackboardDecorator.NotifyRule.RunConditionChange, // We only notify when the run condition is changed, if theres a new target we dont care                                                                                    
                                                                                  BlackboardDecorator.NotifyAbort.both);

        RootSelector.AddChild(attackTargetDecorator);

        // LastSeen Location?
        Sequencer CheckLastSeenLocSequencer = new Sequencer();
        BTTask_MoveToLoc moveToLastSeenLoc = new BTTask_MoveToLoc(this, "LastSeenLoc", 2f);
        BTTask_Wait waitAtLastSeenLoc = new BTTask_Wait(4f);
        BTTask_RemoveBlackboardData removeLastSeenLoc = new BTTask_RemoveBlackboardData(this, "LastSeenLoc");

        CheckLastSeenLocSequencer.AddChild(moveToLastSeenLoc);
        CheckLastSeenLocSequencer.AddChild(waitAtLastSeenLoc);
        CheckLastSeenLocSequencer.AddChild(removeLastSeenLoc);


        BlackboardDecorator CheckLastSeenLocDecorator = new BlackboardDecorator(this, // Check if there is a last seen location                                                                                  
                                                                                  CheckLastSeenLocSequencer,
                                                                                  "LastSeenLoc",
                                                                                  BlackboardDecorator.RunCondition.KeyExists,
                                                                                  BlackboardDecorator.NotifyRule.RunConditionChange,
                                                                                  BlackboardDecorator.NotifyAbort.none
                                                                                  );
        RootSelector.AddChild(CheckLastSeenLocDecorator);



        Sequencer patrollingSeq = new Sequencer(); // Patrolling Part
        BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(this, "PatrolPoint");
        BTTask_MoveToLoc moveToPatrolPoint = new BTTask_MoveToLoc(this, "PatrolPoint", 3f);
        BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(4f);

        patrollingSeq.AddChild(getNextPatrolPoint);
        patrollingSeq.AddChild(moveToPatrolPoint);
        patrollingSeq.AddChild(waitAtPatrolPoint);
        RootSelector.AddChild(patrollingSeq);
        
        rootNode = RootSelector;


    }
}
