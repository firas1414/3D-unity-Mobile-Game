using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp; // This will be taking care of the enemy's all senses(Seing, Feeling)

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
            Target = target;
            Debug.Log($"I sensed {Target}");
        }
        else // The enemy does not have any targets
        {
            Target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if(Target != null)
        {
            Vector3 drawTargetPos = Target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }
}
