using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// RESPONSIBLE FOR MANAGING LEVELS, THIS CLASS MAKE US FLEXIBLE, WE CAN EASILY CREATE NEW LEVELS WITHOUT CHANGING MUCH IN THE CODE WHEN WE HAVE THIS CLASS
[CreateAssetMenu(menuName ="LevelManager")]
public class LevelManager : ScriptableObject
{
    [SerializeField] int MainMenuBuildIndex = 0;
    [SerializeField] int FirstLevelBuildIndex = 1;

    public delegate void OnLevelFinished();
    public static event OnLevelFinished onLevelFinished;

    internal static void LevelFinished()
    {
        onLevelFinished?.Invoke();
    }


    // This line tells Unity to load the scene that corresponds to the index stored in the variable MainMenuBuildIndex.
    // This means it will load the scene that you've designated as the main menu scene in your game.
    // It's like telling Unity, "Hey, go load the main menu scene!"
    public void GoToMainMenu()
    {
        LoadSceneByIndex(MainMenuBuildIndex);
    }


    // Here, you're instructing Unity to load the scene that corresponds to the index stored in the variable FirstLevelBuildIndex.
    // This means it will load the scene you've designated as the first level of your game.
    // So, when your game starts or when you want to move on to the first level, this line makes it happen.
    public void LoadFirstLevel()
    {
        LoadSceneByIndex(FirstLevelBuildIndex);
    }


    // This line is a bit different. SceneManager.GetActiveScene().buildIndex gets the index of the currently active scene.
    // So, when you call LoadSceneByIndex with this argument, you're telling Unity to reload the scene that is currently running. It's like saying,
    // "Hey, reload the scene we're in right now!" This might be used for things like restarting a level after the player fails a task or wants to try again.
    public void RestartCurrentLevel()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
        GameplayStatics.SetGamePaused(false);
    }
}
