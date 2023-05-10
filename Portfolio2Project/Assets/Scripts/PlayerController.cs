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


    private int jumpsUsed;
    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isSprinting;
    private int iHPOriginal;

    [Header("----- Player Weapon -----")]
    //the distance the player can shoot
    [SerializeField] int ShootRange;
    //the cooldown the player has between shots
    [SerializeField] float ShotCooldown;
    //checks if the player is currently shooting
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        iHPOriginal = iHP;
        Debug.Log(iHPOriginal);
        Debug.Log(iHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (skills.canMove())
        {
            Movement();
        }


        Sprint();

        if (Input.GetButton("Shoot") && !isShooting && !gameManager.instance.isPaused)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            skills.invisible();
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
        controller.Move(move * Time.deltaTime * playerSpeed);



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
        //adds the amount to the player's hp (adds a negative if taking damage)
        if (iHP - amount <= iHPOriginal)
        {
            if(iHP - amount < iHP)
            gameManager.instance.showDamage();
            iHP -= amount;
            gameManager.instance.UpdateHealthBar();
            if (iHP <= 0)
            {
                gameManager.instance.youLose();
            }
        }
        else
        {
            iHP = iHPOriginal;
            gameManager.instance.UpdateHealthBar();
        }
    }


    IEnumerator Shoot()
    {

        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootRange))
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * ShootRange, out hit))
            {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                Debug.Log("Shot");
                damageable.TakeDamage(2);
            }
        }

        yield return new WaitForSeconds(ShotCooldown);

        isShooting = false;
    }


    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerRespawn.transform.position;
        iHP = iHPOriginal;
        gameManager.instance.ResetHpBar();
        controller.enabled = true;
    }

    public int getHealth()
    {
        return iHP;
    }

    public int getOriginalHealth()
    {
        return iHPOriginal;
    }

    public void changeJumpsUsed(int ammount)
    {
        jumpsUsed += ammount;
    }

    //Changer Gravity and Returns Original Gravity
    public float changeGravity(float ammount)
    {
        float gravityOrig = gravityValue;
        gravityValue = ammount;
        return gravityOrig;
    }

}

