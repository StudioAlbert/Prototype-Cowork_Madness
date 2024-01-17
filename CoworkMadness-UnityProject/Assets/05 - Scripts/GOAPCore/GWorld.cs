using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAPCore
{
    public class GWorld : MonoBehaviour
    {

        private List<GState> _states = new List<GState>();
        public List<GState> States => _states;

        public bool HasState(string hash)
        {
            return _states.Exists(gp => gp.Hash == hash);
        }

        private void AddState(string hash, int value)
        {
            _states.Add(new GState(hash ,value));
        }

        public void SetState(string hash, int newValue)
        {
            var state = _states.Where(gp => gp.Hash == hash).FirstOrDefault();
        
            if (state != null)
                state.Count = newValue;
            else
                AddState(hash, newValue);
        }

        public void RemoveState(string hash)
        {
            var state = _states.Where(gp => gp.Hash == hash).FirstOrDefault();
        
            if (state != null)
                _states.Remove(state);
        
        }
    }
}
