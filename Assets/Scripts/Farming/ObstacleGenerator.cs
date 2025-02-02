using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [Range(0, 100)]
    public int percentageFilled;

    public void GenerateObstacles(List<Land> landPlots)
    {
        // lay % phu tren tong so
        int plotsToFill = Mathf.RoundToInt((float)percentageFilled/100 * landPlots.Count);

        // 1 list voi land IDs jumbled up
        List<int> shuffledList = ShuffleLandIndexes(landPlots.Count);

        for(int i = 0; i < plotsToFill; i++)
        {
            // take land id
            int index = shuffledList[i];

            // random ob to spawn
            Land.FarmObstacleStatus status = (Land.FarmObstacleStatus)Random.Range(1, 4);

            // set the land plot accordingly
            landPlots[index].SetObstacleStatus(status);
        }
    }

    // xoa tron index
    List<int> ShuffleLandIndexes(int count)
    {
        List<int> listToReturn = new List<int>();
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, i + 1);
            listToReturn.Insert(index, i);
        }
        return listToReturn;
    }
}
