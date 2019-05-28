using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour
{


    public TextMeshProUGUI questionDisplayText;
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;
    public dynamicPoolObject answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private float timeRemaining;
    public int questionIndex=1;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    public CanvasGroup canvasGroupQuiz;


    // Use this for initialization
    void Start()
    {
       /* dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();



        questionPool = currentRoundData.Preguntas;
        timeRemaining = currentRoundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();

        playerScore = 0;
        //questionIndex = 0;

        ShowQuestion();
        //isRoundActive = true;
        roundEndDisplay.GetComponent<CanvasGroup>().alpha = 0;
        roundEndDisplay.GetComponent<CanvasGroup>().interactable = false;
        roundEndDisplay.GetComponent<CanvasGroup>().blocksRaycasts = false;
        */
    }

    public void startQuiz()
    {

        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();



        questionPool = currentRoundData.Preguntas;
        timeRemaining = 5000;//currentRoundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        //isRoundActive = true;
        roundEndDisplay.GetComponent<CanvasGroup>().alpha = 0;
        roundEndDisplay.GetComponent<CanvasGroup>().interactable = false;
        roundEndDisplay.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    

    public void setupAnswers(QuestionData dataArray) {
        int i = 0;
        newPosition_Template.z = 1;
        RemoveAnswerButtons();
        GameObject buttonTemplate;
        foreach (AnswerData Answer in dataArray.answers) {
                
            //Debug.Log(Answer.answerText); //know if wich answer is beeing inserted into button
            buttonTemplate = answerButtonObjectPool.retrieve_And_ManageObjects();
            answerButtonGameObjects.Add(buttonTemplate);

            buttonTemplate.transform.SetParent(GameObject.FindGameObjectWithTag("answerParentContainer").transform);

           

            buttonTemplate.GetComponentInChildren<TextMeshProUGUI>().SetText(Answer.answerText);
            
            /*setup string answer in answeEventButton*/
            buttonTemplate.GetComponent<Answer_evtBtn>().answer=Answer.answerText;
            buttonTemplate.GetComponent<Answer_evtBtn>().Setup(dataArray.answers[i]);
            i++;
        }
    }

    Vector3 newPosition_Template;
    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionDisplayText.text = questionData.questionText;

        newPosition_Template = Vector3.one;

        for (int i = 0; i < questionData.answers.Length; i++) {
            GameObject button_Template = answerButtonObjectPool.retrieve_And_ManageObjects();
            answerButtonGameObjects.Add(button_Template);

            button_Template.transform.SetParent(GameObject.FindGameObjectWithTag("answerParentContainer").transform);
            button_Template.GetComponentInChildren<Text>().text = questionData.answers[i].answerText;

            button_Template.name = "Answer";

            button_Template.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            button_Template.GetComponent<RectTransform>().rotation = GameObject.FindGameObjectWithTag("answerParentContainer").transform.rotation;
            button_Template.GetComponent<RectTransform>().position = GameObject.FindGameObjectWithTag("answerParentContainer").transform.position;
            Debug.Log(newPosition_Template);

            /*setup string answer in answeEventButton*/
            button_Template.GetComponent<Answer_evtBtn>().answer = questionData.answers[i].answerText;
            button_Template.GetComponent<Answer_evtBtn>().Setup(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            playerScore = currentRoundData.pointsAddedForCorrectAnswer + playerScore;
            scoreDisplayText.text =  playerScore.ToString();
        }

            if (questionPool.Length > questionIndex + 1)
            {
                questionIndex++;
                ShowQuestion();
                
                Debug.Log(questionIndex);
            }
            else
            {
                EndRound();
            }
    }

 
    /*****************************************UPDATE*******************************************/
    // Update is called once per frame
    void Update()
    {
        if (roundEndDisplay.GetComponent<CanvasGroup>().alpha == 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)
            {
                EndRound();
            }

        }
    }

    /************************************************************************************/
    
    #region Go back to main menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainLobbyRoom");
    }
    #endregion
    
    #region End of a round of questions
    public void EndRound()
    {
        // isRoundActive = false;
        //questionDisplay.SetActive(false);
        roundEndDisplay.GetComponent<CanvasGroup>().alpha          = 1;
        roundEndDisplay.GetComponent<CanvasGroup>().interactable   = true;
        roundEndDisplay.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Codigo Agregado por Alex
        canvasGroupQuiz.alpha = 0;
        canvasGroupQuiz.blocksRaycasts = false;
        canvasGroupQuiz.interactable = false;

    }
    #endregion

    #region Update time in game
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
    }
    #endregion


}