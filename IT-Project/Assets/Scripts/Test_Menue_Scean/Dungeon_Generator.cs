using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Materials")]
    public Material floorMaterial;
    public Material wallMaterial;
    public Material ceilingMaterial;


    [Header("Bounds")]
    public Transform topLeft;
    public Transform topRight;


    [Header("Rooms")]
    public int roomCount = 20;
    public float minSize = 3f;
    public float maxSize = 8f;
    public float padding = 2f;
    public int maxAttempts = 50;


    [Header("Corridors")]
    public float corridorWidth = 1f;


    [Header("Dungeon Height")]
    public float wallHeight = 3f;
    public float ceilingHeight = 3f;

    [Header("Player")]
    public GameObject playerPrefab;
    public float playerHeight = 1f;


    [Header("Exit")]
    public GameObject exitPrefab;
    public float exitRadius = 2f;

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int enemyCount = 10;

    public float enemySpawnHeight = 2f;

    public NavMeshSurface navMeshSurface;


    private Room startRoom;
    private Room endRoom;



    private List<Room> rooms = new();
    private List<Edge> mstEdges = new();

    private HashSet<Vector2Int> dungeonTiles = new();



    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        rooms = GenerateRooms();

        List<Edge> edges = BuildFullGraph(rooms);

        mstEdges = BuildMST(edges, rooms);

        AssignStartAndEnd();

        SpawnRooms();

        foreach (Edge edge in mstEdges)
        {
            CreateCorridor(edge.a, edge.b);
        }

        CreateFloorMesh();
        CreateWallMesh();
        CreateCeilingMesh();
        BakeNavMesh();
        SpawnPlayer();
        CreateExit();
        SpawnEnemies();
    }

    void AssignStartAndEnd()
    {
        if (rooms.Count < 2)
            return;


        startRoom =
            rooms[Random.Range(0, rooms.Count)];


        startRoom.type = RoomType.Start;



        endRoom = null;

        float maxDistance = 0;



        foreach (Room r in rooms)
        {
            if (r == startRoom)
                continue;


            float distance =
                Vector2.Distance(
                    startRoom.position,
                    r.position
                );


            if (distance > maxDistance)
            {
                maxDistance = distance;
                endRoom = r;
            }
        }



        if (endRoom != null)
            endRoom.type = RoomType.End;
    }

    void BakeNavMesh()
    {
        if(navMeshSurface == null)
        {
            Debug.Log("Kein NavMesh GEsetzt");
            return;
        }

        navMeshSurface.BuildNavMesh();
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
            return;


        List<Vector2Int> possiblePositions =
            new List<Vector2Int>(dungeonTiles);



        // Start und Endbereich entfernen
        foreach (Room room in rooms)
        {
            if (room.type == RoomType.Start ||
               room.type == RoomType.End)
            {
                int startX =
                    Mathf.RoundToInt(
                        room.position.x - room.size.x / 2
                    );

                int startZ =
                    Mathf.RoundToInt(
                        room.position.y - room.size.y / 2
                    );


                int width =
                    Mathf.RoundToInt(room.size.x);

                int height =
                    Mathf.RoundToInt(room.size.y);



                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < height; z++)
                    {
                        possiblePositions.Remove(
                            new Vector2Int(
                                startX + x,
                                startZ + z
                            )
                        );
                    }
                }
            }
        }



        for (int i = 0; i < enemyCount; i++)
        {
            if (possiblePositions.Count == 0)
                break;



            int index =
                Random.Range(
                    0,
                    possiblePositions.Count
                );



            Vector2Int position =
                possiblePositions[index];



            possiblePositions.RemoveAt(index);



            GameObject enemy =
                enemyPrefabs[
                    Random.Range(
                        0,
                        enemyPrefabs.Length
                    )
                ];



            Instantiate(
                enemy,
                new Vector3(
                    position.x,
                    enemySpawnHeight,
                    position.y
                ),
                Quaternion.identity
            );
        }
    }

    List<Room> GenerateRooms()
    {
        List<Room> result = new();



        float xMin = topLeft.position.x;
        float xMax = topRight.position.x;


        float zMin = topRight.position.z;
        float zMax = topLeft.position.z;




        for (int i = 0; i < roomCount; i++)
        {
            bool placed = false;

            int attempts = 0;



            while (!placed && attempts < maxAttempts)
            {
                attempts++;



                Vector2 size = new Vector2(
                    Random.Range(minSize, maxSize),
                    Random.Range(minSize, maxSize)
                );



                Vector2 position = new Vector2(
                    Random.Range(xMin, xMax),
                    Random.Range(zMin, zMax)
                );



                Room room =
                    new Room(position, size);



                if (IsValid(room, result))
                {
                    result.Add(room);
                    placed = true;
                }
            }
        }


        return result;
    }

    bool IsValid(Room room, List<Room> others)
    {
        foreach (Room r in others)
        {
            float dx =
                Mathf.Abs(
                    room.position.x -
                    r.position.x
                );


            float dz =
                Mathf.Abs(
                    room.position.y -
                    r.position.y
                );



            float minX =
                (room.size.x + r.size.x)
                * 0.5f
                + padding;



            float minZ =
                (room.size.y + r.size.y)
                * 0.5f
                + padding;



            if (dx < minX && dz < minZ)
                return false;
        }



        return true;
    }

    List<Edge> BuildFullGraph(List<Room> rooms)
    {
        List<Edge> edges = new();



        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i + 1; j < rooms.Count; j++)
            {
                edges.Add(
                    new Edge(
                        rooms[i],
                        rooms[j]
                    )
                );
            }
        }



        return edges;
    }

    List<Edge> BuildMST(
        List<Edge> edges,
        List<Room> rooms)
    {

        edges.Sort(
            (a, b) =>
            a.length.CompareTo(b.length)
        );



        Dictionary<Room, Room> parent =
            new();



        foreach (Room r in rooms)
            parent[r] = r;



        Room Find(Room r)
        {
            if (parent[r] != r)
                parent[r] = Find(parent[r]);

            return parent[r];
        }



        void Union(Room a, Room b)
        {
            parent[Find(a)] = Find(b);
        }



        List<Edge> result = new();



        foreach (Edge e in edges)
        {
            if (Find(e.a) != Find(e.b))
            {
                result.Add(e);
                Union(e.a, e.b);
            }
        }



        return result;
    }

    void SpawnRooms()
    {
        foreach (Room r in rooms)
        {
            int startX =
                Mathf.RoundToInt(
                    r.position.x - r.size.x / 2
                );


            int startZ =
                Mathf.RoundToInt(
                    r.position.y - r.size.y / 2
                );



            int width =
                Mathf.RoundToInt(r.size.x);


            int height =
                Mathf.RoundToInt(r.size.y);




            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    dungeonTiles.Add(
                        new Vector2Int(
                            startX + x,
                            startZ + z
                        )
                    );
                }
            }
        }
    }

    void CreateCorridor(Room a, Room b)
    {
        Vector2Int current = new Vector2Int(
            Mathf.RoundToInt(a.position.x),
            Mathf.RoundToInt(a.position.y)
        );


        Vector2Int target = new Vector2Int(
            Mathf.RoundToInt(b.position.x),
            Mathf.RoundToInt(b.position.y)
        );



        bool horizontalFirst = Random.value > 0.5f;



        if (horizontalFirst)
        {
            while (current.x != target.x)
            {
                current.x +=
                    current.x < target.x ? 1 : -1;

                CarveCorridor(current);
            }



            while (current.y != target.y)
            {
                current.y +=
                    current.y < target.y ? 1 : -1;

                CarveCorridor(current);
            }
        }

        else
        {
            while (current.y != target.y)
            {
                current.y +=
                    current.y < target.y ? 1 : -1;

                CarveCorridor(current);
            }



            while (current.x != target.x)
            {
                current.x +=
                    current.x < target.x ? 1 : -1;

                CarveCorridor(current);
            }
        }
    }

    void CarveCorridor(Vector2Int pos)
    {
        int width =
            Mathf.RoundToInt(corridorWidth);



        for (int x = -width; x <= width; x++)
        {
            for (int z = -width; z <= width; z++)
            {
                dungeonTiles.Add(
                    new Vector2Int(
                        pos.x + x,
                        pos.y + z
                    )
                );
            }
        }
    }

    void CreateFloorMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();



        foreach (Vector2Int tile in dungeonTiles)
        {
            int index = vertices.Count;



            vertices.Add(
                new Vector3(tile.x, 0, tile.y)
            );

            vertices.Add(
                new Vector3(tile.x + 1, 0, tile.y)
            );

            vertices.Add(
                new Vector3(tile.x + 1, 0, tile.y + 1)
            );

            vertices.Add(
                new Vector3(tile.x, 0, tile.y + 1)
            );



            triangles.Add(index);
            triangles.Add(index + 2);
            triangles.Add(index + 1);


            triangles.Add(index);
            triangles.Add(index + 3);
            triangles.Add(index + 2);
        }



        CreateMeshObject(
            "Dungeon Floor",
            vertices,
            triangles,
            floorMaterial
        );
    }

    void CreateWallMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();



        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };



        foreach (Vector2Int tile in dungeonTiles)
        {
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbour =
                    tile + dir;



                if (!dungeonTiles.Contains(neighbour))
                {
                    AddWall(
                        tile,
                        dir,
                        vertices,
                        triangles
                    );
                }
            }
        }



        CreateMeshObject(
            "Dungeon Walls",
            vertices,
            triangles,
            wallMaterial
        );
    }

    void AddWall(
        Vector2Int tile,
        Vector2Int dir,
        List<Vector3> vertices,
        List<int> triangles)
    {

        int index = vertices.Count;



        float x = tile.x;
        float z = tile.y;



        if (dir == Vector2Int.up)
        {
            vertices.Add(
                new Vector3(x, 0, z + 1)
            );

            vertices.Add(
                new Vector3(x + 1, 0, z + 1)
            );

            vertices.Add(
                new Vector3(x + 1, wallHeight, z + 1)
            );

            vertices.Add(
                new Vector3(x, wallHeight, z + 1)
            );
        }




        if (dir == Vector2Int.down)
        {
            vertices.Add(
                new Vector3(x + 1, 0, z)
            );

            vertices.Add(
                new Vector3(x, 0, z)
            );

            vertices.Add(
                new Vector3(x, wallHeight, z)
            );

            vertices.Add(
                new Vector3(x + 1, wallHeight, z)
            );
        }




        if (dir == Vector2Int.right)
        {
            vertices.Add(
                new Vector3(x + 1, 0, z + 1)
            );

            vertices.Add(
                new Vector3(x + 1, 0, z)
            );

            vertices.Add(
                new Vector3(x + 1, wallHeight, z)
            );

            vertices.Add(
                new Vector3(x + 1, wallHeight, z + 1)
            );
        }




        if (dir == Vector2Int.left)
        {
            vertices.Add(
                new Vector3(x, 0, z)
            );

            vertices.Add(
                new Vector3(x, 0, z + 1)
            );

            vertices.Add(
                new Vector3(x, wallHeight, z + 1)
            );

            vertices.Add(
                new Vector3(x, wallHeight, z)
            );
        }



        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 1);


        triangles.Add(index);
        triangles.Add(index + 3);
        triangles.Add(index + 2);
    }

    void CreateCeilingMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();



        foreach (Vector2Int tile in dungeonTiles)
        {
            int index = vertices.Count;



            vertices.Add(
                new Vector3(
                    tile.x,
                    ceilingHeight,
                    tile.y
                )
            );


            vertices.Add(
                new Vector3(
                    tile.x,
                    ceilingHeight,
                    tile.y + 1
                )
            );


            vertices.Add(
                new Vector3(
                    tile.x + 1,
                    ceilingHeight,
                    tile.y + 1
                )
            );


            vertices.Add(
                new Vector3(
                    tile.x + 1,
                    ceilingHeight,
                    tile.y
                )
            );



            // Richtung nach unten
            triangles.Add(index);
            triangles.Add(index + 2);
            triangles.Add(index + 1);


            triangles.Add(index);
            triangles.Add(index + 3);
            triangles.Add(index + 2);
        }



        CreateMeshObject(
            "Dungeon Ceiling",
            vertices,
            triangles,
            ceilingMaterial
        );
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null || startRoom == null)
            return;


        Vector3 spawnPosition =
            new Vector3(
                startRoom.position.x,
                playerHeight,
                startRoom.position.y
            );


        Instantiate(
            playerPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    void CreateExit()
    {
        if (exitPrefab == null || endRoom == null)
            return;



        Vector3 position =
            new Vector3(
                endRoom.position.x,
                0.05f,
                endRoom.position.y
            );



        GameObject exit =
            Instantiate(
                exitPrefab,
                position,
                Quaternion.identity
            );


        exit.name = "Dungeon Exit";
    }

    void CreateMeshObject(
    string name,
    List<Vector3> vertices,
    List<int> triangles,
    Material material)
    {
        Mesh mesh = new Mesh();

        mesh.name = name;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        GameObject obj = new GameObject(name);


        MeshFilter filter =
            obj.AddComponent<MeshFilter>();

        MeshRenderer renderer =
            obj.AddComponent<MeshRenderer>();


        filter.mesh = mesh;

        renderer.material = material;



        
        if (name == "Dungeon Floor" ||
           name == "Dungeon Walls")
        {
            MeshCollider collider =
                obj.AddComponent<MeshCollider>();

            collider.sharedMesh = mesh;
        }
    }

    public enum RoomType
    {
        Normal,
        Start,
        End
    }

    public class Room
    {
        public Vector2 position;
        public Vector2 size;

        public RoomType type =
            RoomType.Normal;



        public Room(
            Vector2 position,
            Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
    }

    public class Edge
    {
        public Room a;
        public Room b;

        public float length;



        public Edge(
            Room a,
            Room b)
        {
            this.a = a;
            this.b = b;


            length =
                Vector2.Distance(
                    a.position,
                    b.position
                );
        }
    }
}