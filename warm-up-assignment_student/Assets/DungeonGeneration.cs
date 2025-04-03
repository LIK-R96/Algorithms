using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Drawer : MonoBehaviour
{
    public int[] baseParam;
    public int roomCount = 1;
    RectInt rectangleMain;
    //make a list of the total rooms, that are unsplit
    private bool initalCut;
    public int totalRoomCount;
    //get a minimal room size
    public int minRoomSize;
    Queue<RectInt> roomQueue = new Queue<RectInt>(); // Initialize an empty queue

    [SerializeField]private bool horizontalCut;
   [SerializeField]private bool verticalCut;
    //make a list so that rooms dont repeat after they split once.
    public List<RectInt> CompleteRooms = new();
    void Start()
    {
        CompleteRooms = new List<RectInt>();
        rectangleMain = new RectInt(0, 0, baseParam[0], baseParam[1]);
        AlgorithmsUtils.DebugRectInt(rectangleMain, Color.red, float.MaxValue);
        CompleteRooms.Add(rectangleMain);
        // Enqueue only the initial room, instead of using roomList reference
        roomQueue.Enqueue(rectangleMain);

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !horizontalCut && !verticalCut)
        //{
        //    RectInt roomA = new RectInt(0, 0, rectangleMain.width, rectangleMain.height / 2);
        //    roomList.Add(roomA);
        //    RectInt roomB = new RectInt(0, rectangleMain.height, rectangleMain.width, -rectangleMain.height / 2 - 1);
        //    roomList.Add(roomB);
        //    AlgorithmsUtils.DebugRectInt(roomA, Color.yellow);
        //    AlgorithmsUtils.DebugRectInt(roomB, Color.yellow);
        //    verticalCut = true;
        //   // StartCoroutine(CuttingAdditionalRooms());
        //}
        //else if (Input.GetKeyDown(KeyCode.F) && !verticalCut && !horizontalCut)
        //{
        //    RectInt roomA = new RectInt(0, 0, rectangleMain.width / 2 + 1, rectangleMain.height);
        //    roomList.Add(roomA);
        //    RectInt roomB = new RectInt(rectangleMain.width, 0, -rectangleMain.width / 2 - 1, rectangleMain.height);
        //    roomList.Add(roomB);
        //    AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
        //    AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);
        //    horizontalCut = true;
        //    StartCoroutine(CuttingAdditionalRooms());
        //}
        StartCoroutine(CutRooms());
       // StartCoroutine(SplitHorizontally());
        foreach (var room in CompleteRooms)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.yellow);
        }
    }

    //public IEnumerator CuttingAdditionalRooms() 
    //{
    //    List<RectInt> tempList = new List<RectInt>();
    //    foreach (var room in roomList)
    //    {
    //        if (verticalCut)
    //        {
    //            RectInt newRoomA = new RectInt(room.x, room.y, room.width / 2, room.height);
    //            tempList.Add(newRoomA);
    //            RectInt newRoomB = new RectInt(-room.x, room.y, room.width / 2 - 1, room.height);
    //            tempList.Add(newRoomA);
    //            RectInt roomC = AlgorithmsUtils.Intersect(newRoomA, newRoomB);
    //            AlgorithmsUtils.DebugRectInt(newRoomA, Color.yellow, float.MaxValue);
    //            AlgorithmsUtils.DebugRectInt(newRoomB, Color.yellow, float.MaxValue);
    //            Debug.Log($"{roomList.IndexOf(room)} {newRoomA}");
    //            Debug.Log($"{roomList.IndexOf(room)} {newRoomB}");

    //        }
    //        else if (horizontalCut)
    //        {
    //            RectInt newRoomA = new RectInt(room.x, room.y, room.width, room.height / 2);
    //            RectInt newRoomB = new RectInt(room.x, -room.y, room.width, room.height / 2 - 1);
    //            AlgorithmsUtils.DebugRectInt(newRoomA, Color.yellow, float.MaxValue);
    //            AlgorithmsUtils.DebugRectInt(newRoomB, Color.yellow, float.MaxValue);
    //            Debug.Log($"{newRoomA.width} + {newRoomA.height}");
    //            Debug.Log($"{newRoomB.width} + {newRoomB.height}");
    //        }
    //    }
    //    roomList.Clear();
    //    roomList = tempList;
    //    yield return null;
    //}
    public IEnumerator CutRooms()
    {
        while (roomQueue.Count > 0)
        {
            RectInt currentRoom = roomQueue.Dequeue(); // Remove first room from queue

            // Check if room is too small to split further
            bool canSplitVertically = currentRoom.width >= 2 * minRoomSize;
            bool canSplitHorizontally = currentRoom.height >= 2 * minRoomSize;

            if (!canSplitVertically && !canSplitHorizontally)
            {
                CompleteRooms.Add(currentRoom); // Store completed rooms
                continue;
            }

            bool splitHorizontally;

            if (canSplitVertically && canSplitHorizontally)
            {
                // Randomly choose split direction if both are possible
                splitHorizontally = Random.value > 0.5f;
            }
            else
            {
                // Force split in the only possible direction
                splitHorizontally = canSplitHorizontally;
            }

            if (splitHorizontally)
            {
                // Horizontal Split (splitting by height)
                int randomY = Random.Range(minRoomSize, currentRoom.height - minRoomSize);

                RectInt roomA = new RectInt(currentRoom.x, currentRoom.y, currentRoom.width, randomY + 1); // Overlap by 1
                RectInt roomB = new RectInt(currentRoom.x, currentRoom.y + randomY, currentRoom.width, currentRoom.height - randomY);

                // Debug visualization
                AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
                AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);

                if (roomA.height >= minRoomSize) roomQueue.Enqueue(roomA);
                else CompleteRooms.Add(roomA);

                if (roomB.height >= minRoomSize) roomQueue.Enqueue(roomB);
                else CompleteRooms.Add(roomB);
            }
            else
            {
                // Vertical Split (splitting by width)
                int randomX = Random.Range(minRoomSize, currentRoom.width - minRoomSize);

                RectInt roomA = new RectInt(currentRoom.x, currentRoom.y, randomX + 1, currentRoom.height); // Overlap by 1
                RectInt roomB = new RectInt(currentRoom.x + randomX, currentRoom.y, currentRoom.width - randomX, currentRoom.height);

                // Debug visualization
                AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
                AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);

                if (roomA.width >= minRoomSize) roomQueue.Enqueue(roomA);
                else CompleteRooms.Add(roomA);

                if (roomB.width >= minRoomSize) roomQueue.Enqueue(roomB);
                else CompleteRooms.Add(roomB);
            }

            yield return new WaitForSeconds(0.5f); // Delay to visualize changes
        }


    }
    //IEnumerator SplitHorizontally()
    //{
    //    while (roomQueue.Count > 0)
    //    {
    //        RectInt currentRoom = roomQueue.Dequeue(); // Remove first room from queue
    //        int randomY = Random.Range(minRoomSize, currentRoom.height);

    //        if (randomY >= minRoomSize && (currentRoom.height - randomY) >= minRoomSize)
    //        {
    //            RectInt roomA = new RectInt(currentRoom.x, currentRoom.y, currentRoom.width, randomY);
    //            RectInt roomB = new RectInt(currentRoom.x, currentRoom.y + randomY, currentRoom.width, currentRoom.height - randomY);

    //            // Debug visualization
    //            AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
    //            AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);

    //            // Add rooms to list
    //            roomList.Add(roomA);
    //            roomList.Add(roomB);

    //            // Enqueue only if they meet min size
    //            if (roomA.height >= minRoomSize) roomQueue.Enqueue(roomA);
    //            if (roomB.height >= minRoomSize) roomQueue.Enqueue(roomB);
    //        }
    //        else
    //        {
    //            CompleteRooms.Add(currentRoom);
    //        }

    //        yield return new WaitForSeconds(0.5f); // Delay to visualize changes
    //    }
    //}
}
