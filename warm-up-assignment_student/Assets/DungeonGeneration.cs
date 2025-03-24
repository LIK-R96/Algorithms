using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Drawer : MonoBehaviour
{
    public int[] baseParam;
    public int roomCount = 1;
    RectInt rectangleMain;
    //make a list of the total rooms, that are unsplit
    public List<RectInt> roomList;
    private bool initalCut;
    public int totalRoomCount;
    //get a minimal room size
    public int minRoomSize;
    Queue<RectInt> roomQueue = new Queue<RectInt>(); // Initialize an empty queue

    [SerializeField]private bool horizontalCut;
   [SerializeField]private bool verticalCut;
    //make a list so that rooms dont repeat after they split once.
    private List<RectInt> CompleteRooms = new();
    void Start()
    {
        roomList = new List<RectInt>();
        rectangleMain = new RectInt(0, 0, baseParam[0], baseParam[1]);
        AlgorithmsUtils.DebugRectInt(rectangleMain, Color.red, float.MaxValue);
        roomList.Add(rectangleMain);
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
        foreach (var room in roomList)
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

            int randomX = Random.Range(minRoomSize, currentRoom.width);

            // Ensure the split is valid
            if (randomX >= minRoomSize)
            {
                RectInt roomA = new RectInt(currentRoom.x, currentRoom.y, randomX, currentRoom.height);
                RectInt roomB = new RectInt(currentRoom.x + randomX, currentRoom.y, currentRoom.width - randomX, currentRoom.height);

                // Debug visualization
                AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
                AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);

                // Add rooms to list (tracking all rooms)
                roomList.Add(roomA);
                roomList.Add(roomB);

                // Enqueue new rooms for further splitting
                roomQueue.Enqueue(roomA);
                roomQueue.Enqueue(roomB);
            }
            else
            {
                roomQueue.Enqueue(currentRoom);
            }

            yield return new WaitForSeconds(0.5f); // Delay to visualize changes
        }

        Debug.Log($"Total Rooms Generated: {CompleteRooms.Count}");
        Debug.Log($": {roomQueue.Count}");

    }
}
