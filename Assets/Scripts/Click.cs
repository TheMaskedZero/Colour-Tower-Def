using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{

    [SerializeField] GameObject point;
    public Vector2 id;
    public static List<Vector2> allColours = new List<Vector2>();
    public static List<Vector2> chosenColours = new List<Vector2>();
    public static List<Vector2> sortedColours = new List<Vector2>();
    public static List<Vector2> letThroughColours = new List<Vector2>();

    public static List<GameObject> letThroughGO = new List<GameObject>();

    public static int disabledClick = 0;

    [SerializeField] GameObject arrows;

    private void OnMouseDown()
    {
        chosenColours.Add(point.GetComponent<Click>().id);
        //allColours.Remove(point.GetComponent<Click>().id);

        if (Spawncolours.stage2 == false)
        {
            GameObject arrow = Instantiate(arrows, new Vector2(Random.Range(9.6f, 9.6f), Random.Range(-4.5f, 4.5f)), Quaternion.Euler(0, 0, 90));
            arrow.GetComponent<Arrow>().donutTarget = point;
        }
        
        //point.SetActive(false);
        /*foreach (var i in chosenColours)
        {
            Debug.Log(i[0] +", " + i[1]);
        }
        foreach (var u in allColours)
        {
            Debug.Log(u[0] + ", " + u[1]);

        }*/
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
