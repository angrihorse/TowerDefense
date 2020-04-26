using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	public Transform target;
	public float speed;
	public float maxHeight;
	public float damage;
	public float explosionRadius;
	public LayerMask enemyLayer;
	public ParticleSystem explosion;

	public void Launch(Vector3 targetPos) {
		StartCoroutine(Move(targetPos));
	}

	IEnumerator Move(Vector3 targetPos) {
		Vector3	startPos = transform.position;
		Vector2 horizontalStartPos = new Vector2(startPos.x, startPos.z);
		Vector2 horizontalTargetPos = new Vector2(targetPos.x, targetPos.z);

		Vector2 horizontalPos;
		float verticalPos;

		float progress = 0;
		while (progress < 1) {
			progress += speed * Time.deltaTime;
			horizontalPos = Vector2.Lerp(horizontalStartPos, horizontalTargetPos, progress);
			verticalPos = startPos.y + maxHeight * OneToOneParabola(progress);
			transform.position = new Vector3(horizontalPos.x, verticalPos, horizontalPos.y);
			yield return null;
		}

		OnImpact();
	}

	void OnImpact() {
		Collider[] casualties = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
		foreach (Collider casualty in casualties) {
			Enemy enemy = casualty.transform.parent.GetComponent<Enemy>();
			enemy.Damage(damage);
		}

		if (explosion != null) {
			ParticleSystem explosionInstance = Instantiate(explosion, transform.localPosition, Quaternion.Euler(-90, 0, 0));
			Destroy(explosionInstance.gameObject, explosion.main.startLifetime.constant);
		}

		Destroy(gameObject);
	}

	float OneToOneParabola(float x) {
		return 4 * x * (1 - x);
	}
}
