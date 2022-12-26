using UnityEngine;

public class Gamefield
{
    public enum CellType
    {
        Path,
        Border,
        Pillar,
        Obstacle
    }

    public class Cell
    {
        public CellType type;

        public Cell()
        {
            type = CellType.Path;
        }
    }

    public Cell[][] Cells { get; set; }

    public delegate void GenerateHandle();

    public event GenerateHandle OnGenerate;

    public Gamefield()
    {
        Cells = null;
    }

    public Gamefield(int width, int height, float density)
    {
        Generate(width, height, density);
    }

    public void Generate(int width, int height, float density)
    {
        density = 1.0f - density;
        validateSize(ref width);
        validateSize(ref height);
        Cells = new Cell[height][];
        for (int i = 0; i < height; ++i)
        {
            Cells[i] = new Cell[width];
            for (int j = 0; j < width; ++j)
            {
                Cells[i][j] = new Cell();
            }
        }
        int h = height - 1;
        int w = width - 1;
        // top and bottom borders
        for (int i = 0; i < width; ++i)
        {
            Cells[0][i].type = CellType.Border;
            Cells[h][i].type = CellType.Border;
        }
        // left and right borders
        for (int i = 1; i < h; ++i)
        {
            Cells[i][0].type = CellType.Border;
            Cells[i][w].type = CellType.Border;
        }
        // pillars, paths and obstacles
        for (int i = 1; i < h; ++i)
        {
            for (int j = 1; j < w; ++j)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    Cells[i][j].type = CellType.Pillar;
                }
                else
                {
                    bool obstacle = Random.Range(0.0f, 1.0f) > density;
                    if (obstacle)
                    {
                        Cells[i][j].type = CellType.Obstacle;
                    }
                    else
                    {
                        Cells[i][j].type = CellType.Path;
                    }
                }
            }
        }
        RiseOnGenerate();
    }

    private void RiseOnGenerate()
    {
        if (OnGenerate != null)
        {
            OnGenerate();
        }
    }

    private void validateSize(ref int size)
    {
        if (size < 5)
        {
            Debug.LogWarning($"Gamefield: gamefield size {size} must be > 5");
            size = 5;
        }
        if (size % 2 == 0) 
        {
            Debug.LogWarning($"Gamefield: gamefield size {size} must be an odd number");
            size += 1;
        }
    }
}