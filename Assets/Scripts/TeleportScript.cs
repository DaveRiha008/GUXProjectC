using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
  // Start is called before the first frame update
  public TeleportScript DestinationTeleport;
  public enum Direction { UP, DOWN, LEFT, RIGHT};
  public Direction arrivalDirection = Direction.DOWN;

  private Vector3 destination;
    void Start()
    {
    switch (DestinationTeleport.arrivalDirection)
    {
      case Direction.UP:
        destination = DestinationTeleport.transform.position + Vector3.up;
        break;
      case Direction.DOWN:
        destination = DestinationTeleport.transform.position + Vector3.down;
        break;
      case Direction.LEFT:
        destination = DestinationTeleport.transform.position + Vector3.left;
        break;
      case Direction.RIGHT:
        destination = DestinationTeleport.transform.position + Vector3.right;
        break;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    PlayerController player;
    if(collision.gameObject.TryGetComponent(out player))
    {
      if (player.recentlyTeleported)
      {
        player.StopMovingAndReturn();
        return;
      }
      StartCoroutine(player.StartTeleportCooldown(0.2f));
      player.originalPosition = destination;
      player.StopMovingAndReturn();
      player.transform.position = destination;
    }
  }

  // Update is called once per frame
  void Update()
    {
        
    }
}
