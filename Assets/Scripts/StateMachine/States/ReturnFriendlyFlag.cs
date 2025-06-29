using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFriendlyFlag : StateBase
{
    /// <summary>
    /// Friendly flag game object.
    /// </summary>
    private GameObject _friendlyFlag;

    /// <summary>
    /// If we have check the base for our flag.
    /// </summary>
    private bool _checkedBase = false;

    public ReturnFriendlyFlag(FiniteStateMachine fsm) : base(fsm)
    {
    }


    public override void Enter(AI entity)
    {
        // set checked base to false to check the base first.
        _checkedBase = false;

        // Go to our base.
        entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
    }

    public override void Execute(AI entity)
    {
        // ! Turns out the player cannot see the flag of the carrier because its meant to be hidden :c. Renders attack flag carrier pointless.

        // check if we can see the flag.
        _friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();

        // We go to the base to search it.
        if (!_checkedBase && _friendlyFlag == null && !entity.AgentData.HasFriendlyFlag)
        {

            if (entity.HasReachedDestination() || Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) <= AI.MinDistanceToBase)
            {
                // we are at the base. lets check for the flag.

                _checkedBase = true;
            }

            return;
        }
        // we then check the enemy base because it has to be there.
        else if (_friendlyFlag == null && !entity.AgentData.HasFriendlyFlag)
        {
            entity.AgentActions.MoveTo(entity.AgentData.EnemyBase);

            if (Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) <= AI.MinDistanceToBase)
            {
                _owningFSM.ChangeState(_owningFSM.GoToBase);
                return;
            }

            return;
        }

        // if we can see the flag we attack the owner if its an enemy, go home if its a friendly or go get the flag. 
        if (_friendlyFlag != null && !entity.AgentData.HasFriendlyFlag)
        {
            if (_friendlyFlag.transform.parent != null)
            {
                if (_friendlyFlag.transform.parent.tag == entity.AgentData.EnemyTeamTag)
                {
                    _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                    return;
                }
                else
                {
                    _owningFSM.ChangeState(_owningFSM.GoToBase);
                    return;
                }
            }

            entity.AgentActions.MoveTo(_friendlyFlag);
            entity.AgentActions.CollectItem(_friendlyFlag);
        }
        // Quick, we must go to base with our flag.
        else if (entity.AgentData.HasFriendlyFlag && Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase) // magic number
        {
            entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
        }
        // We can the resume normal devious activities.
        else if (entity.AgentData.HasFriendlyFlag && Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) <= AI.MinDistanceToBase)
        {

            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }

    }

    public override void Exit(AI entity)
    {
        // drop the flag if we have it still.
        if (entity.AgentInventory.HasItem(entity.AgentData.FriendlyFlagName))
            entity.AgentActions.DropItem(entity.AgentInventory.GetItem(entity.AgentData.FriendlyFlagName));
    }
}
