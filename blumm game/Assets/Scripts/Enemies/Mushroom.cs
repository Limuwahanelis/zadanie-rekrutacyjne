using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy,IAnimatable
{
    public List<Transform> patrolPoints = new List<Transform>();
    private List<Vector3> _patrolpositions = new List<Vector3>();
    private int _patrolPointIndex = 0;

    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<patrolPoints.Count;i++)
        {
            _patrolpositions.Add(patrolPoints[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolPoints.Count > 1) MoveToPatrolPoint();
    }


    public void MoveToPatrolPoint()
    {
        PlayAnimation("Move");
        transform.position = Vector3.MoveTowards(transform.position, _patrolpositions[_patrolPointIndex], speed * Time.deltaTime);
        //RaiseOnWalkEvent();
        if (Mathf.Abs(transform.position.x- _patrolpositions[_patrolPointIndex].x) <0.1)
        {

            if (_patrolPointIndex + 1 > _patrolpositions.Count-1) _patrolPointIndex = 0;
            else _patrolPointIndex++;

            if (_patrolpositions[_patrolPointIndex].x < transform.position.x) RotateEnemy(-1);
            else RotateEnemy(1);

        }
    }

    private void RotateEnemy(int direction)
    {
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }


    public void PlayAnimation(string name)
    {
        OnPlayAnimation?.Invoke(name);
    }

    public float GetAnimationLength(string name)
    {
        throw new NotImplementedException();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("col");
        IDamagable tmp = collision.gameObject.GetComponentInParent<IDamagable>();
        if (tmp != null)
        {
            tmp.TakeDamage(1);
        }
    }
    //private void OnTriggerEnter2D(Collision2D collision)
    //{

    //}
}
