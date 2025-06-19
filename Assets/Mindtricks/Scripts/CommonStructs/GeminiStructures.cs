
using System;

[Serializable]
public struct StructuredRecipe
{
    public string recipeName;
    public string recipeDescription;
}

[Serializable]
public struct StructuredScore
{
    public int score;
    public string motivation;
}

[Serializable]
public struct StructuredAnswer
{
    public string answer;
}

