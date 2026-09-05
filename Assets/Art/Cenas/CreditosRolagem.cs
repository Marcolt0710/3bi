using UnityEngine;

public class CreditosRolagem : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidade = 2f;
    public float posicaoFinalY = 0f;
    public float tempoEspera = 2f;

    private bool movendo = false;

    void Start()
    {
        Invoke("IniciarMovimento", tempoEspera);
    }

    void IniciarMovimento()
    {
        movendo = true;
    }

    void Update()
    {
        if (!movendo)
            return;

        Vector3 destino = new Vector3(
            transform.position.x,
            posicaoFinalY,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidade * Time.deltaTime
        );

        // Para exatamente na posição final
        if (Mathf.Approximately(transform.position.y, posicaoFinalY))
        {
            transform.position = destino;
            movendo = false;
        }
    }
}
