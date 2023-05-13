using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES
    public bool showGizmos = false;

    [Header("Player movement")]
    public float movementSpeed;
    public float rotationSpeed;
    public float acceleration;
    private float horizontal = 0f;
    private float vertical = 0f;
    private Vector3 direction;
    private Vector3 desiredVelocity;

    [Header("Collision Predetection")]
    public LayerMask checkLayer;
    public Transform checkPoint;
    public float checkSize = 0.3f;
    [Range(0,3)]public float checkDistance = 2;

    [Header("Physics")]
    public Rigidbody rb;
    public LayerMask groundLayer;
    public Transform groundCheck;
    [SerializeField]private bool grounded = false;
    [SerializeField] private bool wallFront = false;
    public Vector3 checkGroundSize;
    //Solo vamos a comprobar si es > 0, no necesitamos más capacidad. este buffer es para ver que collider está en el area
    private Collider[] collideBuffer = new Collider[1];
    public float jumpForce = 10;
    public float dashForce = 10;

    [Header("Animator")]
    public Animator animator;
    public Animator animatorL;
    public Animator animatorR;

    [Header ("FX")]
    public AudioSource audioSource;
    public float enginePitch = 0.4f;
    public float tankMaxPitch = 3;
    public float tankMinPitch = 0.4f;
    public ParticleSystem [] dust;

    [Header("Aiming")]
    public float camRayLength = 100; //distancia máxima para el apuntado
    public LayerMask pointerlayer;
    public Transform aimingPivot;
    public Camera cameraMain;

    [Header("Shooting")]
    public PlayerShooting playerShooting;

    #endregion
    #region EVENTS
    void Start()
    {
        cameraMain = Camera.main;
        if (!playerShooting)
        {
            playerShooting = GetComponent<PlayerShooting>();
        }
        playerShooting.animator = animator;
    }

    void Update()
    {
        Controls();
        Movement();
        AimingBehaviour();
        Groundcheck();
        AnimationFeed();
        MovementFX();
        CollisionPredetection();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, checkGroundSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(checkPoint.position, checkSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyable"))
        {
            other.GetComponent<Destroyable>().Destroy();
        }
    }
    #endregion
    #region METHODS
    /// <summary>
    /// 
    //Recupera la información de los componentes
    /// </summary>
    [ContextMenu("Initialize components")]
    public void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// recupera la información delos inputs
    /// </summary>
    private void Controls()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Dash"))
        {
            Dash();
        }
        if (Input.GetButton("Fire1"))
        {
            playerShooting?.Shoot();
        }
        if (Input.GetButton("Fire2"))
        {
            playerShooting?.ShootSpecial();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerShooting?.ChangeBulletType();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            playerShooting?.ChangeBulletType(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            playerShooting?.ChangeBulletType(-1);
        }
    }

    /// <summary>
    /// Verifica si estamos en contacto con el suelo
    /// </summary>
    private void Groundcheck()
    {
        collideBuffer[0] = null;
        //comprobando si hay contacto con el suelo, se usa un nonAlloc para no consumir ás recursos de lo necesario
        //Ya que la coprobación se va a hacer de forma continua en el update
        Physics.OverlapBoxNonAlloc(groundCheck.position, checkGroundSize/2, collideBuffer, transform.rotation, groundLayer);
        //Si no es nulo entonces estamos tocado el suelo :D
        grounded = collideBuffer[0] != null;
    }

    /// <summary>
    /// detecta si hay un collider del layer indicado en contacto con el check de desplazamiento
    /// </summary>
    /// <returns></returns>
    private void CollisionPredetection()
    {
        collideBuffer[0] = null;
        Physics.OverlapSphereNonAlloc(checkPoint.position, checkSize, collideBuffer, checkLayer);
        wallFront = collideBuffer[0] != null;
    }

    /// <summary>
    /// Alimenta los params del animator
    /// </summary>
    private void AnimationFeed ()
    {
        animator.SetBool("Is Grounded", grounded);
        animatorL.SetBool("Is Grounded", grounded);
        animatorR.SetBool("Is Grounded", grounded);
        animator.SetFloat("Forward Speed", rb.velocity.magnitude);
        animatorL.SetFloat("Forward Speed", rb.velocity.magnitude);
        animatorR.SetFloat("Forward Speed", rb.velocity.magnitude);

        animator.SetFloat("Turn", rb.angularVelocity.magnitude);
        animatorR.SetFloat("Turn", rb.angularVelocity.magnitude);
        animatorL.SetFloat("Turn", rb.angularVelocity.magnitude);

        animator.SetFloat("Velocity V", rb.velocity.y);
        animatorL.SetFloat("Velocity V", rb.velocity.y);
        animatorR.SetFloat("Velocity V", rb.velocity.y);

    }

    /// <summary>
    /// encargado de realizar el movimiento
    /// </summary>
    private void Movement()
    {
        direction.Set(horizontal, 0, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);
        desiredVelocity = direction * movementSpeed;

        Vector3 temp = transform.position + direction * checkDistance;
        temp.y = checkPoint.position.y;
        checkPoint.position = temp;
        if (wallFront)
        {
            desiredVelocity = Vector3.zero;
        }

        if( (horizontal != 0 || vertical != 0)  && grounded && !wallFront)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, Time.deltaTime * acceleration);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(desiredVelocity),
                Time.deltaTime * rotationSpeed);
        }
        //Para evitar rotaciones ni deseadas
        if ((horizontal == 0 && vertical == 0) || wallFront)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Aplica una fuerza en la direccion de movimiento
    /// </summary>
    private void Dash()
    {
        if (grounded)
        {
            rb.AddForce( transform.forward * dashForce, ForceMode.Impulse );
        }
    }

    /// <summary>
    /// Reproduce efectos de sonido y efectos visuales
    /// </summary>
    private void MovementFX()
    {
        audioSource.pitch = Mathf.Clamp(enginePitch + rb.velocity.magnitude, tankMinPitch, tankMaxPitch);
        if (grounded)
        {
            foreach(ParticleSystem ps in dust)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
        }
        else
        {
            foreach (ParticleSystem ps in dust)
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                }
            }
        }
    }

    /// <summary>
    /// Metodo de apuntado de la torreta mediante raycast
    /// </summary>
    private void AimingBehaviour()
    {
        Ray camRay = cameraMain.ScreenPointToRay(Input.mousePosition);//tira un rayo de la pantalla hacia la posición equivalente en el mundo
        RaycastHit groundhit = new RaycastHit();//Para almacenar el resultado del raycast
        //Lanzo raycast 
        if (Physics.Raycast(camRay, out groundhit, camRayLength, pointerlayer))
        {
            aimingPivot.transform.position = new Vector3(groundhit.point.x , aimingPivot.position.y, groundhit.point.z);
        }
    }

    #endregion
}
