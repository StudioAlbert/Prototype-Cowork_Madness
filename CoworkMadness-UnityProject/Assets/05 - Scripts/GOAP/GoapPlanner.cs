using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace GOAP
{

    public interface IGoapPlanner
    {
        GoapPlan Plan(GoapAgent agent, HashSet<GoapGoal> goals, GoapGoal mostRecentGoal);
    }

    class GoapPlanner : IGoapPlanner
    {

        public GoapPlan Plan(GoapAgent agent, HashSet<GoapGoal> goals, GoapGoal mostRecentGoal)
        {
            // ==
            if (agent.Actions == null || agent.Actions.Count == 0)
            {
                Debug.LogWarning($"{agent.name} No action available... No plan..");
                return null;
            }
            
            // ==
            if (goals == null || goals.Count == 0)
            {
                Debug.LogWarning($"{agent.name} No goal available... No plan...");
                return null;
            }
            
            // Ordered goals by priority,
            // But the last goal is not executed every time
            List<GoapGoal> orderedGoals = goals
                .Where(g => g.DesiredEffects.Any(b => !b.Evaluate()))
                .OrderByDescending(g => g == mostRecentGoal ? g.Priority - 0.001 : g.Priority)
                .ToList();

            // Try every Goal
            foreach (GoapGoal goal in orderedGoals)
            {
                Node goalNode = new Node(null, null, goal.DesiredEffects, 0);

                if (FindActionsPath(goalNode, agent.Actions))
                {
                    // TODO
                    if (goalNode.IsLeafDead) continue;

                    Stack<GoapAction> actionsOfThePlan = new Stack<GoapAction>();
                    while (goalNode.Children.Count > 0)
                    {
                        var cheapestChild = goalNode.Children.OrderBy(c => c.Cost).First();
                        goalNode = cheapestChild;
                        actionsOfThePlan.Push(cheapestChild.Action);
                    }

                    return new GoapPlan(goal, actionsOfThePlan, goalNode.Cost);
                }
                
            }
            
            Debug.LogWarning("No Plan Found for any Goal");
            return null;
            
        }
        
        private bool FindActionsPath(Node parentNode, HashSet<GoapAction> actions)
        {
            var orderedActions = actions.OrderBy(a => a.Cost);
            
            foreach (GoapAction action in orderedActions)
            {
                var requiredEffects = parentNode.RequiredEffects;
                requiredEffects.RemoveWhere(b => b.Evaluate());

                // No required effects so every thing is done, Success
                if (requiredEffects.Count == 0)
                {
                    return true;
                }

                if (action.Effects.Any(requiredEffects.Contains))
                {
                    var newRequiredEffect = new HashSet<GoapBelief>(requiredEffects);
                    newRequiredEffect.ExceptWith(action.Effects);
                    newRequiredEffect.UnionWith(action.Preconditions);

                    var newAvailableActions = new HashSet<GoapAction>(actions);
                    newAvailableActions.Remove(action);

                    var newNode = new Node(parentNode, action, newRequiredEffect, parentNode.Cost + action.Cost);
                    parentNode.Children.Add(newNode);
                    
                    if (FindActionsPath(newNode, newAvailableActions))
                    {
                        //parentNode.Children.Add(newNode);
                        newRequiredEffect.ExceptWith(newNode.Action.Preconditions);
                    }

                    if (newRequiredEffect.Count == 0)
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }

    }

    public class Node
    {
        
        public Node Parent { get; }
        public GoapAction Action { get; set; }
        public HashSet<GoapBelief> RequiredEffects { get; }
        public List<Node> Children { get; }
        public float Cost { get; }
        
        public Node(Node parent, GoapAction action, HashSet<GoapBelief> requiredEffects, float cost)
        {
            Parent = parent;
            Action = action;
            RequiredEffects = new HashSet<GoapBelief>(requiredEffects);
            Children = new List<Node>();
            Cost = cost;
        }
        public bool IsLeafDead => Children.Count == 0 && Action == null;
    }

    public class GoapPlan
    {

        private GoapGoal _goal;
        private Stack<GoapAction> _actions;
        private float _totalCost;

        public GoapGoal Goal => _goal;
        public Stack<GoapAction> Actions => _actions;
        public float TotalCost => _totalCost;

        public GoapPlan(GoapGoal goal, Stack<GoapAction> actions, float totalCost)
        {
            _goal = goal;
            _actions = actions;
            _totalCost = totalCost;
        }

    }
}
