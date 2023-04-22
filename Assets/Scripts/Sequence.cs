using System;
using System.Collections.Generic;
using GameInformation;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class Sequence
{
	[Tooltip("Duration of the sequence in seconds")]
	[SerializeField] private float _duration;
	
	[Tooltip("Interval between spawns in seconds")] 
	[SerializeField] private float _spawnInterval = 1;
	
	[SerializeField] private List<EnemyCountPair> _enemies;
		
	public float duration => _duration;
	public float spawnInterval => _spawnInterval;
	
	public List<Enemy> GetEnemies()
	{
		List<Enemy> sequence = new List<Enemy>();

		foreach (EnemyCountPair enemyCountPair in _enemies)
			for (int i = 0; i < enemyCountPair._count; i++)
				sequence.Add(GameManager.instance.enemies[(int)enemyCountPair._enemy].GetComponentInChildren<Enemy>());

		return sequence;
	}

	[Serializable]
	private class EnemyCountPair
	{
		public EnumToEnemy _enemy;
		public int _count;
		
		[Serializable]
		public enum EnumToEnemy
		{
			Normal,
			Fast,
			Tank,
		}
	}
}