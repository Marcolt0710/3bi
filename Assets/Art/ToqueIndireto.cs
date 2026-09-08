using UnityEngine; // Importa o namespace UnityEngine, necessário para todas as classes Unity

// Script que vai no TRONCO da palmeira. Tag do tronco: "ToqueIndireto"
//
// Enquanto o jogador estiver dentro do colisor de gatilho do tronco, a gravidade
// dele e desligada e as setas para cima e para baixo fazem ele subir e descer.
// Ao sair do tronco, a gravidade e devolvida.
//
// Tecnicas usadas: MovimentoVertical() da Aula 02x05 (Input.GetAxis("Vertical"),
// montagem do Vector2 de velocidade), gravityScale do Bimestre 1, e os eventos
// OnTriggerEnter2D / OnTriggerStay2D / OnTriggerExit2D da Aula 02x04 (tipos de colisao).
public class ToqueIndireto : MonoBehaviour
{

	// Variaveis publicas: ajustaveis no Inspector
	public float VelocidadeDeEscalada; // velocidade com que o jogador sobe e desce (sugestao: 4)
	public float GravidadeDoJogador;   // a mesma gravidade usada no Personagem (no seu jogo: 3)

	Rigidbody2D CorpoRigidoJogador; // referencia ao corpo rigido do jogador
	string TagObjetoTrigger;        // guarda a tag do objeto que entrou no gatilho

	float GravidadeGuardada; // gravidade que o jogador tinha antes de entrar no tronco

	float DirecaoVertical; // direcao vertical do input (-1 para baixo, 1 para cima, 0 parado)
	float VelocidadeX;     // velocidade do jogador no eixo X
	float VelocidadeY;     // velocidade do jogador no eixo Y

	Vector2 VetorVelocidadeJogador; // vetor que junta VelocidadeX e VelocidadeY

	// Metodo chamado uma vez no inicio
	void Start()
	{
		TagObjetoTrigger = ""; // inicializa a variavel vazia
		DirecaoVertical = 0f;  // comeca sem direcao
		VelocidadeX = 0f;      // comeca parado no eixo X
		VelocidadeY = 0f;      // comeca parado no eixo Y

		GravidadeGuardada = 0f; // ainda nao guardamos a gravidade de ninguem
	}

	// Metodo chamado no primeiro frame em que um objeto entra no gatilho
	void OnTriggerEnter2D(Collider2D objetoTrigger)
	{
		TagObjetoTrigger = objetoTrigger.gameObject.tag;

		if (TagObjetoTrigger.Contains("Player") == true)
		{
			CorpoRigidoJogador = objetoTrigger.GetComponent<Rigidbody2D>();

			if (CorpoRigidoJogador == null)
			{
				return;
			}

			// Guarda a gravidade REAL do jogador antes de zerar. Assim o valor
			// devolvido na saida nao depende do campo GravidadeDoJogador estar
			// preenchido certo no Inspector (um valor errado ali fazia o jogador
			// sair da palmeira flutuando ou caindo para cima).
			GravidadeGuardada = CorpoRigidoJogador.gravityScale;

			// Chega no tronco parado no eixo Y, senao a queda anterior
			// continuaria empurrando ele para baixo durante a escalada.
			CorpoRigidoJogador.velocity = new Vector2(CorpoRigidoJogador.velocity.x, 0f);
		}
	}

	// Metodo chamado a cada frame enquanto um objeto estiver dentro do gatilho
	void OnTriggerStay2D(Collider2D objetoTrigger)
	{
		// Recupera a tag do objeto que esta dentro do tronco
		TagObjetoTrigger = objetoTrigger.gameObject.tag;

		// So o jogador pode escalar
		if (TagObjetoTrigger.Contains("Player") == true)
		{
			// Pega o corpo rigido do jogador
			CorpoRigidoJogador = objetoTrigger.GetComponent<Rigidbody2D>();

			if (CorpoRigidoJogador == null)
			{
				return;
			}

			// Desliga a gravidade: sem isso o jogador escorregaria para baixo
			CorpoRigidoJogador.gravityScale = 0f;

			// Le a seta para cima ou para baixo (mesma ideia do MovimentoVertical da aula)
			DirecaoVertical = Input.GetAxis("Vertical");

			// Preserva a velocidade horizontal, que continua sendo do Personagem.cs
			VelocidadeX = CorpoRigidoJogador.velocity.x;

			// Calcula a velocidade vertical da escalada
			VelocidadeY = DirecaoVertical * VelocidadeDeEscalada;

			// Monta o novo vetor de velocidade
			VetorVelocidadeJogador = new Vector2(VelocidadeX, VelocidadeY);

			// Aplica a velocidade no corpo rigido do jogador
			CorpoRigidoJogador.velocity = VetorVelocidadeJogador;
		}
	}

	// Metodo chamado quando um objeto sai do gatilho do tronco
	void OnTriggerExit2D(Collider2D objetoTrigger)
	{
		// Recupera a tag do objeto que saiu
		TagObjetoTrigger = objetoTrigger.gameObject.tag;

		if (TagObjetoTrigger.Contains("Player") == true)
		{
			// Pega o corpo rigido do jogador
			CorpoRigidoJogador = objetoTrigger.GetComponent<Rigidbody2D>();

			if (CorpoRigidoJogador == null)
			{
				return;
			}

			// Devolve a gravidade, senao o jogador ficaria flutuando pela fase.
			// Usa a gravidade guardada na entrada; o campo do Inspector so entra
			// como reserva, caso por algum motivo nada tenha sido guardado.
			if (GravidadeGuardada > 0f)
			{
				CorpoRigidoJogador.gravityScale = GravidadeGuardada;
			}
			else
			{
				CorpoRigidoJogador.gravityScale = GravidadeDoJogador;
			}
		}
	}
}
