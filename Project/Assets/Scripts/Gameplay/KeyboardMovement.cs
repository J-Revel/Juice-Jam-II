using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovement : MonoBehaviour
{
    public InputAction horizontalAction;
    public InputAction verticalAction;
    public InputAction dashAction;
    public InputAction shootAction;
    private MovementController movementController;
    public Transform weaponTransform;
    public LayerMask pointerRaycastLayerMask;
    private Vector3 lastInputDirection;
    public float raycastRange = 20;
    public float shootInterval = 0.1f;
    public float shootTime = 0;
    public float shootPrecision = 5;
    public Transform projectilePrefab;
    private Vector3 shootDirection = Vector3.right;
    public float shootingMovementSlowdown = 0.5f;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        horizontalAction.Enable();
        verticalAction.Enable();
        dashAction.Enable();
        shootAction.Enable();
    }

    void Update()
    {
        movementController.inputDirection = new Vector3(horizontalAction.ReadValue<float>(), 0, verticalAction.ReadValue<float>());
        if(movementController.inputDirection.x != 0 && !shootAction.IsPressed())
        {
            shootDirection = Vector3.right * Mathf.Sign(movementController.inputDirection.x);
        }
        if(movementController.inputDirection != Vector3.zero)
        {
            lastInputDirection = movementController.inputDirection;
        }
        if(dashAction.WasPressedThisFrame())
        {
            movementController.Dash(lastInputDirection);
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        weaponTransform.rotation = Quaternion.LookRotation(shootDirection, Vector3.up);
        shootTime -= Time.deltaTime;
        if(shootAction.IsPressed() && shootTime <= 0)
        {
            shootTime = shootInterval;
            Instantiate(projectilePrefab, weaponTransform.position, weaponTransform.rotation * Quaternion.AngleAxis(Random.Range(-shootPrecision, shootPrecision), Random.insideUnitSphere));
        }
        movementController.speedMultiplier = shootAction.IsPressed() ? shootingMovementSlowdown : 1;
    }
}
