using Time;
using UnityEngine;

namespace UI
{
	/// <summary>
	/// Cant afford message script
	/// </summary>
	[RequireComponent(typeof(SimpleTimer))]
	public class CantAfford : MonoBehaviour
	{
		private SimpleTimer _timer;

		private void Awake()
		{
			_timer = GetComponent<SimpleTimer>();
		}

		/// <summary>
		/// Shows or hides the can't afford message
		/// </summary>
		/// <param name="pShow"> show or not</param>
		public void Show(bool pShow)
		{
			gameObject.SetActive(pShow);
		}

		/// <summary>
		/// Shows can't afford message for a specified duration in seconds
		/// </summary>
		/// <param name="pDuration"> duration in seconds </param>
		public void ShowFor(float pDuration)
		{
			Show(true);

			_timer.StartTimer(pDuration, () => Show(false));
		}
	}
}