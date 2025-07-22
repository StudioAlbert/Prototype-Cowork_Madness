using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private List<SimplePlace> _allPlaces = new List<SimplePlace>();

        private void Awake()
        {
            foreach (var place in GetComponentsInChildren<SimplePlace>().ToList())
            {
                RegisterPlace(place);
            }
        }

        private void RegisterPlace(SimplePlace place)
        {
            if (!place) return;
            _allPlaces.Add(place);
        }
        private void UnregisterPlace(SimplePlace place)
        {
            if (!place && !_allPlaces.Contains(place)) return;
            _allPlaces.Remove(place);
        }
        public SimplePlace GetBestPlaceOfType(Vector3 positionFrom, PlaceType type)
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
