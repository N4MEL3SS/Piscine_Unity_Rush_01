using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillZipper : MonoBehaviour
{
	public uint Damage = 100;
	public uint TimeAction = 3;

	private ParticleSystem ps;

	void Start()
	{
		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Stop();
		var main = ps.main;
		main.duration = TimeAction;
		ps.Play();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<EnemyLogic>())
			other.gameObject.GetComponent<EnemyLogic>().TakeDamage(Damage);
	}
}
