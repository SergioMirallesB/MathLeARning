using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFinalController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntuacion;



    void Start()
    {
        puntuacion.text = "" + GameManager.getPuntos();
    }

    public void onVolverAJugar()
    {
        SceneManager.LoadScene(1);
    }

    public void onMenu()
    {
        SceneManager.LoadScene(0);
    }

}
