using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoCena : MonoBehaviour
{
    // Variável pública que pode ser ajustada no Inspector
    public string CenaParaCarregar; // Nome EXATO da cena a carregar ao clicar.
    // A cena precisa estar listada em File > Build Settings,
    // senão o LoadScene não encontra e o clique não faz nada.

    // OnMouseDown é chamado pela Unity quando o jogador clica com o mouse
    // em cima do colisor deste objeto. Funciona com Collider2D normalmente,
    // desde que exista uma câmera enxergando o objeto.
    // Como este objeto não tem sprite nenhum, o colisor é uma área invisível
    // posicionada exatamente por cima do botão desenhado na imagem de fundo.
    void OnMouseDown()
    {
        if (CenaParaCarregar == "")
        {
            print("BotaoCena sem cena definida no objeto: " + gameObject.name);
            return;
        }

        print("carregando cena: " + CenaParaCarregar);

        SceneManager.LoadScene(CenaParaCarregar);
    }
}
