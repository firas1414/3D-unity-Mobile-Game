using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBehaviorTreeInterface, ITeamInterface, ISpawnInterface
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp;
    [SerializeField] BehaviorTree behaviorTree;
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] int TeamID = 2;
    
    Vector3 prevPos;
    [SerializeField] Reward killReward;

    public int GetTeamID()
    {
        return TeamID;
    }
    public Animator Animator
    {
        get { return animator; }
        private set { animator = value; }
    }

    private void Awake()
    {
        perceptionComp.onPerceptionTargetChanged += TargetChanged;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(healthComponent!=null)
        {
            healthComponent.onHealthEmpty += StartDeath;
            healthComponent.onTakeDamage += TakenDamage;
        }
        prevPos = transform.position;
    }

    private void TargetChanged(GameObject target, bool sensed)
    {
        if(sensed)
        {
            behaviorTree.Blackboard.SetOrAddData("Target", target);
        }
        else
        {
            behaviorTree.Blackboard.SetOrAddData("LastSeenLoc", target.transform.position);
            behaviorTree.Blackboard.RemoveBlackboardData("Target");
        }
    }

    private void TakenDamage(float health, float delta, float maxHealth, GameObject Instigator)
    {
        
    }

    private void StartDeath(GameObject Killer)
    {
        TriggerDeathAnimation();
        behaviorTree.StopLogic();
        GetComponent<CapsuleCollider>().enabled = false;
        IRewardListener[] RewardListeners = Killer.GetComponents<IRewardListener>();
        foreach(IRewardListener listener in RewardListeners)
        {
            listener.Reward(killReward);
        }
    }

    private void TriggerDeathAnimation()
    {
        if(animator!= null)
        {
            animator.SetTrigger("Dead");
        }
    }

    public void OnDeathAnimationFinished()
    {
        Dead();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSpeed();
        if (transform.position.y < -100)
        {
            StartDeath(gameObject);
            Debug.Log("Enemy Dropped to oblivion");
        }
    }

    private void CalculateSpeed()
    {
        if (movementComponent == null) return; // THIS MAKES SURE THAT THIS FUNCTION ONLY WORKS WITH CHOMPER AND SPITTER BECAUSE THE SPAWNER DOESENT A MOVEMENT MECHANISM? ITS FIXED

        Vector3 posDelta = transform.position - prevPos;
        float speed = posDelta.magnitude / Time.deltaTime; // .magnitude gives us the distance moved(length of the vector), Time.deltaTime is the time passed, so distance/time = speed
        Animator.SetFloat("Speed", speed);
        prevPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        if(behaviorTree && behaviorTree.Blackboard.GetBlackboardData("Target", out GameObject target))
        {
            Vector3 drawTragetPos = target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTragetPos, 0.7f);

            Gizmos.DrawLine(transform.position + Vector3.up, drawTragetPos);
        }
    }

    public void RotateTowards(GameObject target, bool vertialAim = false)
    {
        Vector3 AimDir = target.transform.position - transform.position;
        AimDir.y = vertialAim ? AimDir.y : 0;
        AimDir = AimDir.normalized;

        movementComponent.RotateTowards(AimDir);
    }

    public virtual void AttackTarget(GameObject target)
    {
        //override in child
    }

    public void SpawnedBy(GameObject spawnerGameobject) // CALLED WHEN THIS ENEMY IS SPAWNED BY A SPAWNER
    {
        BehaviorTree spawnerBehaviorTree = spawnerGameobject.GetComponent<BehaviorTree>();
        if(spawnerBehaviorTree!=null && spawnerBehaviorTree.Blackboard.GetBlackboardData<GameObject>("Target", out GameObject spawnerTarget))
        {
            PerceptionStimuli targetStimuli = spawnerTarget.GetComponent<PerceptionStimuli>();
            if(perceptionComp && targetStimuli)
            {
                perceptionComp.AssignPercievedStimui(targetStimuli);
            }
        }
    }

    protected virtual void Dead() { }
}
