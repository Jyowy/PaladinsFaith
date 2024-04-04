using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageInker : MonoBehaviour
{
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Color ink = Color.white;

    private Color baseColor = Color.white;

    private void Awake()
    {
        baseColor = image.color;
    }

    [Button]
    public void Ink()
    {
        image.color = baseColor * ink;
    }

    [Button]
    public void RemoveInk()
    {
        image.color = baseColor;
    }
}
