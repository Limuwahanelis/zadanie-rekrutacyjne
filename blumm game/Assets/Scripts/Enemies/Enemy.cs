using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HealthSystem))]
public abstract class Enemy : MonoBehaviour,IAnimatable
{
    protected HealthSystem hpSys;
    public float speed;
    public int dmg;
    public int collisionDmg;

    protected bool _isAlive = true;
    protected bool _isIdle = false;

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;

    protected EnemyEnums.State currentState;
    protected Stack<EnemyEnums.State> states = new Stack<EnemyEnums.State>();
    protected virtual void SetPlayerInRange() {}
    protected virtual void SetPlayerNotInRange() {}
    protected virtual void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
    }


    protected virtual void StopCurrentActions()
    {
        StopAllCoroutines();
    }
    protected virtual void ResumeActions()
    {
        currentState = states.Pop();
    }



    protected IEnumerator StayIdleCor(int numberOfIdleCycles = 1)
    {
        _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(numberOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
    }
    protected IEnumerator WaitSomeTimeAndDoSmth(float timeToWait, Action functionToPerform)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToPerform();
    }


    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }
    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }
}