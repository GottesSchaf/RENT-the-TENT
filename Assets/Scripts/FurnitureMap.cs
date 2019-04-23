using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMap : MonoBehaviour {

    public static FurnitureMap Instance;

    [SerializeField] bool debug;
    [SerializeField] Vector2Int houseSize;

    private Plane surfacePlane;

    private float tileSize;

    private bool[,] occupationMap;

	void Awake () {

		if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        surfacePlane = new Plane(Vector3.up, Vector3.zero);
        tileSize = 1;

        occupationMap = new bool[houseSize.x, houseSize.y];
	}

    private void Update()
    {
        //if (debug)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector2Int? v2Null = GetTilePositionFromRay(Camera.main.ScreenPointToRay(Input.mousePosition));
        //
        //        if (v2Null.HasValue)
        //        {
        //            Vector2Int v2 = v2Null.Value;
        //            occupationMap[v2.x, v2.y] = !occupationMap[v2.x, v2.y];
        //        }
        //    }
        //}
    }

    public Vector2Int? GetTilePositionFromRay(Ray ray)
    {
        Vector2Int? tilePos = null;

        float distance;

        if(surfacePlane.Raycast(ray, out distance))
        {
            Vector3 point = ray.origin + ray.direction * distance;

            tilePos = GetTilePositionFromPoint(point);
        }

        return tilePos;
    }

    private Vector2Int? GetTilePositionFromPoint(Vector3 point)
    {
        //Debug.Log("Point: " + point);

        Vector2Int? tilePos = null;

        Vector2Int temp = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.z));

        //Debug.Log("Rounded: " + temp);

        if (temp.x >= 0 && temp.x < houseSize.x && temp.y >= 0 && temp.y < houseSize.y)
        {
            tilePos = temp;
        }

        return tilePos;
    }

    public bool CanPlaceFurniture(Furniture f, Vector2Int displacement) //Fix: for loop should start at -1 when size is relative negative, change everything
    {
        Vector2Int size = f.GetOrientedMapSize();

        if(size.magnitude <= 1)
        {
            Debug.LogError("Furniture has incorrect size.");
            return false;
        }

        int xAdd = (size.x > 0) ? 1 : -1;
        int yAdd = (size.y > 0) ? 1 : -1;

        for (int x = 0; (xAdd>0) ? x < size.x : x > size.x; x+= xAdd)  
        {
            for (int y = 0; (yAdd>0) ?  y < size.y : y > size.y; y+= yAdd)
            {
                if (CheckMap(displacement.x + x,displacement.y + y))
                {
                    return false;
                } 
            }
        }

        return true;
     }

    private bool CheckMap(int x, int y)
    {
        if (x < 0 || y < 0 || x >= occupationMap.GetLength(0) || y >= occupationMap.GetLength(1))
        {
            return true;
        }
        else
        {
            return occupationMap[x, y];
        }
    }

    public void PlaceFurniture(Furniture f, Vector2Int displacement)
    {
        if (!CanPlaceFurniture(f, displacement))
        {
            throw new System.Exception("Trying to place furniture that cant be placed. Check CanPlaceFurniture first!");
        }

        Vector2Int size = f.GetOrientedMapSize();

        int xAdd = (size.x > 0) ? 1 : -1;
        int yAdd = (size.y > 0) ? 1 : -1;

        for (int x = 0; (xAdd > 0) ? x < size.x : x > size.x; x += xAdd)
        {
            for (int y = 0; (yAdd > 0) ? y < size.y : y > size.y; y += yAdd)
            {
                occupationMap[displacement.x + x, displacement.y + y] = true;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if(occupationMap == null || !debug)
        {
            return;
        }

        for (int x = 0; x < houseSize.x; x++)
        {
            for (int y = 0; y < houseSize.y; y++)
            {
                Gizmos.color = occupationMap[x, y] ? new Color(0,0,0,0.3f) : new Color(1,1,1,0.3f);
                Gizmos.DrawCube(new Vector3(x * tileSize + tileSize / 2, 0 , y * tileSize + tileSize / 2), new Vector3(tileSize,0, tileSize));
            }
        }
    }

}