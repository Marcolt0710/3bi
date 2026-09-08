using UnityEngine;

// Contador de moedas desenhado na tela (OnGUI, sem Canvas — mesma
// abordagem do HudProgresso, pra nao misturar dois jeitos de fazer HUD).
//
// O total fica numa variavel "static": static pertence a CLASSE e nao
// ao objeto, entao ela nao se perde quando o SceneManager troca de cena.
// E isso que faz as moedas juntadas na fase01 continuarem valendo na fase02.
public class ContadorMoedas : MonoBehaviour
{
    public static int Total;

    // Marcado so na fase01, que e onde uma partida comeca de verdade.
    // Na fase02 fica desmarcado, senao o total zeraria no meio do jogo.
    public bool ZerarAoComecar;

    // Folha moeda_giro.png, usada so pra desenhar o iconezinho ao lado
    // do numero. E opcional: sem ela o HUD mostra a palavra "Moedas".
    public Texture FolhaMoeda;
    public int QuadrosDaFolha = 13;
    public float QuadrosPorSegundo = 12f;

    // Posicao e tamanho do HUD, em pixels, a partir do canto superior esquerdo
    public float MargemEsquerda = 10f;
    public float MargemTopo = 10f;
    public float TamanhoDoIcone = 32f;

    GUIStyle EstiloNumero;

    // Chamada pela Moeda quando o jogador encosta nela.
    // E static para a moeda nao precisar procurar o objeto do HUD na cena.
    public static void Adicionar(int quantidade)
    {
        Total = Total + quantidade;

        // Debug.Log em vez de print: print e um metodo de instancia do
        // MonoBehaviour e nao pode ser chamado de dentro de um metodo static.
        Debug.Log("moedas: " + Total);
    }

    public static void Zerar()
    {
        Total = 0;
    }

    void Awake()
    {
        if (ZerarAoComecar == true)
        {
            Zerar();
        }
    }

    void OnGUI()
    {
        // GUI.skin so existe dentro do OnGUI, por isso o estilo e montado
        // aqui na primeira passada, e nao no Start.
        if (EstiloNumero == null)
        {
            EstiloNumero = new GUIStyle(GUI.skin.label);
            EstiloNumero.fontSize = 22;
            EstiloNumero.fontStyle = FontStyle.Bold;
            EstiloNumero.alignment = TextAnchor.MiddleLeft;
        }

        float x = MargemEsquerda;

        if (FolhaMoeda != null)
        {
            // A folha tem os 13 quadros lado a lado. Em vez de fatiar a
            // textura, o DrawTextureWithTexCoords desenha so o pedaco
            // correspondente ao quadro atual — assim o icone do HUD gira junto.
            int quadro = (int)(Time.time * QuadrosPorSegundo) % QuadrosDaFolha;

            float larguraDoQuadro = 1f / QuadrosDaFolha;

            Rect areaNaTela = new Rect(x, MargemTopo, TamanhoDoIcone, TamanhoDoIcone);
            Rect recorteNaFolha = new Rect(quadro * larguraDoQuadro, 0f, larguraDoQuadro, 1f);

            GUI.DrawTextureWithTexCoords(areaNaTela, FolhaMoeda, recorteNaFolha);

            x = x + TamanhoDoIcone + 6f;
        }
        else
        {
            GUI.Label(new Rect(x, MargemTopo + 4f, 90f, 28f), "Moedas:", EstiloNumero);

            x = x + 90f;
        }

        Rect areaDoNumero = new Rect(x, MargemTopo, 120f, TamanhoDoIcone);

        // Desenha o numero duas vezes: uma preta deslocada 1px (sombra) e
        // outra branca por cima. Sem isso o texto some em cima do ceu claro.
        Color corAnterior = GUI.color;

        GUI.color = Color.black;
        GUI.Label(new Rect(areaDoNumero.x + 1f, areaDoNumero.y + 1f, areaDoNumero.width, areaDoNumero.height), "x " + Total, EstiloNumero);

        GUI.color = Color.white;
        GUI.Label(areaDoNumero, "x " + Total, EstiloNumero);

        GUI.color = corAnterior;
    }
}
