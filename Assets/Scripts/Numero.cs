using UnityEngine;

public class Numero : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField,Range(0,9)] private int valor;



    public void darNumero()
    {
        gameManager.recibirNumero(valor);
    }
}
