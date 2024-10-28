using System.Collections.Generic;
using GOAPCore;
using UnityEngine;

namespace GOAPCore
{
    public class Action_UseMachine: GAction
    {
        [SerializeField] private BaseMachine _machine;
        
        public override bool CanPerform()
        {
            
            if (base.CanPerform())
            {
                bool machineCanPerform = (_machine.User == null || _machine.User == _gAgent);
                if (machineCanPerform)
                    _machine.OnTaskEnded += PostPerform;

                return machineCanPerform;
                
            }
            else
            {
                return false;
            }
            
        }

        public override void Perform()
        {
            
            // Go to target point
            _gAgent.NavMesh.destination = _machine.QueuePosition.position;
            _machine.Lock(_gAgent);
            
            // If done wait
            if(Vector3.Distance(transform.position,  _machine.QueuePosition.position) <= _gAgent.NavMesh.stoppingDistance)
            {
                // Wait for the machine
                _machine.BeginTask();
            }
        }

    }
}
