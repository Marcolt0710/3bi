using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    // Variáveis públicas que podem ser ajustadas no Inspector
    public Transform Alvo;        // Objeto que a câmera segue (arraste o Personagem aqui)

    public float Suavidade;       // Tempo, em segundos, que a câmera leva para alcançar o alvo:
    // - 0f      → câmera colada no alvo, sem suavização (pode tremer)
    // - 0.15f   → seguimento rápido, bom para jogo de plataforma
    // - 0.30f   → câmera mais preguiçosa, efeito cinematográfico
    // - 1.0f    → câmera muito lenta, o alvo escapa da tela ao correr

    public Vector2 Deslocamento;  // Quanto a câmera fica afastada do alvo (x, y):
    // - (0, 0)  → alvo exatamente no centro da tela
    // - (0, 1)  → alvo um pouco abaixo do centro, mostra mais cenário acima
    // - (2, 0)  → câmera adiantada para a direita, mostra mais do caminho à frente

    public bool LimitarNasBordas; // Se true, a câmera trava nas bordas da fase em vez
                                  // de mostrar o vazio fora do cenário

    // Bordas da fase, em unidades do mundo. Só são usadas se LimitarNasBordas for true.
    public float LimiteEsquerda;
    public float LimiteDireita;
    public float LimiteBaixo;
    public float LimiteCima;

    Camera CameraDoJogo;             // Componente Camera deste objeto
    Vector3 VelocidadeDaSuavizacao;  // Usada internamente pelo SmoothDamp
    float ProfundidadeZ;             // Z original da câmera, precisa ser preservado

    void Start()
    {
        // Obtém o componente Camera do objeto atual
        CameraDoJogo = GetComponent<Camera>();

        // Guarda o Z inicial (-10 nesta cena). Em 2D a câmera precisa ficar
        // atrás do cenário: se o Z virar 0 ela "entra" na fase e não renderiza nada.
        ProfundidadeZ = transform.position.z;

        // Zera a velocidade que o SmoothDamp vai acumulando
        VelocidadeDaSuavizacao = Vector3.zero;

        // Já começa enquadrada no alvo. Sem isto a câmera nasce na posição
        // salva na cena e desliza até o personagem no primeiro segundo de jogo.
        if (Alvo != null)
        {
            transform.position = posicaoDesejada();
        }
    }

    // LateUpdate roda depois de TODOS os Update do frame.
    // É aqui que a câmera deve se mover, porque neste ponto o personagem
    // já terminou de andar neste frame. Se fosse no Update, a câmera poderia
    // rodar antes do personagem e ficaria sempre um frame atrasada — o que
    // aparece na tela como tremedeira.
    void LateUpdate()
    {
        // Sem alvo definido no Inspector não há o que seguir
        if (Alvo == null)
        {
            return;
        }

        seguir();
    }

    // Função que move a câmera até o alvo
    void seguir()
    {
        // SmoothDamp move suavemente até o destino, desacelerando na chegada.
        // Ele guarda a velocidade atual em VelocidadeDaSuavizacao (por isso o "ref"),
        // o que dá um movimento bem mais natural que um Lerp simples.
        transform.position = Vector3.SmoothDamp(
            transform.position,
            posicaoDesejada(),
            ref VelocidadeDaSuavizacao,
            Suavidade);
    }

    // Calcula onde a câmera deveria estar neste instante, já respeitando
    // o deslocamento e as bordas da fase.
    Vector3 posicaoDesejada()
    {
        // Posição que a câmera quer alcançar: o alvo mais o deslocamento
        float destinoX = Alvo.position.x + Deslocamento.x;
        float destinoY = Alvo.position.y + Deslocamento.y;

        if (LimitarNasBordas == true)
        {
            // Metade do que a câmera enxerga, em unidades do mundo.
            // orthographicSize é a METADE da altura visível;
            // a largura depende da proporção da tela (aspect).
            float metadeAltura = CameraDoJogo.orthographicSize;
            float metadeLargura = metadeAltura * CameraDoJogo.aspect;

            // Trava o CENTRO da câmera de modo que a BORDA da tela
            // nunca ultrapasse o limite da fase.
            destinoX = Mathf.Clamp(destinoX, LimiteEsquerda + metadeLargura, LimiteDireita - metadeLargura);
            destinoY = Mathf.Clamp(destinoY, LimiteBaixo + metadeAltura, LimiteCima - metadeAltura);
        }

        return new Vector3(destinoX, destinoY, ProfundidadeZ);
    }
}
