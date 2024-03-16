using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    void Start()
    {
        if(healthComponent != null){
            healthComponent.onDied += StartDeath;
            healthComponent.onTakeDamage += TakenDamge;
        }
    }

    private void TakenDamge(float health , float delta , float maxHealth){

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
