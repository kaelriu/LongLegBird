using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour               //Shake camera when player is dead
{
    private Vector3 _initialLoc;
    private float _shakeTime;
    private bool _flag;

    void Awake()
    {
        _initialLoc=this.transform.position;
        _shakeTime=0.0f;
        _flag=false;
    }

    public void ShakeCamera(float ShakeTime)
    {
        _shakeTime=ShakeTime;
        _flag=true;
    }

    void Update()
    {
        if(!_flag) return;

        if(_shakeTime>0.0f)
        {
            this.transform.position=Random.insideUnitSphere*(0.35f)+_initialLoc;
            _shakeTime-=Time.deltaTime;
        }
        else
        {
            _shakeTime=0.0f;
            _flag=false;
        }
    }
}
