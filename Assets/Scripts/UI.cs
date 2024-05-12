using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    public GameObject tutorialScreen;

    public GameObject levelSelect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void Level1()
    {
        levelSelect.SetActive(false);

        Time.timeScale = 1f;
    }
}
