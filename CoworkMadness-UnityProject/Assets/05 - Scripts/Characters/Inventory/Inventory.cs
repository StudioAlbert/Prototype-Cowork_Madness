using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{

    [FormerlySerializedAs("_coffeeCup")] [SerializeField] private InventoryItem coffeeCup;
    [FormerlySerializedAs("_briefcase")] [SerializeField] private InventoryItem briefcase;

    public bool CoffeeEquipped
    {
        get => coffeeCup.IsEquipped;
        set => coffeeCup.IsEquipped = value;
    }
    public bool BriefcaseEquipped
    {
        get => briefcase.IsEquipped;
        set => briefcase.IsEquipped = value;
    }


}
