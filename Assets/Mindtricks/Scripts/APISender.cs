using System;
using System.Collections;
using UnityEngine;

public abstract class APISender : MonoBehaviour
{
    public abstract void PostStringStep1(string m, Action<string> onSuccess, Action<string, string> onFailure = null);
    public abstract void PostStringStep2(string m, Action<string> onSuccess, Action<string, string> onFailure = null);
    public abstract void PostStringStep3(string m, Action<string> onSuccess, Action<string, string> onFailure = null);
}
