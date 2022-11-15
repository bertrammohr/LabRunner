using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject coinPrefab;
    public GameObject player;
    
    private float sizeX = 25f;
    private float sizeY = 10f;

    public int stage = 0;

    void Update() {
        if (player.transform.position.x > stage * (int)sizeX - 13) {
            GenerateLevel();
            stage++;
        }
    }

    void GenerateLevel() {
        Vector2[,] nodes = new Vector2[(int)sizeX * (stage+1), (int)sizeY];
        GameObject[,] walls = new GameObject[(int)sizeX * (stage+1) * 2, (int)sizeY * 2]; // walls are indexed at 2x their positions
        GameObject[] borders = new GameObject[2];

        // Generate nodes and walls
        for (float x = 0f + (sizeX * stage); x<sizeX * (stage+1) - 0.5f; x+=0.5f) {
            for (float y = 0f; y<sizeY - 0.5f; y+=0.5f) {
                if (y % 1 != 0 && x % 1 != 0) continue;

                // Create node if y and x are even
                if (y % 1 == 0 && x % 1 == 0) {
                    // Debug.Log("Attempting to create node at " + x + ", " + y);
                    Vector2 node = new Vector2(x, y);
                    nodes[(int)x, (int)y] = node;

                    if (Random.Range(0, 100) < 2) {
                        GameObject coin = Instantiate(coinPrefab, new Vector2(x, y), new Quaternion());
                        coin.name = "Coin " + x + ", " + y;
                    }

                    continue;
                };


                // Create vertical wall if y is even
                if (y % 1 == 0) {
                    // Debug.Log("Attempting to create vertical wall at " + x + ", " + y);
                    GameObject wall = Instantiate(wallPrefab, new Vector2(x, y), new Quaternion());
                    wall.name = "Wall " + x + ", " + y;
                    walls[(int)(2*x), (int)(2*y)] = wall;
                }


                // Create horizontal wall if x is even
                if (x % 1 == 0) {
                    // Debug.Log("Attempting to create horizontal wall at " + x + ", " + y);
                    GameObject wall = Instantiate(wallPrefab, new Vector2(x, y), new Quaternion(0f, 0f, 0f, 0f));
                    wall.transform.Rotate(0f, 0f, 90f);
                    wall.name = "Wall " + x + ", " + y;
                    walls[(int)(2*x), (int)(2*y)] = wall;
                }
            }
        }

        // Generate connections
        Vector2 currentNode = nodes[(int)sizeX * stage, 0]; // THIS IS WHERE THE NEXT MAZE STARTS
        List<Vector2> Path = new List<Vector2>();
        List<Vector2> Visited = new List<Vector2>();

        bool completed = false;
        bool isGoingBackwards = false;

        while (!completed) {
            // Debug.Log("currentNode is " + currentNode);

            if (!isGoingBackwards) {
                Visited.Add(currentNode);
                Path.Add(currentNode);
            }

            List<Vector2> neighbours = GetNeighbourNodes(currentNode, Visited, nodes);

            // Debug.Log("neighbours.Count is " + neighbours.Count);
            // Debug.Log("Visited.Count is " + Visited.Count);

            if (neighbours.Count > 0) {
                isGoingBackwards = false;
                Vector2 randomNeighbour = neighbours[Random.Range(0, neighbours.Count)];
                GameObject wallBetween = GetWallBetweenNodes(currentNode, randomNeighbour, walls);
                wallBetween.SetActive(false);
                // Debug.Log("Removing wall between " + currentNode + " and " + randomNeighbour);
                // Debug.Log("Next cell is " + new Vector2(randomNeighbour.x, randomNeighbour.y));
                currentNode = randomNeighbour;
            } else {
                isGoingBackwards = true;
                currentNode = Path[Path.Count-1];
                Path.RemoveAt(Path.Count-1);
            }

            if (Path.Count == 0) {
                completed = true;
            }
        }

        // Generate side borders
        borders[0] = Instantiate(wallPrefab, new Vector2(sizeX/2 - 0.5f, sizeY), new Quaternion());
        borders[0].transform.localScale = new Vector2(sizeX+ 0.25f, 1f);
        borders[0].name = "Border 1";
        borders[1] = Instantiate(wallPrefab, new Vector2(sizeX/2 - 0.5f, -1f), new Quaternion());
        borders[1].transform.localScale = new Vector2(sizeX + 0.25f, 1f);
        borders[1].name = "Border 2";
    }

    List<Vector2> GetNeighbourNodes(Vector2 Node, List<Vector2> Visited, Vector2[,] nodes) {
        List<Vector2> neighbours = new List<Vector2>();

        if (Node.x > sizeX * stage) { // left
            Vector2 neighbour = nodes[(int)Node.x-1, (int)Node.y];
            if (!Visited.Contains(neighbour)) neighbours.Add(neighbour);
        };
        if (Node.x < sizeX * (stage + 1) - 1) { // right
            Vector2 neighbour = nodes[(int)Node.x+1, (int)Node.y];
            if (!Visited.Contains(neighbour)) neighbours.Add(neighbour);
        };
        if (Node.y > 0) { // down
            Vector2 neighbour = nodes[(int)Node.x, (int)Node.y-1];
            if (!Visited.Contains(neighbour)) neighbours.Add(neighbour);
        };
        if (Node.y < sizeY - 1) { // up
            Vector2 neighbour = nodes[(int)Node.x, (int)Node.y+1];
            if (!Visited.Contains(neighbour)) neighbours.Add(neighbour);
        };
        
        return neighbours;
    }

    GameObject GetWallBetweenNodes(Vector2 Node1, Vector2 Node2, GameObject[,] walls) {
        if (Node1.x == Node2.x) {
            float averageY = (Node1.y + Node2.y) / 2;
            return walls[(int)(2*Node1.x), (int)(2*averageY)];
        }
        else if (Node1.y == Node2.y) {
            float averageX = (Node1.x + Node2.x) / 2;
            return walls[(int)(2*averageX), (int)(2*Node1.y)];
        }
        else return null;
    }
}
