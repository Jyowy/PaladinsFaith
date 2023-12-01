using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public enum Type
    {
        Camera,
        Target
    }

    [SerializeField]
    private Type type = Type.Camera;

    [SerializeField, ShowIf("type", Type.Camera)]
    private GameObject target = null;

    private void Awake()
    {
        CheckEnabled();
    }

    private void CheckEnabled()
    {
        if (type == Type.Camera)
        {
            enabled = true;
        }
        else if (type == Type.Target)
        {
            enabled = target != null;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        CheckEnabled();
    }

    private void LateUpdate()
    {
        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        Vector3 targetPosition = GetLookAtPosition();
        transform.LookAt(targetPosition, Vector2.up);
    }

    private Vector3 GetLookAtPosition()
    {
        Vector3 position = Vector3.zero;

        if (type == Type.Camera)
        {
            position = Camera.main.transform.position;
        }
        else if (type == Type.Target)
        {
            position = target.transform.position;
        }

        return position;
    }
}
