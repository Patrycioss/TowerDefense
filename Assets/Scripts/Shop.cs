using GameInformation;
using UI;
using UnityEngine;

/// <summary>
/// Handles the shop and its prices
/// </summary>
[RequireComponent(typeof(ShopUI))]
public class Shop : MonoBehaviour
{
	private ShopUI _shopUI;

	public ShopUI shopUI
	{
		get
		{
			if (_shopUI == null) _shopUI = GetComponent<ShopUI>();
			return _shopUI;
		}
	}

	/// <summary>
	/// Buys a tower from the shop
	/// </summary>
	/// <param name="pTower"> tower to buy</param>
	public void BuyTower(Tower pTower)
	{
		GameManager gameManager = GameManager.instance;
		if (GameManager.instance.wallet.CanAfford(pTower.cost))
		{
			gameManager.towerPlacer.GiveTower(pTower);
			shopUI.Close();
		}
		else
		{
			_shopUI.ShowCantAffordMessageFor(2);
		}
	}
}
