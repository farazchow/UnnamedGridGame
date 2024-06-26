using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public bool isFlatTopped;
    public Material material;
    
    public float minimumHeight = 1f;
    public float maximumHeight = 10f;
    public float noiseFrequency = 4f;
    public int octaves = 2;

    public bool useGreyScale;

    public float xOffset;
    public float yOffset;

    private void Start()
    {
        xOffset = UnityEngine.Random.Range(-1000, 1000);
        yOffset = UnityEngine.Random.Range(-1000, 1000);
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {   
            // clear out old hexes
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            LayoutGrid();
        }
    }

    private void LayoutGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x}, {y}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                
                float nx = (float)(x/((float) gridSize.x));
                float ny = (float)(y/((float) gridSize.y));

                float heightCoefficient = getHeightCoefficient(nx, ny);
                float height = Mathf.Round(heightCoefficient * (maximumHeight + 1 - minimumHeight)) + minimumHeight;

                hexRenderer.height = height;
                hexRenderer.SetMaterial(material);

                hexRenderer.DrawMesh();
                if (useGreyScale)
                {
                    hexRenderer.SetColor(Color.HSVToRGB(0, 0, 1-heightCoefficient));
                }
                tile.transform.SetParent(transform, false);
            }
        }
    }

    private float getHeightCoefficient(float nx, float ny)
    {
        float heightCoefficient = 0;
        float oSum = 0;

        for (int octave = 0; octave < octaves ; octave++)
        {
            float o = Mathf.Pow(2f, octave);
            heightCoefficient +=  Mathf.PerlinNoise(noiseFrequency * o * nx + xOffset, noiseFrequency * o * ny + yOffset) / o;
            oSum += 1/o;
        }
        return heightCoefficient/oSum;
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;

        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f/4f);

            offset = shouldOffset ? width/2 : 0;

            xPosition = (column * horizontalDistance) + offset;
            yPosition = row * verticalDistance;
        }
        else 
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = shouldOffset ? height/2 : 0;
            xPosition = column * horizontalDistance;
            yPosition = (row * verticalDistance) - offset;
        }


        return new Vector3(xPosition, 0, -yPosition);

    }
}
