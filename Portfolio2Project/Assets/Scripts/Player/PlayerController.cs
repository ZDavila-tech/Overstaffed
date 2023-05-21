using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Skills skills;

    [Header("----- Player Stats -----")]
    [Range(1, 25)][SerializeField] int iHP;
    [Range(1, 20)][SerializeField] float playerSpeed;
    [Range(1, 20)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int maxJumps;
    [Range(2, 5)][SerializeField] float sprintMod;
    [SerializeField] private float damagecoolDown;
    public NewStaff.Element playerElement;

    [Header("----- Player Weapon -----")]
    public NewStaff playerWeapon; //Set somehow
    [SerializeField] int ShootRange; //the distance the player can shoot
    [SerializeField] float ShotCooldown; //the cooldown the player has between shots    
    private bool isShooting; //checks if the player is currently shooting

    private int jumpsUsed;
    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isSprinting;
    private int iHPOriginal;
    private bool damagedRecently;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

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
        if (groundedPlayer && playerVelocity.y < 0)
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
        controller.Move(playerVelocity * Time.deltaTime);
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
            GameManager.instance.UpdateHealthBar();
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
            }

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, ShootRange))
            {
                //Debug.Log("Shot");
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.TakeDamage(1);
                }
            }
        }

        yield return new WaitForSeconds(ShotCooldown);

        isShooting = false;
    }

    public int GetHealth()
    {
        return iHP;
    }

    public int GetOriginalHealth()
    {
        return iHPOriginal;
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
}