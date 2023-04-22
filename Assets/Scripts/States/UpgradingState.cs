using GameInformation;
using Time;
using UI;
using UnityEngine;

namespace States
{
	public class UpgradingState : IState
	{
		private GameManager _gameManager;


		public void Start()
		{
			_gameManager = GameManager.instance;

			ShowUpgrade(true);
			_gameManager.shop.shopUI.shopButton.SetActive(true);

			_gameManager.timer.StartTimer(_gameManager.upgradingPeriodDuration, () => { _gameManager.gameStateManager.SetState(new WaveState()); });

			_gameManager.header.waveStartsObject.SetActive(true);
			_gameManager.header.waveStartsObject.GetComponentInChildren<CountDown>().StartCountDown(_gameManager.upgradingPeriodDuration);
		}

		public void Update()
		{
		}

		public void FixedUpdate()
		{
		}

		public void Stop()
		{
			ShowUpgrade(false);

			_gameManager.shop.shopUI.Close();
			_gameManager.towerPlacer.CancelPlacing();
			_gameManager.shop.shopUI.shopButton.SetActive(false);
		}

		private void ShowUpgrade(bool pShow)
		{
			Transform container = _gameManager.towerPlacer.towerContainer.transform;

			for (int i = 0; i < container.childCount; i++)
			{
				TowerUI towerUI = container.GetChild(i).GetComponent<TowerUI>();
				Tower tower = container.GetChild(i).GetComponent<Tower>();

				if (towerUI == null) Debug.LogError("TowerUI is null");
				if (tower == null) Debug.LogError("Tower is null");

				if (towerUI == null || tower == null) continue;
				if (tower.level == tower.maxLevel) continue;

				towerUI.ShowUpgrade(pShow);
			}
		}
	}
}