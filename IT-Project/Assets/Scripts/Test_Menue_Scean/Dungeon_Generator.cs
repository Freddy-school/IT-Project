using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject floorPrefab;

    [Header("Bounds")]
    public Transform topLeft;
    public Transform topRight;

    [Header("Rooms")]
    public int roomCount = 20;
    public float minSize = 3f;
    public float maxSize = 8f;
    public float padding = 2f;
    public int maxAttempts = 50;

    [Header("Corridor")]
    public float corridorWidth = 1f;

    private List<Room> rooms = new List<Room>();
    private List<Edge> mstEdges = new List<Edge>();

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
        List<Room> mainPath = BuildMainPath();

        SpawnRooms();

        // normale MST-Verbindungen
        foreach (var edge in mstEdges)
        {
            CreateCorridor(edge.a, edge.b);
        }

        // optional: Main Path hervorheben (kann später wichtig werden)
        foreach (var r in mainPath)
        {
            // hier könntest du z.B. besondere Floors setzen
            // oder Marker spawnen
        }
    }


    void AssignStartAndEnd()
    {
        if (rooms.Count < 2) return;

        Room start = rooms[Random.Range(0, rooms.Count)];
        start.type = RoomType.Start;

        Room end = null;
        float maxDist = 0f;

        foreach (var r in rooms)
        {
            if (r == start) continue;

            float d = Vector2.Distance(start.position, r.position);

            if (d > maxDist)
            {
                maxDist = d;
                end = r;
            }
        }

        if (end != null)
            end.type = RoomType.End;
    }


    List<Room> BuildMainPath()
    {
        Room start = null;
        Room end = null;

        foreach (var r in rooms)
        {
            if (r.type == RoomType.Start) start = r;
            if (r.type == RoomType.End) end = r;
        }

        if (start == null || end == null)
            return new List<Room>();

        Dictionary<Room, Room> cameFrom = new Dictionary<Room, Room>();
        Queue<Room> queue = new Queue<Room>();

        HashSet<Room> visited = new HashSet<Room>();

        queue.Enqueue(start);
        visited.Add(start);

        // BFS über MST (stabiler als direkte Distanz)
        while (queue.Count > 0)
        {
            Room current = queue.Dequeue();

            if (current == end)
                break;

            foreach (var edge in mstEdges)
            {
                Room neighbor = null;

                if (edge.a == current) neighbor = edge.b;
                else if (edge.b == current) neighbor = edge.a;

                if (neighbor != null && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Pfad rekonstruieren
        List<Room> path = new List<Room>();

        Room step = end;

        while (step != start)
        {
            path.Add(step);

            if (!cameFrom.ContainsKey(step))
                break;

            step = cameFrom[step];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }


    List<Room> GenerateRooms()
    {
        List<Room> result = new List<Room>();

        float xMin = topLeft.position.x;
        float xMax = topRight.position.x;
        float zMin = topLeft.position.z;
        float zMax = topRight.position.z;

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

                Vector2 pos = new Vector2(
                    Random.Range(xMin, xMax),
                    Random.Range(zMin, zMax)
                );

                Room newRoom = new Room(pos, size);

                if (IsValid(newRoom, result))
                {
                    result.Add(newRoom);
                    placed = true;
                }
            }
        }

        return result;
    }

    bool IsValid(Room room, List<Room> others)
    {
        foreach (var r in others)
        {
            float dx = Mathf.Abs(room.position.x - r.position.x);
            float dz = Mathf.Abs(room.position.y - r.position.y);

            float minX = (room.size.x + r.size.x) * 0.5f + padding;
            float minZ = (room.size.y + r.size.y) * 0.5f + padding;

            if (dx < minX && dz < minZ)
                return false;
        }

        return true;
    }


    List<Edge> BuildFullGraph(List<Room> rooms)
    {
        List<Edge> edges = new List<Edge>();

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i + 1; j < rooms.Count; j++)
            {
                edges.Add(new Edge(rooms[i], rooms[j]));
            }
        }

        return edges;
    }

    List<Edge> BuildMST(List<Edge> edges, List<Room> rooms)
    {
        edges.Sort((a, b) => a.length.CompareTo(b.length));

        Dictionary<Room, Room> parent = new Dictionary<Room, Room>();

        foreach (var r in rooms)
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

        List<Edge> result = new List<Edge>();

        foreach (var e in edges)
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
        foreach (var r in rooms)
        {
            GameObject obj = Instantiate(
                floorPrefab,
                new Vector3(r.position.x, 0, r.position.y),
                Quaternion.identity
            );

            obj.transform.localScale = new Vector3(r.size.x, 1, r.size.y);
        }
    }

    void CreateCorridor(Room a, Room b)
    {
        Vector2 current = a.position;

        while (Vector2.Distance(current, b.position) > 1f)
        {
            current = Vector2.MoveTowards(current, b.position, 1f);

            CarveCorridor(current);
        }
    }

    void CarveCorridor(Vector2 pos)
    {
        int w = Mathf.RoundToInt(corridorWidth);

        for (int x = -w; x <= w; x++)
        {
            for (int y = -w; y <= w; y++)
            {
                Vector2 offset = new Vector2(x, y);

                if (offset.magnitude <= corridorWidth)
                {
                    Instantiate(
                        floorPrefab,
                        new Vector3(pos.x + offset.x, 0, pos.y + offset.y),
                        Quaternion.identity
                    );
                }
            }
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
        public RoomType type = RoomType.Normal;

        public Room(Vector2 p, Vector2 s)
        {
            position = p;
            size = s;
        }
    }

    public class Edge
    {
        public Room a;
        public Room b;
        public float length;

        public Edge(Room a, Room b)
        {
            this.a = a;
            this.b = b;
            this.length = Vector2.Distance(a.position, b.position);
        }
    }
}