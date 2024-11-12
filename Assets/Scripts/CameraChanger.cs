using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{

  private Camera gameCamera;

  //list of movables and other objects to reset
  public List<GameObject> objectsToReset;

  //interactable to hide after interaction
  public InteractableController npcToSave;

  // Start is called before the first frame update
  void Start()
  {
  gameCamera = Camera.main;
  }

  private bool isMoving = false;
  public float timeToMove = 0.4f;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerController player;
    if (collision.TryGetComponent(out player))
    {
      // move camera
      if (isMoving) StopAllCoroutines();
      Vector3 startPos = gameCamera.transform.position;
      Vector3 finalPos = gameObject.transform.position + new Vector3(0.001f, 0.001f, 0);
      if (player.recentlyTeleported) gameCamera.transform.position = finalPos;
      else StartCoroutine(MoveCamera(startPos, finalPos));

      //set reset position and current room for player
      player.ChangeFirstPosInRoom();
      player.currentRoom = gameObject.GetComponent<CameraChanger>();

    }
  }

  private void OnTriggerExit2D()
  {
    if(npcToSave != null && npcToSave.isSaved)
    {
      npcToSave.gameObject.SetActive(false);
      npcToSave.npcEvent.Invoke();
    }
  }

  private IEnumerator MoveCamera(Vector3 originalPosition, Vector3 targetPosition)
  {
    isMoving = true;

    float elapsedTime = 0f;

    while (elapsedTime < timeToMove)
    {
      gameCamera.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / timeToMove));
      elapsedTime += Time.deltaTime;
      yield return null;
    }

    transform.position = targetPosition;

    isMoving = false;
  }

  // Update is called once per frame
  void Update()
    {
        
    }

  public void ResetRoom()
  {
    foreach (GameObject resetObj in objectsToReset)
    {
      switch (resetObj.tag)
      {
        case "Movable object":
          MovableCrateScript crate;
          if (resetObj.TryGetComponent(out crate)) {
            bool origEnabled = crate.originalEnabled;
            resetObj.transform.position = crate.originalPosInRoom;
            if (origEnabled) crate.Materialize();
            else crate.Dematerialize();
          }
          break;

        case "Pit":
          resetObj.GetComponent<BoxCollider2D>().enabled = true;
          break;

        case "Door":
          break;

        default:
          break;
      }
    }
  }
}
