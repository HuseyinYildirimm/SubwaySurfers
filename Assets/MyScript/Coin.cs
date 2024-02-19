using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody rb;
    //Pos
    [SerializeField] private Ease easePos;
    [SerializeField] private float moveValueY;
    [SerializeField] private float posTime;
    private Tween rotTween;
    private Tween posTween;

    //Rot
    [SerializeField] private Ease easeRot;
    [SerializeField] private float rotTime;

    float randomTime;
    public bool isMoving;
    public bool killTween;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        //To prevent all coins from moving the same.
        randomTime = Random.Range(1f, 1.5f);
        isMoving = true;
        Invoke(nameof(CoinMove), randomTime);
    }

    public void Update()
    {
        if (isMoving)
        {
            if (killTween)
            {
                CoinMove();
            }
        }
        else
        {
            if (!killTween)
            {
                StopMove();
            }
        }
    }

    public void CoinMove()
    {
        killTween = false;

        posTween = transform.DOMoveY(transform.position.y + moveValueY, posTime)
              .SetLoops(-1, LoopType.Yoyo).SetEase(easePos);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Vector3 targetRot = new Vector3(0f, 180f, 0f);
        rotTween = transform.DORotate(targetRot, rotTime)
             .SetLoops(-1).SetEase(easeRot);
    }

    public void StopMove()
    {
        killTween = true;

        posTween.Complete();
        rotTween.Complete();
        posTween.Kill();
        rotTween.Kill();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            gameObject.SetActive(false);
            GameManager._instance.CollectCoin();
        }
    }
}
