using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  //for camera
  //[HideInInspector]
  public bool recentlyTeleported = false;

  //for movement
  public Coroutine currentMovingCoroutine;
  public bool isMoving;
  public Vector3 originalPosition, targetPosition;
  public float timeToMove = 0.2f;

  public Vector3 lastDirection;

  public Tilemap collisionObjects;

  //for reset purposes
  public CameraChanger currentRoom;
  public Vector3 firstPositionInRoom; //to remember where player entered a room
  private Coroutine resetCoroutine;

  //for special abilities
  public bool canJumpOverPits = false;
  public bool jumpedOverPit;

  public bool canPullBoxes = false;
  public bool isPullingCrate = false;
  public MovableCrateScript pulledCrate;


  //for coins and inventory
  public int coinsInInventory;
  public GameObject inventoryBox;
  private Text inventoryCountText;
  private bool firstTimePickingUpCoins;

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(SetFirstPos());

    canJumpOverPits = false;
    jumpedOverPit = false;

    coinsInInventory = 0;
    inventoryCountText = inventoryBox.GetComponent<RectTransform>().Find("Coin Count").GetComponent<Text>();
    firstTimePickingUpCoins = true;
    inventoryBox.SetActive(false);
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
  }

  private void HandleInput()
  {
    if (isMoving == false)
      jumpedOverPit = false;

    if (Input.GetKey(KeyCode.W) && !isMoving)
    {
      currentMovingCoroutine = StartCoroutine(MovePlayer(Vector3.up));
      lastDirection = Vector3.up;
    }

    if (Input.GetKey(KeyCode.S) && !isMoving)
    {
      currentMovingCoroutine = StartCoroutine(MovePlayer(Vector3.down));
      lastDirection = Vector3.down;
    }

    if (Input.GetKey(KeyCode.A) && !isMoving)
    {
      currentMovingCoroutine = StartCoroutine(MovePlayer(Vector3.left));
      lastDirection = Vector3.left;
    }

    if (Input.GetKey(KeyCode.D) && !isMoving)
    {
      currentMovingCoroutine = StartCoroutine(MovePlayer(Vector3.right));
      lastDirection = Vector3.right;
    }

    if (Input.GetKeyDown(KeyCode.R))
      resetCoroutine = StartCoroutine(Reset());

    if (Input.GetKeyUp(KeyCode.R))
      StopCoroutine(resetCoroutine);

    if (Input.GetKeyDown(KeyCode.E) && canPullBoxes)
      isPullingCrate = true;
    if(Input.GetKeyUp(KeyCode.E))
      isPullingCrate = false;
  }

  public IEnumerator MovePlayer(Vector3 direction)
  {
    isMoving = true;

    float elapsedTime = 0f;
    originalPosition = transform.position;
    targetPosition = originalPosition + direction;

    var smt = Physics2D.OverlapCircleAll(targetPosition, 0.1f);
    bool canGo = true;
    bool canPullCrate = true;
    foreach (var obj in smt)
    {
      if (obj.gameObject.CompareTag("Colliding decors")) canGo = false;
      if (obj.gameObject.CompareTag("Pit")) canPullCrate = false;
    }
    if (canGo)
    {
      if(canPullCrate)CheckCratesToPull(originalPosition, -direction);

      while (elapsedTime < timeToMove)
      {
        transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / timeToMove));
        elapsedTime += Time.deltaTime;
        yield return null;
      }

      pulledCrate = null;
      transform.position = targetPosition;
    }

    isMoving = false;
  }

  public void StopMovingAndReturn()
  {
    if (!isMoving) return;
    if (pulledCrate != null) pulledCrate.StopMovingAndReturn();
    StopCoroutine(currentMovingCoroutine);
    transform.position = originalPosition;
    isMoving = false;
  }

  public void ChangeFirstPosInRoom()
  {
    firstPositionInRoom = targetPosition;
  }

  IEnumerator Reset()
  {
    yield return new WaitForSeconds(1.2f);

    transform.position = firstPositionInRoom;
    currentRoom.ResetRoom();
  }

  IEnumerator SetFirstPos()
  {
    yield return new WaitForSeconds(0.1f);
    firstPositionInRoom = transform.position;
  }

  public void AddCoinToInventory()
  {
    coinsInInventory++;
    UpdateInventoryWindow();
  }

  public void RemoveCoinsFromInventory(int coinsLost)
  {
    coinsInInventory -= coinsLost;
    UpdateInventoryWindow();
  }

  void UpdateInventoryWindow()
  {
    if (firstTimePickingUpCoins)
    {
      inventoryBox.SetActive(true);
      firstTimePickingUpCoins = false;
    }

    inventoryCountText.text = coinsInInventory.ToString();
  }
  void CheckCratesToPull(Vector3 playerPos, Vector3 crateDirection)
  {
    if (!isPullingCrate) return;
    var colliders = Physics2D.OverlapCircleAll(playerPos + crateDirection, 0.1f);
    foreach (var obj in colliders)
    {
      if (obj.TryGetComponent(out pulledCrate))
      {
        pulledCrate.timeToMove = timeToMove;
        pulledCrate.TryMove(playerPos + 2 * crateDirection);
      }
    }
  }

  public IEnumerator StartTeleportCooldown(float seconds = 0.5f)
  {
    recentlyTeleported = true;
    yield return new WaitForSeconds(seconds);
    recentlyTeleported = false;
  }
}
