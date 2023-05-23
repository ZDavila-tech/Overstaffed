using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Skills skills;

    [Header("----- Player Stats -----")]
    [Range(1, 25)][SerializeField] int iHP;
    [Range(0, 100)][SerializeField] float utCharge;
    [Range(1, 20)][SerializeField] float playerSpeed;
    [Range(1, 20)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int maxJumps;
    [Range(2, 5)][SerializeField] float sprintMod;
    [SerializeField] private float damagecoolDown;
    [SerializeField] float pushBackResolve;
    public NewStaff.Element playerElement;

    [Header("----- Player Weapon -----")]
    public NewStaff playerWeapon; //set in awake
    [SerializeField] int ShootRange; //the distance the player can shoot
    [SerializeField] float ShotCooldown; //the cooldown the player has between shots    
    private bool isShooting; //checks if the player is currently shooting

    private int jumpsUsed;
    private Vector3 move;
    private Vector3 playerVelocity;
    public Vector3 pushBack;
    private bool groundedPlayer;
    private bool isSprinting;
    private int iHPOriginal;
    private bool damagedRecently;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] float AttackVolume;
    bool ShootSoundInPlay; //checks for the audio cooldown between shots

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerWeapon = GetComponentInChildren<NewStaff>();
    }

    void Start()
    {
        iHPOriginal = iHP;
        //Debug.Log(iHPOriginal);
        //Debug.Log(iHP);
    }

    void Update()
    {
        if (skills.canMove())
        {
            Movement();
        }

        Sprint();

        if (Input.GetButton("Shoot") && !isShooting && !GameManager.instance.isPaused)
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
            playerVelocity.y = 0f;
            jumpsUsed = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * playerSpeed * Time.deltaTime);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpsUsed < maxJumps)
        {
            jumpsUsed++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
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
                GameManager.instance.ShowDamage();
                if (iHP <= 0)
                {
                    iHP = 0;
                    GameManager.instance.YouLose();
                }
            }
            else
            {
                if (iHP > iHPOriginal)
                {
                    iHP = iHPOriginal;
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
        GameManager.instance.playerHealthBar.fillAmount = (float)iHP / iHPOriginal;
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

    IEnumerator PlayShootSound()
    {
        if (!ShootSoundInPlay)
        {
            aud.PlayOneShot(playerWeapon.GetShootAudio(), AttackVolume);
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
            GameManager.instance.UpdateUtCharge(utCharge / 100);
        }
        else
        {
            GameManager.instance.UpdateUtCharge(100);
        }
    }

    public bool canUt()
    {
        return utCharge >= 100;
    }

    public void Knockback(Vector3 dir)
    {
        pushBack += dir;
        Debug.Log("Knocked Back");
    }
}