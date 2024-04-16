using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardDecorator : Decorator // This will be reacting to blacboard changes, and do someting about the child
{   // This is designed to inform wether the key exists or not, so that the AI know wether to run the task or not run it at all
	public enum RunCondition 
	{
		KeyExists,
		KeyNotExists
	}
	// This is designed to notify whenever the RunCondition changes while the task is already running
	public enum NotifyRule 
	{
		RunConditionChange, // Means RunCondition changed
		KeyValueChange // Value changed
	}
	// This is designed to tell the AI what to do after being Notified about the change(it's time tointerupt something)')
	public enum NotifyAbort
	{
		none, // Mean the AI won't interrupt any task
		self, // In case the enemy is sensing a target and then immidiately is loses track
		lower, // When RunCondition changes from KeyExists to KeyNotExists
		both
	}

	BehaviorTree tree;
	string key;

	RunCondition runCondition;
	NotifyRule notifyRule;
	NotifyAbort notifyAbort;


	public BlackboardDecorator(BTNode child, BehaviorTree tree, string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(child)
	{
		this.tree = tree;
		this.key = key;
		this.runCondition = runCondition;
		this.notifyRule = notifyRule;
		this.notifyAbort = notifyAbort;
	}

	protected override NodeResult Execute()
	{
		Blackboard blackboard = tree.Blackboard;
		if(blackboard == null)
		{
			return NodeResult.Failure;
		}
	    blackboard.onBlackboardValueChanged += CheckNotify;
	    if(CheckRunCondition())
		{
			return NodeResult.InProgress;
		}
		else
		{
			return NodeResult.Failure;
		}
	}

	private bool CheckRunCondition()
	{
		return true;
	}

	private void CheckNotify(string key, object val)
	{

	}

	protected override NodeResult Update()
	{
		return GetChild().UpdateNode();
	}


}
