using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected FiniteStateMachine _owningFSM;

    public StateBase(FiniteStateMachine fsm)
    {
        _owningFSM = fsm;
    }

    abstract public void Enter(AI entity);
    abstract public void Execute(AI entity);
    abstract public void Exit(AI entity);
}
