using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum Operadores
{
    suma,
    resta,
    multiplicacion,
}

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtPregunta;
    [SerializeField] private Slider sliderVidas;

    [Header("Juego")]
    private int dificultad = 1;
    private int resultado = 0;
    private int digitosResultado = 0;
    private int respuestaUsuario = 0;
    private static int puntos = 0;
    private int vidas = 3;

    private string pregunta;


    void Start()
    {
        puntos = 0;

        nuevaPregunta();
    }

    private void nuevaPregunta()
    {
        // Variables calculadas
        int cantidadOperaciones = (int)Mathf.Ceil(dificultad/5.0f);

        pregunta = "";

        respuestaUsuario = 0;

        Operadores[] listaOperadores = new Operadores[cantidadOperaciones];

        int[] listaNumeros = new int[cantidadOperaciones+1];

        // Rellenamos todos los operadores y valores numéricos
        for (int i = 0; i < cantidadOperaciones; i++)
        {
            listaOperadores[i] = (Operadores) Random.Range(0, System.Enum.GetValues(typeof(Operadores)).Length - 1);
            
            listaNumeros[i] = Random.Range(0, 9 * dificultad);

        }

        listaNumeros[listaNumeros.Length-1] = Random.Range(0, 9 * dificultad);

        calcularResultado(listaOperadores, listaNumeros);

        // Mostramos por pantalla
        mostrarPregunta(listaOperadores, listaNumeros);

    }

    private void calcularResultado(Operadores[] listaOperadores, int[] listaNumeros)
    {
        resultado = listaNumeros[0];

        for (int i = 0; i < listaOperadores.Length; i++)
        {
            switch (listaOperadores[i])
            {
                case Operadores.suma:
                    resultado = resultado + listaNumeros[i + 1];

                    break;
                case Operadores.resta:
                    resultado = resultado - listaNumeros[i + 1];

                    break;
                case Operadores.multiplicacion:
                    resultado = resultado * listaNumeros[i + 1];

                    break;
            }
        }
    }

    private void mostrarPregunta(Operadores[] listaOperadores, int[] listaNumeros)
    {

        string textoMotrar = listaNumeros[0] + "";

        for (int i = 0; i < listaOperadores.Length; i++)
        {
            string operadorActual = listaOperadores[i] == Operadores.suma ? "+" : listaOperadores[i] == Operadores.resta ? "-" : "*";

            textoMotrar += " " + operadorActual + " " + listaNumeros[i+1];
        }

        int auxiliar = resultado;

        do
        {
            auxiliar = auxiliar / 10;

            digitosResultado++;
        } while (auxiliar != 0);

        textoMotrar += resultado >= 0 ? " =" : " = -";

        pregunta = textoMotrar;

        textoMotrar += string.Concat(Enumerable.Repeat(" _", digitosResultado));

        txtPregunta.text = textoMotrar;

    }

    public void recibirNumero(int numero)
    {
        digitosResultado--;

        string textoMostrar = pregunta + " " + numero;

        textoMostrar += string.Concat(Enumerable.Repeat(" _", digitosResultado));

        txtPregunta.text = textoMostrar;

        respuestaUsuario = (respuestaUsuario * 10) + numero;

        if (digitosResultado == 0)
        {
            if (validarRespuesta())
            {

                puntos++;

                if (puntos % 3 == 0) dificultad++;

                nuevaPregunta();

            } else
            {
                vidas--;

                sliderVidas.value = vidas;

                if (vidas == 0) finalizarPartida();

                nuevaPregunta();
            }
        }
    }


    private bool validarRespuesta()
    {
        return Mathf.Abs(resultado) == respuestaUsuario;
    }
    
    private void finalizarPartida()
    {

        SceneManager.LoadScene(2);

    }

    public static int getPuntos()
    {
        return puntos;
    }

}
