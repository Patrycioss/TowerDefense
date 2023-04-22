using System;
using GameInformation;
using Time;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Enemy : MonoBehaviour
{
	[SerializeField] private Tile _tileAsset;
	public Tile tile => _tileAsset;
	
	[SerializeField] private float _speed;
	public float speed => _hasDebuff? _speed * 0.5f : _speed;

	[SerializeField] private int _worth;
	public int worth => _worth;
	
	[SerializeField] private int _maxHealth;

	public Vitality vitality { get; private set; }

	[SerializeField] private TextMeshProUGUI _healthText;
	[SerializeField] private MoneyPickUp _moneyPickUp;


	private bool _hasDebuff;
	
	private void Awake()
	{
		vitality = new Vitality(_maxHealth);
		vitality.OnDeathEvent += OnDeath;

		if (_healthText == null) Debug.LogWarning("No health text for " + name);
		else vitality.OnHealthChangedEvent += pHealth => _healthText.text = pHealth + " hp";

		vitality.health = _maxHealth;
		
		GetComponent<SpriteRenderer>().sprite = tile.sprite;
	}
	
	private void OnDeath()
	{
		_moneyPickUp.Spawn(worth);
		GameManager.instance.wallet.AddMoney(worth);
		
		GameManager.instance.spawner.RemoveEnemy(gameObject);
		Destroy(transform.parent.gameObject);
	}

	public void ApplyDebuff()
	{
		_hasDebuff = true;

		SimpleTimer simpleTimer = gameObject.GetComponent<SimpleTimer>();
		if (simpleTimer != null) Destroy(simpleTimer);

		simpleTimer = transform.AddComponent<SimpleTimer>();
		simpleTimer.StartTimer(2, ()=>
		{
			_hasDebuff = false;
			Destroy(simpleTimer);
		});
	}
}