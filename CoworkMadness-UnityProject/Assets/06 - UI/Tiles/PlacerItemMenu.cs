using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacerItemMenu
{
    private VisualElement _container;
    private FloorPlacer _controller;

    private Button _btFloorPlacer;
    private VisualElement _statusFloorPlacer;

    private readonly Color _disabledBorderColor = new Color();
    private readonly Color _enabledBorderColor = new Color();

    private readonly Color _disabledBackColor = new Color();
    private readonly Color _enabledBackColor = new Color();

    public PlacerItemMenu(VisualElement root, string name, FloorPlacer controller)
    {
        // _container = new VisualElement();
        _controller = controller;

        _container = root.Q<VisualElement>(name);
        if (_container != null)
        {
            _btFloorPlacer = _container.Q<Button>("Button");
            _btFloorPlacer.clicked += OnClicked;

            _statusFloorPlacer = _container.Q<VisualElement>("Status");

        }

        ColorUtility.TryParseHtmlString("#BD4D4D", out _disabledBorderColor);
        ColorUtility.TryParseHtmlString("#64A254", out _enabledBorderColor);
        ColorUtility.TryParseHtmlString("#5E5E5E", out _disabledBackColor);
        ColorUtility.TryParseHtmlString("#84827E", out _enabledBackColor);
        
    }

    private void OnClicked()
    {
        _controller.enabled = !_controller.enabled;

        // Status -----------------------------
        if (_statusFloorPlacer != null)
            _statusFloorPlacer.style.backgroundColor = _controller.enabled ? _enabledBorderColor : _disabledBorderColor;

        // Button background
        _btFloorPlacer.style.backgroundColor = _controller.enabled ? _enabledBackColor : _disabledBackColor;
        // Button border
        _btFloorPlacer.style.borderBottomColor = _controller.enabled ? _enabledBorderColor : _disabledBorderColor;
        _btFloorPlacer.style.borderLeftColor = _controller.enabled ? _enabledBorderColor : _disabledBorderColor;
        _btFloorPlacer.style.borderRightColor = _controller.enabled ? _enabledBorderColor : _disabledBorderColor;
        _btFloorPlacer.style.borderTopColor = _controller.enabled ? _enabledBorderColor : _disabledBorderColor;

    }


}
