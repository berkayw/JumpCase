using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    
    [Header("Movement")] 
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float turnSpeed = 15f;
    private float moveInput;

    [Header("Jump")]
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 10f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float gravityMultiplier = 4f;

    private float currentJumpForce;
    private bool isChargingJump;
    private bool shouldJump;
    
    [SerializeField] private float coyoteTime = 0.25f;
    private float coyoteTimer;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private bool isGrounded;
    
    [Header("Animation")] 
    private Animator animator;

    [Header("Model and Particles")] 
    [SerializeField] private SkinnedMeshRenderer slimeRenderer;
    [SerializeField] private ParticleSystem particles;
    private bool isPlayingParticles;
    [SerializeField] private GameObject jumpVFXPrefab;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioSource sfxSource;
    private AudioSource audioSource;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();

        playerInput.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        playerInput.Movement.Move.canceled += ctx => moveInput = 0f;

        playerInput.Movement.Jump.started += ctx => StartCharging();
        playerInput.Movement.Jump.canceled += ctx => ReleaseJump();
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        
        //Coyote
        if (isGrounded)
            coyoteTimer = coyoteTime; 
        else
            coyoteTimer -= Time.deltaTime;
        
        //Turn 
        if (moveInput != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, moveInput > 0 ? 90f : 270f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        //Jump
        if (isChargingJump)
        {
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);

            // TODO: Charge bar UI update
        }

        //Handle Animation
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        //VFX&SFX
        PlayTrailVFXandSFX();
    }

    private void FixedUpdate()
    {
        //Move
        Vector3 movement = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = movement;

        if (rb.velocity.y < -0.1f)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        //Jump
        if (shouldJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, currentJumpForce, 0f);
            currentJumpForce = 0f;
            shouldJump = false;
        }
        
    }

    private void StartCharging()
    {
        if (coyoteTimer <= 0f) return;

        isChargingJump = true;
        currentJumpForce = minJumpForce;
        animator.SetTrigger("Charge");
    }

    private void ReleaseJump()
    {
        if (!isChargingJump || coyoteTimer <= 0f) return;

        isChargingJump = false;
        currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);
        shouldJump = true;
        
        animator.SetTrigger("Jump");
    }
    
    //Calling from animation events
    private void ChangeFaceMaterial(Material mat)
    {
        Material[] materials = slimeRenderer.materials;
        materials[1] = mat;
        slimeRenderer.materials = materials;
    }
    
    private void PlayTrailVFXandSFX()
    {
        bool shouldPlay = isGrounded && Mathf.Abs(rb.velocity.x) > 0.1f;

        if (shouldPlay && !isPlayingParticles)
        {
            audioSource.clip = walkClip;
            audioSource.Play();
            particles.Play();
            isPlayingParticles = true;
        }
        else if (!shouldPlay && isPlayingParticles)
        {
            audioSource.Pause();
            particles.Stop();
            isPlayingParticles = false;
        }
    }
    
    //calling from Landing and Jumping animation events
    private void PlayJumpVFXandSFX()
    {
        sfxSource.PlayOneShot(jumpClip);
        GameObject jumpVFX = Instantiate(jumpVFXPrefab, transform.position, Quaternion.Euler(-90,0,0));
        Destroy(jumpVFX, 2f);
    }
    
}