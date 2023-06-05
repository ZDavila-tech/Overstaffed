using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SusanFromHR : MonoBehaviour
{
    private UIManager uiManager;
    [Header("----- Art -----")]
    [SerializeField] Renderer model;
    private Color originalColor;

    [Header("----- Health Bars -----")]
    public int healthBarOne;
    public int healthBarTwo;
    public int healthBarThree;
    public bool bossIsInvulnerable;
    public GameObject hitBoxCenter;

    [Header("----- Phase One Stuff -----")]
    [SerializeField] GameObject[] phaseOneSpawners;
    [SerializeField] GameObject[] phaseOneCatchers;
    [SerializeField] GameObject[] phaseOneThingToSpawn;
    [SerializeField] GameObject phaseOneBigThingToSpawn;
    [SerializeField] float phaseOneTimeBetweenFirings;
    private int phaseOneCountToFour;
    private int phaseOneCurrentSpawner;
    private bool phaseOneIsFiring;

    [Header("----- Phase Two Stuff -----")]
    [SerializeField] GameObject[] phaseTwoSpawners;
    [SerializeField] GameObject[] phaseTwoCatchers;
    [SerializeField] GameObject[] phaseTwoThingToSpawn;
    [SerializeField] float phaseTwoTimeBetweenFirings;
    private int phaseTwoCurrentSpawner;
    private bool phaseTwoIsFiring;

    [Header("----- Phase Three Stuff -----")]
    [SerializeField] GameObject[] phaseThreeProps;
    [SerializeField] GameObject[] phaseThreePropsFinalPositions;

    [Header("----- Transition Stuff -----")]
    [SerializeField] GameObject transitionCubeOne;
    [SerializeField] GameObject transitionCubeTwo;
    [SerializeField] GameObject transitionCubeThree;
    [SerializeField] GameObject transitionCubeFour;

    [SerializeField] GameObject transitionShieldOne;
    [SerializeField] GameObject transitionShieldTwo;
    [SerializeField] GameObject transitionShieldThree;

    [SerializeField] float timeBetweenTransitionSpawns;
    [SerializeField] GameObject transitionEnemy;

    [SerializeField] Transform[] transitionSpawnLocations;

    private bool transitionIsSpawning;

    [Header("----- Handled by the Game (Ignore) -----")]
    public bool transitionCubeOneBroken;
    public bool transitionCubeTwoBroken;
    public bool transitionCubeThreeBroken;
    public bool transitionCubeFourBroken;

    private bool doPhaseOneSetUp;
    private bool doPhaseOne;
    private bool doTransitionOneSetup;
    private bool doTransitionOne;
    private bool doPhaseTwoSetUp;
    private bool doPhaseThreeSetUp;
    private bool doPhaseTwo;
    private bool doTransitionTwoSetup;
    private bool doTransitionTwo;
    private bool doPhaseThree;
    private bool doBossDeadStuff;

    private void Awake()
    {
        originalColor = model.material.color;
        ResetPhaseOneStuff();
        ResetPhaseTwoStuff();
        SetCubeBoolsToFalse();
        SetPhaseBoolsToFalse();
    }

    private void SetPhaseBoolsToFalse()
    {
        doPhaseOneSetUp = false;
        doPhaseOne = false;
        doTransitionOneSetup = false;
        doTransitionOne = false;
        doPhaseTwoSetUp = false;
        doPhaseThreeSetUp = false;
        doPhaseTwo = false;
        doTransitionTwoSetup = false;
        doTransitionTwo = false;
        doPhaseThree = false;
        doBossDeadStuff = false;
    }

    private void Start()
    {
        uiManager = UIManager.instance;
    }

    private void Update()
    {
        BossFightFlow();
    }

    private void BossFightFlow()
    {
        if (doPhaseOneSetUp)
        {
            PhaseOneSetup();            
        }

        if (doPhaseOne)
        {
            if (healthBarOne <= 0)
            {
                doPhaseOne = false;
                healthBarOne = 0;
                doTransitionOneSetup = true;
            }
            else
            {
                PhaseOneStuff();
            }
        }

        if(doTransitionOneSetup)
        {
            TransitionOneSetup();            
        }

        if (doTransitionOne)
        {
            if (transitionCubeOneBroken && transitionCubeTwoBroken && transitionCubeThreeBroken && transitionCubeFourBroken)
            {
                doTransitionOne = false;
                SetCubeBoolsToFalse();
                doPhaseTwoSetUp = true;
            }
            else
            {
                TransitionOneStuff();
            }
        }

        if (doPhaseTwoSetUp)
        {
            PhaseTwoSetup();
        }

        if (doPhaseTwo)
        {
            if (healthBarTwo <= 0)
            {
                doPhaseTwo = false;
                healthBarTwo = 0;
                doTransitionTwoSetup = true;
            }
            else
            {
                PhaseTwoStuff();
            }
        }

        if(doTransitionTwoSetup)
        {
            TransitionTwoSetup();
        }

        if (doTransitionTwo)
        {
            if (transitionCubeOneBroken && transitionCubeTwoBroken && transitionCubeThreeBroken && transitionCubeFourBroken)
            {
                doTransitionTwo = false;
                SetCubeBoolsToFalse();
                doPhaseThreeSetUp = true;
            }
            else
            {
                TransitionTwoStuff();
            }
        }

        if (doPhaseThreeSetUp)
        {
            PhaseThreeSetup();
        }

        if (doPhaseThree)
        {
            if (healthBarThree <= 0)
            {
                doPhaseThree = false;
                healthBarThree = 0;
                doBossDeadStuff = true;
            }
            else
            {
                PhaseThreeStuff();
            }
        }

        if (doBossDeadStuff)
        {
            BossIsDeadStuff();
        }
    }

    private void PhaseOneSetup()
    {
        doPhaseOneSetUp = false;

        GenericPhaseSetup();

        doPhaseOne = true;
    }

    private void PhaseTwoSetup()
    {
        doPhaseTwoSetUp = false;

        GenericPhaseSetup();

        doPhaseTwo = true;
    }

    private void PhaseThreeSetup()
    {
        doPhaseThreeSetUp = false;

        GenericPhaseSetup();
        ResetPhaseOneStuff();
        ResetPhaseTwoStuff();
        int maxIndexProps = phaseThreeProps.Length;
        int maxIndexTargets = phaseThreePropsFinalPositions.Length;
        for(int i = 0; i < maxIndexProps; ++i)
        {
            int targetIndex = Random.Range(0, maxIndexTargets);
            GameObject launchableProp = phaseThreeProps[i];
            LaunchableProp launchablePropScript = launchableProp.GetComponent<LaunchableProp>();
            launchablePropScript.targetToMoveTo = phaseThreePropsFinalPositions[targetIndex].transform;
            launchablePropScript.LaunchYourself();
            phaseThreePropsFinalPositions[targetIndex].GetComponent<PropCatcher>().expectedProjectile = launchableProp;
        }

        doPhaseThree = true;
    }

    public void GenericPhaseSetup()
    {
        bossIsInvulnerable = false;
        DeactivateCubes();
        DeactivateShields();
    }

    public void DeactivateCubes()
    {
        transitionCubeOne.SetActive(false);
        transitionCubeTwo.SetActive(false);
        transitionCubeThree.SetActive(false);
        transitionCubeFour.SetActive(false);

    }

    public void DeactivateShields()
    {
        transitionShieldOne.SetActive(false);
        transitionShieldTwo.SetActive(false);
        transitionShieldThree.SetActive(false);
    }

    private void ResetPhaseOneStuff()
    {
        phaseOneCurrentSpawner = 0;
    }

    private void ResetPhaseTwoStuff()
    {
        phaseTwoCurrentSpawner = 0;
    }

    private void PhaseOneStuff()
    {
        if(!phaseOneIsFiring)
        {
            StartCoroutine(PhaseOneFire());
        }
    }

    IEnumerator PhaseOneFire()
    {
        phaseOneIsFiring = true;

        int maxIndexSpawners = phaseOneSpawners.Length;
        if (phaseOneCurrentSpawner == maxIndexSpawners)
        {
            phaseOneCurrentSpawner = 0;
        }

        GameObject bossProjectile;
        if(phaseOneCountToFour == 4)
        {
            bossProjectile = Instantiate(phaseOneBigThingToSpawn, phaseOneSpawners[phaseOneCurrentSpawner].transform);
            phaseOneCountToFour = 0;
        }
        else
        {
            int maxIndexThings = phaseOneThingToSpawn.Length;
            int randomThingIndex = Random.Range(0, maxIndexThings);
            bossProjectile = Instantiate(phaseOneThingToSpawn[randomThingIndex], phaseOneSpawners[phaseOneCurrentSpawner].transform);
        }

        BossProjectile bossProjectileScript = bossProjectile.GetComponent<BossProjectile>();
        bossProjectileScript.spawnPosition = phaseOneSpawners[phaseOneCurrentSpawner].transform;
        bossProjectileScript.targetToMoveTo = phaseOneCatchers[phaseOneCurrentSpawner].transform;
        phaseOneCatchers[phaseOneCurrentSpawner].GetComponent<BossProjectileCatcher>().expectedProjectile = bossProjectile;
        ++phaseOneCountToFour;
        ++phaseOneCurrentSpawner;

        yield return new WaitForSeconds(phaseOneTimeBetweenFirings);

        phaseOneIsFiring = false;
    }

    private void PhaseTwoStuff()
    {
        if (!phaseTwoIsFiring)
        {
            StartCoroutine(PhaseTwoFire());
        }
    }

    IEnumerator PhaseTwoFire()
    {
        phaseTwoIsFiring = true;

        int maxIndexSpawners = phaseTwoSpawners.Length;
        if (phaseTwoCurrentSpawner == maxIndexSpawners)
        {
            phaseTwoCurrentSpawner = 0;
        }
        int maxIndexThings = phaseTwoThingToSpawn.Length;
        int randomThingIndex = Random.Range(0, maxIndexThings);
        GameObject bossProjectile = Instantiate(phaseTwoThingToSpawn[randomThingIndex], phaseTwoSpawners[phaseTwoCurrentSpawner].transform);
        BossProjectile bossProjectileScript = bossProjectile.GetComponent<BossProjectile>();
        bossProjectileScript.spawnPosition = phaseTwoSpawners[phaseTwoCurrentSpawner].transform;
        bossProjectileScript.targetToMoveTo = phaseTwoCatchers[phaseTwoCurrentSpawner].transform;
        phaseTwoCatchers[phaseTwoCurrentSpawner].GetComponent<BossProjectileCatcher>().expectedProjectile = bossProjectile;
        ++phaseTwoCurrentSpawner;

        yield return new WaitForSeconds(phaseTwoTimeBetweenFirings);

        phaseTwoIsFiring = false;
    }

    private void PhaseThreeStuff()
    {
        PhaseOneStuff();
        PhaseTwoStuff();
    }

    private void TransitionOneSetup()
    {
        doTransitionOneSetup = false;

        GenericTransitionSetup();

        doTransitionOne = true;
    }

    private void TransitionTwoSetup()
    {
        doTransitionTwoSetup = false;

        GenericTransitionSetup();

        doTransitionTwo = true;
    }

    public void GenericTransitionSetup()
    {
        bossIsInvulnerable = true;
        MakeShieldActive();
        SetCubeBoolsToFalse();
        MakeTransitionCubesActive();
    }

    public void MakeShieldActive()
    {
        transitionShieldOne.SetActive(true);
        transitionShieldTwo.SetActive(true);
        transitionShieldThree.SetActive(true);
    }

    private void SetCubeBoolsToFalse()
    {
        transitionCubeOneBroken = false;
        transitionCubeTwoBroken = false;
        transitionCubeThreeBroken = false;
        transitionCubeFourBroken = false;
    }

    public void MakeTransitionCubesActive()
    {
        transitionCubeOne.transform.position = new Vector3(transitionCubeOne.transform.position.x, Random.Range(3.0f, 7.0f), transitionCubeOne.transform.position.z);
        transitionCubeOne.transform.rotation = Random.rotation;
        transitionCubeTwo.transform.position = new Vector3(transitionCubeTwo.transform.position.x, Random.Range(3.0f, 7.0f), transitionCubeTwo.transform.position.z);
        transitionCubeTwo.transform.rotation = Random.rotation;
        transitionCubeThree.transform.position = new Vector3(transitionCubeThree.transform.position.x, Random.Range(3.0f, 7.0f), transitionCubeThree.transform.position.z);
        transitionCubeThree.transform.rotation = Random.rotation;
        transitionCubeFour.transform.position = new Vector3(transitionCubeFour.transform.position.x, Random.Range(3.0f, 7.0f), transitionCubeFour.transform.position.z);
        transitionCubeFour.transform.rotation = Random.rotation;
        transitionCubeOne.SetActive(true);
        transitionCubeTwo.SetActive(true);
        transitionCubeThree.SetActive(true);
        transitionCubeFour.SetActive(true);
    }

    private void TransitionOneStuff()
    {
        if(transitionCubeOneBroken)
        {
            //play sound
            transitionShieldOne.SetActive(false);
        }

        if(transitionCubeTwoBroken && transitionCubeThreeBroken)
        {
            //play sound
            transitionShieldTwo.SetActive(false);
        }

        if (transitionCubeFourBroken)
        {
            //play sound
            transitionShieldThree.SetActive(false);
        }

        if(!transitionIsSpawning)
        {
            StartCoroutine(TransitionOneSpawnEnemies());
        }
    }

    private void TransitionTwoStuff()
    {
        if (transitionCubeOneBroken)
        {
            //play sound
            transitionShieldOne.SetActive(false);
        }

        if (transitionCubeTwoBroken && transitionCubeThreeBroken)
        {
            //play sound
            transitionShieldTwo.SetActive(false);
        }

        if (transitionCubeFourBroken)
        {
            //play sound
            transitionShieldThree.SetActive(false);
        }

        if (!transitionIsSpawning)
        {
            StartCoroutine(TransitionTwoSpawnEnemies());
        }
    }

    IEnumerator TransitionOneSpawnEnemies()
    {
        transitionIsSpawning = true;

        for (int i = 0; i < 2; ++i)
        {
            Instantiate(transitionEnemy, transitionSpawnLocations[i]);
        }

        yield return new WaitForSeconds(timeBetweenTransitionSpawns);

        transitionIsSpawning = false;
    }

    IEnumerator TransitionTwoSpawnEnemies()
    {
        transitionIsSpawning = true;

        for (int i = 0; i < 4; ++i)
        {
            Instantiate(transitionEnemy, transitionSpawnLocations[i]);
        }

        yield return new WaitForSeconds(timeBetweenTransitionSpawns);

        transitionIsSpawning = false;
    }

    private void BossIsDeadStuff()
    {
        doBossDeadStuff = false;

        GameObject.Destroy(this.gameObject);

        bossIsInvulnerable = true;
    }

    public void StartFight()
    {
        doPhaseOneSetUp = true;
    }

    public int GetBossPhase()
    {
        if(doPhaseOne)
        {
            return 1;
        }
        else if(doPhaseTwo)
        {
            return 2;
        }
        else if(doPhaseThree)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }

    public IEnumerator TakeDamageColorFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }

    public IEnumerator InvulnerableColorFlash()
    {
        model.material.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }
}
