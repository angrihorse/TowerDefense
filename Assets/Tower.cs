using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBaseAttribute]
public class Tower : MonoBehaviour
{
	[Range(1.5f, 10.5f)]
	public float range;
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

	void OnEnable() {
		// Calibrate laser.
		Vector3 laserPosition = laser.transform.localPosition;
		Vector3 laserScale = laser.transform.localScale;

		laserPosition.z = range;
		laserScale.z = range * 2;

		laser.transform.localPosition = laserPosition;
		laser.transform.localScale = laserScale;
	}

	bool targetOutOfRange => Vector3.Distance(transform.position, target.transform.position) > range;

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
		}
    }

	void AcquireTarget() {
		Collider[] targets = Physics.OverlapSphere(transform.position, range, enemyLayer);
		if (targets.Length > 0) {
			target = targets[0].transform.parent.GetComponent<Enemy>();
		}
	}
}
