using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// THIS CLASS IS RESPONSIBLE FOR MAKING THE OBJECT THAT'S BEING DAMAGED BLINK
public class DamageVisualiser : MonoBehaviour
{
    [SerializeField] Renderer mesh;
    [SerializeField] Color DamageEmmisionColor;
    [SerializeField] float BlinkSpeed = 2f;
    [SerializeField] string EmmisionColorPropertyName = "_Addtion";
    [SerializeField] HealthComponent healthComponent;
    Color OrigionalEmissionColor;

    // Start is called before the first frame update
    void Start()
    {
        Material mat = mesh.material; //  It gets the material from the renderer component       
        mesh.material = new Material(mat); // It creates a new material to ensure we're not modifying the shared material

        OrigionalEmissionColor = mesh.material.GetColor(EmmisionColorPropertyName); //  It stores the original emission color of the material
        healthComponent.onTakeDamage += TookDamage; // This means that when the object takes damage, the TookDamage method will be called
    }

    protected virtual void TookDamage(float health, float delta, float maxHealth, GameObject Instigator) // This method is called when the object takes damage
    {
        Color currentEmmisionColor = mesh.material.GetColor(EmmisionColorPropertyName); // It gets the current emission color of the material
        if(Mathf.Abs((currentEmmisionColor - OrigionalEmissionColor).grayscale) < 0.1f) // It checks if the difference between the current emission color and the original emission color is very small
        {
            mesh.material.SetColor(EmmisionColorPropertyName, DamageEmmisionColor); // It sets the emission color to the damage emission color
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color currentEmmisionColor = mesh.material.GetColor(EmmisionColorPropertyName);
        Color newEmmisionColor = Color.Lerp(currentEmmisionColor, OrigionalEmissionColor, Time.deltaTime*BlinkSpeed);
        mesh.material.SetColor(EmmisionColorPropertyName, newEmmisionColor);
    }
}
