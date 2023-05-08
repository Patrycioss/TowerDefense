using System;
using System.Collections.Generic;
using Event;
using ScriptableObjects;
using States;
using Time;
using UI;
using UnityEngine;

namespace GameInformation
{
	/// <summary>
	/// Manages the different functionalities of the game, serves as the main controller of the game
	/// </summary>
	
	public sealed class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		
		[Header("Settings")]
		[SerializeField] private int _startMoney = 1;
		[SerializeField] private int _startLives = 5;

		public int numberOfWaves => _waves.Count;
		
		[SerializeField] private GameState _beginGameState = GameState.Start;
		[SerializeField] private int _upgradingPeriodDuration = 7;
		public int upgradingPeriodDuration => _upgradingPeriodDuration;

		[Header("Systems")]
		[SerializeField] private Shop _shop;
		public Shop shop => _shop;

		[SerializeField] private TowerPlacer _towerPlacer;
		public TowerPlacer towerPlacer => _towerPlacer;

		[SerializeField] private Spawner _spawner; 
		public Spawner spawner => _spawner;
		
		[Header("UI")]
		[SerializeField] private Header _header;
		public Header header => _header;
		
		[SerializeField] private StartMenu _startMenu;
		public StartMenu startMenu => _startMenu;
		
		[SerializeField] private FinishMenu _finishMenu;
		public FinishMenu finishMenu => _finishMenu;
		
		[Header("Data")]
		[SerializeField] private List<Tower> _towers;
		public List<Tower> towers => _towers;

		[SerializeField] private List<GameObject> _enemies;
		public List<GameObject> enemies => _enemies;
		
		[SerializeField] private List<Wave> _waves;
		public List<Wave> waves => _waves;
		
		public Wallet wallet { get; private set; }
		public GameStateManager gameStateManager { get; private set; }
		public SimpleTimer timer { get; private set; }

		public int lives { get; private set; }
		public int waveCount { get; private set; }
		
		//Events
		public CustomEventT<int> onLivesUpdated { get; private set; }
		public CustomEvent onPlayerDeath { get; private set; }
		public CustomEventT<int> onWaveCountUpdated { get; private set; }

		private void OnValidate()
		{
			if (_shop == null) Debug.LogWarning("No shop set for " + name);
			if (_towerPlacer == null) Debug.LogWarning("No towerPlacer set for " + name);
			if (_spawner == null) Debug.LogWarning("No spawner set for " + name);
			if (_header == null) Debug.LogWarning("No header set for " + name);
			if (_startMenu == null) Debug.LogWarning("No startMenu set for " + name);
			if (_finishMenu == null) Debug.LogWarning("No finishMenu set for " + name);
			if (_towers == null) Debug.LogWarning("No towers set for " + name);
			if (_enemies == null) Debug.LogWarning("No enemies set for " + name);
			if (_waves == null) Debug.LogWarning("No waves set for " + name);
		}

		private void Awake()
		{
			if (instance == null) instance = this;
			else Destroy(this);
			
			lives = _startLives;
			onLivesUpdated = new CustomEventT<int>();
			
			wallet = new Wallet(_startMoney);
			_header.SendUpdate(wallet);

			gameStateManager = new GameStateManager(_beginGameState);
			timer = gameObject.AddComponent<SimpleTimer>();
			
			//Events
			onWaveCountUpdated = new CustomEventT<int>();
			onPlayerDeath = new CustomEvent();
			onPlayerDeath.AddListener(() =>
			{
				waveCount--;
				gameStateManager.SetState(GameState.Finish);
			});
		}

		private void Update() => gameStateManager.Update();
		private void FixedUpdate() => gameStateManager.FixedUpdate();

		/// <summary>
		/// Removes one life from the player
		/// </summary>
		public void RemoveALife()
		{
			lives--;
			onLivesUpdated.Raise(lives);

			if (lives == 0) onPlayerDeath.Raise();
		}

		/// <summary>
		/// Updates the money and lives events (for UI)
		/// </summary>
		public void RequestUpdates()
		{
			wallet.onMoneyChanged.Raise(wallet.money);
			onLivesUpdated.Raise(lives);
		}

		/// <summary>
		/// Adds one wave to the wave count
		/// </summary>
		public void AddWave()
		{
			waveCount++;
			onWaveCountUpdated.Raise(waveCount);
		}

		/// <summary>
		/// Resets all of the player data
		/// </summary>
		public void ResetPlayerData()
		{
			lives = _startLives;
			onLivesUpdated.Raise(lives);
			wallet.RemoveMoney(wallet.money);
			wallet.AddMoney(_startMoney);
			waveCount = 0;
			towerPlacer.RemoveAllTowers();
		}
	}
}

