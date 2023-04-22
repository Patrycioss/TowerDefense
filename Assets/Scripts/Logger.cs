using UnityEngine;

public class Logger
{
	private readonly bool _logging;
	
	public Logger()
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD || DEBUG
			_logging = true;
		#else 
			logging = false;
		#endif
	}
	
	public void Log(string pMessage)
	{
		if (!_logging) return;
		Debug.Log(pMessage);
	}

	public void Warn(string pMessage)
	{
		if (!_logging) return;
		Debug.LogWarning(pMessage);
	}

	public void Error(string pMessage)
	{
		if (!_logging) return;
		Debug.LogError(pMessage);
	}
}