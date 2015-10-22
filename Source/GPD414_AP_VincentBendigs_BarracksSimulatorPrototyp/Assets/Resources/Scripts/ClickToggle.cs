using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[Serializable]
public class ClickToggle : MonoBehaviour
{
    [SerializeField]
    private toggleEvent toggleEvent;

    private bool value;

    public void Reset()
    {
        value = false;
        toggleEvent.Invoke(value);
    }

    private void OnDisable()
    {
        Reset();
    }
    public void Handle()
    {
        toggleEvent.Invoke(value = !value);
    }

}
[Serializable]
public class toggleEvent : UnityEvent<bool>
{

}
