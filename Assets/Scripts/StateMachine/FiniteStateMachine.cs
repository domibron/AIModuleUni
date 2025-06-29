using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private AI _aiOwner;

    // states
    // public Wonder WonderState = new Wonder();

    public GoToEnemyBase GoToEnemyBase;
    public GoToBase GoToBase;
    public Flee Flee;

    public AttackEnemy AttackEnemy;
    public AttackFlagCarrier AttackFlagCarrier;

    public PickUpItem PickUpItem;
    public StealEnemyFlag StealEnemyFlag;
    public Heal Heal;

    public ReturnFriendlyFlag ReturnFriendlyFlag;
    public DefendBase DefendBase;


    // current state
    private StateBase _currentState = null;

    public FiniteStateMachine(AI aIOwner)
    {
        _aiOwner = aIOwner;

        GoToEnemyBase = new GoToEnemyBase(this);
        GoToBase = new GoToBase(this);
        Flee = new Flee(this);

        AttackEnemy = new AttackEnemy(this);
        AttackFlagCarrier = new AttackFlagCarrier(this);

        PickUpItem = new PickUpItem(this);
        StealEnemyFlag = new StealEnemyFlag(this);
        Heal = new Heal(this);

        ReturnFriendlyFlag = new ReturnFriendlyFlag(this);
        DefendBase = new DefendBase(this);

        _currentState = GoToEnemyBase; // set default state.


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

    public override string ToString()
    {
        return $"{_currentState.ToString()}";
    }
}
