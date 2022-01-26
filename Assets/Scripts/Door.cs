using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Enter");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        print("Exit");
    }
}
