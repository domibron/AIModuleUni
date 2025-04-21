using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the goals used for the utility AI
public enum GoalLabels
{
    Health,
    Aggression,
    Fear,
    AttackObjective,
    DefendObjective,
}

/// <summary>
/// Storage for the curve functions used by the goals
/// </summary>
public static class CurveFunctions
{
    private static float _lowerValueRange = 0.0f;
    private static float _upperValueRange = 1.0f;

    // Returns maximum insistence satisfaction when the upper range is reached
    public static float StepAtUpper(float lowerValueRange, float upperValueRange, float val)
    {
        if (val >= upperValueRange)
        {
            return _upperValueRange;
        }
        return _lowerValueRange;
    }

    // Returns a normalised insistence satisfaction value linearly to the input value
    public static float Linear(float lowerValueRange, float upperValueRange, float val)
    {
        return _lowerValueRange + (_upperValueRange - _lowerValueRange) * (val - lowerValueRange) / (upperValueRange - lowerValueRange);
    }

    // Returns a normalised exponetial insistence satisfaction value
    public static float Exponential(float lowerValueRange, float upperValueRange, float val)
    {
        // A power of 3.5 gives a fairly steep curve
        const float power = 3.5f;
        return (_upperValueRange - _lowerValueRange) * Mathf.Pow((val - lowerValueRange) / (upperValueRange - lowerValueRange), power);
    }

    // Returns a normalised logarithmic insistence satisfaction value
    public static float Logarithmic(float lowerValueRange, float upperValueRange, float val)
    {
        // A power of 0.2 gives a fairly steep logarithmic curve
        const float power = 0.2f;
        return _lowerValueRange + (_upperValueRange - _lowerValueRange) * Mathf.Pow((val - lowerValueRange) / (upperValueRange - lowerValueRange), power);
    }

    // Returns a normalised inverse exponetial insistence satisfaction value
    public static float InverseLinear(float lowerValueRange, float upperValueRange, float val)
    {
        return 1 - _lowerValueRange + (_upperValueRange - _lowerValueRange) * (val - lowerValueRange) / (lowerValueRange - upperValueRange);
    }
}

// Delegate for the curve functions
public delegate float CurveFunction(float lowerValueRange, float upperValueRange, float val);

/// <summary>
/// Represents a goal. Stores the range of values, from lowerValueRange to upperValueRange,
/// the current insistance value of the goal and the curve function to get the final insistence satisfaction of
/// the goal
/// </summary>
public class Goal
{
    // The range of values we are evaluating
    private float _lowerValueRange;
    private float _upperValueRange;

    private float _value;

    CurveFunction _curveFunction;
    // Goal label
    GoalLabels _type;

    public Goal(GoalLabels type, float val, float lowerValueRange, float upperValueRange, CurveFunction curveFunction)
    {
        _type = type;
        _lowerValueRange = lowerValueRange;
        _upperValueRange = upperValueRange;

        _value = val;
        _curveFunction = curveFunction;
    }

    public GoalLabels Type
    {
        get { return _type; }
    }

    public float BaseValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public float FinalValue
    {
        get { return _curveFunction.Invoke(_lowerValueRange, _upperValueRange, _value); }
    }
}
