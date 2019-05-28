using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenario : MonoBehaviour {

    public string name;
    public bool canBeSeen;
    public GameObject Scenario;
    int indexScenario;
    public Transform Player;


    public void ActivateThisScenario(bool activate) {
        canBeSeen = activate;
        Scenario.SetActive(canBeSeen);
    }


    public void deActivateThisScenario()
    {
        canBeSeen = false;
        Scenario.SetActive(canBeSeen);
    }

    public void GetPlayerPosition() {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    public void CheckPosition( int indexScenario) {
        switch (indexScenario) {
            case 1: {
                    if (this.name == "TecnologiaLTH" && Scenario.transform.position == Player.transform.position ) {
                        canBeSeen = true;
                        ActivateThisScenario(canBeSeen);
                        Debug.Log("inside tecnologia LTH");
                    }
                    else
                    {
                        deActivateThisScenario();
                    }
                    break;
                }
            case 2:
                {
                    if (this.name == "Optima" )
                    {
                        canBeSeen = true;
                        ActivateThisScenario(canBeSeen);
                        if (Player.transform.position == Scenario.transform.position)
                        {
                            Debug.Log("inside Optima");
                        }
                    }
                    else
                    {
                        deActivateThisScenario();
                    }
                    break;
                }
            case 3:
                {
                    if (this.name == "LineaProductos")
                    {
                        canBeSeen = true;
                        ActivateThisScenario(canBeSeen);
                        if (Player.transform.position == Scenario.transform.position) {
                            
                            Debug.Log("inside Linea de productos");
                        }

                    }
                    else
                    {
                        deActivateThisScenario();
                    }
                    break;
                }
            case 4:
                {
                    if (this.name == "360Visor")
                    {
                        canBeSeen = true;
                        ActivateThisScenario(canBeSeen);
                        if (Player.transform.position == Scenario.transform.position)
                        {
                            Debug.Log("inside 360 videos");
                        }
                    }
                    else
                    {
                        deActivateThisScenario();
                    }
                    break;
                }
            case 5:
                {
                    if (this.name == "Quizzer")
                    {
                        canBeSeen = true;
                        ActivateThisScenario(canBeSeen);
                        if (Player.transform.position == Scenario.transform.position)
                        {
                            Debug.Log("inside Quizzer");
                        }
                    }
                    else {
                        deActivateThisScenario();
                    }
                    break;
                }
            default:
                canBeSeen = false;               
                Debug.Log("Incorrect Scenario");
                break;
        }

    }

    public void getIndexScenario(int index) {
        indexScenario = index;
        Debug.Log("Activate this scenario: "+ indexScenario);
        CheckPosition(indexScenario);
    }


    public void LateUpdate()
    {
        GetPlayerPosition();

    }
}
