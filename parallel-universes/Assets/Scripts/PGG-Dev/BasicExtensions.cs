using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Enumeration;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PGGDev.BasicExtensions {

    public static class BasicExtensions
    {
        #region Booleans

        /// <summary>
        /// Checks if a int is even or odd.
        /// </summary>
        /// <returns>true when num is even, false when odd.</returns>
        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }

        /// <summary>
        /// Converts an int to a direction.
        /// </summary>
        /// <returns>1f if num is even, -1f when odd.</returns>
        public static float ToDirection(this int num)
        {
            return (num % 2 == 0) ? 1f : -1f;
        }

        #endregion

        #region Remapping

        /// <summary>
        /// Remaps a float from one range to another.
        /// </summary>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float t = Mathf.InverseLerp(fromMin, fromMax, value);
            return Mathf.Lerp(toMin, toMax, t);
        }

        /// <summary>
        /// Remaps a float from a specified range to 0 - 1.
        /// </summary>
        public static float RemapTo01(this float value, float fromMin, float fromMax)
        {
            return value.Remap(fromMin, fromMax, 0f, 1f);
        }

        /// <summary>
        /// Remaps a float from 0 - 1 to a specified range.
        /// </summary>
        public static float RemapFrom01(this float value, float toMin, float toMax)
        {
            return value.Remap(0f, 1f, toMin, toMax);
        }

        /// <summary>
        /// Remaps a Vector3 from one range to another.
        /// </summary>
        public static Vector3 Remap(this Vector3 value, Vector3 fromMin, Vector3 fromMax, Vector3 toMin, Vector3 toMax)
        {
            float x = value.x.Remap(fromMin.x, fromMax.x, toMin.x, toMax.x);
            float y = value.y.Remap(fromMin.y, fromMax.y, toMin.y, toMax.y);
            float z = value.z.Remap(fromMin.z, fromMax.z, toMin.z, toMax.z);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Remaps a Vector3 from a specified range to Vector3.zero - Vector3.one.
        /// </summary>
        public static Vector3 RemapTo01(this Vector3 value, Vector3 fromMin, Vector3 fromMax)
        {
            return value.Remap(fromMin, fromMax, Vector3.zero, Vector3.one);
        }

        /// <summary>
        /// Remaps a Vector3 from Vector3.zero - Vector3.one to a specified range.
        /// </summary>
        public static Vector3 RemapFrom01(this Vector3 value, Vector3 toMin, Vector3 toMax)
        {
            return value.Remap(Vector3.zero, Vector3.one, toMin, toMax);
        }

        #endregion

        #region Enums

        public class Enum<T> where T : struct, IConvertible
        {
            public static int Count
            {
                get
                {
                    if (!typeof(T).IsEnum)
                        throw new ArgumentException("T must be an enumerated type");

                    return Enum.GetNames(typeof(T)).Length;
                }
            }
        }

        // Enum Extensions (https://stackoverflow.com/questions/15388072/how-to-add-extension-methods-to-enums)
        public static int ToInt<T>(this T soure) where T : IConvertible//enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return (int)(IConvertible)soure;
        }

        // Returns length of an enum
        public static int Count<T>(this T soure) where T : IConvertible//enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Enum.GetNames(typeof(T)).Length;
        }

        #endregion

        #region Strings

        /// <summary>
        /// Splits a camel case formatted string.
        /// <br/>
        /// Source: <a href="https://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token">here</a>
        /// </summary>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        /// <summary>
        /// Converts an integer to a decimal string based on NumberFormatInfo.
        /// <br/>
        /// Source: <a href="https://stackoverflow.com/questions/42683761/c-sharp-how-to-separate-an-int-with-decimal-points">here</a>
        /// </summary>
        public static string ToDecimalPointString(this int value)
        {
            string convertResult = "";

            convertResult = value.ToString("N0", new NumberFormatInfo()
            {
                NumberGroupSizes = new[] { 3 },
                NumberGroupSeparator = "."
            });

            return convertResult;
        }

        /// <summary>
        /// Convertrs a float to a string, leading with a $-Sign based on en-US CultureInfo.
        /// </summary>
        public static string ToDollar(this float value)
        {
            return value.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        #endregion

        #region Vectors

        /// <summary>
        /// Checks if a target is in range without using expensive Mathf.Sqrt().
        /// </summary>
        public static bool IsInRange(Vector3 position, Vector3 target, float range)
        {
            return (target - position).sqrMagnitude < range * range;
        }

        /// <summary>
        /// Get points that are equally distributed on a unit sphere.
        /// <br/>
        /// Source: <a href="https://forum.unity.com/threads/evenly-distributed-points-on-a-surface-of-a-sphere.26138/">here</a>
        /// </summary>
        public static Vector3[] GetEqualDistributedPointsOnUnitSphere(int n)
        {
            List<Vector3> upts = new List<Vector3>();
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2.0f / n;
            float x = 0;
            float y = 0;
            float z = 0;
            float r = 0;
            float phi = 0;

            for (var k = 0; k < n; k++)
            {
                y = k * off - 1 + (off / 2);
                r = Mathf.Sqrt(1 - y * y);
                phi = k * inc;
                x = Mathf.Cos(phi) * r;
                z = Mathf.Sin(phi) * r;

                upts.Add(new Vector3(x, y, z));
            }

            Vector3[] pts = upts.ToArray();
            return pts;
        }

        #endregion

        #region Quaternions

        /// <summary>
        /// Get the directions to points that are equally distributed on a unit sphere.
        /// <br/>
        /// Source (used to calculate points): <a href="https://forum.unity.com/threads/evenly-distributed-points-on-a-surface-of-a-sphere.26138/">here</a>
        /// </summary>
        public static Quaternion[] GetEqualDistributedRotations(int n)
        {
            Vector3[] points = GetEqualDistributedPointsOnUnitSphere(n);

            Quaternion[] rotations = new Quaternion[points.Length];

            for (int i = 0; i < rotations.Length; i++)
            {
                rotations[i] = Quaternion.LookRotation(points[i], Vector3.up);
            }

            return rotations;
        }

        /// <summary>
        /// Get random rotation, calculated for each axis in range.
        /// </summary>
        public static Quaternion RandomRotationInRange(MinMax range)
        {
            return Quaternion.Euler(range.Random, range.Random, range.Random);
        }

        #endregion

        #region Colors

        /// <summary>
        /// Converts an RGB color within 0f to 255f range to a unity color. alpha is 255f.
        /// </summary>
        public static Color FromRGB255(this Color color, float r, float g, float b)
        {
            return new Color().FromRGB255(r, g, b, 255f);
        }

        /// <summary>
        /// Converts an RGBA color within 0f to 255f range to a unity color.
        /// </summary>
        public static Color FromRGB255(this Color color, float r, float g, float b, float a)
        {
            return new Color(r.RemapTo01(0f, 255f), g.RemapTo01(0f, 255f), b.RemapTo01(0f, 255f), a.RemapTo01(0f, 255f));
        }

        /// <summary>
        /// Changes the color of a renderer without duplicating the material.
        /// <br/>
        /// Source: <a href="https://youtu.be/hQE8lQk9ikE?t=1245">here</a>
        /// </summary>
        public static void ChangeColor(this Renderer renderer, Color color)
        {
            MaterialPropertyBlock propertyBlock = null;

            // Get the current value of the material properties in the renderer
            renderer.GetPropertyBlock(propertyBlock);
            // Assign the new value
            propertyBlock.SetColor("_Color", color);
            // Apply the edited values to the renderer
            renderer.SetPropertyBlock(propertyBlock);
        }

        /// <summary>
        /// Converts a color to a hexadecimal string -> #XXXXXX
        /// </summary>
        public static string ToHexString(this Color color)
        {
            return
                "#" +
                ((byte)(color.r * 255)).ToString("X2") +
                ((byte)(color.g * 255)).ToString("X2") +
                ((byte)(color.b * 255)).ToString("X2");
        }

        /// <summary>
        /// Returns a string with HTML color tags. Can be used in places like Debug.Log() or TextMeshPro.
        /// </summary>
        public static string ToColorTagString(this string message,  Color color)
        {
            return $"<color={color.ToHexString()}>{message}</color>";
        }

        #endregion

        #region Transforms

        /// <summary>
        /// Destroys all children in a transform
        /// </summary>
        public static void ClearChildren(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Resets the global position, rotation and scale of a transform.
        /// </summary>
        public static void ResetGlobal(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Resets the local position, rotation and scale of a transform.
        /// </summary>
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Returns all direct children of a transform.
        /// </summary>
        public static Transform[] GetDirectChildren(this Transform parent)
        {
            int childCount = parent.childCount;
            Transform[] children = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                children[i] = parent.GetChild(i);
            }

            return children;
        }

        /// <summary>
        /// Returns all children of a transform with a specific tag.
        /// </summary>
        /// <param name="deepSearch">Should check childrens children or only direct children?</param>
        public static GameObject[] FindChildrenWithTag(this Transform parent, string tag, bool deepSearch)
        {
            List<GameObject> childsWithTag = new List<GameObject>();

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                if (child.tag == tag)
                {
                    childsWithTag.Add(child.gameObject);
                }

                if (deepSearch && child.childCount > 0)
                {
                    childsWithTag.AddRange(FindChildrenWithTag(child, tag, deepSearch));
                }
            }

            return childsWithTag.ToArray();
        }

        #endregion

        #region Lists & Arrays

        /// <summary>
        /// Forces a list to a fixed length (cut when too long, adding default when too short).
        /// </summary>
        public static void ResizeList<T>(this List<T> list, int newCount)
        {
            if (newCount <= 0)
            {
                list.Clear();
            }
            else
            {
                while (list.Count > newCount) list.RemoveAt(list.Count - 1);
                while (list.Count < newCount) list.Add(default(T));
            }
        }

        /// <summary>
        /// Cuts the end of a list when too long.
        /// </summary>
        public static void CutList<T>(this List<T> list, int maxCount)
        {
            while (list.Count > maxCount) list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Returns random entry of an array
        /// </summary>
        public static T Random<T>(this T[] array)
        {
            if (array.Length == 0) return default(T);
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Returns entry at specified index when index in range, default(T) otherwise.
        /// </summary>
        public static T Entry<T>(this T[] array, int index)
        {
            if (0 <= index && index < array.Length) return array[index];
            return default(T);
        }

        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        public static T[] Shuffle<T>(this T[] array)
        {
            for (int t = 0; t < array.Length; t++)
            {
                T tmp = array[t];
                int r = UnityEngine.Random.Range(t, array.Length);
                array[t] = array[r];
                array[r] = tmp;
            }

            return array;
        }

        #endregion

        #region Physics

        /// <summary>
        /// Checks if a layer mask has a specific layer
        /// </summary>
        public static bool HasLayer(this LayerMask layermask, int layer)
        {
            return layermask == (layermask | (1 << layer));
        }

        public static LayerMask LayerMask_Everything()
        {
            return ~0;
        }

        /// <summary>
        /// Sets the layer of all children in a transform.
        /// </summary>
        public static void SetChildLayers(this Transform target, int layer)
        {
            foreach (Transform child in target)
            {
                child.gameObject.layer = layer;

                if (child.childCount != 0)
                {
                    SetChildLayers(child, layer);
                }
            }
        }

        /// <summary>
        /// Same as Sphere Cast, but in a cone shape
        /// <br/>
        /// Source: <a href="https://github.com/walterellisfun/ConeCast/blob/master/ConeCastExtension.cs">here</a>
        /// </summary>
        public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
            List<RaycastHit> coneCastHitList = new List<RaycastHit>();

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                Vector3 directionToHit = sphereCastHits[i].point - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle) coneCastHitList.Add(sphereCastHits[i]);
            }

            return coneCastHitList.ToArray();
        }

        /// <summary>
        /// Reflects a direction off of the normal plane of the collisions contacts[0].
        /// </summary>
        public static Vector3 Reflection(this Collision collision, Vector3 inDirection)
        {
            return Vector3.Reflect(inDirection, collision.contacts[0].normal);
        }

        #endregion

        #region Meshes

        /// <summary>
        /// Clones a mesh that can be altered without changing the original mesh.
        /// </summary>
        public static Mesh CloneMesh(this Mesh baseMesh)
        {
            Mesh mesh = new Mesh();

            mesh.vertices = baseMesh.vertices;
            mesh.triangles = baseMesh.triangles;
            mesh.uv = baseMesh.uv;

            return mesh;
        }

        #endregion

        #region Debugging

        /// <summary>
        /// For Singleton Pattern -> Logs Error when duplicate singleton is Destroyed
        /// </summary>
        public static void DestroySingleton(this UnityEngine.Object obj)
        {
            Debug.LogError($"Only 1 Instance of {obj.GetType().Name} allowed in a scene, object ({obj.name}) was destroyed.");
            UnityEngine.Object.Destroy(obj);
        }

        #endregion
    }

    #region Data Classes

    // Stores a minimum and a maximum value. Has multiple useful functions needed for most min-max-operations
    [System.Serializable]
    public class MinMax
    {
        public float min;
        public float max;

        public float Random { get { return UnityEngine.Random.Range(min, max); } }

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }

        public virtual float Evaluate(float t)
        {
            return Mathf.Lerp(min, max, t);
        }


        public float InverseLerp(float t)
        {
            return Mathf.InverseLerp(min, max, t);
        }
    }

    // You can directly Evaluate a min max with an animaton curve
    [System.Serializable]
    public class CurveMinMax : MinMax
    {
        public AnimationCurve curve;

        public CurveMinMax(float min, float max) : base(min, max)
        {
            this.min = min;
            this.max = max;
            this.curve = AnimationCurveExtensions.Linear;
        }

        public CurveMinMax(float min, float max, AnimationCurve curve) : base(min, max)
        {
            this.min = min;
            this.max = max;
            this.curve = curve;
        }

        public override float Evaluate(float t)
        {
            float curveValue = curve.Evaluate(t);
            return Mathf.Lerp(min, max, curveValue);
        }
    }

    public class AnimationCurveExtensions
    {
        public static AnimationCurve Linear => AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public static AnimationCurve EaseInOut => AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    [System.Serializable]
    public class MinMaxRange
    {
        public MinMax minRange;
        public MinMax maxRange;

        public float RandomAtT(float t)
        {
            return UnityEngine.Random.Range(Mathf.Lerp(minRange.min, maxRange.min, t), Mathf.Lerp(minRange.max, maxRange.max, t));
        }

        public float Evaluate(float rangeBlend, float minMaxBlend)
        {
            return Mathf.Lerp(
                minRange.Evaluate(minMaxBlend),
                maxRange.Evaluate(minMaxBlend),
                rangeBlend
                );
        }
    }

    [System.Serializable]
    public class MinMaxInt
    {
        public int min;
        public int max;

        public int Random { get { return Mathf.RoundToInt(UnityEngine.Random.Range(min, max)); } }

        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Clamp(int value)
        {
            return Mathf.Clamp(value, min, max);
        }

        public virtual int Evaluate(float t)
        {
            return Mathf.FloorToInt(Mathf.Lerp(min, max, t));
        }
    }

    [System.Serializable]
    public struct Collection<T>
    {
        public T[] entries;
    }

    // https://discussions.unity.com/t/how-do-i-return-a-value-from-a-coroutine/6438/4

    /*
    How to use:

    private IEnumerator LoadSomeStuff() {

        WWW www = new WWW("http://someurl");

        yield return www;

        if (String.IsNullOrEmpty(www.error) {
        yield return "success";
        } else {
            yield return "fail";
        }
    }

    This is the implementation
    V            V            V

    DataCoroutine dc = new DataCoroutine(this, LoadSomeStuff());
    yield return dc.coroutine;
    Debug.Log("result is " + dc.result);  //  'success' or 'fail'
    */
    public class UnifyCoroutine<T>
    {
        public Action<T> callback;

        private IEnumerator target;

        private static readonly GameObject gameObject;
        private static readonly MonoBehaviour mono;

        static UnifyCoroutine()
        {
            gameObject = new GameObject { isStatic = true, name = "[Unify]" };
            mono = gameObject.AddComponent<UnifyMonoBehaviour>();
        }

        public UnifyCoroutine(IEnumerator target, Action<T> callback)
        {
            this.target = target;
            this.callback = callback;
        }

        public UnifyCoroutine(IEnumerator target)
        {
            this.target = target;
            this.callback = null;
        }

        public IEnumerator StartUnifyCoroutine()
        {
            while (target.MoveNext())
            {
                yield return target.Current;
            }

            if(target.Current.GetType() != typeof(T))
            {
                Debug.LogError($"Unify Coroutine finished with the wrong Type: {target.Current.GetType()} (should be '{typeof(T)}'");
                yield return null;
            }
            else
            {
                callback?.Invoke((T)target.Current);
            }
        }

        /*
        private IEnumerator target;
        public Action<T> callback;
        public Coroutine coroutine { get; private set; }
        public T result;

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = (T)target.Current;
                yield return result;
            }
        }

        public DataCoroutine(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.result = default;
            this.coroutine = null;
            this.coroutine = owner.StartCoroutine(Run());
        }
        */
    }

    public class UnifyMonoBehaviour : MonoBehaviour
    {
        // Empty class
    }

    public class DataCoroutine<T>
    {
        private IEnumerator target;
        public Action<T> callback;
        public Coroutine coroutine { get; private set; }
        public T result;

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = (T)target.Current;
                yield return result;
            }
        }

        public DataCoroutine(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.result = default;
            this.coroutine = null;
            this.coroutine = owner.StartCoroutine(Run());
        }
    }

    #endregion
}