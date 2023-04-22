using System;

namespace Event
{
	public class CustomEvent
	{
		private Action _action;
	
		public void AddListener(Action pListener)
		{
			_action += pListener;
		}
	
		public void RemoveListener(Action pListener)
		{
			_action -= pListener;
		}
	
		public void Raise()
		{
			_action?.Invoke();
		}
	}
}