using UnityEngine;

public class Teletransporte : MonoBehaviour
{
    // Variáveis públicas que podem ser ajustadas no Inspector
    public Vector2 Destino;      // Para onde o jogador é enviado ao encostar

    public bool ZerarVelocidade; // Se true, o jogador chega PARADO no destino.
    // Se false, ele leva junto a velocidade que tinha antes de entrar — o que
    // pode fazer ele escorregar ou continuar subindo assim que chega.

    // OnTriggerEnter2D é chamado quando outro colisor ENTRA neste.
    // Só funciona se o colisor deste objeto estiver marcado como "Is Trigger":
    // trigger não empurra nem bloqueia, o jogador atravessa e o evento dispara.
    void OnTriggerEnter2D(Collider2D objetoQueEntrou)
    {
        // Só o jogador teletransporta. Sem esta checagem, uma caixa ou
        // qualquer outro corpo físico também seria levado junto.
        if (objetoQueEntrou.gameObject.tag != "Player")
        {
            return;
        }

        print("teletransportando para: " + Destino);

        objetoQueEntrou.transform.position = Destino;

        if (ZerarVelocidade == true)
        {
            Rigidbody2D corpoRigido = objetoQueEntrou.GetComponent<Rigidbody2D>();

            if (corpoRigido != null)
            {
                corpoRigido.velocity = Vector2.zero;
            }
        }
    }
}
