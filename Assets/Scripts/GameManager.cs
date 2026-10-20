using System.Linq;
using TMPro;
using UnityEngine;

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

    [Header("Juego")]
    private int dificultad = 1;
    private int resultado = 0;
    private int respuestaUsuario = 0;
    private int puntos = 0;


    void Start()
    {
        nuevaPregunta();
    }

    private void nuevaPregunta()
    {
        // Variables calculadas
        int cantidadOperaciones = (int)Mathf.Ceil(dificultad/5.0f);

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

        int count = 0;
        int auxiliar = resultado;
        while (auxiliar != 0)
        {
            auxiliar = auxiliar / 10;
            ++count;
        }

        textoMotrar += resultado >= 0 ? " =" : " = -";


        textoMotrar += string.Concat(Enumerable.Repeat(" _", count));

        txtPregunta.text = textoMotrar;

    }

    public void recibirNumero(int numero)
    {
        txtPregunta.text = "" + numero;
    }

    

}
