using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CharacterHealth : MonoBehaviour
{
    CharacterController characterController;
    public Animator anim;

    [Header("Health")]
    public int health;
    [SerializeField] private List<Image> hearthImage;
    [SerializeField] private Sprite fullHearth;
    [SerializeField] private Sprite emptyHearth;
    [HideInInspector] public bool collision;
    private float opportunityTime;

    [Space(10)]

    [Header("Material")]
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material changeableMaterial;
    private Color startColor;
    public float fadeSpeed = 1f;

    [Space(10)]

    [Header("CameraShake")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float shakeTime;


    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
        var noiseProfile = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseProfile.m_FrequencyGain = 0;
    }

    public void Start()
    {
        health = hearthImage.Count;
        opportunityTime = shakeTime;

        characterController.GetComponentInChildren<SkinnedMeshRenderer>().material = mainMaterial;

        if (mainMaterial != null)
            startColor = mainMaterial.color;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!collision && health !=0)
            {
                if (health > 1 )
                {
                    anim.Play("Hit");
                }

                else if (health == 1 )
                {
                    anim.Play("GameOver");
                }

                collision = true;
                health--;
                characterController.speed -= 0.5f;

                StartCoroutine(CollisionCheck());
                StartCoroutine(CameraShakeControl());

                if (hearthImage[health] != null)
                    hearthImage[health].sprite = emptyHearth;
            }
        }
    }

    IEnumerator CollisionCheck()
    {
        yield return new WaitForSeconds(opportunityTime);
        collision = false;
    }

    IEnumerator CameraShakeControl()
    {
        var noiseProfile = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseProfile.m_FrequencyGain = 2;

        float alfa = 0.0f;
        float t = 0f;
        bool increaseDirection = true;

        characterController.GetComponentInChildren<SkinnedMeshRenderer>().material = changeableMaterial;
        

        while (t < shakeTime)
        {
            if (increaseDirection)
            {
                alfa += fadeSpeed * Time.deltaTime;
            }
            else
            {
                alfa -= fadeSpeed * Time.deltaTime;
            }

            alfa = Mathf.Clamp01(alfa);

            changeableMaterial.color = new Color(mainMaterial.color.r, mainMaterial.color.g, mainMaterial.color.b, alfa);

            if (alfa >= 1.0f || alfa <= 0.0f)
            {
                increaseDirection = !increaseDirection;
            }

            t += Time.deltaTime;
            yield return null;
        }

        noiseProfile.m_FrequencyGain = 0;
        characterController.GetComponentInChildren<SkinnedMeshRenderer>().material = mainMaterial;
    }

    private void OnApplicationQuit()
    {
        if (mainMaterial != null)
        {
            characterController.GetComponentInChildren<SkinnedMeshRenderer>().material = mainMaterial;
        }
    }
}
