using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFinal : MonoBehaviour
{
    int puntosGuardados;
    public TextMeshProUGUI maxPointsText;
    
    void Start()
    {
        puntosGuardados = PlayerPrefs.GetInt("MaxScore", 0);
        maxPointsText.text = "Max Score: " + puntosGuardados.ToString();
       
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GameLevel1");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
