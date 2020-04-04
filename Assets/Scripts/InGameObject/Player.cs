using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Transform _transform = null;
    private Rigidbody2D _rb2D = null;
    private GameManager _gameManager=null;
    private int _legNum;
    private AudioSource _deadSound;
    public void Initialize()                //Initialize Player Object
    {
        _gameManager=GameManager.Instance;

        if(_transform==null)
        {
            _transform=this.transform;
            _rb2D=_transform.GetComponent<Rigidbody2D>();
        }
        
        _legNum=0;
        _deadSound=_transform.GetComponent<AudioSource>();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&EventSystem.current.IsPointerOverGameObject(0)==false)
        {
            Vector2 PrevPos=_rb2D.position;
            if(_legNum<5&&_rb2D.position.x<4.0f) SpawnLeg(PrevPos);
        }
    }


    public GameObject _leg;

    public void SpawnLeg(Vector2 PrevPos)
    {
        _rb2D.position=new Vector2(PrevPos.x, PrevPos.y+0.78f);

        GameObject Leg=Instantiate(_leg, new Vector2(PrevPos.x, PrevPos.y+0.08f), _transform.rotation);
        Leg.GetComponent<Leg>().Initialize();
        Leg.transform.SetParent(_transform);
        _legNum++;
    }

    public void DeleteLeg()
    {
        if(_legNum<0) _legNum=0;
        else _legNum--;
    }

    void OnCollisionEnter2D(Collision2D other)      //Game over
    {
        if(other.gameObject.CompareTag(Tag.OBSTACLE))
        {
            _gameManager.ReturnCam().GetComponent<CameraShaker>().ShakeCamera(0.3f);
            _rb2D.constraints=RigidbodyConstraints2D.None;
            _rb2D.AddForce(new Vector2(-200.0f,0.0f));
            _rb2D.AddTorque(200.0f);
            _deadSound.Play();
            _gameManager.FinishGame();
        }
    }
}
