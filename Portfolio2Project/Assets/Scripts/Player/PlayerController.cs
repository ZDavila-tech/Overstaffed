using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Skills skills;
    [SerializeField] public GameObject screenShake;
    UIManager uiManager;
    AudioManager audioManager;

    [Header("----- Player Stats -----")]
    [SerializeField] Stats playerStats;
    int iHP;
    float playerSpeed;
    int playerDamage;
    [Range(0, 100)][SerializeField] float utCharge;
    [Range(1, 20)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int maxJumps;
    [Range(2, 5)][SerializeField] float sprintMod;
    [SerializeField] private float damagecoolDown;
    [SerializeField] float pushBackResolve;
    public NewStaff.Element playerElement;
    [SerializeField] float coyoteTime;
    private float timeSinceLastGroundTouch = Mathf.Infinity;

    [Header("----- Player Weapon -----")]
    public NewStaff playerWeapon; //set in awake
    [SerializeField] int ShootRange; //the distance the player can shoot
    [SerializeField] float ShotCooldown; //the cooldown the player has between shots    
    private bool isShooting; //checks if the player is currently shooting

    private int jumpsUsed;
    public Vector3 move;
    private Vector3 playerVelocity;
    public Vector3 pushBack;
    private bool groundedPlayer;
    private bool isSprinting;
    private bool damagedRecently;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] float AttackVolume;
    [SerializeField] float JumpVolume;
    [SerializeField] AudioClip jumpAudio;
    bool ShootSoundInPlay; //checks for the audio cooldown between shots

    float origGrav;
    float origSpeed;

    private void Awake()
    {

        playerWeapon = GetComponentInChildren<NewStaff>();
    }

    void Start()
    {
        origGrav = gravityValue;
        origSpeed = playerSpeed;
        gameManager.instance.SetPlayerVariables(this.gameObject);
        UpdateSpeed();
        iHP = playerStats.GetHealth();
        uiManager = UIManager.instance;
        audioManager = AudioManager.instance;
    }

    void Update()
    {
        if (UIManager.instance != null && uiManager == null)
        {
            uiManager = UIManager.instance;
        }
        if (skills.canMove())
        {
            Movement();
        }
        if (audioManager != null)
        {
            AttackVolume = AudioManager.instance.soundEffectsVolume.value;
            JumpVolume = AudioManager.instance.soundEffectsVolume.value;
        }
        Sprint();

        if (Input.GetButton("Shoot") && !isShooting && !gameManager.instance.isPaused)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetAxis("Movement1") != 0)
        {
            skills.useSkill(1);
        }
        if (Input.GetAxis("Movement2") != 0)
        {
            skills.useSkill(2);
        }
        if (Input.GetAxis("Movement3") != 0)
        {
            skills.useSkill(3);
        }
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            timeSinceLastGroundTouch = 0;
            playerVelocity.y = 0f;
            jumpsUsed = 0;
        }
        else if(timeSinceLastGroundTouch < coyoteTime)
        {
            //Starting the timer
            timeSinceLastGroundTouch += Time.deltaTime;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(playerSpeed * Time.deltaTime * move);

        bool canJump = false;

        if (timeSinceLastGroundTouch < coyoteTime)
        {
            canJump = true;
        }

            // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpsUsed < maxJumps && canJump)
        {
            jumpsUsed++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(jumpAudio, JumpVolume);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Wall Running
        if (hit.gameObject.tag == "Wall")
        {
            gravityValue = 2f;
            //playerSpeed += 5f;
        }
        else if (hit.gameObject.tag != "Wall")
        {
            gravityValue = origGrav;
            //playerSpeed = origSpeed;
        }
    }

    void Sprint()
    {
        if (!isSprinting && Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (isSprinting && Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }
    public void TakeDamage(int amount)
    {
        if (damagedRecently == false)
        {
            StartCoroutine(ResetDamagedRecently());

            //Debug.Log("my damage" + amount);        
            iHP -= amount; //-= used, negative amounts heal. 
            if (amount > 0)
            {
                uiManager.ShowDamage();
                if (iHP <= 0)
                {
                    iHP = 0;
                    uiManager.YouLose();
                }
            }
            else
            {
                if (iHP > playerStats.GetHealth())
                {
                    iHP = playerStats.GetHealth();
                }
            }
            UpdateHealthBar();
        }
    }

    IEnumerator ResetDamagedRecently()
    {
        damagedRecently = true;
        yield return new WaitForSeconds(damagecoolDown);
        damagedRecently = false;
    }

    IEnumerator Shoot()
    {
        if (!isShooting)
        {
            isShooting = true;

            if (playerWeapon != null)
            {
                playerWeapon.Shoot();
                StartCoroutine(PlayShootSound());
                //StartCoroutine(TriggerScreenShake(0.1f));
            }

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, ShootRange))
            {
                //Debug.Log("Shot");

                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    IDamage damageable = hit.collider.GetComponent<IDamage>();
                    damageable.TakeDamage(1);
                }
            }
        }

        yield return new WaitForSeconds(ShotCooldown);

        isShooting = false;
    }

    public void UpdateHealthBar()
    {
        uiManager.playerHealthBar.fillAmount = (float)iHP / playerStats.GetHealth();
    }


    public void ChangeJumpsUsed(int ammount)
    {
        jumpsUsed += ammount;
    }
    public float ChangeGravity(float ammount) //Changes Gravity and Returns Original Gravity
    {
        float gravityOrig = gravityValue;
        gravityValue = ammount;
        return gravityOrig;
    }

    public void PlayExternalAudio(AudioClip clip)
    {
        aud.PlayOneShot(clip, AttackVolume);
    }

    public void PlayExternalAudio(AudioClip clip, float volume)
    {
        aud.PlayOneShot(clip, volume);
    }

    IEnumerator PlayShootSound()
    {
        if (!ShootSoundInPlay)
        {
            aud.PlayOneShot(playerWeapon.GetShootAudio(), AttackVolume * 0.17f);
            ShootSoundInPlay = true;
        }
        yield return new WaitForSeconds(ShotCooldown);

        ShootSoundInPlay = false;
    }

    public void ChargeUt(float amount)
    {
        if (utCharge + amount < 100)
        {
            utCharge += amount;
            if (uiManager != null)
            {
                uiManager.UpdateUtCharge(utCharge / 100);
            }
        }
        else
        {
            utCharge = 100;
            if (uiManager != null)
            {
                uiManager.UpdateUtCharge(100);
            }
        }
    }

    public bool CanUltimate()
    {
        return utCharge >= 100;
    }

    public void Knockback(Vector3 dir)
    {
        pushBack += dir;
        //Debug.Log("Knocked Back");
    }

    public void UpdateSpeed()
    {
        playerSpeed = 5 + (playerStats.GetSpeed()/10);
    }

    public IEnumerator TriggerScreenShake(float duration)
    {
        screenShake.GetComponent<ScreenShake>().duration = duration;

        screenShake.GetComponent<ScreenShake>().start = true;
        yield return new WaitForSeconds(0.5f);
    }
}