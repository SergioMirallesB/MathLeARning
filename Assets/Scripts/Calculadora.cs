using System.Collections.Generic;

public enum Operadores
{
    Suma = 43,
    Resta = 45,
    Multiplicacion = 42,
    Division = 47
}

public static class Calculadora
{
    public static int Calcular(Operadores[] operadores, int[] numeros)
    {
        // Creamos copias para no modificar los arrays originales.
        List<int> nums = new List<int>(numeros);

        List<Operadores> ops = new List<Operadores>(operadores);


        // PASADA 1: multiplicaciones y divisiones.
        for (int i = 0; i < ops.Count; i++)
        {
            if (ops[i] == Operadores.Multiplicacion ||
                ops[i] == Operadores.Division)
            {
                int resultado;

                if (ops[i] == Operadores.Multiplicacion)
                {
                    resultado = nums[i] * nums[i + 1];
                }
                else
                {

                    resultado = nums[i] / nums[i + 1];
                }

                // Guardamos el resultado en la posición del
                // primer número y eliminamos el segundo.
                nums[i] = resultado;

                nums.RemoveAt(i + 1);
                
                ops.RemoveAt(i);

                // Revisamos de nuevo esta posición.
                i--;
            }
        }

        // PASADA 2: sumas y restas de izquierda a derecha.
        int total = nums[0];

        for (int i = 0; i < ops.Count; i++)
        {
            switch (ops[i])
            {
                case Operadores.Suma:
                    total += nums[i + 1];
                    break;

                case Operadores.Resta:
                    total -= nums[i + 1];
                    break;

                default:
                    break;
            }
        }

        return total;
    }
}
