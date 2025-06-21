using UnityEngine;

public class Test : MonoBehaviour
{
    [TextArea(11,25)]
    public string toSend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // APIOllama.Instance.PostString(toSend, onSuccess, OnFailure);
        Debug.Log("Sending: " + toSend);
    }

    public void onSuccess(string m)
    {
        Debug.Log("Received: " + m);
    }

    public void OnFailure(string m1, string m2)
    {
        Debug.Log(m1);
        Debug.Log(m2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
