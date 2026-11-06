using UnityEngine;
using UnityEngine.SceneManagement;

// Controlador de la interfaz
public class MenuInicialManager : MonoBehaviour
{
    [SerializeField] private Canvas instrucciones;

    private void Start()
    {


        OcultarCanvas();

    }


    // Métodos para las acciones de los botones

    public void onIniciarJuego()
    {
        SceneManager.LoadScene("Juego");
    }

    public void OnComoJugar()
    {

        MostrarCanvas();

    }

    public void onSalir()
    {
        Application.Quit();
    }

    public void OnVolver()
    {

        OcultarCanvas();

    }

    // ----------------------------------------

    private void OcultarCanvas()
    {

        CanvasGroup grupo = instrucciones.GetComponent<CanvasGroup>();

        grupo.alpha = 0f;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;

    }

    private void MostrarCanvas()
    {

        CanvasGroup grupo = instrucciones.GetComponent<CanvasGroup>();

        grupo.alpha = 1f;
        grupo.interactable = true;
        grupo.blocksRaycasts = true;

    }
}