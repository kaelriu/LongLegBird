using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgScroll : MonoBehaviour                  //Repeat and scroll background image
{
    public float _scrollSpeed;
    private float _offset;
    private Renderer _renderer;

    void Awake()
    {
        _renderer=this.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        _offset+=Time.deltaTime*_scrollSpeed;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(_offset, 0.0f));
    }
}
