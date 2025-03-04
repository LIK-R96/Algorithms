using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Drawer : MonoBehaviour
{
    public int[] baseParam;
    public int roomCount = 1;
    RectInt rectangleMain;
    public List<RectInt> roomList;
    private bool initalCut;
    public int totalRoomCount;
    public RectInt minRoomSize;
    public RectInt maxRoomSize;
    private bool horizontalCut;
    private bool verticalCut;
    void Start()
    {
        roomList = new List<RectInt>();
        rectangleMain = new RectInt(0, 0, baseParam[0], baseParam[1]);
        //AlgorithmsUtils.DebugRectInt(rectangleMain, Color.blue, float.MaxValue);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !horizontalCut)
        {
            RectInt roomA = new RectInt(0, 0, rectangleMain.width, rectangleMain.height / 2 + 1);
            roomList.Add(roomA);
            RectInt roomB = new RectInt(0, rectangleMain.height, rectangleMain.width, -rectangleMain.height / 2 - 1);
            roomList.Add(roomB);
            AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
            AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);
            verticalCut = true;
            StartCoroutine(CuttingAdditionalRooms());
        }
        else if (Input.GetKeyDown(KeyCode.F) && !verticalCut)
        {
            RectInt roomA = new RectInt(0, 0, rectangleMain.width / 2 + 1, rectangleMain.height);
            RectInt roomB = new RectInt(rectangleMain.width, 0, -rectangleMain.width / 2 - 1, rectangleMain.height);
            AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
            AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);
            horizontalCut = true;
        }
        foreach (var room in roomList)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.yellow, float.MaxValue);
        }
    }

    public IEnumerator CuttingAdditionalRooms() 
    {
        List<RectInt> tempList = new List<RectInt>();
        foreach (var room in roomList)
        {
            if (verticalCut)
            {
                RectInt newRoomA = new RectInt(room.x, room.y, room.width / 2 - 1, room.height);
                tempList.Add(newRoomA);
                RectInt newRoomB = new RectInt(-room.x, room.y, room.width / 2 + 1, room.height);
                tempList.Add(newRoomA);
                RectInt roomC = AlgorithmsUtils.Intersect(newRoomA, newRoomB);
                AlgorithmsUtils.DebugRectInt(newRoomA, Color.yellow, float.MaxValue);
                AlgorithmsUtils.DebugRectInt(newRoomB, Color.yellow, float.MaxValue);

            }
            else if (horizontalCut)
            {
                RectInt newRoomA = new RectInt(room.x, room.y, room.width, room.height / 2 + 1);
                RectInt newRoomB = new RectInt(room.x, -room.y, room.width, room.height / 2 - 1);
                AlgorithmsUtils.DebugRectInt(newRoomA, Color.yellow, float.MaxValue);
                AlgorithmsUtils.DebugRectInt(newRoomB, Color.yellow, float.MaxValue);
            }
        }
        roomList = tempList;
        yield return null;
    }



}
