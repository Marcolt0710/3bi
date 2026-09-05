using UnityEngine;

// Mecanica de "toque indireto": o jogador encosta na ALAVANCA,
// mas o efeito acontece em OUTRO objeto, em outro lugar da fase
// (uma plataforma que estava parada passa a se mover, ou uma
// barreira desaparece). Diferente dos itens que afetam a si
// mesmos ou o jogador diretamente ao serem tocados.
//
// O colisor deste objeto precisa estar marcado como "Is Trigger".
public class Alavanca : MonoBehaviour
{
    public Plataforma[] PlataformasParaAtivar; // plataformas dormentes que essa alavanca liga
    public GameObject[] ObjetosParaDesativar;  // ex.: uma barreira/porta que deve sumir

    bool JaAcionada;

    void OnTriggerEnter2D(Collider2D objetoQueEntrou)
    {
        if (objetoQueEntrou.gameObject.tag != "Player")
        {
            return;
        }

        if (JaAcionada == true)
        {
            return;
        }

        JaAcionada = true;

        foreach (Plataforma plataforma in PlataformasParaAtivar)
        {
            if (plataforma != null)
            {
                plataforma.Ativar();
            }
        }

        foreach (GameObject objeto in ObjetosParaDesativar)
        {
            if (objeto != null)
            {
                objeto.SetActive(false);
            }
        }

        print("alavanca acionada: " + gameObject.name);
    }
}
