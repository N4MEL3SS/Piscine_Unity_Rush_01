using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFire : MonoBehaviour
{
	public uint Damage = 10;
	public uint TimeAction = 3;
	public uint SpeedMove = 20;
	public GameObject Explosion;
	public GameObject Light;
	private bool exp = false;
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
		if (!exp)
			gameObject.transform.Translate(Vector3.forward * SpeedMove * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!exp)
		{
			exp = true;
			Destroy(Light, 0.1f);
			Instantiate(Explosion, gameObject.transform);
			if (other.gameObject.GetComponent<EnemyLogic>())
				other.gameObject.GetComponent<EnemyLogic>().TakeDamage(Damage);
		}	
	}

}
