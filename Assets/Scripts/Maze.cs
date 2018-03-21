using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public float generationStepDelay;
    public IntVector2 coordinates;
    public IntVector2 size;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

    private bool mazeVisible = true;
    private GameObject[] ghosts, walls;

    public void ToggleMaze()
    {
        mazeVisible = mazeVisible == true ? false : true;
        float scale = mazeVisible == true ? 1f : 0f;

        //GameObject.Find("mazeInstance").gameObject.transform.localScale = new Vector3(scale, scale, scale);
        walls = GameObject.FindGameObjectsWithTag("ExtWall");
        ghosts = GameObject.FindGameObjectsWithTag("ghost");
        foreach (GameObject ghost in ghosts) ghost.gameObject.transform.localScale = new Vector3(scale, scale, scale);

        switch (mazeVisible)
        {
            case false:
                foreach (GameObject wall in walls) wall.gameObject.transform.localScale = new Vector3(scale, scale, scale);
                GameObject.FindGameObjectWithTag("floor").transform.localScale = new Vector3(scale, scale, scale);
                break;
            case true:
            default:
                foreach (GameObject wall in walls) wall.gameObject.transform.localScale = new Vector3(.5f, 5f, 41f);
                GameObject.FindGameObjectWithTag("floor").transform.localScale = new Vector3(4f, 1f, 4f);
                break;
        }
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public void Generate()
    //public IEnumerator Generate()
    {
        //WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            //yield return delay;
            //yield return null;
            DoNextGenerationStep(activeCells);
        }
        Debug.Log("Maze Complete, resuming time");
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
                // No longer remove the cell here.
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
            // No longer remove the cell here.
        }
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, -.5f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }
}
