using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTimeCountingScript : MonoBehaviour
{
	[SerializeField]
	public int id = 0;
	bool playerInside = false;

	PlayerController player;
    // Update is called once per frame
    void Update()
    {
		if (playerInside && player != null && player.isActive)
			TimeSpentCounter.AddTime(id, Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<PlayerController>(out player))
		{
			playerInside = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<PlayerController>(out player))
		{
			playerInside = false;
		}
	}
}
