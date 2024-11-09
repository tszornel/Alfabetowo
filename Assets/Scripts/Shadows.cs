using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class Shadows : MonoBehaviour
{
    public Vector3 Offset = new Vector3(-1.0f, -1.0f, 1.0f);
    public Material Material;
    GameObject _shadow;
    SpriteRenderer spriteRenderer;
    SpriteRenderer sr;
    void Start()
    {
        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = transform;
        _shadow.transform.localPosition = Offset;
        _shadow.transform.localRotation = Quaternion.identity;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sr = _shadow.AddComponent<SpriteRenderer>();
        sr.sprite = spriteRenderer.sprite;
        sr.material = Material;
        Vector3 myVector = new Vector3(0.1f, 0.1f, 0.1f);
        sr.transform.localScale = myVector;
        sr.sortingLayerName = spriteRenderer.sortingLayerName;
        sr.sortingOrder = spriteRenderer.sortingOrder - 1;
        Animator animator = transform.GetComponent<Animator>();
        Animator _shador_animator = _shadow.AddComponent(typeof(Animator)) as Animator;
        _shador_animator = transform.GetComponent<Animator>();
        _shador_animator.enabled = true;
        _shador_animator.runtimeAnimatorController = animator.runtimeAnimatorController;
        //Behaviour halo = transform.FindChild("Halo").GetComponent<Halo>();
    }
    void LateUpdate()
    {
        //sr.sprite = renderer.sprite;
        GameLog.LogMessage("LateUpdate");
        _shadow.transform.localPosition = Offset;
        Vector3 myVector = new Vector3(1.1f, 1.1f, 1f);
        _shadow.transform.localScale = myVector;
    }
}