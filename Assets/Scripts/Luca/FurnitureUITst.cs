using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FurnitureUITst : MonoBehaviour {

    [SerializeField] GameObject prefab;
    [SerializeField] Canvas myCanvas;
    bool selected = false;
    Vector3 startPos;
    bool hovering = false;

    Image im;

    Furniture spawnedFurniture;

	void Start () {
        startPos = transform.position;
        im = GetComponent<Image>();
	}
	
	
	void Update () {

        if (selected)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos);


            if(Input.mousePosition.x <0.8f * Screen.width)
            {
                im.color = Color.clear;
                if (spawnedFurniture == null)
                {
                    spawnedFurniture =  Instantiate(prefab).GetComponent<Furniture>();
                    spawnedFurniture.OnPlaced += OnFurniturePlaced;
                }
            }
            else
            {
                im.color = Color.white;
                if(spawnedFurniture!=null)
                Destroy(spawnedFurniture.gameObject);

                if (Input.GetMouseButtonDown(0))
                {
                    transform.position = startPos;
                    selected = false;
                }

            }

        }
        else
        {
            if (hovering)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    selected = true;
                }
            }
        }

        
        

	}

    public void OnFurniturePlaced()
    {
        Destroy(gameObject);
    }


    public void OnPointerEnter(BaseEventData data)
    {
        hovering = true;
    }

    public void OnPointerExit(BaseEventData data)
    {
        hovering = false;
    }
}
