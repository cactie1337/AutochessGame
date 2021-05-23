using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool isReady;
    private Transform targetTransform;
    private HealthAndMana targetHealthScript;
    private float damage;
    [SerializeField]
    [Tooltip("How fast the projectile will travel to its target. If 0 at runtime it will default to 25.")]
    private float projectileSpeed;

    protected bool IsReady { get => isReady; set => isReady = value; }
    protected Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    protected HealthAndMana TargetHealthScript { get => targetHealthScript; set => targetHealthScript = value; }
    protected float Damage { get => damage; set => damage = value; }
    protected float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    protected virtual void Awake()
    {
        if (ProjectileSpeed == 0)
            ProjectileSpeed = 25f;
    }

    public virtual void Setup(Transform targetTransform, HealthAndMana targetHealthScript, float damage)
    {
        TargetTransform = targetTransform;
        TargetHealthScript = targetHealthScript;
        Damage = damage;

        IsReady = true;
    }

    protected virtual void Update()
    {
        if (TargetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        if (IsReady)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetTransform.position, ProjectileSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, TargetTransform.position);

            if (distance <= 0.02f)
            {
                 DealDamageAndDestruct();
            }
        }
    }

    protected virtual void DealDamageAndDestruct()
    {
        TargetHealthScript.TakeDamage(Damage);
        
        Destroy(gameObject);
    }
}
