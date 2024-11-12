using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHandeller : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D()
    {
        player.AddCoinToInventory();
        Destroy(gameObject);
    }
}
