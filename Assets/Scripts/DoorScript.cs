using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
  // Start is called before the first frame update

  //public UnityEvent DoorOpen = new UnityEvent();
  //public UnityEvent DoorClose = new UnityEvent();
  public Sprite openDoor;
  public Sprite closedDoor;
  public bool isOpen = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    if (isOpen)
    {
      gameObject.GetComponent<SpriteRenderer>().sprite = openDoor;
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    else
    {
      gameObject.GetComponent<SpriteRenderer>().sprite = closedDoor;
      gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
  }

  public void Open()
  {
    isOpen = true;
  }

  public void Close()
  {
    isOpen = false;
  }
}
