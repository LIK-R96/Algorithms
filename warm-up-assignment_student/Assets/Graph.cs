using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> points;
    public Graph() { points = new Dictionary<T, List<T>>(); }

    public void AddNode(T node)
    {
        if (!points.ContainsKey(node))
        {
            points.Add(node, new List<T>());    
        }

    }
    public void AddEdge(T fromNode, T toNode)
    {
        if (!points[fromNode].Contains(toNode))
        {
            points[fromNode].Add(toNode);
        }
    }
    public void PrintNodes() 
    {
        foreach (var node in points)
        {
            Debug.Log($"{node.Key} connects with {string.Join(", ", node.Value)}");
        }
    }
}