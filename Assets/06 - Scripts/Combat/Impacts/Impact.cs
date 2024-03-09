using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PaladinsFaith.Effects;

namespace PaladinsFaith.Combat
{
    public class Impact : MonoBehaviour
    {
        public float duration = 1f;

        public static void CreateAttackImpact(Impact impactPrefab, Attack attack)
        {
            Vector3 position = attack.impactPoint;
            Quaternion rotation = Quaternion.identity;
            Impact newImpact = GameObject.Instantiate(impactPrefab, position, rotation);
            GameObject.Destroy(newImpact.gameObject, impactPrefab.duration);
        }
    }
}