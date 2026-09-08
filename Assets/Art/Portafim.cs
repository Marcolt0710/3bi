using UnityEngine; // Importa o namespace UnityEngine, necessário para todas as classes Unity
using UnityEngine.SceneManagement; // Necessário para trocar de cena (Aula 02x01)

// Script que vai na PORTA do fim da fase. Tag da porta: "portafim"
// Quando o jogador encosta na porta, o jogo carrega a cena dos creditos.
public class Portafim : MonoBehaviour
{

	// Variavel publica: aparece no Inspector.
	// Nome da cena que sera carregada. Sugestao: "creditos"
	public string CenaParaCarregar;

	string TagObjetoTocado; // guarda a tag do objeto que encostou na porta

	// Metodo chamado uma vez no inicio do jogo
	void Start()
	{
		TagObjetoTocado = ""; // inicializa a variavel vazia
	}

	// Metodo chamado quando ocorre uma colisao 2D com a porta
	void OnCollisionEnter2D(Collision2D objetoTocado)
	{
		// Recupera a tag do objeto que colidiu com a porta
		TagObjetoTocado = objetoTocado.gameObject.tag;

		// Verifica se quem encostou foi o jogador
		if (TagObjetoTocado.Contains("Player") == true)
		{
			// Verifica se o nome da cena foi preenchido no Inspector
			if (CenaParaCarregar != "")
			{
				// Carrega a cena dos creditos
				SceneManager.LoadScene(CenaParaCarregar);
			}
			else
			{
				// Avisa no Console qual porta esta mal configurada
				print("Porta sem cena definida: " + gameObject.name);
			}
		}
	}
}