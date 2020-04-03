using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    private Transform _transform = null;
    private Rigidbody2D _rb2D = null;

    public void Initialize()                //Initialize Object
    {
        if(_transform==null)
        {
            _transform=this.transform;
            _rb2D=_transform.GetComponent<Rigidbody2D>();
        }
    }
 

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(Tag.OBSTACLE))
        {
            _rb2D.constraints=RigidbodyConstraints2D.None;
            _rb2D.AddForce(new Vector2(Random.Range(-300.0f,-100.0f),0.0f));
            _rb2D.AddTorque(Random.Range(-100.0f,100.1f));
            gameObject.GetComponentInParent<Player>().DeleteLeg();
        }
    }
}
