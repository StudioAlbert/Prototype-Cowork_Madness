using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




public class PlacerMenu : MonoBehaviour
{

    private UIDocument _menu;
    private FloorPlacer _controller;

    // Buttons
    private PlacerItemMenu _placerItemMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        _menu = GetComponent<UIDocument>();
        _controller = GetComponent<FloorPlacer>();
        
        _placerItemMenu = new PlacerItemMenu(_menu.rootVisualElement, "FloorContainer", _controller);
        
    }

}
