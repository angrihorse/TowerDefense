using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBaseAttribute]
public class MortarTower : MonoBehaviour
{
	[Range(1.5f, 10.5f)]
	public float range;

	[Range(0.5f, 2f)]
	public float shotsPeriod = 1f;
	float timeSinceLastShot;

	public Shell shell;
	public LayerMask enemyLayer;

	Transform target;

	bool targetOutOfRange => (target.position - transform.position).sqrMagnitude > range * range;

	void Update()
	{
		timeSinceLastShot += Time.deltaTime;
		if (target == null || targetOutOfRange) {
			AcquireTarget();
		} else if (timeSinceLastShot > shotsPeriod){
			Shoot();
		}
	}

	void Shoot() {
		// shootAnimation.Play();
		Shell shellInstance = Instantiate(shell, transform.position, Quaternion.identity);
		shellInstance.Launch(target.position);
		timeSinceLastShot = 0;
	}

	void AcquireTarget() {
		Collider[] targets = Physics.OverlapSphere(transform.position, range, enemyLayer);
		if (targets.Length > 0) {
			target = targets[0].transform;
		}
	}
}
