using System;
using System.Collections.Generic;
using GameInformation;
using Time;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Shop))]
	[RequireComponent(typeof(SimpleTimer))]
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] private GameObject _shopButton;
		public GameObject shopButton => _shopButton;
		
		[SerializeField] private GameObject _cantAffordMessage;
		[SerializeField] private List<ShopPanel> _shopPanels;
		
		private List<Button> _towerButtons;
		private SimpleTimer _cantAffordTimer;
		
		public void Open()
		{
			_cantAffordMessage.SetActive(false);
			gameObject.SetActive(true);
		}

		public void Close()
		{
			_cantAffordMessage.SetActive(false);
			gameObject.SetActive(false);
			
		}

		private void OnValidate()
		{
			if (_shopButton == null) Debug.LogWarning("No shopButton set for " + name);
			if (_cantAffordMessage == null) Debug.LogWarning("No cantAffordMessage set for " + name);
			if (_shopPanels.Count == 0) Debug.LogWarning("No shop panels set for " + name);
		}

		private void Awake()
		{
			_cantAffordTimer = GetComponent<SimpleTimer>();
		}

		private void Start()
		{
			transform.AddComponent<SimpleTimer>();
			UpdatePanels();
		}

		/// <summary>
		/// Updates the shop panels to reflect the towerData stored in the GameManager
		/// </summary>
		public void UpdatePanels()
		{
			for (int i = 0; i < _shopPanels.Count; i++) 
				_shopPanels[i].Load(GameManager.instance.towers[i]);
		}
		
		/// <summary>
		/// Shows or hides the can't afford message
		/// </summary>
		/// <param name="pShow"> show or not</param>
		public void ShowCantAffordMessage(bool pShow)
		{
			_cantAffordMessage.SetActive(pShow);
		}

		/// <summary>
		/// Shows can't afford message for a specified duration in seconds
		/// </summary>
		/// <param name="pDuration"> duration in seconds </param>
		public void ShowCantAffordMessageFor(int pDuration)
		{
			ShowCantAffordMessage(true);

			_cantAffordTimer.StartTimer(pDuration, () => ShowCantAffordMessage(false));
		}
	}
}