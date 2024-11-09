
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using System.Reflection;
using System.Linq;


namespace Webaby.Utils {


    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
    /*
     * Various assorted utilities functions
     * */
    public static class UtilsClass {
        
        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0,-1);
        private static ItemObjectDatabase data;

        public const int sortingOrderDefault = 5000;



        // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder

       /* public static Item GetItem(string name)
        {

            if (!data)
                data = PickupItemAssets.Instance.database;
            GameLog.LogMessage("Spawn item" + name);
            foreach (ItemObject itemObject in ItemDatabaseObject.AllItems)
            {

                // GameLog.LogMessage("item name from allitems= " + itemObject.name);
                if (name.Equals(itemObject.name))
                {
                    GameLog.LogMessage("MATCH ditem name from allitems= " + itemObject.name);
                    return new Item(itemObject);
                }



            }
            GameLog.LogError("return null for name " + name);
            return null;
        }*/




        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault) {
            return (int)(baseSortingOrder - position.y) + offset;
        }


        // Get Main Canvas Transform
        private static Transform cachedCanvasTransform;
        public static Transform GetCanvasTransform() {
            if (cachedCanvasTransform == null) {
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (canvas != null) {
                    cachedCanvasTransform = canvas.transform;
                }
            }
            return cachedCanvasTransform;
        }
        public static TFieldType GetFieldValue<TFieldType, TObjectType>(this TObjectType obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            return (TFieldType)fieldInfo.GetValue(obj);
        }

        public static TFieldType[] GetStringFieldValueArray<TFieldType, TObjectType>(this TObjectType obj, string fieldName)
        {
            

            var fieldInfo = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            Array a = (Array)fieldInfo.GetValue(obj);

            List<TFieldType> newArrayList = new List<TFieldType>();
            for (int i = 0; i < a.Length; i++)
            {
                object o = a.GetValue(i);
                GameLog.LogMessage("string from array:"+o.ToString());
                newArrayList.Add((TFieldType)o);
            }
            return newArrayList.ToArray();
        }



        public static T GetValueDirectTest<T, TObj>(this FieldInfo fieldInfo, ref TObj instance)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            var tr = __makeref(instance);
            object val = fieldInfo.GetValueDirect(tr);
            return (T)val;
        }


      
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        //Get language from prefs
        public static DialogLanguage GetLanguageFromPrefs(string prefsLanguage)
        {
            GameLog.LogMessage("GetLanguageFrom Prefs Entered");
            DialogLanguage language;
            switch (prefsLanguage)
            {
                case "Polski":
                    language = DialogLanguage.Polski;
                    break;
                case "Niemiecki":
                    language = DialogLanguage.Niemiecki;
                    break;
                case "Angielski":
                    language = DialogLanguage.Angielski;
                    break;
                case "Francuski":
                    language = DialogLanguage.Francuski;
                    break;
                default:
                    language = DialogLanguage.Polski;
                    break;
            }
            return language;
        }

        static public bool ToggleLight(bool lightOn, Light2D gameObjectLight,float timeBeetwenLightTween, float tweenTime)
        {

            GameLog.LogMessage("Toggle lights from utils entered");
            if (Time.time >= timeBeetwenLightTween)
            {
                LeanTween.cancel(gameObjectLight.gameObject);

                tweenTime = 1f;
                if (!lightOn)
                    LeanTween.value(gameObjectLight.gameObject, 1, 10, tweenTime).setEaseLinear().setOnUpdate((System.Action<float, object>)setLight);
                else
                    LeanTween.value(gameObjectLight.gameObject, 10, 1, tweenTime).setEaseLinear().setOnUpdate((System.Action<float, object>)setLight);
                lightOn = !lightOn;
            }

            return lightOn;

        }

  
        static private void setLight(float value, object lightObject)
        {

            GameLog.LogMessage("SET lights from utils entered"+value);
            Light2D go = (Light2D)lightObject;
            go.intensity = value;

            GameLog.LogMessage("SET lights from utils intensity:" + go.intensity);
        }

        public static bool AnimatorHasParameter(Animator animatorTest, string name)
        {
            bool exists= false;
            if (animatorTest)
             exists = animatorTest.parameters.Any(x => x.name == name);

            return exists;
        }


        public static bool AnimatorHasLayer(Animator animatorTest, string name)
        {

            var layerExists = animatorTest.GetLayerIndex(name) != -1;
            bool exists = layerExists ? true : false;

            return exists;
        }


        // Get Default Unity Font, used in text objects if no font given
        public static Font GetDefaultFont() {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }


        // Create a Sprite in the World, no parent
        public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale, int sortingOrder, Color color) {
            return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }
        
        // Create a Sprite in the World
        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color) {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        // Create a Sprite in the World with Button_Sprite, no parent
        public static Button_Sprite CreateWorldSpriteButton(string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color) {
            return CreateWorldSpriteButton(null, name, sprite, localPosition, localScale, sortingOrder, color);
        }

        // Create a Sprite in the World with Button_Sprite
        public static Button_Sprite CreateWorldSpriteButton(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color) {
            GameObject gameObject = CreateWorldSprite(parent, name, sprite, localPosition, localScale, sortingOrder, color);
            gameObject.AddComponent<BoxCollider2D>();
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            return buttonSprite;
        }

        // Creates a Text Mesh in the World and constantly updates it
        public static FunctionUpdater CreateWorldTextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null) {
            TextMesh textMesh = CreateWorldText(GetTextFunc(), parent, localPosition);
            return FunctionUpdater.Create(() => {
                textMesh.text = GetTextFunc();
                return false;
            }, "WorldTextUpdater");
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        
        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }


        // Create a Text Popup in the World, no parent
        public static void CreateWorldTextPopup(string text, Vector3 localPosition) {
            CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, 20), 1f);
        }
        
        // Create a Text Popup in the World
        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate () {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (popupTime <= 0f) {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                } else {
                    return false;
                }
            }, "WorldTextPopup");
        }

        // Create Text Updater in UI
        public static FunctionUpdater CreateUITextUpdater(Func<string> GetTextFunc, Vector2 anchoredPosition) {
            Text text = DrawTextUI(GetTextFunc(), anchoredPosition,  20, GetDefaultFont());
            return FunctionUpdater.Create(() => {
                text.text = GetTextFunc();
                return false;
            }, "UITextUpdater");
        }


        // Draw a UI Sprite
        public static RectTransform DrawSprite(Color color, Transform parent, Vector2 pos, Vector2 size, string name = null) {
            RectTransform rectTransform = DrawSprite(null, color, parent, pos, size, name);
            return rectTransform;
        }
        
        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Transform parent, Vector2 pos, Vector2 size, string name = null) {
            RectTransform rectTransform = DrawSprite(sprite, Color.white, parent, pos, size, name);
            return rectTransform;
        }
        
        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Color color, Transform parent, Vector2 pos, Vector2 size, string name = null) {
            // Setup icon
            if (name == null || name == "") name = "Sprite";
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.SetParent(parent, false);
            goRectTransform.sizeDelta = size;
            goRectTransform.anchoredPosition = pos;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;

            return goRectTransform;
        }

        public static Text DrawTextUI(string textString, Vector2 anchoredPosition, int fontSize, Font font) {
            return DrawTextUI(textString, GetCanvasTransform(), anchoredPosition, fontSize, font);
        }

        public static Text DrawTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize, Font font) {
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(parent, false);
            Transform textGoTrans = textGo.transform;
            textGoTrans.SetParent(parent, false);
            textGoTrans.localPosition = Vector3zero;
            textGoTrans.localScale = Vector3one;

            RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
            textGoRectTransform.sizeDelta = new Vector2(0,0);
            textGoRectTransform.anchoredPosition = anchoredPosition;

            Text text = textGo.GetComponent<Text>();
            text.text = textString;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            return text;
        }


        // Parse a float, return default if failed
	    public static float Parse_Float(string txt, float _default) {
		    float f;
		    if (!float.TryParse(txt, out f)) {
			    f = _default;
		    }
		    return f;
	    }
        
        // Parse a int, return default if failed
	    public static int Parse_Int(string txt, int _default) {
		    int i;
		    if (!int.TryParse(txt, out i)) {
			    i = _default;
		    }
		    return i;
	    }
	    public static int Parse_Int(string txt) {
            return Parse_Int(txt, -1);
	    }



        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        

        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public static bool IsPointerOverUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            } else {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position =  Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll( pe, hits );
                return hits.Count > 0;
            }
        }


        
		// Returns 00-FF, value 0->255
	    public static string Dec_to_Hex(int value) {
		    return value.ToString("X2");
	    }

		// Returns 0-255
	    public static int Hex_to_Dec(string hex) {
		    return Convert.ToInt32(hex, 16);
	    }
        
		// Returns a hex string based on a number between 0->1
	    public static string Dec01_to_Hex(float value) {
		    return Dec_to_Hex((int)Mathf.Round(value*255f));
	    }

		// Returns a float between 0->1
	    public static float Hex_to_Dec01(string hex) {
		    return Hex_to_Dec(hex)/255f;
	    }

        // Get Hex Color FF00FF
	    public static string GetStringFromColor(Color color) {
		    string red = Dec01_to_Hex(color.r);
		    string green = Dec01_to_Hex(color.g);
		    string blue = Dec01_to_Hex(color.b);
		    return red+green+blue;
	    }
        
        // Get Hex Color FF00FFAA
	    public static string GetStringFromColorWithAlpha(Color color) {
		    string alpha = Dec01_to_Hex(color.a);
		    return GetStringFromColor(color)+alpha;
	    }

        // Sets out values to Hex String 'FF'
	    public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha) {
		    red = Dec01_to_Hex(color.r);
		    green = Dec01_to_Hex(color.g);
		    blue = Dec01_to_Hex(color.b);
		    alpha = Dec01_to_Hex(color.a);
	    }
        
        // Get Hex Color FF00FF
	    public static string GetStringFromColor(float r, float g, float b) {
		    string red = Dec01_to_Hex(r);
		    string green = Dec01_to_Hex(g);
		    string blue = Dec01_to_Hex(b);
		    return red+green+blue;
	    }
        
        // Get Hex Color FF00FFAA
	    public static string GetStringFromColor(float r, float g, float b, float a) {
		    string alpha = Dec01_to_Hex(a);
		    return GetStringFromColor(r,g,b)+alpha;
	    }
        
        // Get Color from Hex string FF00FFAA
	    public static Color GetColorFromString(string color) {
		    float red = Hex_to_Dec01(color.Substring(0,2));
		    float green = Hex_to_Dec01(color.Substring(2,2));
		    float blue = Hex_to_Dec01(color.Substring(4,2));
            float alpha = 1f;
            if (color.Length >= 8) {
                // Color string contains alpha
                alpha = Hex_to_Dec01(color.Substring(6,2));
            }
		    return new Color(red, green, blue, alpha);
	    }


        // Generate random normalized direction
        public static Vector3 GetRandomDir() {
            return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
        }
        

        public static Vector3 GetVectorFromAngle(int angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        
        public static Vector3 GetVectorFromAngle(float angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        
        public static Vector3 GetVectorFromAngleInt(int angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static int GetAngleFromVector(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static int GetAngleFromVector180(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation) {
            return ApplyRotationToVector(vec, GetAngleFromVectorFloat(vecRotation));
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
            return Quaternion.Euler(0,0,angle) * vec;
        }


        
        public static FunctionUpdater CreateMouseDraggingAction(Action<Vector3> onMouseDragging) {
            return CreateMouseDraggingAction(0, onMouseDragging);
        }

        public static FunctionUpdater CreateMouseDraggingAction(int mouseButton, Action<Vector3> onMouseDragging) {
            bool dragging = false;
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    dragging = true;
                }
                if (Input.GetMouseButtonUp(mouseButton)) {
                    dragging = false;
                }
                if (dragging) {
                    onMouseDragging(UtilsClass.GetMouseWorldPosition());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickFromToAction(Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            return CreateMouseClickFromToAction(0, 1, onMouseClickFromTo, onWaitingForToPosition);
        }

        public static FunctionUpdater CreateMouseClickFromToAction(int mouseButton, int cancelMouseButton, Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            int state = 0;
            Vector3 from = Vector3.zero;
            return FunctionUpdater.Create(() => {
                if (state == 1) {
                    if (onWaitingForToPosition != null) onWaitingForToPosition(from, UtilsClass.GetMouseWorldPosition());
                }
                if (state == 1 && Input.GetMouseButtonDown(cancelMouseButton)) {
                    // Cancel
                    state = 0;
                }
                if (Input.GetMouseButtonDown(mouseButton) && !UtilsClass.IsPointerOverUI()) {
                    if (state == 0) {
                        state = 1;
                        from = UtilsClass.GetMouseWorldPosition();
                    } else {
                        state = 0;
                        onMouseClickFromTo(from, UtilsClass.GetMouseWorldPosition());
                    }
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickAction(Action<Vector3> onMouseClick) {
            return CreateMouseClickAction(0, onMouseClick);
        }

        public static FunctionUpdater CreateMouseClickAction(int mouseButton, Action<Vector3> onMouseClick) {
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    onMouseClick(GetWorldPositionFromUI());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateKeyCodeAction(KeyCode keyCode, Action onKeyDown) {
            return FunctionUpdater.Create(() => {
                if (Input.GetKeyDown(keyCode)) {
                    onKeyDown();
                }
                return false; 
            });
        }

        

        // Get UI Position from World Position
        public static Vector2 GetWorldUIPosition(Vector3 worldPosition, Transform parent, Camera uiCamera, Camera worldCamera) {
            Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            Vector3 uiCameraWorldPosition = uiCamera.ScreenToWorldPoint(screenPosition);
            Vector3 localPos = parent.InverseTransformPoint(uiCameraWorldPosition);
            return new Vector2(localPos.x, localPos.y);
        }

        public static Vector3 GetWorldPositionFromUIZeroZ() {
            Vector3 vec = GetWorldPositionFromUI(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        // Get World Position from UI Position
        public static Vector3 GetWorldPositionFromUI() {
            return GetWorldPositionFromUI(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI(Camera worldCamera) {
            return GetWorldPositionFromUI(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    
        public static Vector3 GetWorldPositionFromUI_Perspective() {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Camera worldCamera) {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Vector3 screenPosition, Camera worldCamera) {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0f));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }


        // Screen Shake
        public static void ShakeCamera(float intensity, float timer) {
            Vector3 lastCameraMovement = Vector3.zero;
            FunctionUpdater.Create(delegate () {
                timer -= Time.unscaledDeltaTime;
                Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
                Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;
                return timer <= 0f;
            }, "CAMERA_SHAKE");
        }

    }

}