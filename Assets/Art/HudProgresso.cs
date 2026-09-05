using UnityEngine;

// HUD simples (OnGUI, sem Canvas) mostrando quanto tempo falta
// para o boost de pulo (bonusPuloTemporario) acabar.
public class HudProgresso : MonoBehaviour
{
    public Personagem Jogador;

    void Start()
    {
        if (Jogador == null)
        {
            GameObject jogadorEncontrado = GameObject.FindGameObjectWithTag("Player");

            if (jogadorEncontrado != null)
            {
                Jogador = jogadorEncontrado.GetComponent<Personagem>();
            }
        }
    }

    void OnGUI()
    {
        if (Jogador == null)
        {
            return;
        }

        if (Jogador.TempoRestanteBoost > 0f)
        {
            GUI.Label(new Rect(10, 10, 260, 25), "Boost de pulo: " + Jogador.TempoRestanteBoost.ToString("F1") + "s");
        }
    }
}
