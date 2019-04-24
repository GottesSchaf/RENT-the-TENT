using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMap : MonoBehaviour {

    public static FurnitureMap Instance;

    [SerializeField] bool debug;

    [SerializeField] Texture2D mapStartingTexture;
    [Header("Ignored if Texture is used")]
    [SerializeField] Vector2Int houseSize;
    [SerializeField] bool inFurnitureEditMode;

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

        if(mapStartingTexture == null)
        {
            occupationMap = new bool[houseSize.x, houseSize.y];
        }
        else
        {
            occupationMap = new bool[mapStartingTexture.width, mapStartingTexture.height];

            for (int x = 0; x < occupationMap.GetLength(0); x++)
            {
                for (int y = 0; y < occupationMap.GetLength(1); y++)
                {
                    occupationMap[x, y] = mapStartingTexture.GetPixel(x, y).r <= 0.5f;
                }
            }

        }

	}

    private void Update()
    {
        if(inFurnitureEditMode && Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hits = Physics.RaycastAll(r);
            foreach (var hit in hits)
            {
                if(hit.transform.tag == "Furniture")
                {
                    hit.transform.GetComponent<Furniture>().PickUp();
                    break;
                }
            }
        }
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

    public Vector2Int? GetTilePositionFromPoint(Vector3 point)
    {
        Vector2Int temp = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.z));
        return temp;
    }

    public bool CanPlaceFurniture(Furniture f, Vector2Int displacement) //Fix: for loop should start at -1 when size is relative negative, change everything
    {
        Vector2Int size = f.GetOrientedMapSize();

        if (size.magnitude <= 1)
        {
            Debug.LogError("Furniture has incorrect size.");
            return false;
        }

        #region checking if

        if (size.x > 0)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (size.y > 0)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        if (CheckMap(displacement.x + x, displacement.y + y))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int y = -1; y >= size.y; y--)
                    {
                        if (CheckMap(displacement.x + x, displacement.y + y))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = -1; x >= size.x; x--)
            {
                if (size.y > 0)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        if (CheckMap(displacement.x + x, displacement.y + y))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int y = -1; y >= size.y; y--)
                    {
                        if (CheckMap(displacement.x + x, displacement.y + y))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        #endregion

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

        SetTiles(size, displacement, true);
    }

    public void RemoveFurniture(Furniture f, Vector2Int displacement)
    {
        Vector2Int size = f.GetOrientedMapSize();

        SetTiles(size, displacement, false);
    }

    private void SetTiles(Vector2Int size, Vector2Int displacement, bool state)
    {
        #region set code

        if (size.x > 0)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (size.y > 0)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        occupationMap[x + displacement.x, y + displacement.y] = state;
                    }
                }
                else
                {
                    for (int y = -1; y >= size.y; y--)
                    {
                        occupationMap[x + displacement.x, y + displacement.y] = state;
                    }
                }
            }
        }
        else
        {
            for (int x = -1; x >= size.x; x--)
            {
                if (size.y > 0)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        occupationMap[x + displacement.x, y + displacement.y] = state;
                    }
                }
                else
                {
                    for (int y = -1; y >= size.y; y--)
                    {
                        occupationMap[x + displacement.x, y + displacement.y] = state;
                    }
                }
            }
        }

        #endregion
    }


    private void OnDrawGizmos()
    {
        if(occupationMap == null || !debug)
        {
            return;
        }

        for (int x = 0; x < occupationMap.GetLength(0); x++)
        {
            for (int y = 0; y < occupationMap.GetLength(1); y++)
            {
                Gizmos.color = occupationMap[x, y] ? new Color(0,0,0,0.3f) : new Color(1,1,1,0.3f);
                Gizmos.DrawCube(new Vector3(x * tileSize + tileSize / 2, 0 , y * tileSize + tileSize / 2), new Vector3(tileSize,0, tileSize));
            }
        }
    }

}