using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTimeCountingScript : MonoBehaviour
{
	[SerializeField]
	int id = 0;
	bool playerInside = false;
    // Update is called once per frame
    void Update()
    {
		if (playerInside)
			TimeSpentCounter.AddTime(id, Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<PlayerController>(out _))
		{
			playerInside = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<PlayerController>(out _))
		{
			playerInside = false;
		}
	}
}
