using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    public Color COLOR_BACKGROUND = Color.grey;
    private Pixel[,] pixels = null;
    public int NumX = 50;
    public int NumY = 50;
    public GameObject pixelPrefab;

    public Vector2Int P0 = new Vector2Int(5, 5);
    public Vector2Int P1 = new Vector2Int(20, 40);
    public Vector2Int P2 = new Vector2Int(20, 40);

    // Start is called before the first frame update
    void Start()
    {
        pixels = new Pixel[NumX, NumY];
        for (int i = 0; i < NumX; i++)
        {
            for (int j = 0; j < NumY; j++)
            {
                GameObject obj = Instantiate(
                  pixelPrefab,
                  new Vector3(
                    i,
                    j,
                    0.0f),
                  Quaternion.identity);

                obj.name = "Pixel_" + i.ToString() + "_" + j.ToString();
                Pixel pix = obj.GetComponent<Pixel>();
                pix.SetColor(COLOR_BACKGROUND);
                pixels[i, j] = pix;
                obj.transform.SetParent(transform);
            }

        }

        SetCameraPosition();

        // Draw triangle
        StartCoroutine(Coroutine_DrawTriangle(pixels, 5, 5, 20, 40, 35, 25));

        // Print grid
        PrintGrid(pixels);
    }

    void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(
          (NumX - 1) / 2,
          (NumY - 1) / 2,
          -100.0f);

        float min_value = Mathf.Min((NumX - 1), (NumY - 1));
        Camera.main.orthographicSize = min_value * 0.75f;
    }

    static void DrawLine(Pixel[,] grid, int x0, int y0, int x1, int y1)
    {
        // Bresenham's line algorithm
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // Draw pixel
            grid[x0, y0].SetColor(Color.green);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    IEnumerator Coroutine_DrawLine(Pixel[,] grid, int x0, int y0, int x1, int y1)
    {
        // Bresenham's line algorithm
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // Draw pixel
            grid[x0, y0].SetColor(Color.green);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    static void DrawTriangle(Pixel[,] grid, int x0, int y0, int x1, int y1, int x2, int y2)
    {
        // Draw triangle edges
        DrawLine(grid, x0, y0, x1, y1);
        DrawLine(grid, x1, y1, x2, y2);
        DrawLine(grid, x2, y2, x0, y0);

        // Find the top and bottom of the triangle
        int top = Math.Max(Math.Max(y0, y1), y2);
        int bottom = Math.Min(Math.Min(y0, y1), y2);
        int height = grid.GetLength(1); // Height of the grid

        // Initialize left and right boundaries
        int[] left = new int[height];
        int[] right = new int[height];
        for (int i = 0; i < height; i++)
        {
            left[i] = int.MaxValue;
            right[i] = int.MinValue;
        }

        // Update left and right boundaries based on triangle edges
        UpdateBoundaries(left, right, x0, y0, x1, y1);
        UpdateBoundaries(left, right, x1, y1, x2, y2);
        UpdateBoundaries(left, right, x2, y2, x0, y0);

        // Scan each horizontal line within the triangle and fill pixels
        for (int y = bottom; y <= top; y++)
        {
            // Clamp xLeft and xRight within grid bounds
            int xLeft = Math.Max(0, Math.Min(left[y], grid.GetLength(0) - 1));
            int xRight = Math.Max(0, Math.Min(right[y], grid.GetLength(0) - 1));

            // Fill pixels between xLeft and xRight
            for (int x = xLeft; x <= xRight; x++)
            {
                grid[x, y].SetColor(Color.green);
            }
        }
    }
    IEnumerator Coroutine_DrawTriangle(Pixel[,] grid, int x0, int y0, int x1, int y1, int x2, int y2)
    {
        // Draw triangle edges
        yield return StartCoroutine(Coroutine_DrawLine(grid, x0, y0, x1, y1));
        yield return StartCoroutine(Coroutine_DrawLine(grid, x1, y1, x2, y2));
        yield return StartCoroutine(Coroutine_DrawLine(grid, x2, y2, x0, y0));

        // Find the top and bottom of the triangle
        int top = Math.Max(Math.Max(y0, y1), y2);
        int bottom = Math.Min(Math.Min(y0, y1), y2);
        int height = grid.GetLength(1); // Height of the grid

        // Initialize left and right boundaries
        int[] left = new int[height];
        int[] right = new int[height];
        for (int i = 0; i < height; i++)
        {
            left[i] = int.MaxValue;
            right[i] = int.MinValue;
        }

        // Update left and right boundaries based on triangle edges
        UpdateBoundaries(left, right, x0, y0, x1, y1);
        UpdateBoundaries(left, right, x1, y1, x2, y2);
        UpdateBoundaries(left, right, x2, y2, x0, y0);

        // Scan each horizontal line within the triangle and fill pixels
        for (int y = bottom; y <= top; y++)
        {
            // Clamp xLeft and xRight within grid bounds
            int xLeft = Math.Max(0, Math.Min(left[y], grid.GetLength(0) - 1));
            int xRight = Math.Max(0, Math.Min(right[y], grid.GetLength(0) - 1));

            // Fill pixels between xLeft and xRight
            for (int x = xLeft; x <= xRight; x++)
            {
                grid[x, y].SetColor(Color.green);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    static void UpdateBoundaries(int[] left, int[] right, int x0, int y0, int x1, int y1)
    {
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        int x = x0;
        int y = y0;

        while (true)
        {
            // Update left and right boundaries
            if (x < left[y]) left[y] = x;
            if (x > right[y]) right[y] = x;

            if (x == x1 && y == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
    }

    static void PrintGrid(Pixel[,] grid)
    {
        for (int y = grid.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                //Console.Write(grid[x, y] == '*' ? '*' : '.');
                //grid[x, y].SetColor(Color.green);
            }
            //Console.WriteLine();
        }
    }
}
