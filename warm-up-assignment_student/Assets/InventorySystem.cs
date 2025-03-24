using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public string item = "item";
    public Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
    public List<string> ItemKeys = new();
    public Graph<string> graph = new Graph<string>();
    public string startPoint;
    public string edgePoint;
    [Button("Add Node")]
    public void Add()
    {
        graph.AddNode(startPoint);
    }
    [Button("Add Edge")]
    public void Edge()
    {
        graph.AddEdge(startPoint, edgePoint);
    }
    [Button("Print Nodes")]
    public void CheckItem()     
    {
        graph.PrintNodes();
    }
    
}
