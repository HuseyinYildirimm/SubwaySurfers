using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject SubwayRoadParent;
    [SerializeField] private List<Transform> SubwayRoadList;

    int index;

    [HideInInspector] public GameObject passedRoad;
    [HideInInspector] public bool roadTrigger;

    public void Start()
    {
        index = SubwayRoadList.Count - 2;
    }

    public void OnTriggerEnter(Collider road)
    {
        if (road.gameObject.CompareTag("RoadControl"))
        {
            roadTrigger = true;

            int roadIndex = road.gameObject.transform.GetSiblingIndex();
            GameObject childObj;
            if (roadIndex > 0)
            {
                childObj = SubwayRoadList[roadIndex - 1].gameObject;
                if (childObj != null)
                {
                    RoadListControl(childObj);
                    passedRoad = childObj;
                }
            }
            else
            {
                childObj = SubwayRoadList[SubwayRoadList.Count-1].gameObject;
                if (childObj != null)
                {
                    RoadListControl(childObj);
                    passedRoad = childObj;
                }
            }

            Transform obstacleParent = childObj.gameObject.transform.GetChild(0);

            RandomObstacleSystem[] obstacleSystems = obstacleParent.GetComponentsInChildren<RandomObstacleSystem>();

            foreach (RandomObstacleSystem obstacle in obstacleSystems)
            {
                obstacle.RandomObstacle();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        roadTrigger = false;
    }

    void RoadListControl(GameObject child)
    {
        index++;
        if (index >= SubwayRoadList.Count)
            index = 0;

        Vector3 lastObjectPos = SubwayRoadList[index].transform.position;
        child.transform.position = new Vector3(1, 0, lastObjectPos.z + 84f);
    }


}
