using System.Collections.Generic;
using Event;
using GameInformation;
using Time;
using UnityEngine;

namespace FiringBehaviours
{
	[RequireComponent(typeof(Tower))]
	[RequireComponent(typeof(SimpleTimer))]
	public class FiringBehaviour : MonoBehaviour
	{
		[SerializeField] protected float _range = 3.2f;
		[SerializeField] private float _fireDelay = 1.0f;
		[SerializeField] private float _rangeIncreasePerLevel = 0.2f;
		[SerializeField] private float _fireDelayDecreasePerLevel = 0.1f;
		
		protected SimpleTimer simpleTimer;

		protected Tower tower;
		protected List<GameObject> enemiesInRange { get; } = new();
		protected CustomEvent onTimerComplete { get; } = new();
		
		
		protected virtual void Start()
		{
			tower = transform.GetComponent<Tower>();
			tower.onLevelUp.AddListener(OnLevelUp);
			simpleTimer = GetComponent<SimpleTimer>();
			simpleTimer.StartTimer(_fireDelay, OnTimerEnd);
		}

		protected virtual void OnLevelUp()
		{
			_range += _rangeIncreasePerLevel;
			_fireDelay -= _fireDelayDecreasePerLevel;
		}

		private void OnTimerEnd()
		{
			onTimerComplete.Raise();
			if (CanFire()) Fire();
			simpleTimer.StartTimer(_fireDelay, OnTimerEnd);
		}

		protected virtual void Fire()
		{
			Debug.Log("Firing base FiringBehaviour, maybe you want to override this?");
		}
		
		protected virtual bool CanFire()
		{
			return enemiesInRange.Count != 0;
		}

		private void OnDrawGizmos()
		{
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) return;
			#endif
			Gizmos.DrawSphere(transform.position, _range);
		}

		protected virtual void FixedUpdate()
		{
			AddNewEnemiesInRange();
			RemoveIncorrectEnemiesInRange(EnemiesNotInRange());
		}

		private void AddNewEnemiesInRange()
		{
			foreach (GameObject enemyObject in GameManager.instance.spawner.enemies)
			{
				if (enemyObject == null) continue;
				
				bool enemiesInRangeContainsEnemyObject = enemiesInRange.Contains(enemyObject);
				
				if (Vector2.Distance(enemyObject.transform.position, tower.gameObject.transform.position) <= _range)
				{
					if (enemiesInRangeContainsEnemyObject) continue;
					enemiesInRange.Add(enemyObject);
				}
				else if (enemiesInRangeContainsEnemyObject) 
					enemiesInRange.Remove(enemyObject);
			}
		}
		
		private List<GameObject> EnemiesNotInRange()
		{
			List<GameObject> enemiesNotInRange = new();
			foreach (GameObject enemy in enemiesInRange)
			{

				if (enemy == null ||
				    Vector2.Distance(enemy.transform.position, tower.gameObject.transform.position) > _range)
				{
					enemiesNotInRange.Add(enemy);
				}
			}

			return enemiesNotInRange;
		}

		private void RemoveIncorrectEnemiesInRange(List<GameObject> pEnemiesNotInRange)
		{
			foreach (GameObject enemy in pEnemiesNotInRange) 
				enemiesInRange.Remove(enemy);
		}
	}
}