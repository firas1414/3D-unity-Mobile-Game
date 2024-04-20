using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToTarget : BTNode
{
	NavMeshAgent agent;
	string targetKey;
	GameObject target;
	float acceptableDistance = 1f;
	BehaviorTree tree; // Specify which tree we're working with, since each enemy can have his own AI Behavior Tree


	public BTTask_MoveToTarget(BehaviorTree tree, string targetKey, float acceptableDistance = 1f)
	{
		this.targetKey = targetKey;
		this.acceptableDistance = acceptableDistance;
		this.tree = tree;
	}
	protected override NodeResult Execute()
	{
		Blackboard blackboard = tree.Blackboard;
		if(blackboard == null || !blackboard.GetBlackboardData(targetKey, out target)) // In case the target not sensed
		{
			return NodeResult.Failure;
		}
		agent = tree.GetComponent<NavMeshAgent>(); // Refernce for the NavMeshAgent
		if(agent == null) // In case we don't have the agent, but most likely we have it
		{
			return NodeResult.Failure;
		}
		if(IsTargetInAcceptableDistance()) // Check if the AI Agent reached the max distance allowed between him and the target
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

	protected override void End()
	{
		agent.isStopped = true;
		tree.Blackboard.onBlackboardValueChanged -= BlackboardValueChanged;
		base.End();
	}
}
