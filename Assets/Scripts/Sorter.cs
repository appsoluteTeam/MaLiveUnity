﻿using Model;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour {
    
    //이 스크립트는 잘 모르겠다. 
    //unit들
    private LinkedList<BaseUnit> sortList;

    public void SortAll()
    {
        sortList = new LinkedList<BaseUnit>();
        foreach (Transform child_n in transform)
        {
            var unit = child_n.GetComponent<BaseUnit>();
            if (unit == null) continue;
            Add(unit);
        }

        Order();
    }

    public void SortUnit(BaseUnit unit)
    {
        if (sortList == null)
            return;
        sortList.Remove(unit);
        Add(unit);
        Order();
    }

    private void Order()
    {
        var node = sortList.First;
        int i = sortList.Count;
        while (node != null)
        {
            var sortedUnit = node.Value;
            sortedUnit.order = i--;
            node = node.Next;
        }
    }

    private void Add(BaseUnit unit)
    {
        if (unit.origin == null)
            return;
        
        if (sortList.Count == 0) {
            sortList.AddFirst(unit);
            return;
        }

        bool isSorted = false;
        var node = sortList.First;
        while (node != null)
        {
            if (OrderCompare(unit, node.Value))
            {
                sortList.AddBefore(node, unit);
                isSorted = true;
                break;
            }
            node = node.Next;
        }

        if (!isSorted)
            sortList.AddLast(unit);
    }

    private bool OrderCompare(BaseUnit unit, BaseUnit sortedUnit)
    {
        var tiles = sortedUnit.GetTiles();
        if (tiles.Count == 0)
            tiles.Add(sortedUnit.origin);

        for (int x = unit.origin.x; x < unit.origin.x + unit.width; x++)
        {
            foreach (var tile in tiles)
            {
                if (tile.x == x && tile.z == sortedUnit.origin.z)
                {
                    if (unit.origin.z < sortedUnit.origin.z)
                        return true;
                }
            }
        }

        for (int z = unit.origin.z; z < unit.origin.z + unit.length; z++)
        {
            foreach (var tile in tiles)
            {
                if (tile.x == sortedUnit.origin.x && tile.z == z)
                {
                    if (unit.origin.x < sortedUnit.origin.x)
                        return true;
                }
            }
        }
       
        if (unit.origin.x + unit.origin.z < sortedUnit.origin.z + sortedUnit.origin.x)
            return true;

        return false;
    }
}  
