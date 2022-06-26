using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public uint Damage = 50;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<EnemyLogic>())
			other.gameObject.GetComponent<EnemyLogic>().TakeDamage(Damage);
	}
}
