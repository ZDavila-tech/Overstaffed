using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Skills skills;
    [SerializeField] public GameObject screenShake;
    [SerializeField] public ProceduralRecoil proceduralRecoil;
    [SerializeField] public WeaponSway weaponSway;
    UIManager uiManager;
    AudioManager audioManager;

    [Header("----- Player Stats -----")]
    [SerializeField] int baseAttack;
    [SerializeField] int baseHealth;
    [SerializeField] float baseSpeed;
    public int iHP;
    public int totalHP;
    public float playerSpeed;
    public int playerDamage;
    [Range(0, 100)][SerializeField] float utCharge;
    [Range(1, 20)][SerializeField] float jumpHeight;
    [Range(0, 3)][SerializeField] float shootScreenshakeIntensity;
    [Range(0, 3)][SerializeField] float damagedScreenshakeIntensity;

    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 50)][SerializeField] float wallrunGravity;
    [Range(1, 3)][SerializeField] int maxJumps;
    [Range(2, 5)][SerializeField] float sprintMod;
    [SerializeField] private float damagecoolDown;
    [SerializeField] float pushBackResolve;
    public NewStaff.Element playerElement;
    [SerializeField] float coyoteTime;
    private float timeSinceLastGroundTouch = Mathf.Infinity;
    [Header("----- Sliding Stats -----")]
    [SerializeField] float crouchHeight;
    public float standingHeight;
    [SerializeField] float slideMultiplier; // How much speed is increased while sliding
    [SerializeField] float slideLimit; //How long you can slide
    public float currHeight;

    public float height
    {
        get => controller.height;
        set => controller.height = value;
    }

    [Header("----- Player Weapon -----")]
    public NewStaff playerWeapon; //set in awake
    [SerializeField] int ShootRange; //the distance the player can shoot
    [SerializeField] float ShotCooldown; //the cooldown the player has between shots    
    public bool isShooting; //checks if the player is currently shooting
    public bool canShoot;

    private int jumpsUsed;
    public Vector3 move;
    private Vector3 playerVelocity;
    public Vector3 pushBack;
    public bool groundedPlayer;
    public bool isSprinting;
    private bool damagedRecently;
    public bool isCrouching;
    public bool isStepping;
    public bool enableCamShake;

    [Header("----- Items -----")]
    //public List<GameObject> items = new List<GameObject>();
    public int itemSelected;
    public int potionsAvailable;
    

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    bool ShootSoundInPlay; //checks for the audio cooldown between shots

    [Header("----- Other -----")]
    public bool godMode;
    Color orig = Color.red;
    float origGrav;
    float origSpeed;


    [SerializeField] CinemachineCamshake camShake;
    private void Awake()
    {
        playerWeapon = GetComponentInChildren<NewStaff>();
    }

    void Start()
    {
        UpdatePlayerStats();
        //enableCamShake = true;
        potionsAvailable = 3;
        godMode = false;
        standingHeight = height;
        origGrav = gravityValue;
        gameManager.instance.SetPlayerVariables(this.gameObject);
        uiManager = UIManager.instance;
        audioManager = AudioManager.instance;
        UpdateHealthBar();
    }

    public void UpdatePlayerStats()
    {
        UpdateSpeed();
        playerDamage = Stats.Attack + baseAttack;
        UpdateHP(true);

    }

    void Update()
    {
        currHeight = height;
        if (UIManager.instance != null && uiManager == null)
        {
            uiManager = UIManager.instance;
        }
        if (skills.canMove())
        {
            Movement();
        }
        Sprint();
        Crouching();

        if (Input.GetButton("Shoot") && !isShooting && !gameManager.instance.isPaused)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetAxis("Movement1") != 0 && LevelManager.instance.currentLevel >= 2)
        {
            skills.useSkill(1);
        }
        if (Input.GetAxis("Movement2") != 0 && LevelManager.instance.currentLevel >= 3)
        {
            skills.useSkill(2);
        }
        if (Input.GetAxis("Movement3") != 0 && LevelManager.instance.currentLevel >= 4)
        {
            skills.useSkill(3);
        }
        HealingPotion();
    }
    public bool CheckGround()
    {
        float _distanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, _distanceToTheGround + 0.1f);
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            if (!isStepping && move.normalized.magnitude > 0.5f)
            {
                StartCoroutine(playSteps());
            }
            timeSinceLastGroundTouch = 0;
            playerVelocity.y = 0f;
            jumpsUsed = 0;
        }
        else if (timeSinceLastGroundTouch < coyoteTime)
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
            audioManager.JumpSound();
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);

        weaponSway.currentSpeed = AllowWeaponSway() ? playerSpeed : 0f;
    }

    private bool AllowWeaponSway()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            return true;
        }

        return false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Wall Running
        if (hit.gameObject.tag == "Wall" && !groundedPlayer)
        {
            //gravityValue = wallrunGravity;
            //playerSpeed = Stats.wallrunSpeed;
        }
        else if (hit.gameObject.tag != "Wall")
        {
            //gravityValue = origGrav;
            //if (!isSprinting && !isCrouching)
            //{
            //    playerSpeed = origSpeed;
            //}
        }
    }
    void Crouching()
    {
        if (Input.GetButtonDown("Crouch") && groundedPlayer && isSprinting)
        {
            isCrouching = true;
            controller.height = crouchHeight;
            StartCoroutine(Slide());
        }
        else if (Input.GetButtonDown("Crouch") && groundedPlayer)
        {
            isCrouching = true;
            controller.height = crouchHeight;
            playerSpeed *= 0.6f;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            controller.height = standingHeight;
            playerSpeed = origSpeed;
        }
    }
    IEnumerator Slide()
    {
        controller.Move(move.normalized * playerSpeed * 1.3f * Time.deltaTime);
        yield return new WaitForSeconds(slideLimit);
        playerSpeed = origSpeed;
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
        if (godMode)
        {
            Debug.Log("Returned");
            return;
        }


        if (damagedRecently == false)
        {
            StartCoroutine(ResetDamagedRecently());
            iHP -= amount;
            if (amount > 0)
            {
                audioManager.PlayerHurt();
                uiManager.ShowDamage();
                camShake.Shake(damagedScreenshakeIntensity, .1f);
                if (iHP <= 0)
                {
                    iHP = 0;
                    StopAllCoroutines();
                    TurnOfStatusEffects();
                    uiManager.YouLose();
                }
            }
            else
            {
                audioManager.HealAud();
                if (iHP > totalHP)
                {
                    iHP = totalHP;
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

    public IEnumerator playSteps()
    {
        isStepping = true;
        audioManager.WalkingSound();
        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        isStepping = false;
    }
     IEnumerator Shoot()
    {
        if (!isShooting)
        {
            isShooting = true;

            if (playerWeapon != null)
            {
                playerWeapon.Shoot();
               

                if (enableCamShake)
                {
                    camShake.Shake(shootScreenshakeIntensity, 0.1f);
                }
                    proceduralRecoil.Recoil();
            }
            
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, ShootRange))
            {
                if (hit.collider.GetComponent<IDamage>() != null && !hit.transform.CompareTag("Player"))
                {
                    IDamage damageable = hit.collider.GetComponent<IDamage>();
                    damageable.TakeDamage(playerDamage);
                }
            }
        }

        yield return new WaitForSeconds(ShotCooldown);

        isShooting = false;
    }

    public void UpdateHealthBar()
    {
        uiManager.playerHealthBar.fillAmount = (float)iHP / totalHP;
     
        uiManager.hpText.text = iHP.ToString() + "/" + totalHP.ToString();
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


   public IEnumerator PlayShootSound()
    {
        if (!ShootSoundInPlay)
        {
            audioManager.ShootSound();
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
    }

    public void UpdateSpeed()
    {
        origSpeed = playerSpeed = baseSpeed + (Stats.Speed / 10);
        Debug.Log("Speed Calculated");
    }

    IEnumerator Freezing(float duration)
    {
        float timePassed = 0;
        uiManager.freezeIndicator.SetActive(true);
        uiManager.playerHealthBar.color = Color.cyan;
        float origSpd = playerSpeed;
        playerSpeed = 0;
        if (playerElement == NewStaff.Element.Water)
        {
            while (timePassed <= duration)
            {
                timePassed += Time.deltaTime;
                yield return new WaitForSeconds(1);
                TakeDamage(-1);
            }
        }
        yield return new WaitForSeconds(duration);
        playerSpeed = origSpeed;
        uiManager.freezeIndicator.SetActive(false);
        uiManager.playerHealthBar.color = orig;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(Freezing(duration));
    }

    IEnumerator Burning(float duration, float timeBetween, int damage)
    {
        float timePassed = 0;
        uiManager.playerHealthBar.color = Color.yellow;
        uiManager.burnIndicator.SetActive(true);
        float oldSpeed = 0;
        if (playerElement == NewStaff.Element.Fire)
        {
            oldSpeed = playerSpeed;
            playerSpeed *= 1.5f;
        }
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(timeBetween);
            TakeDamage(damage);
        }
        yield return new WaitForSeconds(duration);
        if (playerElement == NewStaff.Element.Fire)
        {
            playerSpeed = oldSpeed;
        }
        uiManager.burnIndicator.SetActive(false);
        uiManager.playerHealthBar.color = orig;
    }

    IEnumerator Venom(float duration, float timeBetween, int damage)
    {
        float timePassed = 0;
        uiManager.playerHealthBar.color = Color.magenta;
        uiManager.poisonIndicator.SetActive(true);
        int oldAttack = 0;
        if (playerElement == NewStaff.Element.Earth)
        {
            oldAttack = playerDamage;
            playerDamage = (int)(playerDamage * 1.5f);
        }
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(timeBetween);
            TakeDamage(damage);
        }
        if (playerElement == NewStaff.Element.Earth)
        {
            playerDamage = oldAttack;
        }
        uiManager.poisonIndicator.SetActive(false);
        uiManager.playerHealthBar.color = orig;
    }

    public void Burn(float duration, float timeBetween)
    {
        StartCoroutine(Burning(duration, timeBetween, gameManager.instance.playerStats.GetHealth() * (1)));
    }

    public void Poison(float duration, float timeBetweeen)
    {
        StartCoroutine(Venom(duration, timeBetweeen, 1));
    }

    public void ChangeBaseStats(NewStaff.Element element)
    {
        switch (element)
        {
            case NewStaff.Element.Fire:
                baseHealth = 10;
                baseSpeed = 8.5f;
                baseAttack = 1;
                break;
            case NewStaff.Element.Water:
                baseHealth = 15;
                //UpdateHealthBar();
                baseSpeed = 8;
                baseAttack = 1;
                break;
            case NewStaff.Element.Earth:
                baseHealth = 10;
                baseSpeed = 8;
                baseAttack = 2;
                break;
        }
        UpdateHP(true);
        UpdateHealthBar();
    }

    //void ChangeItem()
    //{
    //    if (itemSelected > items.Count - 1 || itemSelected < 0)
    //    {
    //        itemSelected = 0;
    //    }
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && itemSelected < items.Count - 1)
    //    {
    //        itemSelected++;
    //        items[itemSelected].GetComponent<Item>().isSelected = true;
    //        //items[itemSelected-1].GetComponent<MeshRenderer>().enabled = false;
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && itemSelected > 0)
    //    {
    //        itemSelected--;
    //        items[itemSelected].GetComponent<Item>().isSelected = true;
    //        //items[itemSelected+1].GetComponent<MeshRenderer>().enabled = false;
    //    }
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        if (items[i] != items[itemSelected])
    //        {
    //            items[i].GetComponent<Item>().isSelected = false;
    //        }

    //    }
    //}

    public void HealingPotion()
    {
        if(iHP >= totalHP || uiManager.activeMenu != null) //Checks if the player is at full health and no menus are open
        {
            return;
        }
        else
        {
            if (potionsAvailable > 0 && Input.GetKeyDown(KeyCode.Tab))
            {
                TakeDamage(-5);
                potionsAvailable--;
            }
        }

        
    }

    public void UpdateHP(bool DoesHeal)
    {
        totalHP = baseHealth + (gameManager.instance.playerStats.GetHealth());
        if (DoesHeal)
            iHP = totalHP;
    }

       public void TurnOfStatusEffects()
    {
        uiManager.poisonIndicator.SetActive(false);
        uiManager.burnIndicator.SetActive(false);
        uiManager.freezeIndicator.SetActive(false);

        uiManager.playerHealthBar.color = orig;
    }
}