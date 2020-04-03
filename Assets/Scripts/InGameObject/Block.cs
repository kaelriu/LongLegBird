using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Transform _transform=null;
    private Rigidbody2D _rb2D = null;
    private float _time;

    public void Initialize(Vector2 newVel)                //Initialize Block Object
    {
        if(_transform==null)
        {
            _transform=this.transform;
            _rb2D=_transform.GetComponent<Rigidbody2D>();
        }
        _rb2D.velocity=newVel;
        _rb2D.constraints=RigidbodyConstraints2D.FreezeRotation;

    }


    void Update()
    {
        if(_rb2D.position.x<-10.0f||_rb2D.position.y<-10.0f) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(Tag.LEG)||other.gameObject.CompareTag(Tag.PLAYER)) 
        {
            _rb2D.GetComponent<CapsuleCollider2D>().isTrigger=true;
            other.rigidbody.GetComponent<CapsuleCollider2D>().isTrigger=true;
            _rb2D.constraints=RigidbodyConstraints2D.None;
            _rb2D.AddForce(new Vector2(Random.Range(100.0f,300.0f),0.0f));
            _rb2D.AddTorque(Random.Range(-100.0f,100.1f));
                        
            Destroy(other.gameObject, 2.0f);
        }
       
    }
}
