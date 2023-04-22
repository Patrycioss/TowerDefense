using System;
using UnityEngine;

namespace FiringBehaviours
{ 
	public class AOEFiringBehaviour : FiringBehaviour
	{
		[SerializeField] private int _aoeDamage = 1;
		[SerializeField] private int _levelsPerDamageUpgrade = 2;

		[SerializeField] private float _explosionEffectModifier = 2.0f;
		[SerializeField] private float _explosionEffectDuration = 0.2f;
		[SerializeField] private GameObject _explosionEffectPrefab;

		private GameObject _explosionEffect;

		private int _levelsAccumulated = 0;

		private void OnValidate()
		{
			if (_explosionEffectPrefab == null) Debug.LogWarning("No explosionEffectPrefab set for " + name);
		}

		protected override void OnLevelUp()
		{
			base.OnLevelUp();

			_levelsAccumulated++;

			if (_levelsAccumulated == _levelsPerDamageUpgrade)
			{
				_aoeDamage++;
				_levelsAccumulated = 0;
			}
		}

		protected override void Start()
		{
			base.Start();
			_explosionEffectPrefab.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(_range + _explosionEffectModifier, _range + _explosionEffectModifier, 1);
		}

		protected override void Fire()
		{
			_explosionEffect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
			
			foreach (GameObject enemy in enemiesInRange)
			{
				enemy.GetComponentInChildren<Enemy>().vitality.Damage(_aoeDamage);
			}
			
			simpleTimer.StartTimer(_explosionEffectDuration, () => Destroy(_explosionEffect));
		}		
	}
}