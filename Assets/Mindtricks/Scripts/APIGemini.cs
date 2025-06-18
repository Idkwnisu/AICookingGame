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
    public string generationConfig;
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
public struct GeminiGenerationConfigStep1
{
    public string responseMimeType;
    public ResponseSchemaStep1 responseSchema;
}

[Serializable]
public struct ResponseSchemaStep1
{
    public string type;
    public PropertiesStep1 properties;
}


[Serializable]
public struct PropertiesStep1
{
    public SingleProperty recipeName;
    public SingleProperty recipeDescription;
}


[Serializable]
public struct GeminiGenerationConfigStep2
{
    public string responseMimeType;
    public ResponseSchemaStep2 responseSchema;
}


[Serializable]
public struct ResponseSchemaStep2
{
    public string type;
    public PropertiesStep2 properties;
}


[Serializable]
public struct PropertiesStep2
{
    public SingleProperty score;
    public SingleProperty motivation;
}


[Serializable]
public struct GeminiGenerationConfigStep3
{
    public string responseMimeType;
    public ResponseSchemaStep3 responseSchema;
}


[Serializable]
public struct ResponseSchemaStep3
{
    public string type;
    public PropertiesStep3 properties;
}

[Serializable]
public struct PropertiesStep3
{
    public SingleProperty answer;
}

public struct SingleProperty
{
    public string type;
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

            public TextAsset key;
            GeminiRoot output;
            GeminiInput inputStep1;
            GeminiInput inputStep2;
            GeminiInput inputStep3;
            string json;
            UnityWebRequest request;

            public UnityEvent<string> error;

            private void Awake()
            {
                inputStep1 = new GeminiInput();
                inputStep1.contents = new GeminiParts[1];
                inputStep1.contents[0].parts = new GeminiText[1];
                GeminiGenerationConfigStep1 geminiStep1 = new GeminiGenerationConfigStep1();
                geminiStep1.responseMimeType = "application/json";
                geminiStep1.responseSchema = new ResponseSchemaStep1();
                geminiStep1.responseSchema.type = "OBJECT";
                geminiStep1.responseSchema.properties = new PropertiesStep1();
                geminiStep1.responseSchema.properties.recipeDescription = new SingleProperty();
                geminiStep1.responseSchema.properties.recipeDescription.type = "STRING";
                geminiStep1.responseSchema.properties.recipeName = new SingleProperty();
                geminiStep1.responseSchema.properties.recipeName.type = "STRING";
                inputStep1.generationConfig = JsonUtility.ToJson(geminiStep1);


                inputStep2 = new GeminiInput();
                inputStep2.contents = new GeminiParts[1];
                inputStep2.contents[0].parts = new GeminiText[1];
                GeminiGenerationConfigStep2 geminiStep2 = new GeminiGenerationConfigStep2();
                geminiStep2.responseMimeType = "application/json";
                geminiStep2.responseSchema = new ResponseSchemaStep2();
                geminiStep2.responseSchema.type = "OBJECT";
                geminiStep2.responseSchema.properties = new PropertiesStep2();
                geminiStep2.responseSchema.properties.score = new SingleProperty();
                geminiStep2.responseSchema.properties.score.type = "INTEGER";
                geminiStep2.responseSchema.properties.motivation = new SingleProperty();
                geminiStep2.responseSchema.properties.motivation.type = "STRING";
                inputStep2.generationConfig = JsonUtility.ToJson(geminiStep2);

                inputStep3 = new GeminiInput();
                inputStep3.contents = new GeminiParts[1];
                inputStep3.contents[0].parts = new GeminiText[1];
                GeminiGenerationConfigStep3 geminiStep3 = new GeminiGenerationConfigStep3();
                geminiStep3.responseSchema = new ResponseSchemaStep3();
                geminiStep3.responseSchema.type = "OBJECT";
                geminiStep3.responseSchema.properties = new PropertiesStep3();
                geminiStep3.responseMimeType = "application/json";
                geminiStep3.responseSchema.properties.answer = new SingleProperty();
                geminiStep3.responseSchema.properties.answer.type = "STRING";
                inputStep3.generationConfig = JsonUtility.ToJson(geminiStep3);


    }

            public void ErrorDefault(string m1, string m2)
            {
                Debug.Log(m1);
                Debug.Log(m2);
            }

            public override void PostStringStep1(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep1.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep1);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
            }
            public override void PostStringStep2(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep2.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep2);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
             }
            public override void PostStringStep3(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep3.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep3);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
            }

            protected IEnumerator PostStringCoroutine(string m, Action<string> onSuccess = null, Action<string, string> onFailure = null)
            {

                request = new UnityWebRequest(url + key.text, "POST");
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

