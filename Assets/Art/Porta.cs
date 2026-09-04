using UnityEngine;
using UnityEngine.SceneManagement;

public class Porta : MonoBehaviour
{
    // Variável pública que pode ser ajustada no Inspector
    public string CenaParaCarregar; // Nome EXATO da cena que a porta abre.
    // A cena precisa estar listada em File > Build Settings,
    // senão o LoadScene não encontra e a porta não faz nada.

    // OnTriggerEnter2D é chamado quando outro colisor ENTRA neste.
    // Só funciona se o colisor da porta estiver marcado como "Is Trigger":
    // assim o jogador atravessa a porta em vez de esbarrar nela.
    void OnTriggerEnter2D(Collider2D objetoQueEntrou)
    {
        // Só o jogador abre a porta. Sem isto, uma caixa empurrada
        // até aqui também trocaria de fase.
        if (objetoQueEntrou.gameObject.tag != "Player")
        {
            return;
        }

        if (CenaParaCarregar == "")
        {
            print("Porta sem cena definida no objeto: " + gameObject.name);
            return;
        }

        print("porta: carregando a cena " + CenaParaCarregar);

        SceneManager.LoadScene(CenaParaCarregar);
    }
}
