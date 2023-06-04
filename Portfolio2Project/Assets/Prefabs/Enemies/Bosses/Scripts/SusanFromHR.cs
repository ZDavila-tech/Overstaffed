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

    [SerializeField] GameObject transitionEnemy;

    [SerializeField] GameObject transitionSpawnerOne;
    [SerializeField] GameObject transitionSpawnerTwo;
    [SerializeField] GameObject transitionSpawnerThree;
    [SerializeField] GameObject transitionSpawnerFour;

    [Header("----- Handled by the Game (Ignore) -----")]
    public bool transitionCubeOneBroken;
    public bool transitionCubeTwoBroken;
    public bool transitionCubeThreeBroken;
    public bool transitionCubeFourBroken;

    private bool doPhaseOneSetUp = true;
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
        phaseOneCurrentSpawner = 0;
        phaseTwoCurrentSpawner = 0;
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
            bossIsInvulnerable = false;
            PhaseOneSetup();
            doPhaseOneSetUp = false;
            doPhaseOne = true;
        }

        if (doPhaseOne)
        {
            if (healthBarOne <= 0)
            {
                healthBarOne = 0;
                doPhaseOne = false;
                doTransitionOneSetup = true;
            }
            else
            {
                PhaseOneStuff();
            }
        }

        if(doTransitionOneSetup)
        {
            bossIsInvulnerable = true;
            TransitionOneSetup();
            doTransitionOneSetup = false;
            doTransitionOne = true;
        }

        if (doTransitionOne)
        {
            if (transitionCubeOneBroken && transitionCubeTwoBroken && transitionCubeThreeBroken && transitionCubeFourBroken)
            {
                doTransitionOne = false;
                doPhaseTwoSetUp = true;
                SetCubeBoolsToFalse();
            }
            else
            {
                TransitionOneStuff();
            }
        }

        if (doPhaseTwoSetUp)
        {
            bossIsInvulnerable = false;
            PhaseTwoSetup();
            doPhaseTwoSetUp = false;
            doPhaseTwo = true;
        }

        if (doPhaseTwo)
        {
            if (healthBarTwo <= 0)
            {
                healthBarTwo = 0;
                doPhaseTwo = false;
                doTransitionTwoSetup = true;
            }
            else
            {
                PhaseTwoStuff();
            }
        }

        if(doTransitionTwoSetup)
        {
            bossIsInvulnerable = true;
            TransitionTwoSetup();

            doTransitionTwoSetup = false;
            doTransitionTwo = true;
        }

        if (doTransitionTwo)
        {
            if (transitionCubeOneBroken && transitionCubeTwoBroken && transitionCubeThreeBroken && transitionCubeFourBroken)
            {
                doTransitionTwo = false;
                doPhaseThreeSetUp = true;
                SetCubeBoolsToFalse();
            }
            else
            {
                TransitionTwoStuff();
            }
        }

        if (doPhaseThreeSetUp)
        {
            bossIsInvulnerable = false;
            PhaseThreeSetup();
            doPhaseThreeSetUp = false;
            doPhaseThree = true;
        }

        if (doPhaseThree)
        {
            if (healthBarThree <= 0)
            {
                healthBarThree = 0;
                doBossDeadStuff = true;
                doPhaseThree = false;
            }
            else
            {
                PhaseThreeStuff();
            }
        }

        if (doBossDeadStuff)
        {
            bossIsInvulnerable = true;
            BossIsDeadStuff();
            doBossDeadStuff = false;
        }
    }

    private void PhaseOneSetup()
    {

    }

    private void PhaseOneStuff()
    {

    }

    private void TransitionOneSetup()
    {

    }

    private void TransitionOneStuff()
    {

    }

    private void PhaseTwoSetup()
    {

    }

    private void PhaseTwoStuff()
    {

    }

    private void TransitionTwoSetup()
    {

    }

    private void TransitionTwoStuff() 
    {
    
    }

    private void PhaseThreeSetup()
    {

    }

    private void PhaseThreeStuff()
    {

    }

    private void BossIsDeadStuff()
    {

    }

    private void SetCubeBoolsToFalse()
    {
        transitionCubeOneBroken = false;
        transitionCubeTwoBroken = false;
        transitionCubeThreeBroken = false;
        transitionCubeFourBroken = false;
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
