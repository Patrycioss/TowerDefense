using GameInformation;

namespace States
{
	public class StartState : IState
	{
		private GameManager _gameManager;
		
		public void Start()
		{
			_gameManager = GameManager.instance;
			_gameManager.ResetPlayerData();
			_gameManager.startMenu.gameObject.SetActive(true);
		}

		public void Update() {}

		public void FixedUpdate() {}
		
		public void Stop()
		{
			_gameManager.startMenu.gameObject.SetActive(false);
		}
	}
}