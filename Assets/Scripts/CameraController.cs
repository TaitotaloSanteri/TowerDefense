﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float marginX = 100f, marginY = 100f, speed = 5f;
    private Vector2 mapStart, mapEnd;
    private Camera cam;
    [SerializeField] private bool buildingMode;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        Vector2 mapSize = GridManager.instance.mapSize;
        float cellSize = GridManager.instance.cellSize;

        // Asetetaan kameran aloituspositio keskelle pelikenttää
        transform.position = new Vector3(mapSize.x / 2 * cellSize,
                                         mapSize.y / 2 * cellSize,
                                         -10f);
        // Width muuttujaan haetaan kameran poulikas koko vaakasuunnassa. Kameran koko
        // pystysuunnassa (puolikas) löytyy muuttujasta cam.orthographicSize
        float width = cam.orthographicSize * Screen.width / Screen.height;

        // Asetetaan mapEnd -muuttujan arvoksi kartan koko kerrottuna yhen tilen koko
        mapStart = new Vector2(width, cam.orthographicSize);
        mapEnd = new Vector2(mapSize.x * cellSize - width, 
                             mapSize.y * cellSize - cam.orthographicSize);
    }

    private void Update()
    {
        HandleScrolling();
        if (buildingMode)
        {
            HandleBuilding();
        }
    }

    private void HandleBuilding()
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = GridManager.instance.WorldToBGCell(worldPos);
        Debug.Log(cellPos);
    }

    private void HandleScrolling()
    {
        Vector2 mPos = Input.mousePosition;
        // Tarkistetaan onko hiiren kursori vasemmalla puolella ruutua.
        if (mPos.x < marginX)
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
        // Tarkistetaan onko hiiren kursori oikealla puolella ruuutua.
        else if (mPos.x > Screen.width - marginX)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        // Tarkistetaan onko hiiren kursori ylhäällä.
        if (mPos.y < marginY)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }
        // Tarkistetaan onko hiiren kursori alhaalla.
        else if (mPos.y > Screen.height - marginY)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        // Käytetään Mathf.Clamp metodia pitämään kameran x ja y koordinaatit
        // pelikentän rajoissa.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapStart.x, mapEnd.x),
                                         Mathf.Clamp(transform.position.y, mapStart.y, mapEnd.y),
                                         -10f);
    }
}

