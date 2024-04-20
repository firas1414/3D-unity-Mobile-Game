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
    float rotationSpeed = 5f; // Adjust the rotation speed as needed
    BehaviorTree tree; 

    public BTTask_MoveToTarget(BehaviorTree tree, string targetKey, float acceptableDistance = 1f)
    {
        this.tree = tree;
        this.targetKey = targetKey;
        this.acceptableDistance = acceptableDistance;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = tree.Blackboard;
        if (blackboard == null || !blackboard.GetBlackboardData(targetKey, out target))
        {
            return NodeResult.Failure;
        }

        agent = tree.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            return NodeResult.Failure;
        }

        blackboard.onBlackboardValueChanged += BlackboardValueChanged;
        agent.SetDestination(target.transform.position);
        agent.isStopped = false;

        return NodeResult.InProgress;
    }

    public void BlackboardValueChanged(string key, object val)
    {
        if (key == targetKey)
        {
            target = (GameObject)val;
        }
    }

    protected override NodeResult Update()
    {
        if (target == null)
        {
            agent.isStopped = true;
            return NodeResult.Failure;
        }

        agent.SetDestination(target.transform.position);

        RotateTowardsTarget();

        if (IsTargetInAcceptableDistance())
        {
            agent.isStopped = true;
            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }

    bool IsTargetInAcceptableDistance()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, tree.transform.position.y, target.transform.position.z);
        return Vector3.Distance(targetPosition, tree.transform.position) <= acceptableDistance;
    }

    protected override void End()
    {
        agent.isStopped = true;
        tree.Blackboard.onBlackboardValueChanged -= BlackboardValueChanged;
        base.End();
    }

    // Rotate towards the target smoothly
    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - tree.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            tree.transform.rotation = Quaternion.Lerp(tree.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
