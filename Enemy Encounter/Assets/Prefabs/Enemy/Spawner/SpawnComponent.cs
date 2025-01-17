using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] Transform spawnTransform;
    [Header("Audio")]
    
    [SerializeField] AudioClip SpawnAudio;
    [SerializeField] float volume = 1f;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool StartSpawn()
    {
        if (objectsToSpawn.Length == 0) return false;

        if(animator!=null)
        {
            animator.SetTrigger("Spawn");
        }
        else
        {
            SpawnImpl();
        }

        Vector3 SpawnAudioLoc = transform.position;
        GameplayStatics.PlayAudioAtLoc(SpawnAudio, SpawnAudioLoc, volume);

        return true;

    }

    public void SpawnImpl() // THIS IS TRIGGERED FROM THE ANIMATOR, WHEN THE SPAWN VARIABLE IS SET TO TRUE
    {
        int randomPick = Random.Range(0, objectsToSpawn.Length);
        GameObject newSpawn = Instantiate(objectsToSpawn[randomPick], spawnTransform.position, spawnTransform.rotation);
        ISpawnInterface newSpawnInterface = newSpawn.GetComponent<ISpawnInterface>();
        if(newSpawnInterface!=null)
        {
            newSpawnInterface.SpawnedBy(gameObject); // CALLS THE SawnedBy function implemented by the Enemy Class
        }
    }
}
