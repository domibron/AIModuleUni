using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base state class to implement and use in finite state machine.
/// </summary>
public abstract class StateBase
{
    /// <summary>
    /// The owning finite state machine.
    /// </summary>
    protected FiniteStateMachine _owningFSM;

    /// <summary>
    /// Constructor for the state so we can pass in the owner.
    /// </summary>
    /// <param name="fsm">The owning finite state machine.</param>
    public StateBase(FiniteStateMachine fsm)
    {
        _owningFSM = fsm;
    }

    /// <summary>
    /// Runs once when transitioning to this state.
    /// </summary>
    /// <param name="entity">The owning AI entity.</param>
    abstract public void Enter(AI entity);

    /// <summary>
    /// Constant ticking function, needs to be in update or fixed update to run properly.
    /// </summary>
    /// <param name="entity">The owning AI entity.</param>
    abstract public void Execute(AI entity);

    /// <summary>
    /// Runs once when transitioning out of this state.
    /// </summary>
    /// <param name="entity">The owning AI entity.</param>
    abstract public void Exit(AI entity);
}
