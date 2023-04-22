using System;
using Time;
using TMPro;
using UnityEngine;

namespace UI
{
	public class MoneyPickUp : MonoBehaviour
	{
		[SerializeField] private GameObject _pickupPrefab;
		[SerializeField] private float _duration = 1.0f;
		private SimpleTimer _simpleTimer;

		private GameObject _pickup;

		private void OnValidate()
		{
			if (_pickupPrefab == null) Debug.LogWarning("No pickupPrefab set for " + name);
		}

		private void Start()
		{
			_simpleTimer = _pickupPrefab.GetComponent<SimpleTimer>();
			if (_simpleTimer == null) Debug.LogError("Pickup prefab has no SimpleTimer component");
		}

		public void Spawn(int pAmount)
		{
			_pickup = Instantiate(_pickupPrefab, transform.position, Quaternion.identity);

			TextMeshProUGUI text = _pickup.GetComponentInChildren<TextMeshProUGUI>();
			if (text == null)
			{
				Debug.LogError("Pickup prefab has no TextMeshProUGUI component in children");
				return;
			}
			text.text = "+ " + pAmount;

			_pickup.GetComponent<SimpleTimer>().StartTimer(_duration, () => Destroy(_pickup));
		}
	}
}