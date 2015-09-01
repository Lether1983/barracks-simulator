using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class QuickSort
{
	public void Sort(List<GroundTile> openList,int left,int right)
    {
        if(left < right)
        {
            int pivot = Partition(openList, left, right);
            Sort(openList, left, pivot - 1);
            Sort(openList, pivot + 1, right);
        }

    }

    public int Partition(List<GroundTile> openList,int left,int right)
    {
        int i = left;
        int j = right - 1;
        float pivot = openList[right].finalCost;

        while (i < j)
        {
            while (openList[i].finalCost <= pivot && i < right)
            {
                i++;
            }
            while (openList[j].finalCost >= pivot && j > left)
            {
                j--;
            }
            if (i < j)
            {
                GroundTile temp = openList[i];
                openList[i] = openList[j];
                openList[j] = temp;
            }
        }

        if (openList[i].finalCost > pivot)
        {
            GroundTile temp = openList[i];
            openList[i] = openList[right];
            openList[right] = temp;
        }
        return i;
    }
}
