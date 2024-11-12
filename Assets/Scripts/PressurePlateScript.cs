using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateScript : MonoBehaviour
{
  public bool isPressed = false;
  public Sprite pressedSprite;
  public Sprite releasedSprite;
  public UnityEvent triggerEvent;
  public UnityEvent releaseEvent;

  private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
    sr = gameObject.GetComponent<SpriteRenderer>();
    }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    isPressed = true;
    triggerEvent.Invoke();
    sr.sprite = pressedSprite;
  }
  private void OnTriggerExit2D(Collider2D collision)
  {
    { //Check if there is still something on the pressure plate
      List<Collider2D> colliders = new List<Collider2D>();
      gameObject.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), colliders);
      if (colliders.Count > 0) return;
    }
    isPressed = false;  
    releaseEvent.Invoke();
    sr.sprite = releasedSprite;
  }

  // Update is called once per frame
  void Update()
  {
        
  }

}
