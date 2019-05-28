using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Answer_evtBtn : MonoBehaviour
{

    //public Text answerText;

    public Text answerText;
    private AnswerData answerData;

    private QuizManager quiz_controller;
    public string answer;
    public bool isCorrect;
    // Use this for initialization
    void Start()
    {
    quiz_controller = GameObject.FindGameObjectWithTag("quizManager").GetComponent<QuizManager>();
    answerText = GetComponentInChildren<Text>();
    }






    public void Setup(AnswerData data)
    {
        answerData = data;        
      
        answer = answerData.answerText; // setup string value of this class
        isCorrect = answerData.isCorrect;

       // answerText.text = answerData.answerText;
    }


    public void HandleClick()
    {
        quiz_controller.AnswerButtonClicked(answerData.isCorrect);
        Debug.Log("click made: " + answerData.isCorrect);

    }



    public void clickedAnswer() {
        quiz_controller.AnswerButtonClicked(answerData.isCorrect);
    }

}
