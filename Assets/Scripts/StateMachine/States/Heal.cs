using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : StateBase
{
    public override void Enter(AI entity)
    {
        entity.AgentInventory.HasItem("Health Kit");
    }

    public override void Execute(AI entity)
    {

    }

    public override void Exit(AI entity)
    {

    }
}
