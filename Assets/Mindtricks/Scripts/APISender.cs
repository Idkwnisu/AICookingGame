using System;
using System.Collections;
using UnityEngine;

public abstract class APISender : MonoBehaviour
{
    public virtual void PostString(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
    {
        StartCoroutine(PostStringCoroutine(m, onSuccess, onFailure));
    }

    protected abstract IEnumerator PostStringCoroutine(string m, Action<string> onSuccess, Action<string, string> onFailure = null);
}
