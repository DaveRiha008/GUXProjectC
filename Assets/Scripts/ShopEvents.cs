using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEvents : MonoBehaviour
{
    public GameObject player;

    public void GrantJumpAbility()
    {
        player.GetComponent<PlayerController>().canJumpOverPits = true;
    }

    public void GrantSkin(Sprite skin)
    {
        player.GetComponent<SpriteRenderer>().sprite = skin;
    }

    public void GrantPullAbility()
    {
        player.GetComponent<PlayerController>().canPullBoxes = true;
    }
}
