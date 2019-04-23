﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

    [SerializeField] bool debug;
    
    [SerializeField] Vector2Int size;
    [SerializeField] Material movingMat;

    private bool moving;
    private Orientation orientation;

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

    public Orientation Orient
    {
        get
        {
            return orientation;
        }

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


    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = FurnitureMap.Instance.GetTilePositionFromRay(ray);

        //interaction
        if (debug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!Moving)
                {
                    Moving = true;
                }
                else
                {
                    if (position.HasValue)
                    {
                        if (FurnitureMap.Instance.CanPlaceFurniture(this, position.Value))
                        {
                            FurnitureMap.Instance.PlaceFurniture(this, position.Value);
                            Moving = false;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateOrientation(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateOrientation(true);
            }
        }

        //visuals
        if (Moving)
        {
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
        if(Orient == Orientation.Up)
        {
            return size;
        }
        else if( Orient == Orientation.Down)
        {
            return new Vector2Int(-size.x , -size.y);
        }
        else if(Orient == Orientation.Right)
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
        int temp = (int) modulo((int)Orient + addition, 4);

        Orient = (Orientation)temp;
    }

    float modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    private void OnDrawGizmos()
    {
        if (!debug)
        {
            return;
        }


        Vector2Int pos = GetOrientedMapSize();

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(pos.x / 2, 0.5f, pos.y / 2) + GetOrientationVector(Orient)/2, new Vector3(pos.x, 1, pos.y));
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
