using GameInformation;

namespace States
{
	public class WaveState : IState
	{
		private GameManager _gameManager;
		
		public void Start()
		{
			_gameManager = GameManager.instance;

			_gameManager.header.waveStartsObject.SetActive(false);
			_gameManager.spawner.SpawnNextWave(() => { });
			_gameManager.AddWave();
		}

		public void Update() {}

		public void FixedUpdate()
		{
			if (!_gameManager.spawner.spawning && _gameManager.spawner.enemies.Count == 0)
			{
				if (_gameManager.waveCount == _gameManager.numberOfWaves) 
					_gameManager.gameStateManager.SetState(GameState.Finish);
				
				else _gameManager.gameStateManager.SetState(new UpgradingState());
			}
		}

		public void Stop() {}
	}
}