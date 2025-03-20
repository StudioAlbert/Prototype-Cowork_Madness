using System;
using System.Collections;
using System.Collections.Generic;
using Places;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace GOAP
{
    public class GoapAgent : MonoBehaviour
    {
        
        // Physics and regular components
        private NavMeshAgent _navMesh;
        private Rigidbody _rb;
        private Animator _animator;

        // TODO Place provider
        [Header("Places")]
        [SerializeField] private Transform desk;
        [SerializeField] private Transform coffeeMachine;
        [SerializeField] private Transform discussion;
        
        // TODO Mood manager => updates the priority according to goal types
        [SerializeField] private float workOverTime;
        [SerializeField] private float breakOverTime;
        [SerializeField] private float socialOverTime;
        
        // GOAP Machinery
        private GoapGoal _lastGoal;
        private GoapGoal _currentGoal;
        private GoapPlan _actionPlan;
        private GoapAction _currentAction;

        private Dictionary<string, GoapBelief> _beliefs;
        private HashSet<GoapAction> _actions;
        private HashSet<GoapGoal> _goals;
        
        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            
        }

        private void OnEnable()
        {
            StartCoroutine("UpdatePriorities");
        }
        private void OnDisable()
        {
            StopCoroutine("UpdatePriorities");
        }

        IEnumerator UpdatePriorities()
        {
            do
            {
                // TODO Moodmanager
                foreach (GoapGoal goal in _goals)
                {
                    switch (goal.PlaceType)
                    {
                        case PlaceType.Work:
                            goal.Priority += workOverTime * Time.deltaTime;
                            break;
                        case PlaceType.Break:
                            goal.Priority += breakOverTime * Time.deltaTime;
                            break;
                        case PlaceType.Social:
                            goal.Priority += socialOverTime * Time.deltaTime;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    goal.Priority = Mathf.Clamp(goal.Priority, 0, 100);

                }
                
                yield return new WaitForSeconds(1);
            } while (true);
        }

    }

}
