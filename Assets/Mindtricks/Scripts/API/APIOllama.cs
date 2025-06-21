using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public struct OllamaInput
{
    public string model;
    public string prompt;
    public bool stream;
}

[Serializable]
public struct OllamaOutput
{
    public string model;
    public string response;
    public bool done;
}

public class APIOllama : APISender
{

    public string url = "http://localhost:11434/api/generate";

    public string model = "gemma3";
    OllamaOutput output;
    OllamaInput input;
    string json;
    UnityWebRequest request;

    public UnityEvent<string> error;


    private void Awake()
    {


            input = new OllamaInput();
            input.model = model;
            input.stream = false;
    }

    public void ErrorDefault(string m1, string m2)
    {
        Debug.Log(m1);
        Debug.Log(m2);
    }

    public override void PostStringStep1(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
    {
        StartCoroutine(PostSimpleStringCoroutine(m, onSuccess, onFailure));
    }

    public override void PostStringStep2(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
    {
        StartCoroutine(PostSimpleStringCoroutine(m, onSuccess, onFailure));
    }

    public override void PostStringStep3(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
    {
        StartCoroutine(PostSimpleStringCoroutine(m, onSuccess, onFailure));
    }

    protected IEnumerator PostSimpleStringCoroutine(string m, Action<string> onSuccess = null, Action<string, string> onFailure = null)
    {
        input.prompt = m;

        json = JsonUtility.ToJson(input);

        request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        if (request.result != UnityWebRequest.Result.Success)
        {
            onFailure?.Invoke(request.error, request.downloadHandler.text);
            error.Invoke(request.downloadHandler.text);
            if (onFailure == null) 
                ErrorDefault(request.error, request.downloadHandler.text);
        }
        else
        {
            output = JsonUtility.FromJson<OllamaOutput>(request.downloadHandler.text);
            onSuccess?.Invoke(output.response);
        }
    }

}
