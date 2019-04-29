﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

    [SerializeField] bool debug;
    [SerializeField] Orientation orientation;
    [SerializeField] Vector2Int size;
    [SerializeField] Material movingMat;

    private bool moving;


    private Material[] normalMats;

    public bool Moving
    {
        get
        {
            return moving;
        }
        set
        {
            if(value != moving)
            {
                if (value)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }

            moving = value;
        }
    }


    private void Enable()
    {
        //disable collision, navmash blocking

        var renderer = GetComponent<Renderer>();
        normalMats = renderer.materials;

        renderer.material = movingMat;
    }

    private void Disable()
    {
        // enable collision, navmash blocking

        var renderer = GetComponent<Renderer>();
        renderer.materials = normalMats;
    }

    void Update()
    {
        if (debug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Moving = !Moving;
            }
        }


        if (!Moving)
        {
            return;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position =  FurnitureMap.Instance.GetTilePositionFromRay(ray);

        if (position.HasValue)
        {
            transform.position = new Vector3(position.Value.x,0, position.Value.y);
            if(FurnitureMap.Instance.CanPlaceFurniture(this, position.Value))
            {
                GetComponent<Renderer>().material.SetFloat("colRed", 0);
            }
            else
            {
                GetComponent<Renderer>().material.SetFloat("colRed", 1);
            }
        }
    }


    public Vector2Int GetOrientedMapSize()
    {
        if(orientation == Orientation.Up)
        {
            return size;
        }
        else if( orientation == Orientation.Down)
        {
            return new Vector2Int(-size.x , -size.y);
        }
        else if(orientation == Orientation.Right)
        {
            return new Vector2Int(size.y, -size.x);
        }
        else
        {
            return new Vector2Int(-size.y, size.x);
        }
    }

    private void OnDrawGizmos()
    {
        if (!debug)
        {
            return;
        }


        Vector2Int pos = GetOrientedMapSize();

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(pos.x / 2, 0, pos.y / 2) + GetOrientationVector(orientation)/2, new Vector3(pos.x, 1, pos.y));
    }

    public Vector3 GetOrientationVector(Orientation o)
    {
        switch (o)
        {
            case Orientation.Up:
                return new Vector3(1, 0, 0);
            case Orientation.Right:
                return new Vector3(0, 0, -1);
            case Orientation.Down:
                return new Vector3(-1, 0, 0);
            case Orientation.Left:
                return new Vector3(0, 0, 1);
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
