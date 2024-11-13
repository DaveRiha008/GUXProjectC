using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonResetScript : MonoBehaviour
{
    [SerializeField]
    List<CameraChanger> dungeonRooms;
    [SerializeField]
    Vector3 resetPos;
	[SerializeField]
	PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetDungeon()
    {
		player.ChangeFirstPosInDungeon(transform.position + resetPos);
		foreach (var dungeon in dungeonRooms)
        {
            dungeon.ResetRoom();
        }
        //Debug.Log($"Changing player position to {transform.position + resetPos}");
    }
}
