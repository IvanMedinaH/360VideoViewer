using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioVisibilityManager : MonoBehaviour {

    [Header("Scenarios")]
    public scenario Scenario_LTHtechnology;
    public scenario Scenario_ProductLine;
    public scenario Scenario_Optima;
    public scenario Visor360;
    public scenario Quizzer;

    [Header("------------------------------")]
    [Header("Cabinet")]
    public GameObject cabinet;
    public bool isVisible;
    public bool canBeRevealed;
    public bool canHide;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
