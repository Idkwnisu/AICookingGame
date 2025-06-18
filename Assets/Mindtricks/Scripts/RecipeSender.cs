using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;


public class RecipeSender : MonoBehaviour
{
    public APISender apiSender;

    public string requestSelected;
    [TextArea(11, 25)]
    public string step1; //getting a recipe
    
    [TextArea(11, 25)]
    public string step2; //getting a score
    
    [TextArea(11, 25)]
    public string step3; //getting a response

    public UnityEvent<string> step1Response;
    public UnityEvent<string> step2Response;
    public UnityEvent<string> step3Response;

    string sending;

    public bool addRequest = false;

    private string recipe;
    private string score;
    private string response;

    public void SendRecipe(List<Ingredient> ingredients)
    {
        sending = step1 + "\n \n Ingredients \n";
        for (int i = 0; i < ingredients.Count; i++)
        {
                sending = sending + "\n" + ingredients[i].nomeIngrediente + (ingredients[i].descrizione == "" ? "" : "-" + ingredients[i].descrizione);
        }
        apiSender.PostStringStep1(sending, ReceivedResponseStep1);    
    }

    public void ReceivedResponseStep1(string m)
    {
        Debug.Log("Response received: " + m);
        recipe = m;
        SendStep2();
        step1Response.Invoke(m);
    }

    public void SendStep2()
    {
        sending = step2 + "\n" + recipe + "\n Request: \n" + requestSelected;
        apiSender.PostStringStep2(sending, ReceivedResponseStep2);
    }

    public void ReceivedResponseStep2(string m)
    {
        score = m;
        SendStep3();
        step2Response.Invoke(m);
        Debug.Log("Response received: " + m);
    }

    public void SendStep3()
    {
        sending = step3 + "\n Recipe:" + recipe + "\n Request:" + requestSelected + "\n Score: \n" + score;
        apiSender.PostStringStep3(sending, ReceivedResponseStep3);
    }

    public void ReceivedResponseStep3(string m)
    {
        Debug.Log("Response received: " + m);
        step3Response.Invoke(m);
    }
}
