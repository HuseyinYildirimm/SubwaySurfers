using System.Collections;
using UnityEngine;

public class MeshTrailSystem : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderers;
    [SerializeField] private Material mat;

    public Transform positionSpawn;
    public float activeTime = 2f;
    public float meshRefresh = 0.1f;
    public float meshDestroyDelay;
    public bool isTrailActive;

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !isTrailActive)
        {
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefresh;

            GameObject gObj = new GameObject();
            gObj.transform.SetPositionAndRotation(positionSpawn.position, positionSpawn.rotation);

            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            MeshFilter mf = gObj.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            skinnedMeshRenderers.BakeMesh(mesh);

            mf.mesh = mesh;
            mr.material = mat;
            Destroy(gObj, meshDestroyDelay);

            yield return new WaitForSeconds(meshRefresh);

        }
        isTrailActive = false;
    }
}
