using FiringBehaviours.Bullet;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FiringBehaviours
{
	public class BulletFiringBehaviour : FiringBehaviour
	{

		[SerializeField] private Tile _projectileTile;
		[SerializeField] private GameObject _projectileSpawnPoint;

		[SerializeField] private BulletInformation _bulletInformation;
		[SerializeField] private int _levelsPerDamageUpgrade = 2;
		[SerializeField] private float _speedIncreasePerLevel = 0.1f;
		
		private GameObject _target;
		
		private int _levelsAccumulated;


		protected override void OnLevelUp()
		{
			base.OnLevelUp();
			
			_levelsAccumulated++;

			_bulletInformation._speed += _speedIncreasePerLevel;
			
			if (_levelsAccumulated == _levelsPerDamageUpgrade)
			{
				_bulletInformation._projectileDamage++;
				_levelsAccumulated = 0;
			}
		}


		private void Awake()
		{
			if (_projectileTile == null) Debug.LogWarning("No projectileTile set for " + name);
			if (_projectileSpawnPoint == null) Debug.LogWarning("No projectileSpawnPoint set for " + name);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			_target = enemiesInRange.Count == 0? null : enemiesInRange[0];
		}

		protected override void Fire()
		{
			if (_target == null) return;

			GameObject bullet = CreateBulletObject();
			GiveBulletBehaviour(bullet);
		}

		private GameObject CreateBulletObject()
		{
			GameObject bullet = new("Bullet") {
				transform = {
					position = _projectileSpawnPoint.transform.position
				}
			};
			
			bullet.AddComponent<SpriteRenderer>().sprite = _projectileTile.sprite;
			bullet.transform.parent = tower.transform;

			return bullet;
		}

		private void GiveBulletBehaviour(GameObject pBullet)
		{
			BulletBehaviour bulletBehaviour = pBullet.AddComponent<BulletBehaviour>();
			bulletBehaviour.Inform(_bulletInformation, GetUDirection(_target));
		}
		
		
		
		private Vector3 GetUDirection(GameObject pTarget)
		{
			return (pTarget.transform.position - tower.gameObject.transform.position).normalized;
		}
	}
}