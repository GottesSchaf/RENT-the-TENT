using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Furniture : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] private Material movingMat;

    private bool moving;
    private Orientation orientation;

    private Material[] normalMats;

    Collider collider;
    NavMeshObstacle navCollider;


    public bool Moving
    {
        get{ return moving; }
        set
        {
            if (value != moving)
            {
                if (value)
                {
                    EnableMoving();
                }
                else
                {
                    DisableMoving();
                }
            }

            moving = value;
        }
    }


    private void EnableMoving()
    {
        collider.enabled = false;
        navCollider.enabled = false;

        var renderer = GetComponent<Renderer>();
        normalMats = renderer.materials;

        renderer.material = movingMat;
    }

    private void DisableMoving()
    {
        collider.enabled = true;
        navCollider.enabled = true;

        var renderer = GetComponent<Renderer>();
        renderer.materials = normalMats;
    }

    public Orientation Orient
    {
        get{ return orientation; }
        set
        {
            orientation = value;
            UpdateOrientation();
        }
    }

    private void UpdateOrientation()
    {
        switch (Orient)
        {
            case Orientation.Up:
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Orientation.Right:
                transform.eulerAngles = new Vector3(0, 90, 0);
                break;
            case Orientation.Down:
                transform.eulerAngles = new Vector3(0, 180, 0);
                break;
            case Orientation.Left:
                transform.eulerAngles = new Vector3(0, -90, 0);
                break;
        }
    }

    public void StartMoving()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = FurnitureMap.Instance.GetTilePositionFromRay(ray);
        Moving = true;
        FurnitureMap.Instance.RemoveFurniture(this, position.Value);
    }

    public void StopMoving()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = FurnitureMap.Instance.GetTilePositionFromRay(ray);

        if (position.HasValue)
        {
            if (FurnitureMap.Instance.CanPlaceFurniture(this, position.Value))
            {
                FurnitureMap.Instance.PlaceFurniture(this, position.Value);
                Moving = false;
            }
        }
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
        navCollider = GetComponent<NavMeshObstacle>();
        Moving = true;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = FurnitureMap.Instance.GetTilePositionFromRay(ray);

        //visuals
        if (Moving)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateOrientation(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateOrientation(true);
            }

            if (position.HasValue)
            {
                transform.position = new Vector3(position.Value.x, 0, position.Value.y);
                if (FurnitureMap.Instance.CanPlaceFurniture(this, position.Value))
                {
                    GetComponent<Renderer>().material.SetFloat("colRed", 0);
                }
                else
                {
                    GetComponent<Renderer>().material.SetFloat("colRed", 1);
                }
            }
        }
    }


    public Vector2Int GetOrientedMapSize()
    {
        if (Orient == Orientation.Up)
        {
            return size;
        }
        else if (Orient == Orientation.Down)
        {
            return new Vector2Int(-size.x, -size.y);
        }
        else if (Orient == Orientation.Right)
        {
            return new Vector2Int(size.y, -size.x);
        }
        else
        {
            return new Vector2Int(-size.y, size.x);
        }
    }

    private void RotateOrientation(bool right)
    {
        int addition = right ? 1 : -1;
        int temp = (int)modulo((int)Orient + addition, 4);

        Orient = (Orientation)temp;
    }

    float modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    private void OnDrawGizmos()
    {
        Vector2 pos = GetOrientedMapSize();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(pos.x / 2, 0.5f, pos.y / 2) , new Vector3(pos.x, 1, pos.y));
    }

    public Vector3 GetOrientationVector(Orientation o)
    {
        switch (o)
        {
            case Orientation.Up:
                return new Vector3(0, 0, 1);
            case Orientation.Right:
                return new Vector3(1, 0, 0);
            case Orientation.Down:
                return new Vector3(0, 0, -1);
            case Orientation.Left:
                return new Vector3(-1, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    public enum Orientation
    {
        Up,
        Right,
        Down,
        Left
    }
}
