using System;
using UnityEngine;

[System.Serializable]
public class Animation
{
    [SerializeField, Tooltip("Animation duration in seconds"), Range(0, 15)]
    private float _duration = .5f; // in seconds
    public float Duration { set => _duration = value; }

    [SerializeField, Tooltip("Animation curve")]
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField, Tooltip("Animation start offset duration"), Min(0)]
    private float _startOffsetDuration;
    public float StartOffsetDuration { set => _startOffsetDuration = value; }

    private float _timeLeft = 0f;
    private float _offsetTimeLeft = 0f;

    private Action<float> _updateAction;
    public Action<float> UpdateAction { set => _updateAction = value; }

    private Action _endAction;
    public Action EndAction { set => _endAction = value; }

    public void StartAnimation()
    {
        _timeLeft = _duration;
    }

    public void StartOffset()
    {
        _offsetTimeLeft = _startOffsetDuration;
    }

    public void Update(float deltaTime)
    {
        if (_offsetTimeLeft > 0)
        {
            _offsetTimeLeft -= deltaTime;

            if (_offsetTimeLeft <= 0)
            {
                StartAnimation();
            }

            return;
        }

        if (_timeLeft > 0)
        {
            _timeLeft -= deltaTime;

            float factor = 1 - (_timeLeft / _duration);
            factor = _curve.Evaluate(factor);

            _updateAction.Invoke(factor);

            if (_timeLeft <= 0)
            {
                End();
            }
        }
    }

    public void End()
    {
        _timeLeft = 0;

        _endAction.Invoke();
    }

    public void StopAnimation()
    {
        _timeLeft = 0;
    }
}
