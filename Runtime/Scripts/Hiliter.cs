using UnityEngine;

namespace Lotec.Utils {
    public static class Hiliter {
        static readonly int s_baseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int s_colorId = Shader.PropertyToID("_Color");
        static MaterialPropertyBlock s_hiliteBlock;

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
            s_hiliteBlock = new MaterialPropertyBlock();
            s_hiliteBlock.SetColor(s_colorId, hiliteColor);
            s_hiliteBlock.SetColor(s_baseColorId, hiliteColor);
        }

        /// <summary>
        /// Hilite object by doubling color intensity.
        /// Finds all Renderers in object and sets hilite.
        /// </summary>
        /// <param name="obj">Object to hilite</param>
        /// <param name="on">Turn on/off hilite</param>
        public static void Hilite(GameObject obj, bool on) {
            if (!obj)
                return;

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
                return;

            if (s_hiliteBlock == null)
                InitHilite();

            for (int i = 0; i < renderers.Length; i++) {
                renderers[i].SetPropertyBlock(on ? s_hiliteBlock : null);
            }
        }

        /// <summary>
        /// Hilite object by doubling color intensity.
        /// Finds all Renderers in object and sets hilite.
        /// </summary>
        /// <param name="obj">Object to hilite</param>
        /// <param name="on">Turn on/off hilite</param>
        public static void Hilite(Component obj, bool on) {
            if (obj == null)
                return;
            Hilite(obj.gameObject, on);
        }
    }
}
