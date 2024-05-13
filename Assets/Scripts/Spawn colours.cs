using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawncolours : MonoBehaviour
{
    //Ref colour stuff
    [SerializeField] GameObject bannerRefColour1;
    [SerializeField] GameObject bannerRefColour2;
    [SerializeField] GameObject stageRefColour;

    [SerializeField] GameObject doneButton;

    //Spawning stuff
    [SerializeField] GameObject colourCircle;
    [SerializeField] GameObject borderDonut;
    [SerializeField] GameObject blackBox;
    public int spawnAmount;
    public Vector2 randomizePosition;
    public Color colorConverted;
    private int p;

    //Stage 2 stuff
    public static bool stage2 = false;

    public Transform[] stage2Spots;
    public bool[] availableSpots;
    private List<GameObject> sortingGO = new List<GameObject>();

    //Level value
    public static int selectedLevel;

    Vector2[] CIE1931xyCoordinates = new Vector2[]{
    new Vector2(0.39f,0.237f),
    new Vector2(0.388660254f,0.242f),
    new Vector2(0.385f,0.245660254f),
    new Vector2(0.38f,0.247f),
    new Vector2(0.375f,0.245660254f),
    new Vector2(0.371339746f,0.242f),
    new Vector2(0.37f,0.237f),
    new Vector2(0.371339746f,0.232f),
    new Vector2(0.375f,0.228339746f),
    new Vector2(0.38f,0.227f),
    new Vector2(0.385f,0.228339746f),
    new Vector2(0.388660254f,0.232f),
    new Vector2(0.4f,0.237f),
    new Vector2(0.3973205081f,0.247f),
    new Vector2(0.39f,0.2543205081f),
    new Vector2(0.38f,0.257f),
    new Vector2(0.37f,0.2543205081f),
    new Vector2(0.3626794919f,0.247f),
    new Vector2(0.36f,0.237f),
    new Vector2(0.3626794919f,0.227f),
    new Vector2(0.37f,0.2196794919f),
    new Vector2(0.38f,0.217f),
    new Vector2(0.39f,0.2196794919f),
    new Vector2(0.3973205081f,0.227f),
    new Vector2(0.395f,0.237f),
    new Vector2(0.3929903811f,0.2445f),
    new Vector2(0.3875f,0.2499903811f),
    new Vector2(0.38f,0.252f),
    new Vector2(0.3725f,0.2499903811f),
    new Vector2(0.3670096189f,0.2445f),
    new Vector2(0.365f,0.237f),
    new Vector2(0.3670096189f,0.2295f),
    new Vector2(0.3725f,0.2240096189f),
    new Vector2(0.38f,0.222f),
    new Vector2(0.3875f,0.2240096189f),
    new Vector2(0.3929903811f,0.2295f),
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //totalDisabled = Move.disabledMove + Click.disabledClick;
        if (Move.disabledMove + Click.disabledClick == spawnAmount)
        {
            Debug.Log("if statement ran");
            stage2 = true;
            doneButton.SetActive(true);

            GameObject.Find("Main Camera").transform.position = new Vector3(0, -11, -10);

            spawnAmount++;
        }

        if (stage2)
        {
            Move.disabledMove = 0;
            Click.disabledClick = 0;
            for (int i = 0; i < Click.letThroughGO.Count; i++)
            {
                Stage2SpawnDots();
            }
        }
    }

    public void SpawnDots()
    {
        for (p = 0; p < spawnAmount; p++)
        {
            //var randomXY = Random.Range(0, (CIE1931xyCoordinates.Length - 1));
            colorConverted = blackBox.GetComponent<ColourCoordinateConverter>().ConvertxyTosRGB(CIE1931xyCoordinates[p]);
            
            if (p < spawnAmount - 24)
            {
                randomizePosition = new Vector2(Random.Range(-12f, -6f), Random.Range(4.3f, -4.3f));
            }
            else if (p < spawnAmount - 12)
            {
                randomizePosition = new Vector2(Random.Range(-22f, -16f), Random.Range(4.3f, -4.3f));
            }
            else if (p <= spawnAmount)
            {
                randomizePosition = new Vector2(Random.Range(-32f, -26f), Random.Range(4.3f, -4.3f));
            }

            GameObject circle = Instantiate(colourCircle, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);
            GameObject donut = Instantiate(borderDonut, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);

            colourCircle.GetComponent<SpriteRenderer>().sortingOrder = +p;
            borderDonut.GetComponent<SpriteRenderer>().sortingOrder = +p + 1;

            colourCircle.GetComponent<SpriteRenderer>().color = colorConverted;

            circle.transform.SetParent(donut.transform);

            donut.GetComponent<Click>().id = CIE1931xyCoordinates[p];

            Click.allColours.Add(donut.GetComponent<Click>().id);
        }
    }

    public void Stage2SpawnDots()
    {
        if (Click.letThroughGO.Count >= 1)
        {
            GameObject randGO = Click.letThroughGO[Random.Range(0, Click.letThroughGO.Count)];
            for (int i = 0; i < availableSpots.Length; i++)
            {
                if (availableSpots[i] == true)
                {
                    randGO.gameObject.SetActive(true);
                    randGO.transform.position = stage2Spots[i].position;
                    availableSpots[i] = false;
                    sortingGO.Add(randGO);
                    Click.letThroughGO.Remove(randGO);
                    return;
                }
            }
        }
    }

    public void DoneSorting()
    {
        Debug.Log("Done button pressed");

        p = 0;
        selectedLevel = 0;
        stage2 = false;
        spawnAmount--;

        doneButton.SetActive(false);
        UI.levelSelectScreen.SetActive(true);
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -10);

        foreach (var dot in sortingGO)
        {
            Destroy(dot);
        }
        for (int i = 0; i < availableSpots.Length; i++)
        {
            if (availableSpots[i] == false)
            {
                availableSpots[i] = true;
            }
        }
        CreateText();
        Click.chosenColours.Clear();
        Click.letThroughColours.Clear();
        Click.allColours.Clear();
        Click.sortedColours.Clear();
        Click.letThroughGO.Clear();
    }

    void CreateText()
    {
        string path = Application.persistentDataPath + "/Tower Defense Data log.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "log \n\n");
        }

        if (File.Exists(path))
        {
            File.AppendAllText(path, "\n");
            File.AppendAllText(path, "\n");

            foreach (var y in Click.chosenColours)
            {
                string chosenData = "Chosen ID: " + "(" + y[0] + ", " + y[1] + ") ";
                File.AppendAllText(path, chosenData);

            }

            File.AppendAllText(path, "\n");

            foreach (var x in Click.letThroughColours)
            {
                string data = "Remaining: " + "(" + x[0] + ", " + x[1] + ") ";
                File.AppendAllText(path, data);
            }
            
            foreach (var x in Click.sortedColours)
            {
                string data = "Remaining: " + "(" + x[0] + ", " + x[1] + ") ";
                File.AppendAllText(path, data);
            }
        }
    }
}