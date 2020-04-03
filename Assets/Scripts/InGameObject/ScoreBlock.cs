using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBlock : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioSource _scoreSound;
    
    public void Initialize(Vector2 newVel)
    {
        _gameManager=GameManager.Instance;

        this.GetComponent<Rigidbody2D>().velocity=newVel;
        _scoreSound=this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(this.GetComponent<Rigidbody2D>().position.x<-10.0f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(Tag.PLAYER))
        {
            _gameManager.AddScore();
            _scoreSound.Play();
            Destroy(this.gameObject, 0.7f);
        }
    }
}
