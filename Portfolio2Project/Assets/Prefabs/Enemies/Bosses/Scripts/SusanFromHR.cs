using System.Collections;
using System.Collections.Generic;
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

    [Header("----- Phase One Stuff -----")]
    [SerializeField] GameObject[] phaseOneSpawners;
    [SerializeField] GameObject[] phaseOneCatchers;
    [SerializeField] GameObject[] phaseOneThingToSpawn;
    [SerializeField] GameObject phaseOneBigThingToSpawn;
    private int phaseOneCurrentSpawner;
    private bool phaseOneIsFiring;

    [Header("----- Phase Two Stuff -----")]
    [SerializeField] GameObject[] phaseTwoSpawners;
    [SerializeField] GameObject[] phaseTwoCatchers;
    [SerializeField] GameObject[] phaseTwoThingToSpawn;
    private int phaseTwoCurrentSpawner;
    private bool phaseTwoIsFiring;

    [Header("----- Phase Three Stuff -----")]
    [SerializeField] GameObject[] PhaseThreeProps;
    [SerializeField] GameObject[] PhaseThreePropsFinalPositions;

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

        bossIsInvulnerable = false;

        doPhaseOne = true;
    }

    private void PhaseOneStuff()
    {

    }

    private void TransitionOneSetup()
    {
        doTransitionOneSetup = false;

        bossIsInvulnerable = true;
        SetCubeBoolsToFalse();
        MakeTransitionCubesActive();
        MakeShieldActive();

        doTransitionOne = true;
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
            StartCoroutine(TransitionSpawnEnemies());
        }
    }

    private void PhaseTwoSetup()
    {
        doPhaseTwoSetUp = false;

        bossIsInvulnerable = false;

        doPhaseTwo = true;
    }

    private void PhaseTwoStuff()
    {

    }

    private void TransitionTwoSetup()
    {
        doTransitionTwoSetup = false;

        bossIsInvulnerable = true;

        doTransitionTwo = true;
    }

    private void TransitionTwoStuff() 
    {
    
    }

    private void PhaseThreeSetup()
    {
        doPhaseThreeSetUp = false;

        bossIsInvulnerable = false;
        ResetPhaseOneStuff();
        ResetPhaseTwoStuff();

        doPhaseThree = true;
    }

    private void PhaseThreeStuff()
    {

    }

    private void BossIsDeadStuff()
    {
        doBossDeadStuff = false;



        bossIsInvulnerable = true;
    }

    private void SetCubeBoolsToFalse()
    {
        transitionCubeOneBroken = false;
        transitionCubeTwoBroken = false;
        transitionCubeThreeBroken = false;
        transitionCubeFourBroken = false;
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

    private void ResetPhaseOneStuff()
    {
        phaseOneCurrentSpawner = 0;
    }

    private void ResetPhaseTwoStuff()
    {
        phaseTwoCurrentSpawner = 0;
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

    public void MakeShieldActive()
    {
        transitionShieldOne.SetActive(true);
        transitionShieldTwo.SetActive(true);
        transitionShieldThree.SetActive(true);
    }

    IEnumerator TransitionSpawnEnemies()
    {
        transitionIsSpawning = true;



        yield return new WaitForSeconds(timeBetweenTransitionSpawns);

        transitionIsSpawning = false;
    }
}
