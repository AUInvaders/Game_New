using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI PointsText;
    public void Setup()
    {
        gameObject.SetActive(true);

        PointsText.text = ScoreManager.score.ToString() + " Points";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        PlayerShip.Health = 100;

        ScoreManager.score = 0;

        Weapon.FireRate.canShoot1 = true;
        Weapon.FireRate.canShoot2 = true;
        Weapon.FireRate.canShoot3 = true;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(3); 

        PlayerShip.Health = 100;

        ScoreManager.score = 0;

        Weapon.FireRate.canShoot1 = true;
        Weapon.FireRate.canShoot2 = true;
        Weapon.FireRate.canShoot3 = true;
    }
}
