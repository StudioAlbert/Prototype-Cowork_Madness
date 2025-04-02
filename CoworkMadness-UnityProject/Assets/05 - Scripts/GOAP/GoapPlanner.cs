using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace GOAP
{

    public interface IGoapPlanner
    {
        GoapPlan Plan(GoapAgent agent, List<GoapGoal> goals, GoapGoal mostRecentGoal);
    }

    class GoapPlanner : IGoapPlanner
    {

        public GoapPlan Plan(GoapAgent agent, List<GoapGoal> orderedGoals, GoapGoal mostRecentGoal)
        {
            // ==
            if (agent.Actions == null || agent.Actions.Count == 0)
            {
                Debug.LogWarning($"{agent.name} No action available... No plan..");
                return null;
            }

            // ==
            if (orderedGoals == null || orderedGoals.Count == 0)
            {
                Debug.LogWarning($"{agent.name} No goal available... No plan...");
                return null;
            }

            // Ordered goals by priority,
            // But the last goal is not executed every time
            // If a goal is already accomplished (DesiredEffects Evaluate to true)
            // List<GoapGoal> orderedGoals = goals
            //     //.Where(g => g.DesiredEffects.Any(b => !b.Evaluate()))§
            //     .OrderByDescending(g => g == mostRecentGoal ? g.Priority - 0.001 : g.Priority)
            //     .ToList();
            //


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
                //var desiredEffects = parentNode.DesiredEffects;
                // // Those who have been realized yet does not count
                // desiredEffects.RemoveWhere(b => b.Evaluate());
                //
                // // No desired effects so every thing is done, Success
                // if (desiredEffects.Count == 0)
                //     return true;
                
                if (action.PostConditions.Any(parentNode.DesiredEffects.Contains))
                {
                    // Take previous effects desired
                    var newDesiredEffects = new HashSet<GoapBelief>(parentNode.DesiredEffects);
                    // Except those will be realized
                    newDesiredEffects.ExceptWith(action.PostConditions);
                    // Plus those who we want to be realized
                    newDesiredEffects.UnionWith(action.Preconditions);

                    // get rid of the action concerned
                    var newAvailableActions = new HashSet<GoapAction>(actions);
                    newAvailableActions.Remove(action);

                    // Create a new node
                    var newNode = new Node(parentNode, action, newDesiredEffects, parentNode.Cost + action.Cost);
                    parentNode.Children.Add(newNode);

                    if (FindActionsPath(newNode, newAvailableActions))
                        newDesiredEffects.ExceptWith(newNode.Action.Preconditions);

                    if (newDesiredEffects.Count == 0)
                        return true;

                }
            }
            return false;
        }

    }

    public class Node
    {

        public Node Parent { get; }
        public GoapAction Action { get; set; }
        public HashSet<GoapBelief> DesiredEffects { get; }
        public List<Node> Children { get; }
        public float Cost { get; }

        public Node(Node parent, GoapAction action, HashSet<GoapBelief> requiredEffects, float cost)
        {
            Parent = parent;
            Action = action;
            DesiredEffects = new HashSet<GoapBelief>(requiredEffects);
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
