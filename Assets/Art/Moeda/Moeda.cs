using System.Collections;
using UnityEngine;

// Moeda coletavel das fases 01 e 02.
//
// O giro vem da animacao moeda_giro.anim (Animator), que so troca o
// sprite. A flutuacao e o efeito de coleta ficam aqui no codigo, porque
// mexem na posicao e na escala — coisas que a animacao nao toca, entao
// os dois nao brigam pelo mesmo valor.
//
// O colisor deste objeto precisa estar marcado como "Is Trigger":
// assim o jogador atravessa a moeda em vez de esbarrar nela.
public class Moeda : MonoBehaviour
{
    // Variáveis públicas que podem ser ajustadas no Inspector
    public int Valor = 1;                     // quanto essa moeda soma no contador

    public float AlturaDaFlutuacao = 0.12f;   // quanto ela sobe e desce, em unidades
    public float VelocidadeDaFlutuacao = 2f;

    public float DuracaoDoEfeito = 0.25f;     // tempo do "pop" ao ser coletada
    public float CrescimentoDoEfeito = 1.8f;  // quanto ela incha antes de sumir

    float AlturaInicial;
    float Defasagem;      // tira as moedas de sincronia umas com as outras
    bool JaFoiColetada;

    SpriteRenderer RenderizadorDaMoeda;

    void Start()
    {
        AlturaInicial = transform.position.y;

        RenderizadorDaMoeda = GetComponent<SpriteRenderer>();

        // Sem esta defasagem todas as moedas da fase subiriam e desceriam
        // exatamente no mesmo instante, o que fica artificial. Usar a
        // propria posicao como semente da um valor diferente por moeda
        // e que nao muda a cada partida.
        Defasagem = transform.position.x + transform.position.y;
    }

    void Update()
    {
        if (JaFoiColetada == true)
        {
            return;
        }

        float deslocamento = Mathf.Sin((Time.time + Defasagem) * VelocidadeDaFlutuacao) * AlturaDaFlutuacao;

        transform.position = new Vector3(transform.position.x, AlturaInicial + deslocamento, transform.position.z);
    }

    // OnTriggerEnter2D é chamado quando outro colisor ENTRA neste.
    void OnTriggerEnter2D(Collider2D objetoQueEntrou)
    {
        // Só o jogador coleta. Sem isto, uma caixa empurrada até aqui
        // também somaria moeda.
        if (objetoQueEntrou.gameObject.tag != "Player")
        {
            return;
        }

        // O efeito de coleta dura alguns frames antes do Destroy, e nesse
        // meio tempo o trigger poderia disparar de novo e contar em dobro.
        if (JaFoiColetada == true)
        {
            return;
        }

        JaFoiColetada = true;

        ContadorMoedas.Adicionar(Valor);

        // Desliga o colisor na hora, pelo mesmo motivo acima.
        Collider2D colisorDaMoeda = GetComponent<Collider2D>();

        if (colisorDaMoeda != null)
        {
            colisorDaMoeda.enabled = false;
        }

        StartCoroutine(EfeitoDeColeta());
    }

    // Cresce e vai sumindo, em vez de desaparecer de um frame pro outro.
    IEnumerator EfeitoDeColeta()
    {
        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaFinal = escalaInicial * CrescimentoDoEfeito;

        float tempo = 0f;

        while (tempo < DuracaoDoEfeito)
        {
            tempo = tempo + Time.deltaTime;

            float andamento = tempo / DuracaoDoEfeito;

            if (andamento > 1f)
            {
                andamento = 1f;
            }

            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, andamento);

            // Sobe um pouco enquanto some, pra leitura ficar clara
            transform.position = new Vector3(transform.position.x, AlturaInicial + andamento * 0.6f, transform.position.z);

            if (RenderizadorDaMoeda != null)
            {
                Color cor = RenderizadorDaMoeda.color;
                cor.a = 1f - andamento;
                RenderizadorDaMoeda.color = cor;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
