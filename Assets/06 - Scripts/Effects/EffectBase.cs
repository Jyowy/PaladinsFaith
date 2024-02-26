using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    public static class GameObjectExtensions
    {
        public static bool HasLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return (gameObject.layer & layerMask.value) == 1;
        }
    }

    public enum RangeType
    {
        Self,
        Target,
        Ray,
        Cone,
        Circle,
        All
    }

    [System.Serializable]
    public class TargetDetector
    {
        [SerializeField]
        private RangeType type = RangeType.Target;
        [SerializeField]
        [ShowIf(nameof(CanHaveFriendlyFire))]
        private bool hasFriendlyFire = false;
        [SerializeField]
        private LayerMask possibleTargets = LayerMask.GetMask("Enemy", "Player");

        [SerializeField]
        [ShowIf(nameof(UsesDistance))]
        private float distance = 5f;

        [SerializeField]
        [ShowIf(nameof(type), RangeType.Ray)]
        private float width = 0.25f;

        [SerializeField]
        [ShowIf(nameof(type), RangeType.Cone), Range(0f, 360f)]
        private float coneAngle = 90f;

        private bool CanHaveFriendlyFire() =>
            type != RangeType.Self
            && type != RangeType.Target;

        private bool UsesDistance() =>
            type == RangeType.Ray
            || type == RangeType.Cone
            || type == RangeType.Circle;

        public List<GameObject> GetTargets(GameObject caster, Vector3 center, GameObject impactedTarget)
        {
            List<GameObject> targets = new List<GameObject>();

            if (type == RangeType.Self)
            {
                targets.Add(caster);
                Debug.Log($"{caster.name}");
            }
            else if (type == RangeType.Target)
            {
                targets.Add(impactedTarget);
            }
            else
            {
                List<Collider> targetColliders = null;

                if (type == RangeType.Ray)
                {
                    Vector3 origin = center;
                    Vector3 direction = caster.transform.forward;
                    Vector3 finalPoint = origin + direction * distance;
                    float radius = width * 0.5f;

                    targetColliders = Physics.OverlapCapsule(origin, finalPoint, radius, possibleTargets)
                        .ToList();
                }
                else if (type == RangeType.Circle)
                {
                    Vector3 origin = center;
                    float radius = distance;

                    //targetColliders = Physics.OverlapSphere(origin, radius, possibleTargets)
                    targetColliders = Physics.OverlapSphere(origin, radius, possibleTargets, QueryTriggerInteraction.Ignore)
                        .ToList();

                    Debug.Log($"EffectBase.GetTargets() Circle type: {origin}, {radius}, {possibleTargets}. Colliders detected {targetColliders.Count}");
                }
                else if (type == RangeType.Cone)
                {
                    Vector3 casterPosition = center;
                    float radius = distance;

                    targetColliders = Physics.OverlapSphere(casterPosition, radius)
                        .ToList();
                    Vector3 casterForward = caster.transform.forward;
                    Vector2 casterForward2D = new Vector2(casterForward.x, casterForward.z).normalized;
                    float coneHalfAngle = coneAngle * 0.5f;

                    int index = 0;
                    while (index < targetColliders.Count)
                    {
                        Collider collider = targetColliders[index];
                        GameObject potentialTarget = collider.gameObject;

                        Vector3 casterTargetDirection = (potentialTarget.transform.position - casterPosition);
                        Vector2 casterTargetDirection2D = new Vector2(casterTargetDirection.x, casterTargetDirection.z).normalized;

                        float casterTargetAngle = Vector2.SignedAngle(casterForward2D, casterTargetDirection2D);
                        if (Mathf.Abs(casterTargetAngle) <= coneHalfAngle)
                        {
                            index++;
                        }
                        else
                        {
                            targetColliders.RemoveAt(index);
                        }
                    }
                }

                Debug.Log($"Targets detected: {targetColliders.Count}");

                bool checkFriendlyFire = CanHaveFriendlyFire() && hasFriendlyFire;
                foreach (Collider targetCollider in targetColliders)
                {
                    GameObject possibleTarget = targetCollider.gameObject;
                    Debug.Log($"- Target '{possibleTarget.name}'.");

                    if (caster.layer == possibleTarget.layer)
                    {
                        if (checkFriendlyFire)
                        {
                            Debug.Log($"- Friend '{possibleTarget.name}'.");
                            targets.Add(possibleTarget);
                        }
                    }
                    else
                    {
                        Debug.Log($"- Enemy '{possibleTarget.name}'.");
                        targets.Add(possibleTarget);
                    }
                }
            }

            return targets;
        }
    }

    public abstract class EffectBase
    {
        public TargetDetector targetDetector = new TargetDetector();

        public virtual void Apply(GameObject caster)
        {
            Vector3 center = caster.transform.position;
            List<GameObject> targets = targetDetector.GetTargets(caster, center, null);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target);
            }
        }

        public virtual void ApplyOnImpacted(GameObject caster, GameObject impactedTarget)
        {
            Vector3 center = impactedTarget.transform.position;
            List<GameObject> targets = targetDetector.GetTargets(caster, center, impactedTarget);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target);
            }
        }

        protected virtual void ApplyToTarget(GameObject caster, GameObject target) { }
    }
}