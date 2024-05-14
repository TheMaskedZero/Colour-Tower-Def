using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour
{
    [SerializeField] GameObject donut;

    public static int spotIndex;
    public Spawncolours SC;

    public GameObject selectedObject;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Spawncolours.stage2)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject.tag != "Wall")
                {
                    if (targetObject)
                    {
                        selectedObject = targetObject.transform.gameObject;
                        offset = selectedObject.transform.position - mousePosition;
                    }
                }
            }

            if (selectedObject)
            {
                selectedObject.transform.position = mousePosition + offset;
            }

            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                selectedObject = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Stage2 Gate")
        {
            /*if (Click.letThroughGO.Count >= 1)
            {
                SC.availableSpots[spotIndex] = true;
                SC.Stage2SpawnDots();
            }*/
            Click.sortedColours.Add(Spawncolours.elapsedTime, donut.GetComponent<Click>().id);
            Click.letThroughColours.Remove(donut.GetComponent<Click>().id);
            Destroy(donut);
        }
    }
}
