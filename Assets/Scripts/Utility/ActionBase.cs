using System;
using System.Collections.Generic;
using UnityEngine;

// Base action, stores information about the action and a virtual execute method
public abstract class Action
{
    // The AI agent
    protected AI _agent;
    // The goals satisfied by this action
    protected Dictionary<GoalLabels, float> _goalSatisfaction;

    // Keep track of the time
    protected float _timer = 0.0f;
    // is it complete
    protected bool _complete;

    public Action(AI agent)
    {
        _goalSatisfaction = new Dictionary<GoalLabels, float>();
        _agent = agent;
        _timer = 0.0f;
        _complete = false;
    }

    // Is this action complete?
    public bool IsComplete
    {
        get { return _complete; }
        set { _complete = value; }
    }

    // Returns the amount that this action satisfies the specified goal by
    // if this goal is not in the list return 0 as it does not satisfy the goal atall
    public float EvaluateGoalSatisfaction(GoalLabels goal)
    {
        if (_goalSatisfaction.ContainsKey(goal))
        {
            return _goalSatisfaction[goal];
        }
        return 0.0f;
    }

    public void SetGoalSatisfactionValue(GoalLabels goal, float value)
    {
        if (_goalSatisfaction.ContainsKey(goal))
        {
            _goalSatisfaction[goal] = value;
        }
        else
        {
            _goalSatisfaction.Add(goal, value);
        }
    }

    // Execute this action
    public abstract void Execute(float deltaTime);

    // Get the string name of the action
    public override abstract string ToString();
}

