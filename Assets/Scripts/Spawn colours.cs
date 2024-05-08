using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawncolours : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;

    [SerializeField] GameObject colourCircle;
    [SerializeField] GameObject borderDonut;
    [SerializeField] GameObject blackBox;
    [SerializeField] int totalDisabled;
    public Color colorConverted;
    private int i;

    public int spawnAmount;

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

    public Vector2 randomizePosition;

    public void StartGame()
    {
        startScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;

        for (int i = 0; i < spawnAmount; i++)
        {
            //var randomXY = Random.Range(0, (CIE1931xyCoordinates.Length - 1));
            colorConverted = blackBox.GetComponent<ColourCoordinateConverter>().ConvertxyTosRGB(CIE1931xyCoordinates[i]);
            if (i < spawnAmount - 24)
            {
                randomizePosition = new Vector2(Random.Range(-12f, -6f), Random.Range(4.3f, -4.3f));
            }
            else if (i < spawnAmount - 12)
            {
                randomizePosition = new Vector2(Random.Range(-22f, -16f), Random.Range(4.3f, -4.3f));
            }
            else if (i <= spawnAmount)
            {
                randomizePosition = new Vector2(Random.Range(-36f, -28f), Random.Range(4.3f, -4.3f));
            }

            GameObject circle = Instantiate(colourCircle, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);
            GameObject donut = Instantiate(borderDonut, new Vector2(randomizePosition[0], randomizePosition[1]), Quaternion.identity);

            colourCircle.GetComponent<SpriteRenderer>().sortingOrder = +i;
            borderDonut.GetComponent<SpriteRenderer>().sortingOrder = +i + 1;

            colourCircle.GetComponent<SpriteRenderer>().color = colorConverted;

            circle.transform.SetParent(donut.transform);

            donut.GetComponent<Click>().id = CIE1931xyCoordinates[i];

            Click.allColours.Add(donut.GetComponent<Click>().id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        totalDisabled = Move.disabledMove + Click.disabledClick;
        if (spawnAmount == Move.disabledMove + Click.disabledClick)
        {
            endScreen.SetActive(true);
            CreateText();
            Click.chosenColours.Clear();
            Click.letThroughColours.Clear();
            Click.allColours.Clear();
            spawnAmount++;
        }
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
        }
    }
}