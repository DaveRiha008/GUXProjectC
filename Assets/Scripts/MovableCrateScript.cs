using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovableCrateScript : MonoBehaviour
{
  //for reset
  public Vector3 originalPosInRoom;
  public bool originalEnabled = true;

  //for movement
  private Vector3 originalPosition;
  public bool isMoving = false;
  public float timeToMove;
  private Coroutine currentMovingCoroutine;

  // Start is called before the first frame update
  void Start()
    {
      originalPosInRoom = transform.position;
    if (!originalEnabled)
    {
      gameObject.GetComponent<SpriteRenderer>().enabled = false;
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
  }
  private void OnCollisionEnter2D(Collision2D collision)
  {
    PlayerController playerController;
    if (collision.gameObject.TryGetComponent(out playerController))
    {
      if (isMoving) StopMovingAndReturn();
      timeToMove = playerController.timeToMove-0.05f;
      if (!TryMove(collision.transform.position)) playerController.StopMovingAndReturn();
    }
  }
  public bool TryMove(Vector3 fromPos)
  {
    originalPosition = gameObject.transform.position;
    if (isMoving) return false;
    Vector3 direction = originalPosition - fromPos;
    direction = NormalizeDirection(direction);
    Vector2 targetPosition = originalPosition + direction;

    var smt = Physics2D.OverlapCircleAll(targetPosition, 0.1f);
    bool canGo = true;
    foreach (var obj in smt)
    {
      if (obj.gameObject.CompareTag("Colliding decors")||obj.gameObject.CompareTag("Movable object")||obj.gameObject.CompareTag("Teleport")) canGo = false;
    }
    if (canGo)
    {
      currentMovingCoroutine = StartCoroutine(Move(gameObject.transform.position, targetPosition));
      return true;
    }
    else return false;
  }

  private IEnumerator Move(Vector3 originalPosition, Vector3 targetPosition)
  {
    isMoving = true;

    float elapsedTime = 0f;

    while (elapsedTime < timeToMove)
    {
      transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / timeToMove));
      elapsedTime += Time.deltaTime;
      yield return null;
    }

    transform.position = targetPosition;

    isMoving = false;
  }

  public void StopMovingAndReturn()
  {
    if (!isMoving) return;
    transform.position = originalPosition;
    StopCoroutine(currentMovingCoroutine);
    isMoving = false;
  }


  Vector3 NormalizeDirection(Vector3 input)
  {
    if (input.x > 0.4) return Vector3.right;
    if (input.x < -0.4) return Vector3.left;
    if (input.y > 0.4) return Vector3.up;
    if (input.y < -0.4) return Vector3.down;
    else return Vector3.zero;


  }
  // Update is called once per frame
  void Update()
    {
        
    }

  public void Materialize()
  {
    gameObject.GetComponent<SpriteRenderer>().enabled = true;
    gameObject.GetComponent<BoxCollider2D>().enabled = true;
  }
  public void Dematerialize()
  {
    gameObject.GetComponent<SpriteRenderer>().enabled = false;
    gameObject.GetComponent<BoxCollider2D>().enabled = false;
  }
}
