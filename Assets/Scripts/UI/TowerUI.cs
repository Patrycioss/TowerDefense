﻿using TMPro;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(Tower))]
	public class TowerUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private TextMeshProUGUI _costText;
		[SerializeField] private GameObject _upgrade;
		[SerializeField] private GameObject _cantAffordObject;

		private Tower _tower;
		private CantAfford _cantAfford;

		private void OnValidate()
		{
			if (_levelText == null) Debug.LogWarning("No levelText set for " + name);
			if (_costText == null) Debug.LogWarning("No costText set for " + name);
			if (_upgrade == null) Debug.LogWarning("No upgrade set for " + name);
			if (_cantAffordObject == null) Debug.LogWarning("No cantAffordObject set for " + name);
		}

		public void TryUpgrade()
		{
			if (!_tower.Upgrade()) 
				_cantAfford.ShowFor(0.5f);
		}

		private void Awake()
		{
			_tower = transform.GetComponent<Tower>();
			_tower.onLevelChange.AddListener(UpdateLevelText);
			_tower.onCostChange.AddListener(UpdateCostText);
			
			_cantAfford = _cantAffordObject.GetComponent<CantAfford>();
		}
		
		private void UpdateLevelText(int pLevel)
		{
			_levelText.text = "LV " + pLevel;
			
			if (pLevel == _tower.maxLevel) 
				_upgrade.SetActive(false);
		}
		
		private void UpdateCostText(int pCost)
		{
			_costText.text = pCost.ToString();
		}

		public void ShowUpgrade(bool pShow)
		{
			_upgrade.SetActive(pShow);
		}
		
	}
}