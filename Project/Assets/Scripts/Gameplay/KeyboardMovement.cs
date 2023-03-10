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
    public Transform weaponDirectionTransform;
    private Quaternion initialWeaponRotation;
    public Transform projectileSpawnPos;
    public LayerMask pointerRaycastLayerMask;
    private Vector3 lastInputDirection;
    public float raycastRange = 20;
    public StatEvaluator shootInterval;
    public float shootTime = 0;
    public StatEvaluator shootPrecision;
    public StatEvaluator projectilePerShot;
    public float verticalPrecision = 2;
    public Transform projectilePrefab;
    private Vector3 shootDirection = Vector3.right;
    public float shootingMovementSlowdown = 0.5f;
    private Health health;
    public PostProcessEffect hurtEffect;
    public AnimatedSprite animatedSprite;
    public SpriteRenderer weaponSprite;
    public ProceduralAnimationHandler weaponProceduralAnimHandler;
    public ProceduralEffect shootProceduralEffectRight;
    public ProceduralEffect shootProceduralEffectLeft;
    public ScreenshakeEffect shootScreenshakeEffect;
    public float spriteAngle = 30;
    public Vector3 raycastOffset;
    public AnimationCurve weaponScaleAdjustCurve;
    

    void Start()
    {
        movementController = GetComponent<MovementController>();
        horizontalAction.Enable();
        verticalAction.Enable();
        dashAction.Enable();
        shootAction.Enable();
        health = GetComponent<Health>();
        health.hurtDelegate += (Ray ray) => {hurtEffect.Play();};
        initialWeaponRotation = weaponDirectionTransform.localRotation;
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

        animatedSprite.SelectAnim(movementController.inputDirection != Vector3.zero ? "Run" : "Idle");

        if(dashAction.WasPressedThisFrame())
        {
            movementController.Dash(lastInputDirection);
        }
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, pointerRaycastLayerMask))
        {
            Vector3 targetDirection = hit.point + raycastOffset - weaponDirectionTransform.position;
            targetDirection.y = 0;
            targetDirection = targetDirection.normalized;
            weaponDirectionTransform.localRotation = Quaternion.LookRotation(targetDirection, Vector3.up) * Quaternion.AngleAxis((targetDirection.x > 0 ? -90:90), Vector3.up) * Quaternion.AngleAxis(Mathf.Lerp(90, 45, Mathf.Abs(targetDirection.x)), Vector3.right);
            shootDirection = targetDirection;
            weaponTransform.localRotation = Quaternion.AngleAxis(targetDirection.x < 0 ? -90:90, Vector3.up);
            projectileSpawnPos.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            weaponDirectionTransform.localScale = weaponScaleAdjustCurve.Evaluate(Mathf.Abs(targetDirection.z)) * Vector3.one;
        }
        animatedSprite.flipX = shootDirection.x < 0;
        weaponSprite.flipX = shootDirection.x < 0;
        shootTime -= Time.deltaTime;
        if(shootAction.IsPressed() && shootTime <= 0)
        {
            shootTime = shootInterval.value;
            ScreenshakeHandler.instance.activeEffects.Add(new ScreenshakeEffect(shootScreenshakeEffect));
            if(shootDirection.x > 0)
                weaponProceduralAnimHandler.AddEffect(shootProceduralEffectRight);
            else
                weaponProceduralAnimHandler.AddEffect(shootProceduralEffectLeft);
            for(int i=0; i<projectilePerShot.value; i++)
                Instantiate(projectilePrefab, projectileSpawnPos.position + (Quaternion.AngleAxis(spriteAngle, Vector3.right) * Vector3.up - Vector3.up) * projectileSpawnPos.position.y, projectileSpawnPos.rotation * Quaternion.AngleAxis(Random.Range(-shootPrecision.value, shootPrecision.value), Vector3.up) * Quaternion.AngleAxis(Random.Range(-verticalPrecision, verticalPrecision), transform.right));
        }
        movementController.speedMultiplier = shootAction.IsPressed() ? shootingMovementSlowdown : 1;
    }
}
