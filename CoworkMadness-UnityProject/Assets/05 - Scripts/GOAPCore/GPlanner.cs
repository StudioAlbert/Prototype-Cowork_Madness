using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace GOAPCore
{

    public class GPlanner
    {
        Stack<GAction> _thePlan;
        public Stack<GAction> ThePlan => _thePlan;

        public Action PlanHasEnded;
        
        public Stack<GAction> FindAPlan(List<GAction> actionSet, GGoal goal)
        {

            // First : conditions of the achievement
            List<GState> conditions = new List<GState>{new GState(goal.GoalName, 0)};
            GPlannerNode rootNode = new GPlannerNode(null, null, conditions);

            // Make the graph --------------------------------------------------------------------------
            DoANode(rootNode, new List<GAction>(actionSet));

            // Find the cheapest way in the graph -------------------------------------------------------
            //GPlannerNode currentNode = starterNode;
            var plan = SortThePlan(rootNode);
            //
            // Debug.Log("Planner Goal : "  + goal.GoalName);
            // foreach (var action in plan)
            // {
            //     Debug.Log("Planner Action : " + action.ActionName);
            // }
            
            return plan;

        }

        private static Stack<GAction> SortThePlan(GPlannerNode rootNode)
        {

            Stack<GAction> plan = new Stack<GAction>();
            GPlannerNode currentNode = rootNode;
            do
            {
                currentNode = currentNode.GetCheapestChild();

                if (currentNode != null && currentNode.Action != null)
                {
                    plan.Push(currentNode.Action);
                }
            } while (currentNode != null);
            
            return plan;
        }

        private void DoANode(GPlannerNode parentNode, List<GAction> tempActionSet)
        {
            
            if(parentNode.Effects.Count == 0)
                return;
            
            var actions = tempActionSet.Where(a => a.IsAchievableGiven(parentNode.Effects)).ToList();
            foreach (GAction action in actions)
            {

                GPlannerNode newNode = new GPlannerNode(action, parentNode, action.PreConditions);
                parentNode.AddChild(newNode);
                
                tempActionSet.Remove(action);
                
                DoANode(newNode, new List<GAction>(tempActionSet));
                
            }
        }

    }
}
