using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class OrbitalLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField] private Material mat;
    [SerializeField] private Color col;
    [SerializeField] private int resolution;
    [SerializeField] private float thickness;

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    private void Start()
    {
        DrawOrbitalCircle(resolution, 150f * transform.localScale.x, thickness);
    }

    private void DrawOrbitalCircle(int steps, float radius, float thickness)
    {
        mat.color = col;
        _lineRenderer.material = mat;
        _lineRenderer.positionCount = steps + 1;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.widthMultiplier = thickness;

        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (steps + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            _lineRenderer.SetPosition(i, new Vector3(x, 0, y));

            angle += (360f / steps);
        }
    }
}
