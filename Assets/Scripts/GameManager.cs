using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtPregunta;
    [SerializeField] private Slider sliderVidas;
    [SerializeField] private ScreenManager scManager;

    [Header("Juego")]
    private int dificultad = 1;
    private int resultado = 0;
    private int digitosResultado = 0;
    private int digitosRestantes = 0;
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

        // Limpiar las variables
        pregunta = "";

        respuestaUsuario = 0;

        digitosResultado = 0;


        // Variables calculadas
        int cantidadOperaciones = (int)Mathf.Ceil(dificultad/5.0f);
        
        Operadores[] listaOperadores = new Operadores[cantidadOperaciones];
        
        int[] listaNumeros = new int[cantidadOperaciones + 1];

        // Esto guarda los operadores disponibles (en este caso [+,-,*,/])
        Operadores[] valores = (Operadores[])System.Enum.GetValues(typeof(Operadores));

        // Rellenamos todos los operadores y valores numéricos

        for (int i = 0; i < cantidadOperaciones; i++)
        {
            // Operador aleatorio
            listaOperadores[i] = valores[Random.Range(0, valores.Length)];
            
            // Numero aleatorio
            listaNumeros[i] = Random.Range(0, 9 * dificultad);

        }

        // Como siempre hay uno más, lo rellenamos luego
        listaNumeros[listaNumeros.Length-1] = Random.Range(0, 9 * dificultad);


        // Calculamos el resultado
        resultado = Calculadora.Calcular(listaOperadores, listaNumeros);
        //calcularResultado(listaOperadores, listaNumeros);


        // Calculamos cuantos digitos tiene el resultado
        int auxiliar = resultado;

        do // Pongo un do-while por si el resultado es 0, que mínimo siempre tenga 1
        {
            auxiliar = auxiliar / 10;

            digitosResultado++;

        } while (auxiliar != 0);

        digitosRestantes = digitosResultado;


        // Montamos la pregunta
        montarPregunta(listaOperadores, listaNumeros);

        // Escribimos en pantalla
        scManager.EscribirOperacion(pregunta);

    }

    /*private void calcularResultado(Operadores[] listaOperadores, int[] listaNumeros)
    {
        // De momento no sigue el orden de prioridad (* y /, luego + y -)
        // TODO (?) si sobra tiempo
        resultado = listaNumeros[0];

        for (int i = 0; i < listaOperadores.Length; i++)
        {
            switch (listaOperadores[i])
            {
                case Operadores.Suma:
                    resultado = resultado + listaNumeros[i + 1];

                    break;
                case Operadores.Resta:
                    resultado = resultado - listaNumeros[i + 1];

                    break;
                case Operadores.Multiplicacion:
                    resultado = resultado * listaNumeros[i + 1];

                    break;
            }
        }
    }*/

    private void montarPregunta(Operadores[] listaOperadores, int[] listaNumeros)
    {

        string textoMotrar = listaNumeros[0] + "";

        for (int i = 0; i < listaOperadores.Length; i++)
        {
            char operadorActual = (char)listaOperadores[i];

            textoMotrar += " " + operadorActual + " " + listaNumeros[i+1];

        }

        textoMotrar += resultado >= 0 ? " =" : " = -";

        pregunta = textoMotrar;

        textoMotrar += string.Concat(Enumerable.Repeat(" _", digitosRestantes));

        txtPregunta.text = textoMotrar;

    }

    public void recibirNumero(int numero)
    {
        digitosRestantes--;

        respuestaUsuario = (respuestaUsuario * 10) + numero;

        string textoMostrar = pregunta;

        char[] valoresStr = respuestaUsuario.ToString().ToCharArray();

        for (int i = 0; i < valoresStr.Length; i++)
        {

            textoMostrar += " " + valoresStr[i];

        }

        textoMostrar += string.Concat(Enumerable.Repeat(" _", digitosRestantes));

        txtPregunta.text = textoMostrar;


        if (digitosRestantes == 0)
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

    public void OnBorrar()
    {

        if (digitosRestantes == digitosResultado) return;

        digitosRestantes++;

        string textoMostrar = pregunta;

        respuestaUsuario /= 10;

        if (digitosRestantes == digitosResultado)
        {

            textoMostrar += string.Concat(Enumerable.Repeat(" _", digitosRestantes));

        }
        else
        {
            char[] valoresStr = respuestaUsuario.ToString().ToCharArray();


            for (int i = 0; i < valoresStr.Length; i++)
            {

                textoMostrar += " " + valoresStr[i];

            }

            textoMostrar += string.Concat(Enumerable.Repeat(" _", digitosRestantes));

        }

        txtPregunta.text = textoMostrar;
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
