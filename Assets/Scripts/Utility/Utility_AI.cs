using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores actions and goals
/// Finds the action with the greatest satisfaction for the goal
/// with the highest utility.
/// </summary>
public class Utility_AI
{
    // List of actions the AI can perform
    private List<Action> _actions = new List<Action>();

    // List of goals which need to be satisfied
    private List<Goal> _goals = new List<Goal>();

    // Add an action to the action list
    public void AddAction(Action actionToAdd)
    {
        _actions.Add(actionToAdd);
    }

    // Add a goal to the goal list
    public void AddGoal(Goal goal)
    {
        _goals.Add(goal);
    }

    public Goal GetGoalFromType(GoalLabels goalType)
    {
        foreach (Goal goal in _goals)
        {
            //Debug.Log("before update: goal " + goal.Type.ToString() + " value = " + goal.Value.ToString("F4"));
            if (goal.Type == goalType)
            {
                return goal;
            }
            Debug.Log("Updated goal: " + goal.Type.ToString() + ", value: " + goal.BaseValue.ToString("F4"));
        }

        return null;
    }

    /// <summary>
    /// Update the goal specified by the goalType parameter by the amount specified by the value paramter
    /// </summary>
    /// <param name="goalType">The goal to update</param>
    /// <param name="value">the amount to update it by</param>
    public void UpdateGoals(GoalLabels goalType, float value)
    {
        foreach (Goal goal in _goals)
        {
            //Debug.Log("before update: goal " + goal.Type.ToString() + " value = " + goal.Value.ToString("F4"));
            if (goal.Type == goalType)
            {
                goal.BaseValue = value;
            }
            Debug.Log("Updated goal: " + goal.Type.ToString() + ", value: " + goal.BaseValue.ToString("F4"));
        }
    }

    /// <summary>
    /// Choose an action for the agent by iterating through the list of goals and selecting the
    /// goal with the highest utility and then iterating through the actions looking for the action
    /// which reduces that goals value by the greatest amount
    /// </summary>
    /// <param name="agent">The agent we are choosing the actions for</param>
    /// <returns>The action wich reduces the maximum utility the most (the selected action)</returns>
    public Action ChooseAction(AI agent)
    {
        //find goal with the maximum utility
        Goal maxGoal = _goals[0];

        foreach (Goal goal in _goals)
        {
            // Get the goal with the highest utility generated through the utility function
            if (goal.FinalValue > maxGoal.FinalValue)
            {
                maxGoal = goal;
            }
        }
        Debug.Log("Max goal: " + maxGoal.Type + ", utility: " + maxGoal.FinalValue.ToString("F4"));

        // Find the action which satisfies the highest utility goal most?
        Action maxAction = _actions[0];
        foreach (Action action in _actions)
        {
            // Get the maximum utility action
            if (action.EvaluateGoalSatisfaction(maxGoal.Type) > maxAction.EvaluateGoalSatisfaction(maxGoal.Type))
            {
                maxAction = action;
            }
        }

        Debug.Log(" Best action: " + maxAction.ToString() + ", goal satisfaction: = " + maxAction.EvaluateGoalSatisfaction(maxGoal.Type).ToString("F4"));
        return maxAction;
    }
}
