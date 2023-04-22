namespace States
{
	public interface IState
	{
		public void Start();
		public void Update();
		public void FixedUpdate();
		public void Stop();
	}
}