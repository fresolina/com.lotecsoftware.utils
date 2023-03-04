using UnityEngine;

namespace Lotec.Utils {
    public static class Hiliter {
        private static int _baseColorId = Shader.PropertyToID("_BaseColor");
        private static int _colorId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock _hiliteBlock;

        /// <summary>
        /// Run once at application start.
        /// Initializes with double intensity, new Color(2,2,2,1)
        /// </summary>
        public static void InitHilite() {
            InitHilite(new Color(2f, 2f, 2f, 1));
        }

        /// <summary>
        /// Run once at application start.
        /// </summary>
        /// <param name="hiliteColor"></param>
        public static void InitHilite(Color hiliteColor) {
            _hiliteBlock = new MaterialPropertyBlock();
            _hiliteBlock.SetColor(_colorId, hiliteColor);
            _hiliteBlock.SetColor(_baseColorId, hiliteColor);
        }

        /// <summary>
        /// Hilite object by doubling color intensity.
        /// Finds all Renderers in object and sets hilite.
        /// </summary>
        /// <param name="obj">Object to hilite</param>
        /// <param name="on">Turn on/off hilite</param>
        public static void Hilite(GameObject obj, bool on) {
            if (!obj) return;

            var renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;

            for (int i = 0; i < renderers.Length; i++) {
                renderers[i].SetPropertyBlock(on ? _hiliteBlock : null);
            }
        }

        /// <summary>
        /// Hilite object by doubling color intensity.
        /// Finds all Renderers in object and sets hilite.
        /// </summary>
        /// <param name="obj">Object to hilite</param>
        /// <param name="on">Turn on/off hilite</param>
        public static void Hilite(Component obj, bool on) {
            Hilite(obj?.gameObject, on);
        }
    }
}