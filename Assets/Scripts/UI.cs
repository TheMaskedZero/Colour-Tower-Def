using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    public GameObject tutorialScreen;

    public GameObject levelScreen;
    public static GameObject levelSelectScreen;

    // Start is called before the first frame update
    void Start()
    {
        levelSelectScreen = levelScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void Level1()
    {
        levelSelectScreen.SetActive(false);
        Spawncolours.selectedLevel = 1;
    }

    public void Level2()
    {
        levelSelectScreen.SetActive(false);
        Spawncolours.selectedLevel = 2;
    }

    public void Level3()
    {
        levelSelectScreen.SetActive(false);
        Spawncolours.selectedLevel = 3;
    }
}
