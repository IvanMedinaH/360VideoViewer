using UnityEngine;
using System.Collections;

[System.Serializable]
public class RoundData
{
    public string nombre;
    public int timeLimitInSeconds;
    public int pointsAddedForCorrectAnswer;
    public QuestionData[] Preguntas;

}