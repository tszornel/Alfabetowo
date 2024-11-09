/* 
               unitywebaby.com
    --------------------------------------------------
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Webaby {
    /*
     * Global Asset references
     * Edit Asset references in the prefab Webaby/Resources/WebabyAssets
     * */
    public class WebabyAssets : MonoBehaviour {
        // Internal instance reference
        private static WebabyAssets _i; 
        // Instance reference
        public static WebabyAssets i {
            get {
                if (_i == null) _i = Instantiate(Resources.Load<WebabyAssets>("WebabyAssets")); 
                return _i; 
            }
        }
        // All references
        public Sprite s_White;
        public Sprite s_Circle;
        public Material m_White;
    }
}
