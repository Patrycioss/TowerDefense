using System;

namespace Event
{
	public class CustomEventT<T>
	{
		private Action<T> _action;
		
		public void AddListener(Action<T> pListener)
		{
			_action += pListener;
		}
		
		public void RemoveListener(Action<T> pListener)
		{
			_action -= pListener;
		}
		
		public void Raise(T pValue)
		{
			_action?.Invoke(pValue);
		}
	}
}