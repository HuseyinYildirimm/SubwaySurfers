using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoinSystem : MonoBehaviour
{
    [SerializeField] private RoadController roadController;

    private readonly List<GameObject> coinTypeList = new List<GameObject>();
    public List<Coin> coinList = new List<Coin>();

    public void Start()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform childTransform in this.transform)
            {
                GameObject childObject = childTransform.gameObject;
                coinTypeList.Add(childObject);

                foreach (Transform coin in childObject.transform)
                {
                    Coin coinComponent = coin.GetComponent<Coin>();
                    coinList.Add(coinComponent);
                }
            }

            CreatingRandomCoin();
        }
        else Debug.Log("null");
    }

    public void FixedUpdate()
    {
        if (roadController.passedRoad != null)
        {
            if (roadController.passedRoad.name == FindParentBeforeTop(gameObject.transform).name
                && roadController.roadTrigger)
            {
                CreatingRandomCoin();
            }
        }
    }

    public void CreatingRandomCoin()
    {
        foreach (Coin coin in coinList)
        {
            coin.GetComponent<Rigidbody>().isKinematic = false;
            coin.GetComponent<MeshCollider>().isTrigger = false;
            coin.isMoving = false;

            Vector3 newPosition = new Vector3(coin.transform.position.x, 2.5f, coin.transform.position.z);
            coin.transform.position = newPosition;

            coin.gameObject.SetActive(true);

            StartCoroutine(CoinRigidbodyControl(coin));
        }

        int typeNumber = Random.Range(0, coinTypeList.Count);

        for (int i = 0; i < coinTypeList.Count; i++)
        {
            if (i == typeNumber)
                coinTypeList[i].SetActive(true);
            else
                coinTypeList[i].SetActive(false);
        }
    }

    IEnumerator CoinRigidbodyControl(Coin coin)
    {
        yield return new WaitForSeconds(1f);

        coin.GetComponent<Rigidbody>().isKinematic = true;
        coin.GetComponent<MeshCollider>().isTrigger = true;
        coin.isMoving = true;

    }

    Transform FindParentBeforeTop(Transform childTransform)
    {
        Transform currentParent = childTransform;
        Transform previousParent = null;

        while (currentParent.parent != null)
        {
            previousParent = currentParent;
            currentParent = currentParent.parent;
        }

        return previousParent;
    }
}
