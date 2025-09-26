using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSampler
{
    private const int k = 30;

    private Rect rect;
    private float radius2;
    private float cellSize;
    private Vector2[,] grid;
    private List<Vector2> activeSamples = new List<Vector2>();

    public PoissonDiscSampler(float width, float height, float radius)
    {
        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
                           Mathf.CeilToInt(height / cellSize)];
    }

    public IEnumerable<Vector2> Samples()
    {
        activeSamples.Clear();
        grid = new Vector2[Mathf.CeilToInt(rect.width / cellSize),
                           Mathf.CeilToInt(rect.height / cellSize)];

        yield return AddSample(new Vector2(Random.value * rect.width, Random.value * rect.height));

        while (activeSamples.Count > 0)
        {
            int i = (int)(Random.value * activeSamples.Count); 
            Vector2 sample = activeSamples[i];
            bool found = false;

            for (int j = 0; j < k; j++)
            {
                float angle = 2 * Mathf.PI * Random.value;
                float r = Mathf.Sqrt(Random.value * 3 * radius2 + radius2);
                Vector2 candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                if (rect.Contains(candidate) && IsFarEnough(candidate))
                {
                    found = true;
                    yield return AddSample(candidate);
                    break;
                }
            }

            if (!found)
            {
                activeSamples[i] = activeSamples[activeSamples.Count - 1];
                activeSamples.RemoveAt(activeSamples.Count - 1);
            }
        }
    }

    private bool IsFarEnough(Vector2 sample)
    {
        GridPos pos = new GridPos(sample, cellSize, rect);

        int xmin = Mathf.Max(pos.x - 2, 0);
        int ymin = Mathf.Max(pos.y - 2, 0);
        int xmax = Mathf.Min(pos.x + 2, grid.GetLength(0) - 1);
        int ymax = Mathf.Min(pos.y + 2, grid.GetLength(1) - 1);

        for (int y = ymin; y <= ymax; y++)
        {
            for (int x = xmin; x <= xmax; x++)
            {
                Vector2 s = grid[x, y];
                if (s != Vector2.zero)
                {
                    Vector2 d = s - sample;
                    if (d.sqrMagnitude < radius2) return false;
                }
            }
        }
        return true;
    }

    private Vector2 AddSample(Vector2 sample)
    {
        activeSamples.Add(sample);
        GridPos pos = new GridPos(sample, cellSize, rect);
        grid[pos.x, pos.y] = sample;
        return sample;
    }

    private struct GridPos
    {
        public int x, y;
        public GridPos(Vector2 sample, float cellSize, Rect rect)
        {
            x = Mathf.Min((int)(sample.x / cellSize), (int)(rect.width / cellSize) - 1);
            y = Mathf.Min((int)(sample.y / cellSize), (int)(rect.height / cellSize) - 1);
        }
    }
}
