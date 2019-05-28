using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    public RoundData[] allRoundData;
    public AnswerData[] allAnswerData;
    public int Quiz_id;
    public CanvasGroup canvasGroup;

   public void quizSelect(int id)
    {
        Quiz_id = id;
    }

    public void ActiveCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;
    }


    public void hideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[Quiz_id];
    }

    public AnswerData GetCurrentAnswerData()
    {
        return allAnswerData[0];
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
       // SceneManager.LoadScene("MenuScreen");
    }


}