using GameInformation;
using Time;
using TMPro;
using UnityEngine;

namespace UI
{
	public class Header : MonoBehaviour
	{
		[SerializeField] private GameObject _waveStartsObject;
		public GameObject waveStartsObject => _waveStartsObject;

		[SerializeField] private TextMeshProUGUI _moneyText;
		[SerializeField] private TextMeshProUGUI _livesText;
		[SerializeField] private TextMeshProUGUI _waveCountText;
		
		private void Awake()
		{
			if (_waveStartsObject == null) Debug.LogWarning("No waveStartsObject set for " + name);
			if (_moneyText == null) Debug.LogWarning("No moneyText set for " + name);
			if (_livesText == null) Debug.LogWarning("No livesText set for " + name);
			if (_waveCountText == null) Debug.LogWarning("No waveCountText set for " + name);
		}
		
		private void Start()
		{
			GameManager gameManager = GameManager.instance;
			gameManager.wallet.onMoneyChanged.AddListener(pMoney => _moneyText.text = pMoney.ToString());
			gameManager.onLivesUpdated.AddListener(pLives => _livesText.text = pLives.ToString());
			gameManager.onWaveCountUpdated.AddListener(pWave => _waveCountText.text = pWave.ToString());
			gameManager.RequestUpdates();
		}
	}
}