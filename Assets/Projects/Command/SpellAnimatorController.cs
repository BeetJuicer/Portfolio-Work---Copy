using UnityEngine;

/// <summary>
/// FPS Spell Animator Controller
///
/// Animator Parameters Required:
///   bool  IsCharging   — true while LMB held, charging the spell
///   bool  IsHolding    — true while RMB held, holding spell up/ready
///   trigger Fire       — fires when LMB released after charging
///   trigger CancelCharge — fires if charge is interrupted (e.g. RMB pressed mid-charge)
///   float ChargeTime   — 0..1 normalized charge progress (drive blend trees with this)
///
/// Suggested Animator State Machine:
///   Idle ──(IsHolding)──> Hold
///   Idle ──(IsCharging)─> Charge  (looping charge-up animation, use ChargeTime for blend)
///   Charge ──(Fire)─────> Fire ──> Idle  (Fire is a one-shot, transitions back auto)
///   Charge ──(CancelCharge)──> Idle
///   Hold ──(!IsHolding)──> Idle
/// </summary>
public class SpellAnimatorController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Charge Settings")]
    [Tooltip("How many seconds to fully charge the spell.")]
    public float maxChargeTime = 1.5f;

    [Tooltip("Minimum charge time before a Fire is allowed. Below this, charge is cancelled.")]
    public float minChargeToFire = 0.1f;

    // ─── Animator Parameter Hashes (faster than string lookup) ────────────────
    private static readonly int HashIsCharging   = Animator.StringToHash("IsCharging");
    private static readonly int HashIsHolding    = Animator.StringToHash("IsHolding");
    private static readonly int HashFire         = Animator.StringToHash("Fire");
    private static readonly int HashCancelCharge = Animator.StringToHash("CancelCharge");
    private static readonly int HashChargeTime   = Animator.StringToHash("ChargeTime");

    // ─── State ─────────────────────────────────────────────────────────────────
    private float _chargeTimer  = 0f;
    private bool  _isCharging   = false;
    private bool  _isHolding    = false;

    // ─────────────────────────────────────────────────────────────────────────

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleHold();
        HandleCharge();
    }

    // ─── Right Mouse — Hold Spell Up ──────────────────────────────────────────

    void HandleHold()
    {
        bool rmb = Input.GetMouseButton(1);

        if (rmb != _isHolding)
        {
            _isHolding = rmb;
            animator.SetBool(HashIsHolding, _isHolding);

            // If RMB pressed mid-charge, cancel the charge
            if (_isHolding && _isCharging)
                CancelCharge();
        }
    }

    // ─── Left Mouse — Charge & Fire ───────────────────────────────────────────

    void HandleCharge()
    {
        // Can't charge while holding RMB
        if (_isHolding) return;

        // ── Begin Charge ──
        if (Input.GetMouseButtonDown(0) && !_isCharging)
        {
            _isCharging  = true;
            _chargeTimer = 0f;
            animator.SetBool(HashIsCharging, true);
        }

        // ── Tick Charge ──
        if (_isCharging)
        {
            _chargeTimer += Time.deltaTime;
            float normalized = Mathf.Clamp01(_chargeTimer / maxChargeTime);
            animator.SetFloat(HashChargeTime, normalized);
        }

        // ── Release LMB ──
        if (Input.GetMouseButtonUp(0) && _isCharging)
        {
            if (_chargeTimer >= minChargeToFire)
                FireSpell();
            else
                CancelCharge();
        }
    }

    // ─── Fire ─────────────────────────────────────────────────────────────────

    void FireSpell()
    {
        animator.SetBool(HashIsCharging, false);
        animator.SetFloat(HashChargeTime, 0f);
        animator.SetTrigger(HashFire);

        _isCharging  = false;
        _chargeTimer = 0f;

        // TODO: Spawn your spell projectile here
        // e.g. SpellManager.Instance.Fire(chargeRatio);
    }

    // ─── Cancel ───────────────────────────────────────────────────────────────

    void CancelCharge()
    {
        animator.SetBool(HashIsCharging, false);
        animator.SetFloat(HashChargeTime, 0f);
        animator.SetTrigger(HashCancelCharge);

        _isCharging  = false;
        _chargeTimer = 0f;
    }

    // ─── Public Helpers ───────────────────────────────────────────────────────

    /// <summary>Normalized charge progress 0..1. Use to scale spell power.</summary>
    public float ChargeRatio => Mathf.Clamp01(_chargeTimer / maxChargeTime);

    /// <summary>True when fully charged.</summary>
    public bool IsFullyCharged => _chargeTimer >= maxChargeTime;
}
