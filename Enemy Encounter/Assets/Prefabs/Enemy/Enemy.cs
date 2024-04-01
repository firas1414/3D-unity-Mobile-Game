using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp;

    GameObject Target;


    private void TakenDamge(float health , float amount , float maxHealth)
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
        if(sensed)
        {
            Target = target;
            Debug.Log($"I sensed {Target}");
        }
        else
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
