using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_GetNextPatrolPoint : BTNode
{
	PatrollingComponent patrollingComponent;
	BehaviorTree tree;
	string patrolPointKey;


	public BTTask_GetNextPatrolPoint(BehaviorTree tree, string patrolPointKey )
	{
		patrollingComponent = tree.GetComponent<PatrollingComponent>();
		this.tree = tree;
		this.patrolPointKey = patrolPointKey;
	}

	protected override NodeResult Execute()
	{
		if(patrollingComponent != null && patrollingComponent.GetNextPatrolPoint(out Vector3 point))
		{
			//Debug.Log($"Got the next");
			tree.Blackboard.SetOrAddData(patrolPointKey, point); // We consistently maintain a key named 'Patrol Point,' with its value dynamically updating to reflect the position of the current patrol point.
			return NodeResult.Success;
		}
		return NodeResult.Failure;
	}
}
