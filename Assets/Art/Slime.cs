using UnityEngine;

public class Slime : MonoBehaviour
{
    // Variáveis públicas que podem ser ajustadas no Inspector
    public float Distancia;           // Quanto ele anda antes de dar meia-volta
    public float VelocidadeMovimento; // Velocidade do vaivém

    public Vector2 Direcao;           // Direção do movimento, igual ao da Plataforma:
    // - (1, 0)   → anda para a direita e volta
    // - (-1, 0)  → anda para a esquerda e volta
    // - (0, 1)   → sobe e desce
    // - (1, 1)   → diagonal

    public bool VirarSprite;          // Se true, o sprite espelha ao trocar de sentido,
                                      // para o slime parecer que "olha" para onde anda

    Transform TransformSlime;     // Referência ao Transform deste objeto
    Vector2 PosicaoInicial;       // Onde ele estava quando a fase começou
    float TempoDecorrido;         // Acumula o tempo para controlar o vaivém
    float MovimentoAnterior;      // Valor do vaivém no frame passado
    SpriteRenderer RenderSlime;   // Usado só para espelhar o sprite

    void Start()
    {
        TransformSlime = GetComponent<Transform>();

        // Guarda a posição inicial: todo o movimento é calculado a partir dela
        PosicaoInicial = TransformSlime.position;

        TempoDecorrido = 0f;
        MovimentoAnterior = 0f;

        RenderSlime = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mover();
    }

    // Função que controla o vaivém do slime
    void mover()
    {
        TempoDecorrido += Time.deltaTime * VelocidadeMovimento;

        // Mathf.PingPong devolve um valor que sobe de 0 até Distancia e volta,
        // sem nunca passar disso. É o mesmo truque usado na Plataforma.
        float movimento = Mathf.PingPong(TempoDecorrido, Distancia);

        TransformSlime.position = PosicaoInicial + Direcao.normalized * movimento;

        // Para saber o sentido atual, compara com o frame anterior:
        // quando o PingPong chega na ponta e volta, a diferença troca de sinal.
        if (VirarSprite == true && RenderSlime != null)
        {
            if (movimento > MovimentoAnterior)
            {
                RenderSlime.flipX = false;
            }
            else if (movimento < MovimentoAnterior)
            {
                RenderSlime.flipX = true;
            }
        }

        MovimentoAnterior = movimento;
    }
}
