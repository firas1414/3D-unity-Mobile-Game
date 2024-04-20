using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BTTask_MoveToLoc : BTNode
{
	NavMeshAgent agent;
	string locationKey;
	Vector3 location;
	float acceptableDistance;
	BehaviorTree tree; // Specify which tree we're working with, since each enemy can have his own AI Behavior Tree

	public BTTask_MoveToLoc(BehaviorTree tree, string locationKey, float acceptableDistance)
	{
		this.tree = tree;
		this.locationKey = locationKey;
		this.acceptableDistance = acceptableDistance;
	}

	protected override NodeResult Execute()
	{
		Blackboard blackboard = tree.Blackboard;
		if(blackboard == null || !blackboard.GetBlackboardData(locationKey, out location))
		{
			return NodeResult.Failure;
		}
		agent = tree.GetComponent<NavMeshAgent>(); // Refernce for the NavMeshAgent
		if(agent == null) // In case we don't have the agent, but most likely we have it
		{
			return NodeResult.Failure;
		}
		if(IsLocationInAcceptableDistance()) // Check if the AI Agent reached the max distance allowed between him and the target
		{
			return NodeResult.Success;
		}
		//Debug.Log($"Moving");
		agent.SetDestination(location);
		agent.isStopped = false;
		return NodeResult.InProgress;
	}


	protected override NodeResult Update()
	{
		if(IsLocationInAcceptableDistance())
		{
			agent.isStopped = true;
			//Debug.Log($"Moved");
			return NodeResult.Success;
		}
		return NodeResult.InProgress;
	}


	private bool IsLocationInAcceptableDistance()
	{
		return Vector3.Distance(location, tree.transform.position) <= acceptableDistance;
	}

	protected override void End()
	{
		agent.isStopped = true;
		base.End();
	}
}
