using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timer;
    [SerializeField] private Transform spawnPoint;


    public void Start()
    {
        InvokeRepeating("Spawn", timer, timer);
    }

    public void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * -transform.forward);
    }

    void Spawn()
    {
        transform.position = spawnPoint.position;
    }
}
