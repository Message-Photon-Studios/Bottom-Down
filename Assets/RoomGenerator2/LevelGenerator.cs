using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Door
{
    public Vector2 pos;
    public Direction dir;
    public CustomRoom room;
    public int doorsInRoom;

    public Door(Vector2 pos, Direction dir, CustomRoom room, int doorsInRoom)
    {
        this.pos = pos;
        this.dir = dir;
        this.room = room;
        this.doorsInRoom = room.repeatable ? 0 : doorsInRoom;
    }
}

public static class ListExtensions
{
    // Takes an iterable and shuffles it around
    public static void Shuffle<T>(this IList<T> list)
    {
        var random = new System.Random();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

public class DungeonNode
{
    public bool[] doors = { false, false, false, false };
}

public class DungeonGraph
{
    public LevelGenerator parent;
    public Dictionary<Vector2, DungeonNode> nodes;
    public List<(Vector2, CustomRoom)> rooms;
    public int size => nodes.Count;

    public DungeonGraph(CustomRoom initialRoom, LevelGenerator parent)
    {
        nodes = new Dictionary<Vector2, DungeonNode>();
        rooms = new List<(Vector2, CustomRoom)>();
        placeRoom(new Vector2(0, 1), new Vector2(0, 0), Direction.Down, initialRoom);
        this.parent = parent;
    }

    public bool isTopDoorSuitable(Vector2 door)
    {
        for (var i = 0; i < 4; i++)
        {
            if (i == (int)Direction.Down)
                continue;
            // Check if the graph contains a node adjacent to the door
            if (!nodes.ContainsKey(door + CustomRoom.dirVectors[i]))
                continue;
            if (!nodes[door + CustomRoom.dirVectors[i]].doors[CustomRoom.mirrorDir[i]]) 
                continue;
            return false;
        }
        return true;
    }

    public bool areDoorsBlocked(List<Door> doors, Vector2 shift, RoomNodeHolder room)
    {
        foreach (var door in doors)
        {
            var pos = door.pos + shift;

            // Test in every direction with a door that there is either not a node or that the node has a door
            var dir = (int)door.dir;
            var neighborPos = pos + CustomRoom.dirVectors[dir];
            if (!nodes.ContainsKey(neighborPos))
                continue;
            // Either both are false or both are true, in any other case there is a blockage
            if (!nodes[neighborPos].doors[CustomRoom.mirrorDir[dir]] && !room[door.pos].doors[dir])
                continue;
            if (nodes[neighborPos].doors[CustomRoom.mirrorDir[dir]] && room[door.pos].doors[dir])
                continue;
            return true;
        }
        
        // Check that no node is blocking any open door in the graph
        foreach (var door in parent.remainingDoors)
        {
            var doorNeighborLocal = door.pos + CustomRoom.dirVectors[(int)door.dir] - shift;
            if (room.ContainsKey(doorNeighborLocal) && !room[doorNeighborLocal].doors[CustomRoom.mirrorDir[(int)door.dir]])
                return true;
        }

        // Check that it does not obstruct the top door either
        {
            var doorNeighborLocal = parent.topDoor.pos + Vector2.up - shift;
            if (room.ContainsKey(doorNeighborLocal))
            {
                if (!room[doorNeighborLocal].doors[(int)Direction.Down])
                    return true;

                // Check that there is a new door to use as future potential door
                foreach (var door in doors.Where(door => door.dir == Direction.Up))
                {
                    if (isTopDoorSuitable(door.pos + CustomRoom.dirVectors[(int)Direction.Up]))
                        return false;
                }
                return true;
            }
        }
        return false;
    }

    public (int, Vector2) testRoom(Vector2 doorPos, Direction doorDir, CustomRoom room)
    {
        // Get all entrances of the room that have a mirrored direction to the one provided
        var doors = room.getDoors();
        var mirroredEntrances = doors
            .Where(door => (int)door.dir == CustomRoom.mirrorDir[(int)doorDir])
            .Select(door => door.pos)
            .ToList();
        // If there are no mirrored entrances, the room cannot be placed
        if (mirroredEntrances.Count == 0)
            return (-1, new Vector2(0, 0));
        // Order entrances from lowest y to highest y
        var orderedEntrances = mirroredEntrances.OrderBy(pos => pos.y).Reverse().ToArray();
        // Get the position in the graph of the entrance
        var entrancePos = doorPos + CustomRoom.dirVectors[(int)doorDir];

        // Test the room with each entrance
        var failedTries = 0;
        var found = false;
        var doorsToDecide = new List<(int, Vector2)>();
        foreach (var entrance in orderedEntrances)
        {
            // Get the vector shift needed to move the room to the graph door
            var shift = entrancePos - entrance;
            // Check if the room can be placed at the door
            if (room.roomNodes
                .Select(roomNode => roomNode.Key + shift)
                .Any(nodePos => nodes.ContainsKey(nodePos)))
            {
                failedTries++;
                continue;
            }

            // Check that no door is being blocked
            if (areDoorsBlocked(doors.ToList(), shift, room.roomNodes))
            {
                failedTries++;
                continue;
            }
            
            found = true;
            doorsToDecide.Add((failedTries, entrance));
        }

        doorsToDecide.Shuffle();
        doorsToDecide = doorsToDecide.OrderBy(door => door.Item1).ToList();

        return !found ? (-1, new Vector2()) : (failedTries + (room.repeatable ? 2 : 0), doorsToDecide[Random.Range(0, doorsToDecide.Count)].Item2);
    }

    public List<Door> placeRoom(Vector2 graphPos, Vector2 doorPos, Direction doorDir, CustomRoom prefab)
    {
        // Calculate the shift needed to move the room to the graph door
        var shift = graphPos - doorPos + CustomRoom.dirVectors[(int)doorDir];

        // Place all nodes of the room
        foreach (var node in prefab.roomNodes)
        {
            var pos = node.Key + shift;
            nodes[pos] = new DungeonNode()
            {
                doors = node.Value.doors
            };
        }
        rooms.Add((shift, prefab));

        var remainingDoors = new List<Door>();
        // Return the list of doors that can be used to connect to other rooms
        var doors = prefab.getDoors();

        foreach (var door in doors)
        {
            var neighborPos = door.pos + shift + CustomRoom.dirVectors[(int)door.dir];
            if (nodes.ContainsKey(neighborPos))
                continue;
            door.pos += shift;
            remainingDoors.Add(door);
        }

        return remainingDoors;
    }
#if UNITY_EDITOR
    public void draw()
    {
        foreach (var (position, room) in rooms)
        {
            room.draw(position);
        }

        foreach (var node in nodes)
        {
            for (var i = 0; i < 4; i++)
            {
                var neighborPos = node.Key + CustomRoom.dirVectors[i];
                if (node.Value.doors[i] 
                    && nodes.ContainsKey(neighborPos)
                    && !nodes[neighborPos].doors[CustomRoom.mirrorDir[i]])
                {
                    Gizmos.color = Color.red;
                    var firstCorner = node.Key * 2*LevelGenManager.ROOMSIZE + (CustomRoom.dirVectors[i] + CustomRoom.sideToDirVectors[i]) * LevelGenManager.ROOMSIZE;
                    var secondCorner = node.Key * 2*LevelGenManager.ROOMSIZE + (CustomRoom.dirVectors[i] - CustomRoom.sideToDirVectors[i]) * LevelGenManager.ROOMSIZE;
                    Gizmos.DrawLine(firstCorner, secondCorner);
                }
            }
        }
    }
#endif

    public bool validate()
    {
        foreach (var node in nodes)
        {
            for (var i = 0; i < 4; i++)
            {
                var neighborPos = node.Key + CustomRoom.dirVectors[i];
                if (node.Value.doors[i] 
                    && nodes.ContainsKey(neighborPos)
                    && !nodes[neighborPos].doors[CustomRoom.mirrorDir[i]])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool checkExistNear(Vector2 pos, int distance)
    {
        for (var x = pos.x - distance; x <= pos.x + distance; x++)
        {
            for (var y = pos.y - distance; y <= pos.y + distance; y++)
            {
                if (nodes.ContainsKey(new Vector2(x, y)))
                    return true;
            }
        }

        return false;
    }
}

public class LevelGenerator
{
    private static float ROOMSIZE => LevelGenManager.ROOMSIZE;

    public DungeonGraph graph;
    public Minimap minimap;
    private List<CustomRoom> normalRooms;
    private List<CustomRoom> closingRooms;

    private List<CustomRoom> usedRooms;
    private List<(Vector2, CustomRoom)> prefabs;
    private List<(Vector2, GameObject)> filledPrefabs;

    private GameObject enemyHolder;

    public List<Door> remainingDoors;
    public Door topDoor;

    public Vector3? endRoomPos = null;

    public void stepGenerate(int size, string areaPath)
    {
        if (graph == null)
        {
            initGeneration(areaPath);
        }
        
        var (finished, success) = nextRoom(size, areaPath);
        if (finished) Debug.Log("Finished successfully");
        if (!success) Debug.Log("Failed to generate");
    }

    public void generate(int size, string areaPath)
    {
        var tries = 0;
        do
        {
            tries++;
            if (tries > 20)
            {
                Debug.LogError("Failed to generate level");
                return;
            }

            initGeneration(areaPath);
            var res = tryGenerate(size, areaPath);
            if (!res) continue;
            endGeneration(areaPath);

        } while (!graph.validate());
        insertPrefabs(areaPath);
        
        minimap = new Minimap(graph);
    }

    private void insertPrefabs(string areaPath)
    {
        // Try to find the object RoomHolder and if it exists, delete it
        var dungeon = GameObject.Find("Dungeon");
        if (dungeon != null)
            Object.DestroyImmediate(dungeon);
        dungeon = new GameObject("Dungeon");

        // Create a roomHolder game object
        var roomHolder = new GameObject("RoomHolder");
        roomHolder.transform.parent = dungeon.transform;

        // Create an enemyHolder game object
        enemyHolder = new GameObject("EnemyHolder");
        enemyHolder.transform.parent = dungeon.transform;

        // Create an fillHolder game object
        var fillHolder = new GameObject("FillHolder");
        fillHolder.transform.parent = dungeon.transform;

        // Try to find the object ItemHolder and if it exists, delete it
        var itemHolder = GameObject.Find("ItemHolder");
        if (itemHolder != null)
            Object.DestroyImmediate(itemHolder);

        // Create an itemHolder game object
        itemHolder = new GameObject("ItemHolder");

        foreach (var room in graph.rooms)
        {
            // Instantiate the room
            var pos = room.Item1 * 2 * ROOMSIZE;
            var roomObj = Object.Instantiate(room.Item2, pos, Quaternion.identity);
            // Get child object called "enemies"
            var enemies = roomObj.transform.Find("Enemies");
            if (enemies != null)
            {
                // For all children of type EnemySpawner, obtain the object called enemies
                foreach (var enemySpawner in enemies.GetComponentsInChildren<EnemySpawner>())
                {
                    // Get random value between 0 and 1
                    var rand = Random.value;
                    var cummulativeChance = 0f;
                    foreach (var enemyChance in enemySpawner.enemies.list.OrderBy(enemy => enemy.spawnChance))
                    {
                        if (rand > cummulativeChance + enemyChance.spawnChance)
                        {
                            cummulativeChance += enemyChance.spawnChance;
                            continue;
                        }

                        var enemy = enemyChance.enemy;
                        // Instantiate the enemy
                        var enemyObj = Object.Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
                        // Set as child of enemyHolder
                        enemyObj.transform.parent = enemyHolder.transform;
                        break;
                    }
                }

                foreach (var enemy in enemies.GetComponentsInChildren<EnemyStats>())
                {
                    enemy.transform.parent = enemyHolder.transform;
                }
                // Remove the Enemies object from the room
                Object.DestroyImmediate(enemies.gameObject);
            }

            foreach (var item in roomObj.transform.GetComponentsInChildren<ItemPickup>())
            {
                item.transform.parent = itemHolder.transform;
            }

            // Finish setting up the room
            roomObj.name = room.Item2.name + " | " + room.Item1;
            roomObj.transform.parent = roomHolder.transform;
            prefabs.Add((pos, roomObj));
        }

        // Get minimum and maximum coordinates of the graph
        var minX = graph.rooms.Min(room => room.Item2.getDownLeftCorner().x + room.Item1.x);
        var maxX = graph.rooms.Max(room => room.Item2.getDownLeftCorner().x + room.Item2.getSize().x + room.Item1.x);
        var minY = graph.rooms.Min(room => room.Item2.getDownLeftCorner().y + room.Item1.y);
        var maxY = graph.rooms.Max(room => room.Item2.getDownLeftCorner().y + room.Item2.getSize().y + room.Item1.y);

        var fillRooms = Resources.LoadAll<GameObject>(areaPath + "filledRooms");

        for (var x = minX - 2; x < maxX + 2; x++)
        {
            for (var y = minY - 2; y < maxY + 2; y++)
            {
                // If there already is a node, continue
                if (graph.nodes.ContainsKey(new Vector2(x, y))) continue;

                if (!graph.checkExistNear(new Vector2(x, y), 2)) continue;

                //Pick a random fillRoom and instantiate it
                var fillRoom = fillRooms[Random.Range(0, fillRooms.Length - 1)];
                var pos = new Vector2(x, y) * 2 * ROOMSIZE;
                var fillObj = Object.Instantiate(fillRoom, pos, Quaternion.identity);
                fillObj.transform.position = pos;
                fillObj.transform.parent = fillHolder.transform;
                filledPrefabs.Add((pos, fillObj));
            }
        }

        // Add the roomHolder to the current scene
        SceneManager.MoveGameObjectToScene(dungeon, SceneManager.GetActiveScene());
    }

    public void initGeneration(string areaPath)
    {
        var initRooms = Resources.LoadAll<CustomRoom>(areaPath + "InitRooms");
        var initRoom = initRooms[Random.Range(0, initRooms.Length - 1)];
        remainingDoors = new List<Door>{new Door(new Vector2(0, 0), Direction.Up, initRoom, 0)};
        normalRooms = Resources.LoadAll<CustomRoom>(areaPath + "NormalRooms").ToList();
        closingRooms = Resources.LoadAll<CustomRoom>(areaPath + "ClosingRooms").ToList();
        usedRooms = new List<CustomRoom>();
        prefabs = new List<(Vector2, CustomRoom)>();
        filledPrefabs = new List<(Vector2, GameObject)>();
        graph = new DungeonGraph(initRoom, this);
        topDoor = new Door(new Vector2(0, 0), Direction.Up, initRoom, 0);
    }

    private void endGeneration(string areaPath)
    {
        var endRooms = Resources.LoadAll<CustomRoom>(areaPath + "EndRooms");
        var endRoom = endRooms[Random.Range(0, endRooms.Length - 1)];
        graph.placeRoom(topDoor.pos, new Vector2(0, 0), Direction.Up, endRoom);
        endRoomPos = topDoor.pos * 2 * LevelGenManager.ROOMSIZE;
    }

    private (bool, bool) nextRoom(int size, string areaPath)
    {
        // If we are out of rooms but still have to continue, we refill the normal room list
        if (normalRooms.All(roomElem => roomElem.repeatable))
        {
            normalRooms = Resources.LoadAll<CustomRoom>(areaPath + "NormalRooms").Where(roomElem => !roomElem.repeatable).ToList();
        }

        // Get a random element from the list and remove it
        var weightedDoors = new List<Door>();
        foreach (var door in remainingDoors)
        {
            switch (door.doorsInRoom)
            {
                case 2:
                    for (var i = 0; i < LevelGenManager.twoDoorRoomBias; i++)
                        weightedDoors.Add(door);
                    break;
                case 3:
                    for (var i = 0; i < LevelGenManager.threeDoorRoomBias; i++)
                        weightedDoors.Add(door);
                    break;
                default:
                    weightedDoors.Add(door);
                    break;
            }
        }

        var nextPos = weightedDoors[Random.Range(0, weightedDoors.Count)];
        remainingDoors.Remove(nextPos);
        var (room, entrance) = getRoom(size - graph.size, nextPos);
        if (room == null)
            return (false, false);
        var exits = graph.placeRoom(nextPos.pos, entrance, nextPos.dir, room);

        processNewDoors(exits);
        // For each remainining door, ensure that there is no adjacent node in the graph
        // If there is, remove it from the list
        remainingDoors = remainingDoors
            .Where(door => !graph.nodes.ContainsKey(door.pos + CustomRoom.dirVectors[(int)door.dir]))
            .ToList();

        // If it's a normal room, we remove so it does not repeat
        if (normalRooms.Contains(room) && !room.repeatable)
        {
            normalRooms.Remove(room);
            usedRooms.Add(room);
        }

        // If we have no more doors, we are done
        return (remainingDoors.Count == 0, true);
    }

    private void processNewDoors(ICollection<Door> newDoors)
    {
        // Get all doors that are looking upwards and get the one with the hightest y value
        var topDoors = newDoors
            .Where(door => door.dir == Direction.Up)
            .Where(door => graph.isTopDoorSuitable(door.pos + CustomRoom.dirVectors[(int)Direction.Up]))
            .OrderBy(door => door.pos.y)
            .ToList();
        
        if (topDoors.Count > 0 && topDoors.Last().pos.y > topDoor.pos.y)
        {
            remainingDoors.Add(topDoor);
            topDoor = topDoors.Last();
            newDoors.Remove(topDoor);
        }
        // Add all new doors to the remaining door list
        remainingDoors = remainingDoors.Concat(newDoors).ToList();
    }

    private bool tryGenerate(int size, string areaPath)
    {
        while (true)
        {
            var (finished, success) = nextRoom(size, areaPath);
            if (finished)
            {
                return graph.nodes.Count >= size;
            }
            if (!success) 
                return false;
        }
    }

    private (CustomRoom, Vector2) getRoom(int remainingSize, Door door)
    {
        (CustomRoom, Vector2) retRoom;

        if (remainingSize > 0)
        {
            retRoom = getNormalRoom(door, false);
            if (retRoom.Item1 == null)
                retRoom = getClosingRoom(door);
        }
        else
        {
            retRoom = getClosingRoom(door);
        }

        return retRoom;
    }

    private (CustomRoom, Vector2) getClosingRoom(Door door)
    {
        var rooms = closingRooms.ToList();

        // Get the posisition of the other node of the door
        var otherNodePos = door.pos + CustomRoom.dirVectors[(int)door.dir];

        // Get how many nodes adjacent to it have an open door looking at it
        var doorsToOpen = new[] { false, false, false, false };
        for (var i = 0; i < CustomRoom.dirVectors.Length; i++)
        {
            // Get the node adjacent to the otherNode
            var neighborPos = otherNodePos + CustomRoom.dirVectors[i];
            // If the node exists and has an open door looking at the otherNode, we set the door to open as true
            if (graph.nodes.ContainsKey(neighborPos) && graph.nodes[neighborPos].doors[CustomRoom.mirrorDir[i]])
                doorsToOpen[i] = true;
        }

        // Find closing rooms that have the same doors to open
        rooms = rooms
            .Where(room => room.roomNodes[new Vector2(0, 0)].doors.SequenceEqual(doorsToOpen))
            .ToList();
        
        // Return random room from the options
        if (rooms.Count == 0)
        {
            Debug.LogError("Missing closing room layout!! Do we have all posible rooms?");
            return (null, new Vector2(0, 0));
        }
        return (rooms[Random.Range(0, rooms.Count)], new Vector2(0, 0));
    }

    private (CustomRoom, Vector2) getNormalRoom(Door door, bool secondTry)
    {
        var rooms = normalRooms.ToList();
        if (secondTry)
        {
            rooms = normalRooms
                .Concat(usedRooms)
                .ToList();
        }
        else if (door.room.repeatable)
        {
            rooms = rooms.Where(room => !room.repeatable).ToList();
        }
        rooms = rooms.Where(room => room.name != door.room.name).ToList();
        // Shuffle filtered rooms
        rooms.Shuffle();
        // Find a room that can be placed, order by the lowest door
        var results = rooms
            // Get all result of testing the liability of the room
            .Select(room =>
            {
                var test = graph.testRoom(door.pos, door.dir, room);
                return (room, test.Item2, test.Item1);
            })
            //Filter all invalid ones
            .Where(result => result.Item3 != -1)
            //Order by the lowest door
            .OrderBy(result => result.Item3)
            .Select(result => (result.Item1, result.Item2))
            .ToList();
        if (results.Count == 0)
        {
            return secondTry ? (null, new Vector2()) : getNormalRoom(door, true);
        }
        // We return the best result and if the room is flipped
        return (results[0].Item1, results[0].Item2);
    }

    public void cullElements()
    {
        // Get the player position
        var camPos = Camera.main.transform.position;
        var cameraSize = Camera.main.orthographicSize;
        var cameraWidth = cameraSize * Camera.main.aspect;
        // Make the square that will be used to cull rooms
        var dist = LevelGenManager.cullDistance;
        var cullSquare = new Rect(
            camPos.x - (cameraWidth * dist), 
            camPos.y - (cameraSize * dist), 
            (cameraWidth * dist) * 2, 
            (cameraSize * dist) * 2);
        foreach (var room in prefabs)
        {
            var size = room.Item2.size * 2 * ROOMSIZE;
            var pos = room.Item2.minNode * 2 * ROOMSIZE - Vector2.one * ROOMSIZE + room.Item1;
            // Make room square
            var roomSquare = new Rect(pos.x, pos.y, size.x, size.y);
            room.Item2.gameObject.SetActive(cullSquare.Overlaps(roomSquare));
        }
        
        foreach (var fill in filledPrefabs)
        {
            var pos = fill.Item1 + Vector2.one * ROOMSIZE;
            // Make room square
            var roomSquare = new Rect(pos.x, pos.y, 2 * ROOMSIZE, 2 * ROOMSIZE);
            fill.Item2.gameObject.SetActive(cullSquare.Overlaps(roomSquare));
        }
        
        foreach (Transform enemy in enemyHolder.transform)
        {
            // Create rect with position and size of the enemy
            var enemySquare = new Rect(
                enemy.position.x - enemy.localScale.x / 2, 
                enemy.position.y - enemy.localScale.y / 2, 
                enemy.localScale.x, 
                enemy.localScale.y);

            enemy.gameObject.SetActive(cullSquare.Overlaps(enemySquare));
        }
    }
}