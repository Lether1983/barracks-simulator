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
            if(pivot > 1)
            {
                Sort(openList,left,pivot-1);
            }
            if(pivot +1 < right)
            {
                Sort(openList, pivot + 1, right);
            }
        }

    }

    public int Partition(List<GroundTile> openList,int left,int right)
    {
        float pivot = openList[left].finalCost;

        while (true)
        {
            while (openList[left].finalCost < pivot)
            {
                left++;   
            }
            while (openList[right].finalCost > pivot)
            {
                right--;
            }
            if(left < right)
            {
                float temp = openList[right].finalCost;
                openList[right].finalCost = openList[left].finalCost;
                openList[left].finalCost = temp;
            }
            else
            {
                return right;
            }
        }
    }
}
