using System;
using System.Collections.Generic;
using Event;
using GameInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
// ReSharper disable InconsistentNaming


public class Tower : MonoBehaviour
{
	[SerializeField] private string _shopName = "Tower";
	public string shopName => _shopName;
		
	[SerializeField] private int _cost = 1;
	public int cost => _cost;

	[SerializeField] private Tile _tile;
	public Tile tile => _tile;
	public Sprite sprite => _tile.sprite;

	[SerializeField] private SpriteRenderer _spriteRenderer;

	[SerializeField] private int _maxLevel = 3;
	public int maxLevel => _maxLevel;

	[SerializeField] private List<int> _upgradeCosts;
	public List<int> upgradeCosts => _upgradeCosts;

	[SerializeField] private GameObject _gridPositionObject;
	public Vector3 gridPosition => _gridPositionObject.transform.position;
	
	public int level { get; private set; } = 1;
	
	public CustomEventT<int> onLevelChange { get; } = new();
	public CustomEvent onLevelUp { get; } = new();
	public CustomEventT<int> onCostChange { get; } = new();

	private void OnValidate()
	{
		if (_tile == null) Debug.LogWarning("No tile set for " + name);
		if (_spriteRenderer == null) Debug.LogWarning("No sprite renderer set for " + name);
		if (_gridPositionObject == null) Debug.LogWarning("No grid position object set for " + name);
	}

	public void Start()
	{
		_spriteRenderer.sprite = sprite;
		onLevelChange.AddListener(UpdateCost);
		onLevelChange.Raise(level);
	}

	public void UpdateUI()
	{
		onLevelChange.Raise(level);
	}

	public void SetLevel(int pNewLevel)
	{
		if (pNewLevel <= 0 && pNewLevel > maxLevel) 
			Debug.LogError("Level can't be <= 0 or bigger than " + maxLevel);
		level = pNewLevel;
		onLevelChange.Raise(level);
	}

	public void UpdateCost(int pLevel)
	{
		if (upgradeCosts.Count <= pLevel - 1) return;
		_cost = upgradeCosts[pLevel - 1];
		onCostChange.Raise(cost);
	}
	
	/// <summary>
	/// Tries to update the tower by 1 level
	/// </summary>
	public bool Upgrade()
	{
		//Errors
		if (level >= maxLevel)
		{
			Debug.LogError("Upgrade failed: level is already max");
			return false;
		}

		//Can't afford?
		if (!GameManager.instance.wallet.RemoveMoney(upgradeCosts[level - 1]))
			return false;
			
		level++;
		onLevelUp.Raise();
		onLevelChange.Raise(level);
		return true;
	}
}
