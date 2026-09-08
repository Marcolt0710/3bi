using UnityEngine; // Importa o namespace UnityEngine, necessário para todas as classes Unity

// Script que vai no BOTAO. Tag do botao: "ativadortoqueindireto"
//
// O TRONCO da palmeira deve estar na hierarquia como FILHO deste botao,
// posicionado escondido (embaixo do chao ou fora da area visivel da fase).
// Quando o jogador encosta no botao, o tronco se desvincula, e teletransportado
// para o lugar certo e ganha um colisor de gatilho para poder ser escalado.
//
// Tecnicas usadas: Aula 03x03 e 03x04 (itens dentro de blocos: GetChild,
// parent = null, AddComponent, troca de tag) e Plataforma.cs (script no proprio
// objeto detectando a tag "Player" em OnCollisionEnter2D).
public class BotaoToqueIndireto : MonoBehaviour
{

	// A tag precisa ser exatamente a mesma que esta cadastrada em
	// Edit > Project Settings > Tags and Layers. Se escrever diferente,
	// a linha "tronco.tag = ..." lanca UnityException e a mecanica para.
	const string TagDoTronco = "ToqueIndireto";

	// Variaveis publicas: aparecem no Inspector e podem ser ajustadas sem mexer no codigo.
	// Posicao onde o tronco deve aparecer quando o botao for pisado.
	public float PosicaoDoTroncoX; // coordenada X de destino do tronco
	public float PosicaoDoTroncoY; // coordenada Y de destino do tronco

	string TagObjetoTocado; // guarda a tag do objeto que encostou no botao
	bool BotaoJaFoiPisado;  // impede que o botao funcione mais de uma vez

	// Metodo chamado uma vez no inicio do jogo
	void Start()
	{
		TagObjetoTocado = "";     // inicializa a variavel vazia
		BotaoJaFoiPisado = false; // o botao comeca solto
	}

	// Metodo chamado quando ocorre uma colisao 2D com o botao
	void OnCollisionEnter2D(Collision2D objetoTocado)
	{
		// Recupera a tag do objeto que colidiu com o botao
		TagObjetoTocado = objetoTocado.gameObject.tag;

		// Verifica se quem encostou foi o jogador
		if (TagObjetoTocado.Contains("Player") == true)
		{
			// So faz alguma coisa se o botao ainda nao tiver sido pisado
			if (BotaoJaFoiPisado == false)
			{
				BotaoJaFoiPisado = true; // marca que o botao ja foi usado
				MostrarTronco();         // faz a palmeira aparecer
			}
		}
	}

	// Funcao que tira o tronco de dentro do botao e coloca ele na fase
	void MostrarTronco()
	{
		// Se alguem apagar o tronco da hierarquia, GetChild(0) quebraria o jogo.
		// Entao primeiro conferimos se o botao realmente tem um filho.
		if (transform.childCount == 0)
		{
			print("ERRO: o botao " + gameObject.name + " nao tem o tronco como filho");
			return;
		}

		// Pega o filho de indice 0 deste botao: o tronco da palmeira
		GameObject tronco = transform.GetChild(0).gameObject;

		// Desvincula o tronco do botao, senao ele andaria junto com o botao
		tronco.transform.parent = null;

		// Garante que o tronco esteja ligado, caso tenha sido desativado no editor
		tronco.SetActive(true);

		// Teletransporta o tronco para a posicao escolhida no Inspector.
		// Antes esta posicao estava escrita direto no codigo, entao mudar os
		// campos do Inspector nao adiantava nada e o tronco nascia fora da fase.
		tronco.transform.position = new Vector2(PosicaoDoTroncoX, PosicaoDoTroncoY);

		// Adiciona um colisor ao tronco, se ele ainda nao tiver um.
		// Chamar AddComponent duas vezes deixaria dois colisores empilhados
		// e o OnTriggerStay2D do tronco rodaria em dobro.
		BoxCollider2D colisorDoTronco = tronco.GetComponent<BoxCollider2D>();

		if (colisorDoTronco == null)
		{
			colisorDoTronco = tronco.AddComponent<BoxCollider2D>();
		}

		// Marca o colisor como gatilho: o jogador atravessa em vez de esbarrar
		colisorDoTronco.isTrigger = true;

		// Coloca no tronco a tag combinada
		tronco.tag = TagDoTronco;

		print("tronco liberado em: " + tronco.transform.position);
	}
}
