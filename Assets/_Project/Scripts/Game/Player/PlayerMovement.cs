// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Animator playerAnimator, cameraAnimator;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    public float sensitivity = 50f;

    private float sensMultiplier = 1f;
    [SerializeField] private float tilt, camTilt, camTiltTime;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    LevelController levelController;

    private PlayerGameInput playerGameInput;

    private float mouseSensitivity;
    private float gamepadSensitivity;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpAudioClip;
    [SerializeField] private AudioClip landAudioClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerGameInput = GetComponent<PlayerGameInput>();

        MouseSliderUI.OnMouseSensibilityChanged += HandleOnMouseSensibilityChanged;
        GamepadSliderUI.OnGamepadSensibilityChanged += HandleOnGamepadSensibilityChanged;

        float mouseSensibility = PlayerPrefs.GetFloat(ConfigData.MOUSE_SENSIBILITY_KEY, 0.5f);
        float gamepadSensibility = PlayerPrefs.GetFloat(ConfigData.GAMEPAD_SENSIBILITY_KEY, 0.3f);

        HandleOnMouseSensibilityChanged(mouseSensibility);
        HandleOnGamepadSensibilityChanged(gamepadSensibility);
    }

    void Oestroy()
    {
        MouseSliderUI.OnMouseSensibilityChanged -= HandleOnMouseSensibilityChanged;
        GamepadSliderUI.OnGamepadSensibilityChanged -= HandleOnGamepadSensibilityChanged;
    }

    private void HandleOnMouseSensibilityChanged(float value)
    {
        mouseSensitivity = value * 10f;
        //SetMouseSensitivity(value);
    }

    private void HandleOnGamepadSensibilityChanged(float value)
    {
        gamepadSensitivity = value * 1000;
        //SetGamepadSensitivity(value);
    }

    private void SetMouseSensitivity(float value)
    {
        sensitivity = value * 10f;
    }

    private void SetGamepadSensitivity(float value)
    {
        sensitivity = value;
    }


    void Start()
    {
        levelController = GameObject.Find("Level").GetComponent<LevelController>();
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {
        //Movement();
    }

    private void Update()
    {
        if (!levelController.pause) MyInput();
        if (!levelController.pause) Look();
        if (!levelController.pause) Movement();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        Vector2 movement = playerGameInput.GetMoveValue().normalized;
        bool isJumpInput = playerGameInput.IsJumpInput();
        bool isCrouchHeldInput = playerGameInput.IsCrouchHeldInput();

        /*
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);
        */

        x = Mathf.RoundToInt(movement.x);
        y = Mathf.RoundToInt(movement.y);
        jumping = isJumpInput;
        crouching = isCrouchHeldInput;


        if (x != 0)
        {
            playerAnimator.SetInteger("Movement", (int)Mathf.Abs(x));
            cameraAnimator.SetInteger("Movement", (int)Mathf.Abs(x));
        }
        else if (y != 0)
        {
            playerAnimator.SetInteger("Movement", (int)Mathf.Abs(y));
            cameraAnimator.SetInteger("Movement", (int)Mathf.Abs(y));
        }
        else
        {
            playerAnimator.SetInteger("Movement", 0);
            cameraAnimator.SetInteger("Movement", 0);
        }

        //Crouching
        /*
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
        */

        if (playerGameInput.IsCrouchStartInput())
            StartCrouch();
        if (playerGameInput.IsCrouchStopInput())
            StopCrouch();

    }

    public void StopAnimations()
    {
        playerAnimator.SetBool("Grounded", true);
        playerAnimator.SetInteger("Movement", 0);
        cameraAnimator.SetInteger("Movement", 0);
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            playerAnimator.SetBool("Grounded", false);
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        else playerAnimator.SetBool("Grounded", true);

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            audioSource.PlayOneShot(jumpAudioClip);
            cameraAnimator.SetTrigger("Jump");
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.linearVelocity;
            if (rb.linearVelocity.y < 0.5f)
                rb.linearVelocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {

        sensitivity = mouseSensitivity;

        if (Gamepad.current != null && Mouse.current != null && Mouse.current.delta.value == Vector2.zero)
            sensitivity = gamepadSensitivity;
        else
            sensitivity = mouseSensitivity;

        Vector2 lookInput = playerGameInput.GetLookValue();


        /*
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        */
        float mouseX = lookInput.x * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = lookInput.y * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        if (x == 1) tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (x == -1) tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        else tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    public void LookAtTarget(Transform target)
    {
        if (target == null) return;

        // Direção do playerCam até o alvo
        Vector3 dir = (target.position - playerCam.transform.position).normalized;

        // Rotação desejada (só olhando para frente do alvo)
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Quebrar em euler para manter o mesmo sistema do seu script
        Vector3 euler = lookRot.eulerAngles;

        // Atualizar variáveis internas
        desiredX = euler.y;
        xRotation = euler.x;

        // Aplicar rotação imediatamente
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.linearVelocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.linearVelocity.x, 2) + Mathf.Pow(rb.linearVelocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.linearVelocity.y;
            Vector3 n = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.linearVelocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;


    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void OnCollisionEnter(Collision other)
    {



    }

    private void StopGrounded()
    {
        grounded = false;
    }

}
