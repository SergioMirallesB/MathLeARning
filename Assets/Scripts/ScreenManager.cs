using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [Header("Elementos UI")]
    [SerializeField] private TextMeshProUGUI txtPregunta;
    [SerializeField] private TextMeshProUGUI txtBien;
    [SerializeField] private TextMeshProUGUI txtMal;
    [SerializeField] private Slider sliderVidas;
    [SerializeField] private Image panelRespuestaBien;
    [SerializeField] private Image panelRespuestaMal;

    private void Start()
    {

        panelRespuestaBien.enabled = false;
        panelRespuestaMal.enabled  = false;
        
        txtBien.enabled  = false;
        txtMal.enabled = false;

    }

    public void EscribirOperacion(string operacion)
    {

        txtPregunta.text = operacion;

    }


    public void ActualizarVidas(int vidas)
    {

        sliderVidas.value = vidas;

    }

    public IEnumerator Resultado(bool respuestaBien)
    {

        Image pantallaMostrar = respuestaBien ? panelRespuestaBien : panelRespuestaMal;
        TextMeshProUGUI txt = respuestaBien ? txtBien : txtMal;

        pantallaMostrar.enabled = true;
        txt.enabled = true;

        yield return new WaitForSeconds(1f);

        pantallaMostrar.enabled = false;
        txt.enabled = false;

    }
}
