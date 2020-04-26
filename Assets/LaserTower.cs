using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBaseAttribute]
public class LaserTower : MonoBehaviour
{
	[Range(1.5f, 10.5f)]
	public float range;
	float sqrRange;

	public float damagePerSecond;
	public LayerMask enemyLayer;

	public Transform turret;
	public Transform laser;

	Enemy target;

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(transform.position, range);
		if (target != null) {
			Gizmos.color = Color.yellow;
	        Gizmos.DrawWireSphere(target.transform.position, 0.5f);
		}
	}

	void CalibrateLaser() {
		Vector3 offset = target.transform.position - turret.position;
		float distance = offset.magnitude;

		laser.localScale = new Vector3(laser.localScale.x, laser.localScale.y, distance);
		laser.transform.position = turret.position + offset / 2;
		laser.LookAt(target.transform.position);
	}

	bool targetOutOfRange => (target.transform.position - transform.position).sqrMagnitude > range * range;

    // Update is called once per frame
    void Update()
    {
		if (target == null || targetOutOfRange) {
			laser.gameObject.SetActive(false);
			AcquireTarget();
		} else {
			laser.gameObject.SetActive(true);
			turret.LookAt(target.transform.position);
			target.Damage(damagePerSecond * Time.deltaTime);
			CalibrateLaser();
		}
    }

	void AcquireTarget() {
		Collider[] targets = Physics.OverlapSphere(transform.position, range, enemyLayer);
		if (targets.Length > 0) {
			target = targets[0].transform.parent.GetComponent<Enemy>();
		}
	}
}
