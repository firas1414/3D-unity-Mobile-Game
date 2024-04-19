using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, BehaviorTreeInterface
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp; // This will be taking care of the enemy's all senses(Seing, Feeling)
    [SerializeField] BehaviorTree behaviorTree;
    [SerializeField] MovementComponent movementComponent;
    GameObject Target;


    private void TakenDamge(float health , float amount , float maxHealth, GameObject Hitter)
    {

    }

    private void StartDeath(){
        TriggerDeathAnimation();

    }

    private void TriggerDeathAnimation(){
        if(animator!=null){
            animator.SetTrigger("Dead");
        }
    }

    public void OnDeathAnimationFinished(){
        Destroy(gameObject);
    }


        void Start()
    {
        if(healthComponent != null){
            healthComponent.onDied += StartDeath;
            healthComponent.onTakeDamage += TakenDamge;
        }
        perceptionComp.onTargetChanged += TargetChanged;
    }

    private void TargetChanged(GameObject target, bool sensed)
    {
        if(sensed) // The enemy have a target
        {
            behaviorTree.Blackboard.SetOrAddData("Target", target);
            Debug.Log($"I sensed {Target}");
        }
        else // The enemy does not have any targets
        {
            behaviorTree.Blackboard.SetOrAddData("LastSeenLoc", target.transform.position); // Add data about thelast seen location
            behaviorTree.Blackboard.RemoveBlackboardData("Target");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if(behaviorTree && behaviorTree.Blackboard.GetBlackboardData("Target", out GameObject Target))
        {
            Vector3 drawTargetPos = Target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }

    public void RotationTowards(GameObject target, bool verticalAim = false)
    {
        Vector3 AimDir = target.transform.position - transform.position;
        AimDir.y = verticalAim ? AimDir.y : 0;
        AimDir = AimDir.normalized;
        if(verticalAim)
        movementComponent.RotationTowards(AimDir);
    }
}
