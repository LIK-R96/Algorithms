using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Drawer : MonoBehaviour
{
    public int[] baseParam;
    public int roomCount = 1;
    RectInt rectangleMain;
    List<RectInt> roomList;
    private bool initalCut;
    void Start()
    {
        roomList = new List<RectInt>();
        rectangleMain = new RectInt(0, 0, baseParam[0], baseParam[1]);
        //AlgorithmsUtils.DebugRectInt(rectangleMain, Color.blue, float.MaxValue);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !initalCut)
        {
            RectInt roomA = new RectInt(0, 0, rectangleMain.width, rectangleMain.height / 2 + 1);
            RectInt roomB = new RectInt(0, rectangleMain.height, rectangleMain.width, -rectangleMain.height / 2 - 1);
            AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
            AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);
            initalCut = true;
            StartCoroutine(CuttingAdditionalRooms);
        }
        else if (Input.GetKeyDown(KeyCode.F) && !initalCut)
        {
            RectInt roomA = new RectInt(0, 0, rectangleMain.width / 2 + 1, rectangleMain.height);
            RectInt roomB = new RectInt(rectangleMain.width, 0, -rectangleMain.width / 2 - 1, rectangleMain.height);
            AlgorithmsUtils.DebugRectInt(roomA, Color.yellow, float.MaxValue);
            AlgorithmsUtils.DebugRectInt(roomB, Color.yellow, float.MaxValue);
            initalCut = true;
        }
    }

}
