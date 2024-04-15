using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToTarget : BTNode
{
	NavMeshAgent agent;
	string targetKey;
	GameObject target;
	float acceptableDistance = 2f;
	BehaviorTree tree;
	public BTTask_MoveToTarget(BehaviorTree tree, string targetKey, float acceptableDistance = 2f)
	{
		this.tree = tree;
		this.targetKey = targetKey;
		this.acceptableDistance = acceptableDistance;
	}
	protected override NodeResult Execute()
	{
		Blackboard blackboard = tree.Blackboard;
		if(blackboard == null || !blackboard.GetBlackboardData(targetKey, out target))
		{
			return NodeResult.Failure;
		}
		agent = tree.GetComponent<NavMeshAgent>();
		if(agent == null)
		{
			return NodeResult.Failure;
		}
		if(IsTargetInAcceptableDistance())
		{
			return NodeResult.Success;
		}

		blackboard.onBlackboardValueChanged += BlackboardValueChanged;

		agent.SetDestination(target.transform.position);
		agent.isStopped = false;
		return NodeResult.InProgress;
	}

	public void BlackboardValueChanged(string key, object val)
	{
		if(key == targetKey)
		{
			target = (GameObject)val;
		}
	}

	protected override NodeResult Update()
	{
		if(target == null)
		{
			agent.isStopped = true;
			return NodeResult.Failure;
		}
		agent.SetDestination(target.transform.position);
		if(IsTargetInAcceptableDistance())
		{
			agent.isStopped = true;
			return NodeResult.Success;
		}
		return NodeResult.InProgress;
	}

	bool IsTargetInAcceptableDistance()
	{
		return Vector3.Distance(target.transform.position, tree.transform.position) <= acceptableDistance;
	}
}
