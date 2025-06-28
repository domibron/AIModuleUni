using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyBase : Action
{
    public AttackEnemyBase(AI agent) : base(agent)
    {
    }

    public override void Execute(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
