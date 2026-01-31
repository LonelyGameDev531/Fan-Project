using UnityEngine;

public class UnitGeneric : MonoBehaviour
{
    [Header("Owner")]
    public TotemSystem ownerTotem;

    void Awake()
    {
        SetupUnit();
    }

    void SetupUnit()
    {
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (!TryGetComponent<UnitMovement>(out _))
            gameObject.AddComponent<UnitMovement>();
    }

    /* Not working right now
    private void OnDestroy()
    {
        if (ownerTotem != null)
            ownerTotem.NotifyUnitDeath();
    }
    */
}
