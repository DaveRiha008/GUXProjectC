using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public int optionalNPCsSaved;
    public int shopkeeperSaved;

    public enum State {defaultState, shopkeeperSaved, allNpcsSaved};

    public State gameProgress;

    private void Start()
    {
        gameProgress = State.defaultState;
    }

    public void SaveNPC()
    {
        optionalNPCsSaved++;
        if(optionalNPCsSaved == 3) gameProgress = State.allNpcsSaved;
    }

}