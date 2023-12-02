using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollisionDetect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If it collides with the player, put the player as a child object, then the player goes with the platform
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Remove the player as a child object
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.transform.SetParent(null);
        }
    }
}
