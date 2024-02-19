using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    CharacterHealth characterHealth;
    CapsuleCollider capsuleCol;
    Rigidbody rb;

    public float speed;
    [SerializeField] private float speedIncrease;
    [SerializeField] private float sideSpeed;

    [Space(5)]

    [SerializeField] private float pathIndex;
    [SerializeField] private float indexForTransform;

    [Space(5)]

    //Swipe
    public float swipeThreshold = 50f;
    private Vector2 touchStartPos;

    [Space(10)]

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRad;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    [Space(10)]


    [Header("Crouch")]
    [SerializeField] private float slipHeight;
    [SerializeField] private float slipYvalue;
    [SerializeField] private float slipDelay;
    float colFirstHeight;
    float colFirstY;
    bool isSliping;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCol = GetComponent<CapsuleCollider>();
        characterHealth = GetComponent<CharacterHealth>();
    }

    public void Start()
    {
        pathIndex = 0;
        colFirstHeight = capsuleCol.height;
        colFirstY = capsuleCol.center.y;
    }

    public void FixedUpdate()
    {
        if (characterHealth.health == 0)
        {
            speed = 0f;
        }
        else
        {
            Movement(pathIndex);
            speed += speedIncrease * Time.deltaTime;
        }

    }

    public void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        // Pc & Web
        KeyboardInput();
#elif UNITY_ANDROID || UNITY_IOS
        // Mobile
        MobileInput();
#endif

        if (Physics.CheckSphere(groundCheck.position, groundCheckRad, groundMask))
        {
            isGrounded = true;
        }else
            isGrounded = false;
    }

    #region Input

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (pathIndex == 0)
            {
                pathIndex = -indexForTransform;
            }
            else if (pathIndex == indexForTransform)
            {
                pathIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (pathIndex == 0)
            {
                pathIndex = indexForTransform;
            }
            else if (pathIndex == -indexForTransform)
            {
                pathIndex = 0;
            }
        }

        if (isGrounded && !isSliping && characterHealth.health != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Slip(slipHeight, slipYvalue, true);
                StartCoroutine(CrouchControl());
            }
        }
    }

    private void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    float swipeDistanceX = touch.position.x - touchStartPos.x;
                    float swipeDistanceY = touch.position.y - touchStartPos.y;

                    if (Mathf.Abs(swipeDistanceX) > swipeThreshold || Mathf.Abs(swipeDistanceY) > swipeThreshold)
                    {
                        if (Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY))
                        {
                            // Horizontal Swipe
                            if (swipeDistanceX > 0)
                            {
                                if (pathIndex == 0)
                                {
                                    pathIndex = -indexForTransform;
                                }
                                else if (pathIndex == indexForTransform)
                                {
                                    pathIndex = -indexForTransform;
                                }
                            }
                            else
                            {
                                if (pathIndex == 0)
                                {
                                    pathIndex = indexForTransform;
                                }
                                else if (pathIndex == -indexForTransform)
                                {
                                    pathIndex = 0;
                                }
                            }
                        }
                        else if (isGrounded && !isSliping && characterHealth.health != 0)
                        {
                            // Vertical Swipe
                            if (swipeDistanceY > 0)
                            {
                                Jump();
                            }
                            else
                            {
                                Slip(slipHeight, slipYvalue, true);
                                StartCoroutine(CrouchControl());
                            }
                        }
                    }
                    break;
            }
        }

    }

    #endregion

    private void Movement(float targetXValue)
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);

        Vector3 target = new Vector3(targetXValue, transform.position.y, transform.position.z);

        transform.position = Vector3.Slerp(transform.position, target, sideSpeed * Time.deltaTime);
    }

    private void Slip(float height, float yValue, bool slip)
    {
        isSliping = slip;

        capsuleCol.height = height;
        capsuleCol.center = new Vector3(-0.018f, yValue, 0f);

        if (isSliping)
            characterHealth.anim.Play("Slip");
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * 1000 * jumpForce, ForceMode.Impulse);

        characterHealth.anim.Play("Jump");
    }

    private IEnumerator CrouchControl()
    {
        yield return new WaitForSeconds(slipDelay);

        Slip(colFirstHeight, colFirstY, false);
    }
}
