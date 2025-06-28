using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private AI _aiOwner;

    // states
    public Wonder WonderState = new Wonder();

    public GoToEnemyBase GoToEnemyBase = new GoToEnemyBase();
    public GoToBase GoToBase = new GoToBase();
    public Flee Flee = new Flee();

    public AttackEnemy AttackEnemy = new AttackEnemy();
    public AttackFlagCarrier AttackFlagCarrier = new AttackFlagCarrier();

    public PickUpItem PickUpItem = new PickUpItem();
    public StealEnemyFlag StealEnemyFlag = new StealEnemyFlag();
    public Heal Heal = new Heal();

    public ReturnFriendlyFlag ReturnFriendlyFlag = new ReturnFriendlyFlag();
    public DefendBase DefendBase = new DefendBase();


    // current state
    private StateBase _currentState = null;

    public FiniteStateMachine(AI aIOwner)
    {
        _aiOwner = aIOwner;
        _currentState = WonderState; // set default state.
    }

    public void Update()
    {
        if (_currentState != null) _currentState.Execute(_aiOwner);
    }

    public void ChangeState(StateBase newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit(_aiOwner);
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.Enter(_aiOwner);
        }
    }
}
