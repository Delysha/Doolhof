using Unity.Mathematics;
using UnityEngine;


public class Generator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    private int[,] maze;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // Fill everything with walls (1 = wall, 0 = path)
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        CarvePath(1, 1); // Start from (1,1)
    }

    void CarvePath(int x, int y)
    {
        maze[x, y] = 0;

        int[] dirs = { 0, 1, 2, 3 }; // Up, Down, Left, Right
        Shuffle(dirs);

        foreach (int dir in dirs)
        {
            int dx = 0, dy = 0;

            switch (dir)
            {
                case 0: dy = 1; break;   // Up
                case 1: dy = -1; break;  // Down
                case 2: dx = -1; break;  // Left
                case 3: dx = 1; break;   // Right
            }

            int nx = x + dx * 2;
            int ny = y + dy * 2;

            if (IsInBounds(nx, ny) && maze[nx, ny] == 1)
            {
                maze[x + dx, y + dy] = 0;
                CarvePath(nx, ny);
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = maze[x, y] == 1 ? wallPrefab : floorPrefab;
                Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
            }
    }

    bool IsInBounds(int x, int y)
    {
        return x > 0 && x < width - 1 && y > 0 && y < height - 1;
    }

    void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rand = UnityEngine.Random.Range(i, array.Length);
            int temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }
    }
}