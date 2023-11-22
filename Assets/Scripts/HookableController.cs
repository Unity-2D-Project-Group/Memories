using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookableController : MonoBehaviour
{
    public enum MoveDirection { Left = -1,  Right = 1 };

    public MoveDirection _moveDirection = MoveDirection.Left;
}
