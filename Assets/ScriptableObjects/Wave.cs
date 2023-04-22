using System;
using System.Collections.Generic;
using System.Linq;
using GameInformation;
using Time;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Wave", order = 0)]
	public class Wave : ScriptableObject
	{

		
		[SerializeField] private List<Sequence> _waveSequence = new();
		
		private Queue<Sequence> _sequenceQueue;
		private Queue<Enemy> _enemyQueue;

		private Action<Enemy> _enemyReceiver;
		private Action _waveFinishCallback;

		private GameObject _timerObject;
		private SimpleTimer _timer;

		public float GetDurationSeconds() => _waveSequence.Sum(pSequence => pSequence.duration);

		public void StartWave(Action<Enemy> pEnemyReceiver, Action pWaveFinishCallback)
		{
			_timer = GameManager.instance.timer;

			_enemyReceiver = pEnemyReceiver;
			_waveFinishCallback = pWaveFinishCallback;

			_sequenceQueue = new Queue<Sequence>(_waveSequence);
			NextSequence();
		}

		private void NextSequence()
		{
			if (_sequenceQueue.Count != 0)
			{
				Sequence sequence = _sequenceQueue.Dequeue();
				_enemyQueue = new Queue<Enemy>(sequence.GetEnemies());
				_timer.StartTimer(sequence.duration, NextSequence);

				_spawnInterval = sequence.spawnInterval;
				NextEnemy();
			}
			else
			{
				Destroy(_timerObject);
				_waveFinishCallback();
			}
		}

		
		private float _spawnInterval;
		private void NextEnemy()
		{
			if (_enemyQueue.Count == 0) return;
			_enemyReceiver(_enemyQueue.Dequeue());
			_timer.StartTimer(_spawnInterval, NextEnemy);
		}
	}
}