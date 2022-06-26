using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShield : MonoBehaviour
{
	public uint Damage = 10;
	public uint TimeAction = 5;
	public uint SpeedRotate = 500;

	private ParticleSystem ps;

	void Start()
	{
		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Stop();
		var main = ps.main;
		main.duration = TimeAction;
		ps.Play();
	}

	private void Update()
	{
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, SpeedRotate * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Fire trigger");
		if (other.gameObject.GetComponent<EnemyLogic>())
			other.gameObject.GetComponent<EnemyLogic>().TakeDamage(Damage);
	}
}
