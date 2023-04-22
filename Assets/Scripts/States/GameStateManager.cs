using System;

namespace States
{
	[Serializable] public enum GameState
	{
		Start,
		Finish,
		Wave,
		Upgrading,
	}
	
	public class GameStateManager
	{
		private IState _currentState;

		public GameStateManager(GameState pGameState)
		{
			SetState(pGameState);
		}

		public void Update() => _currentState.Update();
		public void FixedUpdate() => _currentState.FixedUpdate();

		public void SetState(IState pState)
		{
			_currentState?.Stop();
			_currentState = pState;
			_currentState.Start();
		}
		
		public void SetState(GameState pGameState)
		{
			switch (pGameState)
			{
				case GameState.Start:
					SetState(new StartState());
					break;
				case GameState.Finish:
					SetState(new FinishState());
					break;
				case GameState.Wave:
					SetState(new WaveState());
					break;
				case GameState.Upgrading:
					SetState(new UpgradingState());
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(pGameState), pGameState, null);
			}
		}

	}
}