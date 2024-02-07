using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Player
{
    public class PlayerCameraColliderShaderController : MonoBehaviour
    {
        [SerializeField]
        private Shader shader = null;
        [SerializeField]
        private string propertyName = "_Transparent";

        private void OnTriggerEnter(Collider other)
        {
            TryToSetTransparent(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            TryToSetNormal(other.gameObject);
        }

        private void TryToSetTransparent(GameObject gameObject)
        {
            Renderer renderer = GetRendererComponent(gameObject);
            if (renderer == null)
            {
                return;
            }

            renderer.enabled = false;
            return;
            foreach (var material in renderer.materials)
            {
                if (material.shader == shader)
                {
                    SetMaterialOpacity(material, 0f);
                    break;
                }
            }
        }

        private void TryToSetNormal(GameObject gameObject)
        {
            Renderer renderer = GetRendererComponent(gameObject);
            if (renderer == null)
            {
                return;
            }

            renderer.enabled = true;
            return;
            foreach (var material in renderer.materials)
            {
                if (material.shader == shader)
                {
                    SetMaterialOpacity(material, 1f);
                    break;
                }
            }
        }

        private Renderer GetRendererComponent(GameObject gameObject)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            return renderer;
        }

        private void SetMaterialOpacity(Material material, float value)
        {
            material.SetFloat(propertyName, value);
        }
    }
}
