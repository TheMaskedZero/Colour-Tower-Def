using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawncolours : MonoBehaviour
{
    [SerializeField] GameObject doneButton;
    public static float elapsedTime;
    public static bool timeStart = false;

    //Spawning stuff
    [SerializeField] GameObject colourCircle;
    [SerializeField] GameObject borderDonut;
    [SerializeField] GameObject blackBox;
    public Vector2 randomizePosition;
    public Color colorConverted;
    private int p;

    private int spawned;
    public static bool stage1 = false;
    public static List<Vector2> CIE1931xyCoordinates = new List<Vector2>() { new Vector2(0f,0f)};
    public List<Vector2> checkList;
    public static int maxSpawn = 99;

    //Stage 2 stuff
    public static bool stage2 = false;
    public Transform[] stage2Spots;
    public bool[] availableSpots;
    private List<GameObject> sortingGO = new List<GameObject>();

    private float stage2Start;

    //Level value
    public static int selectedLevel;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStart)
        {
            elapsedTime += Time.deltaTime;
        }

        checkList = CIE1931xyCoordinates;
        if (stage1)
        {
            if (spawned - (Move.disabledMove + Click.disabledClick) < 7 && CIE1931xyCoordinates.Count != 0)
            {
                SpawnDots();
                spawned++;
            }
        }
        //totalDisabled = Move.disabledMove + Click.disabledClick;
        if (Move.disabledMove + Click.disabledClick == maxSpawn)
        {
            stage1 = false;
            stage2 = true;
            stage2Start = elapsedTime;
            doneButton.SetActive(true);

            GameObject.Find("Main Camera").transform.position = new Vector3(0, -11, -10);
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
        //var randomXY = Random.Range(0, (CIE1931xyCoordinates.Length - 1));
        Vector2 currentDot = CIE1931xyCoordinates[Random.Range(0, CIE1931xyCoordinates.Count)];
        CIE1931xyCoordinates.Remove(currentDot);
        colorConverted = blackBox.GetComponent<ConvertToP3>().Convert(currentDot);

        randomizePosition = new Vector2(Random.Range(-13f, -11f), Random.Range(-1f, -5f));

        GameObject circle = Instantiate(colourCircle, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);
        GameObject donut = Instantiate(borderDonut, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);

        //colourCircle.GetComponent<SpriteRenderer>().sortingOrder = +p;
        //borderDonut.GetComponent<SpriteRenderer>().sortingOrder = +p + 1;

        circle.GetComponent<SpriteRenderer>().color = colorConverted;

        circle.transform.SetParent(donut.transform);

        donut.GetComponent<Click>().id = currentDot;

        //Click.allColours.Add(donut.GetComponent<Click>().id, elapsedTime);
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

        stage2 = false;
        spawned = 0;
        timeStart = false;
        elapsedTime = 0;

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
        //Click.allColours.Clear();
        Click.sortedColours.Clear();
        Click.letThroughGO.Clear();
    }

    void CreateText()
    {
        string path = Application.persistentDataPath + "/Tower Defense Data log.csv";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Level Time x y");
        }

        if (File.Exists(path))
        {
            File.AppendAllText(path, "\n");
            File.AppendAllText(path, "Selected: \n");

            string level = selectedLevel.ToString();

            foreach (var y in Click.chosenColours)
            {
                string chosenData = level + " " + y.Key + " " + y.Value[0] + " " + y.Value[1];
                File.AppendAllText(path, chosenData);
                File.AppendAllText(path, "\n");
            }

            File.AppendAllText(path, "\n");
            File.AppendAllText(path, "Stage 2 start:" + stage2Start +"\n");
            File.AppendAllText(path, "Sorted: \n");

            foreach (var x in Click.sortedColours)
            {
                string sortedData = level + " " + x.Key + " " + x.Value[0] + " " + x.Value[1];
                File.AppendAllText(path, sortedData);
                File.AppendAllText(path, "\n");
            }

            File.AppendAllText(path, "\n");
            File.AppendAllText(path, "Let through: \n");

            foreach (var x in Click.letThroughColours)
            {
                string letThroughData = level + " " + x[0] + " " + x[1];
                File.AppendAllText(path, letThroughData);
                File.AppendAllText(path, "\n");
            }

            File.AppendAllText(path, "\n");
            File.AppendAllText(path, "\n");
        }
    }
}