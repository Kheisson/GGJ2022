using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Consts

    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    #endregion

    #region Variables

    [SerializeField] private float _speed = 10f;

    #endregion
    
    #region Methods

    private void Update()
    {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        var horizontalInput = Input.GetAxis(HorizontalInput) * Time.deltaTime;
        var verticalInput = Input.GetAxis(VerticalInput) * Time.deltaTime;
        transform.Translate(new Vector3(horizontalInput,verticalInput,transform.position.z) * _speed);
    }

    #endregion
}