using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour
{
    [SerializeField] GameObject donut;

    public GameObject selectedObject;
    Vector3 offset;

    void Update()
    {
        if (Spawncolours.stage2)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

                if (targetObject)
                {
                    selectedObject = targetObject.transform.gameObject;
                    offset = selectedObject.transform.position - mousePosition;
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
            Click.sortedColours.Add(donut.GetComponent<Click>().id);
            Click.letThroughColours.Remove(donut.GetComponent<Click>().id);
            Destroy(donut);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
