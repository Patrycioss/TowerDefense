using System;
using GameInformation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject _towerContainer;
    public GameObject towerContainer => _towerContainer;
    
    [SerializeField] private Tilemap _grassMap;
    [SerializeField] private Tilemap _towerMap;

    public bool holdsTower { get; private set; }

    private Tower _tower;
    private Camera _mainCamera;
    private Vector3Int _prevPos;
    private GameObject _prevTower;

    private void OnValidate()
    {
        if (_towerContainer == null) Debug.LogWarning("No tower container set for " + name);
        if (_grassMap == null) Debug.LogWarning("No grass map set for " + name);
        if (_towerMap == null) Debug.LogWarning("No tower map set for " + name);
    }

    public void GiveTower(Tower pTower)
    {
        _tower = pTower;
        holdsTower = true;
        ResetPreviousValues();
    }
    
    private void ResetPreviousValues()
    {
        _prevPos = new Vector3Int(9999, 9999);
        _prevTower = null;
    }
    
    private void Start() => _mainCamera = Camera.main;

    private void Update()
    {
        if (!holdsTower) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos = _mainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        
        Vector3Int pos = _grassMap.WorldToCell(mousePos);

        bool grassHasPos = _grassMap.HasTile(pos);
        bool towerHasPos = _towerMap.HasTile(pos);
        
        if (pos != _prevPos && grassHasPos && !towerHasPos)
        {
            if (_prevTower != null) Destroy(_prevTower);
            _prevTower = CreateTower(pos);
        }

        if (Input.GetMouseButtonDown(0) && grassHasPos && !towerHasPos) PlaceTower(pos);
        else if (Input.GetMouseButtonDown(1)) CancelPlacing();
    }

    public void RemoveAllTowers()
    {
        _towerMap.ClearAllTiles();
        foreach (Transform child in _towerContainer.transform) Destroy(child.gameObject);
    }

    //Places the tower and definitively buys the tower
    private void PlaceTower(Vector3Int pPosition)
    {
        GameManager.instance.wallet.RemoveMoney(_tower.cost);

        holdsTower = false;
        _towerMap.SetTile(pPosition, _tower.tile);
    }
    
    //Cancel placing tower
    public void CancelPlacing()
    {
        if (!holdsTower) return;
        
        holdsTower = false;
        _towerMap.SetTile(_prevPos, null);
        Destroy(_prevTower);
    }

    /// <summary>
    /// Creates the Tower object from prefab
    /// </summary>
    private GameObject CreateTower(Vector3Int pPosition)
    {
        Vector3 worldPos = _towerMap.CellToWorld(pPosition);

        worldPos += (_tower.gridPosition - _tower.transform.position);

        GameObject towerObject = _tower.gameObject;
        GameObject newTower = Instantiate(towerObject, worldPos, Quaternion.identity, _towerContainer.transform);
        newTower.GetComponent<Tower>().UpdateUI();
        return newTower;
    }
}