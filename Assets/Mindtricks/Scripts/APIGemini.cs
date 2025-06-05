using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public struct GeminiInput
{
    public GeminiParts[] contents;
}

[Serializable]
public struct GeminiParts
{
    public GeminiText[] parts;
}

[Serializable]
public struct GeminiText
{
    public string text;
}

[Serializable]
public class Candidate
{
    public Content content;
    public string finishReason;
    public double avgLogprobs; }

    [Serializable]
    public class CandidatesTokensDetail
    {
        public string modality;
        public int tokenCount;
    }
[Serializable]
public class Content
{
    public List<Part> parts;
    public string role;
}
[Serializable]
public class Part
{
    public string text;
}

[Serializable]
public class PromptTokensDetail
{
    public string modality;
    public int tokenCount;
}

[Serializable]
public class GeminiRoot
{
    public List<Candidate> candidates;
    public UsageMetadata usageMetadata;
    public string modelVersion;
}

public class UsageMetadata
{
    public int promptTokenCount { get; set; }
    public int candidatesTokenCount { get; set; }
    public int totalTokenCount { get; set; }
    public List<PromptTokensDetail> promptTokensDetails { get; set; }
    public List<CandidatesTokensDetail> candidatesTokensDetails { get; set; }
}


        public class APIGemini : APISender
        {

            public string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=";

            public string key = "";
            GeminiRoot output;
            GeminiInput input;
            string json;
            UnityWebRequest request;

            public UnityEvent<string> error;

            private void Awake()
            {
                input = new GeminiInput();
                input.contents = new GeminiParts[1];
                input.contents[0].parts = new GeminiText[1];
            }

            public void ErrorDefault(string m1, string m2)
            {
                Debug.Log(m1);
                Debug.Log(m2);
            }

            protected override IEnumerator PostStringCoroutine(string m, Action<string> onSuccess = null, Action<string, string> onFailure = null)
            {
                input.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(input);

                request = new UnityWebRequest(url + key, "POST");
                request.SetRequestHeader("Content-Type", "application/json");
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
                    Debug.Log(request.downloadHandler.text);
                    output = JsonUtility.FromJson<GeminiRoot>(request.downloadHandler.text);
                    onSuccess.Invoke(output.candidates[0].content.parts[0].text);
                }
            }
        }

