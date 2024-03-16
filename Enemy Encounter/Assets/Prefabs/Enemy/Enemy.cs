using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;




    private void TakenDamge(float health , float amount , float maxHealth){

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
