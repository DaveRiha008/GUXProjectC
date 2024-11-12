using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyScript : MonoBehaviour
{
  // Start is called before the first frame update
  public UnityEvent OnPickup;
    void Start()
    {
        
    }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    PlayerController player;
    if(collision.gameObject.TryGetComponent(out player))
    {
      OnPickup.Invoke();
      gameObject.SetActive(false);
    }
  }

  // Update is called once per frame
  void Update()
    {
        
    }
}
