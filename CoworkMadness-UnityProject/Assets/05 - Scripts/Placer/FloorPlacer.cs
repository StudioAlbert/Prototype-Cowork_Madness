using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class FloorPlacer : MonoBehaviour
{

    [SerializeField] private GameObject _solidTilesParent;
    [SerializeField] private GameObject _ghostTilesParent;
    
    // [SerializeField] private SO_RuntimeSet _solidTiles;
    // [SerializeField] private SO_RuntimeSet _ghostTiles;
    private List<FloorTile> _solidTiles;
    private List<FloorTile> _ghostTiles;

    [SerializeField] private List<FloorTile> _tilesModels;
    private FloorTile _tileModel;
    [SerializeField] private FloorTile _ghostModel;
    [SerializeField] private FloorTile _cursorModel;
    [SerializeField] private float _floorLevel;

    [SerializeField] private Color _okPlaceColor;
    [SerializeField] private Color _koPlaceColor;


    private readonly List<Vector3> _neighbourhood = new List<Vector3>()
    {
        Vector3.forward,
        Vector3.left,
        Vector3.back,
        Vector3.right
    };

    private const float STEP_GRID = 2.5F;
    private const bool IS_MOUSE_CONTROL = true;

    private FloorTile _cursorTile;

    private Camera _mainCamera;
    private Material _cursorMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        StartPlacement();
    }

    private void OnDisable()
    {
        StopPlacement();
    }


    private void StartPlacement()
    {
        _ghostTilesParent.SetActive(true);

        _ghostTiles = _ghostTilesParent.GetComponentsInChildren<FloorTile>().ToList();
        _solidTiles = _solidTilesParent.GetComponentsInChildren<FloorTile>().ToList();

        if (_tilesModels.Count > 0)
            _tileModel = _tilesModels[0];

        if (_cursorTile == null)
        {
            _cursorTile = Instantiate<FloorTile>(_cursorModel, Vector3.zero, Quaternion.identity);
            _cursorTile.name = "Cursor Tile";
            _cursorMaterial = _cursorTile.GetComponentInChildren<MeshRenderer>().material;
        }

        if (_solidTiles.Count <= 0 && _ghostTiles.Count <= 0)
        {
            // Place the first ghost
            PlaceAGhost(Vector3.zero);
        }

    }

    private void StopPlacement()
    {
        _ghostTilesParent.SetActive(false);

        if (_cursorTile != null)
            Destroy(_cursorTile.gameObject);
        
        _cursorMaterial = null;

    }

    // Update is called once per frame
    void Update()
    {

        if (IS_MOUSE_CONTROL)
        {
            var mouseRay = _mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.farClipPlane));

            _cursorMaterial.color = _koPlaceColor;
            _cursorTile.gameObject.SetActive(false);

            if (Physics.Raycast(mouseRay.origin, mouseRay.direction * _mainCamera.farClipPlane, out var mouseHit))
            {

                if (mouseHit.collider.TryGetComponent<FloorTile>(out var ghostCursorCandidate))
                {
                    _cursorMaterial.color = _okPlaceColor;
                    _cursorTile.gameObject.SetActive(true);

                    MoveCursor(ghostCursorCandidate.transform.position);
                    
                    if (Input.GetMouseButton((int)MouseButton.Left))
                    {
                        PlaceTile(ghostCursorCandidate);
                    }
                    
                    if (Input.GetMouseButton((int)MouseButton.Right))
                    {
                        RemoveTile(ghostCursorCandidate);
                    }
                    
                }
            }
        }


    }

    private void OnDrawGizmos()
    {
        if (_mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Infinity), 1);

            var mouseRay = _mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Infinity));
            Gizmos.DrawRay(mouseRay.origin, mouseRay.direction * _mainCamera.farClipPlane);
        }
    }

    public void MoveCursor(Vector3 cursorPosition)
    {
        _cursorTile.transform.position = cursorPosition;
        // _cursorPosition = new Vector3(cursorPosition.x, _floorLevel, cursorPosition.z);
    }


    public void PlaceTile(FloorTile tilePlacement)
    {

        // ----------------------------------------------------------------------------------------
        // If not a ghost tile, return; We only place regular tile on ghost tile --
        if (!_ghostTiles.Exists(t => Vector3.Distance(t.transform.position, tilePlacement.transform.position) < Mathf.Epsilon))
            return;

        // ----------------------------------------------------------------------------------------
        // then replace the ghost with a regular
        // Instantiate a tile
        PlaceARegularTile(tilePlacement.transform.position);

        // Remove the ghost
        RemoveAGhost(tilePlacement.transform.position);


        //
        // Place a ghost if empty
        // ----------------------------------------------------------------------------------------
        // Instantiate 4 ghosts (North, East, South, West)
        foreach (var ghostNeighbour in _neighbourhood)
        {
            // Place a ghost if empty
            var newGhostPosition = _cursorTile.transform.position + ghostNeighbour * STEP_GRID;
            if (!_ghostTiles.Exists(t => Vector3.Distance(t.transform.position, newGhostPosition) < Mathf.Epsilon)
                &&
                !_solidTiles.Exists(t => Vector3.Distance(t.transform.position, newGhostPosition) < Mathf.Epsilon))
            {
                PlaceAGhost(newGhostPosition);
            }
        }

    }

    public void RemoveTile(FloorTile tilePlacement)
    {
        
        // ----------------------------------------------------------------------------------------
        // Remove if it a regular tile
        // If not a regular tile, return;
        if (!_solidTiles.Exists(t => Vector3.Distance(t.transform.position, tilePlacement.transform.position) < Mathf.Epsilon))
            return;
        
        // Remove the tile
        RemoveAPlacedTile(tilePlacement.transform.position);

        // Replace with a ghost if there is a solid tile around
        bool hasSolidNeighbour = false;
        foreach (var solidNeighbour in _neighbourhood)
        {
            if(hasSolidNeighbour) continue;
            
            var solidCandidatePosition = tilePlacement.transform.position + solidNeighbour * STEP_GRID;
            if (_solidTiles.Exists(t => Vector3.Distance(t.transform.position, solidCandidatePosition) < Mathf.Epsilon))
            {
                hasSolidNeighbour = true;
            }
        }
        
        if(hasSolidNeighbour)
            PlaceAGhost(tilePlacement.transform.position);
        
        // ----------------------------------------------------------------------------------------
        // Then remove every possible ghosts around
        // if ghost candidate for removing have solid tiles around they are not removed
        // Instantiate 4 ghosts (North, East, South, West)
        foreach (var ghostNeighbour in _neighbourhood)
        {
            // Place a ghost if empty
            var ghostCandidatePosition = tilePlacement.transform.position + ghostNeighbour * STEP_GRID;
            if (_ghostTiles.Exists(t => Vector3.Distance(t.transform.position, ghostCandidatePosition) < Mathf.Epsilon))
            {
                bool hasGhostASolidNeighbour = false;
                foreach (var solidNeighbour in _neighbourhood)
                {
                    
                    if(hasGhostASolidNeighbour) continue;
                    
                    // Place a ghost if empty
                    var solidPosition = ghostCandidatePosition + solidNeighbour * STEP_GRID;
                    if (_solidTiles.Exists(t => Vector3.Distance(t.transform.position, solidPosition) < Mathf.Epsilon))
                    {
                        hasGhostASolidNeighbour = true;
                    }
                }
                
                if(!hasGhostASolidNeighbour)
                    RemoveAGhost(ghostCandidatePosition);
            }
        }
        
    }

    private void RemoveAPlacedTile(Vector3 position)
    {
        var tileToRemove = _solidTiles.FirstOrDefault(t => Vector3.Distance(t.transform.position, position) < Mathf.Epsilon);
        if (tileToRemove != null)
        {
            _solidTiles.Remove(tileToRemove);
            Destroy(tileToRemove.gameObject);
        }
    }

    private void RemoveAGhost(Vector3 position)
    {
        var tileToRemove = _ghostTiles.FirstOrDefault(t => Vector3.Distance(t.transform.position, position) < Mathf.Epsilon);
        if (tileToRemove != null)
        {
            _ghostTiles.Remove(tileToRemove);
            Destroy(tileToRemove.gameObject);
        }
    }

    private void PlaceARegularTile(Vector3 position)
    {
        var tileToPlace = Instantiate<FloorTile>(_tileModel, position, Quaternion.identity, _solidTilesParent.transform);
        _solidTiles.Add(tileToPlace);
    }

    private void PlaceAGhost(Vector3 position)
    {
        var newGhost = Instantiate<FloorTile>(_ghostModel, position, Quaternion.identity, _ghostTilesParent.transform);
        _ghostTiles.Add(newGhost);
    }
}
