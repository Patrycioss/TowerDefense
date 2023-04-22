using System;
using GameInformation;
using UnityEngine;

namespace FiringBehaviours.Bullet
{
	[Serializable]
	public struct BulletInformation
	{
		public BulletType _bulletType;
		public float _speed;
		public int _projectileDamage;
		public float _projectileHitRadius;
	}
	
	[Serializable]
	public enum BulletType
	{
		Normal,
		Debuff,
	}

	public class BulletBehaviour : MonoBehaviour
	{
		private bool _informed;
		private BulletInformation _bullet;
		private Vector3 _direction;

		public void Inform(BulletInformation pBulletInformation, Vector3 pDirection)
		{
			_bullet = pBulletInformation;
			_direction = pDirection;
			_informed = true;
		}
			
		private void FixedUpdate()
		{
			if (!_informed) return;


			Camera cam = Camera.main!;
			Vector3 pos = cam.WorldToViewportPoint(transform.position);
			Rect cameraRect = cam!.rect;
			
			if (pos.x < cameraRect.xMin || 
			    pos.x > cameraRect.xMax || 
			    pos.y < cameraRect.yMin || 
			    pos.y > cameraRect.yMax) Kill();


			transform.Translate(_direction * _bullet._speed);
				
			foreach (GameObject enemyObject in GameManager.instance.spawner.enemies)
			{
				if (enemyObject == null) continue;
				if (Vector3.Distance(transform.position, enemyObject.transform.position) < _bullet._projectileHitRadius)
				{
					Enemy enemy = enemyObject.GetComponentInChildren<Enemy>();
					
					switch (_bullet._bulletType)
					{
						case BulletType.Normal:
							enemy.vitality.Damage(_bullet._projectileDamage);
							break;
						case BulletType.Debuff:
							enemy.ApplyDebuff();
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					
					Kill();
				}
			}
		}

		private void Kill()
		{
			Destroy(gameObject);
			Destroy(this);
		}
	}
}