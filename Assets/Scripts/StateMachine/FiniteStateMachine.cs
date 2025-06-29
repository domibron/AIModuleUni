using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AI state machine.
/// </summary>
public class FiniteStateMachine
{
    // owning AI.
    private AI _aiOwner;

    // states
    // All AI states

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

    /// <summary>
    /// Runs the AI state ticking. (per frame).
    /// </summary>
    public void Update()
    {
        if (_currentState != null) _currentState.Execute(_aiOwner);
    }

    /// <summary>
    /// Changes the current AI state to the new one.
    /// </summary>
    /// <param name="newState">The new state to switch to.</param>
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

    /// <summary>
    /// Returns the current state the AI is running.
    /// </summary>
    /// <returns>String name of the AI state class.</returns>
    public override string ToString()
    {
        return $"{_currentState.ToString()}";
    }
}
