using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookableController : MonoBehaviour
{
    //Just a script to define the direction that the player can pull the object
    public enum MoveDirection { Left = -1,  Right = 1 };

    public MoveDirection _moveDirection = MoveDirection.Left;
}
