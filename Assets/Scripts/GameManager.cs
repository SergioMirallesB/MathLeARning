using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [SerializeField] private ScreenManager scManager;
    [SerializeField] private AudioClip errorAudio;
    [SerializeField] private AudioClip pointAudio;
    [SerializeField] private AudioSource audioSrc;

    [Header("Juego")]
    private int dificultad = 1;
    private int resultado = 0;
    private int digitosResultado = 0;
    private int digitosRestantes = 0;
    private int respuestaUsuario = 0;
    private static int puntos = 0;
    private int vidas = 3;
    private float tiempoPregunta = 32f;
    private float tmpPregRestante = 0f;
    private float tiempoUpdate = 0.1f;
    private float tmpUpdateRestante = 0f;

    private string pregunta;



    void Start()
    {
        puntos = 0;

        nuevaPregunta();
    }

    private void Update()
    {

        tmpPregRestante -= Time.deltaTime;

        tmpUpdateRestante -= Time.deltaTime;

        if (tmpUpdateRestante <= 0)
        {
            tmpUpdateRestante = tiempoUpdate;

            scManager.UpdateTimer(tmpPregRestante);

        }

        if (tmpPregRestante <= 0)
        {
            Fallar();
        }

    }



    private void nuevaPregunta()
    {

        // Limpiar las variables
        pregunta = "";

        respuestaUsuario = 0;

        digitosResultado = 0;

        tmpPregRestante = tiempoPregunta;

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
            listaOperadores[i] = valores[Random.Range(0, valores.Length -2)];
            
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


        // Montamos la pregunta y
        // Escribimos en pantalla
        scManager.EscribirOperacion(montarPregunta(listaOperadores, listaNumeros));

    }

    private string montarPregunta(Operadores[] listaOperadores, int[] listaNumeros)
    {

        pregunta = listaNumeros[0] + "";

        for (int i = 0; i < listaOperadores.Length; i++)
        {
            char operadorActual = (char)listaOperadores[i];

            pregunta += " " + operadorActual + " " + listaNumeros[i+1];

        }

        pregunta += resultado >= 0 ? " =" : " = -";

        return pregunta + string.Concat(Enumerable.Repeat(" _", digitosRestantes));

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

        scManager.EscribirOperacion(textoMostrar);


        if (digitosRestantes == 0)
        {
            if (validarRespuesta())
            {

                audioSrc.PlayOneShot(pointAudio);

                puntos++;

                if (puntos % 3 == 0) dificultad++;

                StartCoroutine(scManager.Resultado(true));

                nuevaPregunta();

            } else
            {

                Fallar();

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

        scManager.EscribirOperacion(textoMostrar);
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

    private void Fallar()
    {
        audioSrc.PlayOneShot(errorAudio);


        vidas--;

        scManager.ActualizarVidas(vidas);

        if (vidas == 0) finalizarPartida();


        StartCoroutine(scManager.Resultado(false));

        nuevaPregunta();
    }

}
