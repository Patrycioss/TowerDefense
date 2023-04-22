using GameInformation;

namespace States
{
	public class FinishState : IState
	{
		public void Start()
		{
			GameManager.instance.header.gameObject.SetActive(false);
			GameManager.instance.finishMenu.gameObject.SetActive(true);
		}

		public void Update() {}
		public void FixedUpdate() {}

		public void Stop()
		{
			GameManager.instance.ResetPlayerData();
			GameManager.instance.header.gameObject.SetActive(true);
			GameManager.instance.finishMenu.gameObject.SetActive(false);
		}
	}
}