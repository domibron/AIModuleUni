using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private AI _aiOwner;

    // states

    // current state
    private StateBase _currentState = null;

    public FiniteStateMachine(AI aIOwner)
    {
        _aiOwner = aIOwner;
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
