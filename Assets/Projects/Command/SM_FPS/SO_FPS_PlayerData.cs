using UnityEngine;

[CreateAssetMenu(fileName = "SO_FPS_PlayerData", menuName = "Scriptable Objects/SO_FPS_PlayerData")]
public class SO_FPS_PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public int maxJumpCount = 1;
    public float gravity = 9.81f;
    public float fallMultiplier = 2.5f;
    public float maxFallSpeed = 50f;
    public float airControlSpeed = 2.5f;

    //[Header("Crouch")]
    //public float crouchHeight = 1f;
    //public float standHeight = 2f;
    //public float crouchTransitionSpeed = 5f;

    [Header("Camera / Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public float minLookAngle = -80f;
    //public float cameraFOV = 60f;
    //public float runFOV = 75f;
    //public float fovTransitionSpeed = 5f;

    [Header("Shoot")]
    public float shootDistance = 15f;
    public GameObject explosionPrefab;

    //[Header("Health")]
    //public float maxHealth = 100f;
    //public float regenRate = 5f;
    //public float regenDelay = 3f;

    //[Header("Stamina")]
    //public float maxStamina = 100f;
    //public float staminaDrainRate = 20f;
    //public float staminaRegenRate = 10f;
    //public float staminaRegenDelay = 2f;
}
