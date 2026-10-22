using UnityEngine;
using UnityEngine.SceneManagement;

// Controlador de la interfaz
public class MenuInicialManager : MonoBehaviour
{
    // Métodos para las acciones de los botones

    public void onIniciarJuego()
    {
        SceneManager.LoadScene("Juego");
        print("Hola, prueba!");
    }

    public void onSalir()
    {
        Debug.Log("Salimos");
        Application.Quit();
    }

    // ----------------------------------------
}