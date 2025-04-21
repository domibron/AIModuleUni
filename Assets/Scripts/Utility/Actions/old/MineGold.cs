using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class MineGold : Action
// {
// // fatigue increase through mining
// private const int FatigueIncrease = 2;

// public MineGold(AI agent) :base(agent)
// {
// }

// public override void Execute(float deltaTime)
// {
//     // go to the mine, when we get there start mining
//     if(GoToMine())
//     {
//         DigForNuggets(deltaTime);
//     }
// }

// // go to the mine
// private bool GoToMine()
// {
//     // If we're not trying to go to the mine, set mine as target location
//     if (_agent.TargetLocation != Locations.Mine)
//     {
//         _agent.ChangeLocation(Locations.Mine);
//     }

//     if (_agent.CurrentLocation != _agent.TargetLocation)
//     {
//         Debug.Log("Going to the Mine...");
//         _agent.ChangeLocation(Locations.Mine);

//         return false;
//     }
//     return true;
// }

// // mine for gold
// private bool DigForNuggets(float deltaTime)
// {
//     // If we're at the mine start mining
//     AddToGoldCarried(_agent.AmountMined);

//     _agent.Fatigue += FatigueIncrease;
//     _agent.AI.UpdateGoals(GoalLabels.Rest, _agent.Fatigue);
//     Debug.Log("Picking up a nugget and that's..." + _agent.GoldCarried + ", now feeling " + _agent.Fatigue + " tired.");

//     if (_agent.PocketsFull())
//     {
//         _complete = true;
//         return _complete;
//     }
//     return _complete;
// }

// public void AddToGoldCarried(int amount)
// {
//     // Got some gold
//     _agent.GoldCarried += amount;
//     _agent.AI.UpdateGoals(GoalLabels.MineGold, _agent.MoneyInBank + _agent.GoldCarried);
//     _agent.AI.UpdateGoals(GoalLabels.BankGold, _agent.GoldCarried);
// }

// public override string ToString()
// {
//     return "Mine Gold";
// }
// }
