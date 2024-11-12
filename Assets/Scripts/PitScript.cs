using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerController player;
    MovableCrateScript crate;
    if (collision.gameObject.TryGetComponent(out player))
    {
      if (player.canJumpOverPits)
      {
        if (!player.jumpedOverPit)
        {
          JumpOverPit(player);
          player.jumpedOverPit = true;
        }
        else
          StartCoroutine(PlayerFade(player));
      }
      else
        StartCoroutine(PlayerFade(player));
    }
    if (collision.gameObject.TryGetComponent(out crate))
    {
      crate.gameObject.GetComponent<BoxCollider2D>().enabled = false;
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    PlayerController player;
    MovableCrateScript crate;
    if (collision.gameObject.TryGetComponent(out player))
    {
      if (player.canJumpOverPits)
      {
        if (!player.jumpedOverPit)
        {
          JumpOverPit(player);
          player.jumpedOverPit = true;
        }
        else
          StartCoroutine(PlayerFade(player));
      }
      else
        StartCoroutine(PlayerFade(player));
    }
    if (collision.gameObject.TryGetComponent(out crate))
    {
      crate.gameObject.GetComponent<BoxCollider2D>().enabled = false;
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
  }
  private IEnumerator PlayerFade(PlayerController player, float duration = 0.5f)
  {
    float elapsed = 0f;
    Vector3 origPosition = player.originalPosition;
    Color origColor = player.gameObject.GetComponent<SpriteRenderer>().color;

    while (elapsed <= duration)
    {
      player.transform.position = transform.position;
      Color color = origColor;
      color.a =  origColor.a-origColor.a*(elapsed / duration);
      player.gameObject.GetComponent<SpriteRenderer>().color = color;
      elapsed += Time.deltaTime;
      yield return null;
    }
    player.gameObject.GetComponent<SpriteRenderer>().color = origColor;
    player.StopMovingAndReturn();
    player.transform.position = origPosition;
    yield return null;
  }

  private void JumpOverPit(PlayerController player)
  {
    player.targetPosition += player.lastDirection;
  }

}
