using System.Collections.Generic;
using System.Linq;
using AI_Motivation;
using Places;
using TMPro;
using UnityEngine;

namespace Places
{
    public class PlaceProvider : MonoBehaviour
    {

        // TODO Make a place provider :
        // Check type of the place according to the goal : Work, Break, Social
        // Is the place available ? (unoccupied, broken , empty, etc......)

        [SerializeField] private List<BasePlace> _allPlaces = new List<BasePlace>();

        public void RegisterPlace(BasePlace place)
        {
            if (!place) return;
            _allPlaces.Add(place);
        }
        public void UnregisterPlace(BasePlace place)
        {
            if (!place && !_allPlaces.Contains(place)) return;
            _allPlaces.Remove(place);
        }
        public BasePlace GetBestPlaceOfType(Vector3 positionFrom, GoalType type)
        {
            var availablePlaces = _allPlaces.FindAll(p => p.Type == type && p.Available);
            if (availablePlaces.Count == 0)
            {
                availablePlaces = _allPlaces.FindAll(p => p.Type == type);
            }

            return availablePlaces
                .OrderBy(p => Vector3.Distance(p.Position, positionFrom))
                .FirstOrDefault();

        }
    }
}
