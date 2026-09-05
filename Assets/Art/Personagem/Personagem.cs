using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personagem : MonoBehaviour {

	// ----- boost de pulo temporario (bonusPuloTemporario) -----
	public float DuracaoBoostPulo = 5.0f;
	public float MultiplicadorBoostPulo = 1.6f;
	public float TempoRestanteBoost; // usado pelo HUD pra mostrar a contagem regressiva
	float VelocidadePuloBase;
	Coroutine BoostPuloEmAndamento;

	GameObject ObjetoEmContato;      // caixa que esta encostada no personagem
	GameObject ObjetoCarregado;      // caixa que esta nas maos
	Rigidbody2D CorpoRigidoObjeto;   // corpo rigido da caixa carregada
	float AlturaDaCarga;             // distancia acima do personagem
	float DistanciaAoSoltar;         // distancia na frente do personagem ao soltar

	int TotalPulos;
	int ContadorPulos;

	int DirecaoOlhando; // 1 = direita, -1 = esquerda

	SpriteRenderer Rerender;

	Animator PersonagemAnimator;

	string TagObjetoTocado;

	bool PegouChave01;

	bool ApertouBotaoPular;
	float VelocidadePuloSimples;
	Collider2D Colisor2dPersonagem;
	bool EstaTocandoAlgumColisor;

	float VelocidadeX;
	float VelocidadeY;

	float VelocidadeHorizontalMaxima;
	float DirecaoHorizontal;

	float VelocidadeVerticalMaxima;
	float DirecaoVertical;

	Vector2 VetorVelocidadePersonagem;
	Rigidbody2D CorpoRigidoPersonagem;

	void Start () {
		TotalPulos = 1;
		ContadorPulos = 0;

		Rerender = GetComponent<SpriteRenderer> ();

		PersonagemAnimator = GetComponent<Animator> ();

		PegouChave01 = false;
		TagObjetoTocado = "";

		ApertouBotaoPular = false;
		EstaTocandoAlgumColisor = false;

		VelocidadePuloSimples = 10.0f;
		VelocidadePuloBase = VelocidadePuloSimples;

		CorpoRigidoPersonagem = GetComponent<Rigidbody2D> ();
		if (CorpoRigidoPersonagem == null) {
			CorpoRigidoPersonagem = gameObject.AddComponent<Rigidbody2D> ();
		}

		Colisor2dPersonagem = GetComponent<Collider2D> ();

		CorpoRigidoPersonagem.gravityScale = 3.0f;

		CorpoRigidoPersonagem.freezeRotation = true;

		VelocidadeX = 0f;
		VelocidadeY = 0f;
		VelocidadeHorizontalMaxima = 5.0f;
		DirecaoHorizontal = 0f;

		VelocidadeVerticalMaxima = 5.0f;
		DirecaoVertical = 0f;

		VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

		CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;

		// ----- inicializacao da carga -----
		ObjetoEmContato = null;
		ObjetoCarregado = null;
		CorpoRigidoObjeto = null;
		AlturaDaCarga = 1.2f;
		DistanciaAoSoltar = 1.2f;
		DirecaoOlhando = 1;
	}

	void Update () {
		MovimentoPuloMultiplo ();
		MovimentoHorizontalFlip ();
		ControlarCarga ();
	}

	void MovimentoPuloMultiplo(){
		ApertouBotaoPular = Input.GetButtonDown ("Jump");

		if (ApertouBotaoPular == true && ContadorPulos < TotalPulos) {

			ContadorPulos = ContadorPulos + 1;

			VelocidadeX = CorpoRigidoPersonagem.velocity.x;

			VelocidadeY = VelocidadePuloSimples;

			VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

			CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
		}
	}

	void MovimentoPulo(){
		ApertouBotaoPular = Input.GetButtonDown ("Jump");

		if (ApertouBotaoPular == true && ContadorPulos < 2) {

			ContadorPulos = ContadorPulos + 1;

			VelocidadeX = CorpoRigidoPersonagem.velocity.x;

			VelocidadeY = VelocidadePuloSimples;

			VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

			CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
		}
	}

	void MovimentoHorizontalFlip(){
		DirecaoHorizontal = Input.GetAxis ("Horizontal");

		VelocidadeX = VelocidadeHorizontalMaxima * DirecaoHorizontal;

		VelocidadeY = CorpoRigidoPersonagem.velocity.y;

		VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

		if (DirecaoHorizontal != 0) {
			PersonagemAnimator.SetBool ("andando", true);
		} else {
			PersonagemAnimator.SetBool ("andando", false);
		}

		CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;

		if (DirecaoHorizontal < 0) {
			Rerender.flipX = true;
			DirecaoOlhando = -1;
		} else if (DirecaoHorizontal > 0) {
			Rerender.flipX = false;
			DirecaoOlhando = 1;
		}
	}

	void MovimentoPuloUnico(){
		ApertouBotaoPular = Input.GetButtonDown ("Jump");

		EstaTocandoAlgumColisor = Colisor2dPersonagem.IsTouchingLayers ();

		if (ApertouBotaoPular == true && EstaTocandoAlgumColisor == true) {

			VelocidadeX = CorpoRigidoPersonagem.velocity.x;

			VelocidadeY = VelocidadePuloSimples;

			VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

			CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
		}
	}

	void MovimentoPuloSimples(){
		ApertouBotaoPular = Input.GetButtonDown ("Jump");

		if (ApertouBotaoPular == true) {

			VelocidadeX = CorpoRigidoPersonagem.velocity.x;

			VelocidadeY = VelocidadePuloSimples;

			VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

			CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
		}
	}

	void MovimentoVertical(){
		DirecaoVertical = Input.GetAxis ("Vertical");

		VelocidadeX = CorpoRigidoPersonagem.velocity.x;
		VelocidadeY = DirecaoVertical * VelocidadeVerticalMaxima;

		VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

		CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
	}

	void MovimentoHorizontal(){
		DirecaoHorizontal = Input.GetAxis ("Horizontal");

		VelocidadeX = VelocidadeHorizontalMaxima * DirecaoHorizontal;

		VelocidadeY = CorpoRigidoPersonagem.velocity.y;

		VetorVelocidadePersonagem = new Vector2 (VelocidadeX, VelocidadeY);

		CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
	}

	void OnCollisionEnter2D(Collision2D objetoTocado ){
		TagObjetoTocado = objetoTocado.gameObject.tag;

		VerificarDestrutivel (objetoTocado);

		if (TagObjetoTocado.Contains("chao")) {
			print ("Tocou TAG: chao");
			ContadorPulos = 0;
		}

		// Permite pular de novo depois de subir em cima da caixa
		if (TagObjetoTocado.Contains("carregar")) {
			ContadorPulos = 0;
		}

		if (TagObjetoTocado == "bonusPuloUnico") {
			TotalPulos = 1;
			Destroy (objetoTocado.gameObject);
		}
		if (TagObjetoTocado == "bonusPuloDuplo") {
			TotalPulos = 2;
			Destroy (objetoTocado.gameObject);
		}

		// bonusPuloTemporario e a tag do item que pulou pra fora do
		// bloco (ver VerificarDestrutivel). So chega aqui depois de
		// ja ter saido do "objeto dentro de outro".
		if (TagObjetoTocado == "bonusPuloTemporario") {
			IniciarBoostPulo (DuracaoBoostPulo, MultiplicadorBoostPulo);
			Destroy (objetoTocado.gameObject);
		}

		if (TagObjetoTocado == "Fim" && PegouChave01==true) {
			SceneManager.LoadScene ("fase02");
		}

		if (TagObjetoTocado == "chave01") {
			PegouChave01 = true;
			Destroy (objetoTocado.gameObject);
		}
		if (TagObjetoTocado == "Diamante01") {
			print (TagObjetoTocado);
			Destroy (objetoTocado.gameObject);
		}
		if (TagObjetoTocado == "Diamante02") {
			print (TagObjetoTocado);
			Destroy (objetoTocado.gameObject,1.5f);
		}
		if (TagObjetoTocado == "Diamante03") {
			print (TagObjetoTocado);
			objetoTocado.rigidbody.velocity = new Vector2 (10f * DirecaoHorizontal, 10f);
			Destroy (objetoTocado.gameObject, 0.6f);
		}
		if (TagObjetoTocado == "tipo1") {
			if (VelocidadeHorizontalMaxima < 10) {
				VelocidadeHorizontalMaxima = VelocidadeHorizontalMaxima + 1f;
				print ("VelocidadeHorizontalMaxima: " + VelocidadeHorizontalMaxima);
				Destroy (objetoTocado.gameObject, 0.05f);
			}
			
		}
		if (TagObjetoTocado.Contains("morte")) {
			print("game over");
			SceneManager.LoadScene ("morte");
		}
		if (TagObjetoTocado == "tipo2") {
			if (VelocidadeHorizontalMaxima >= 2) {
				VelocidadeHorizontalMaxima = VelocidadeHorizontalMaxima - 1f;
				print ("VelocidadeHorizontalMaxima: " + VelocidadeHorizontalMaxima);
				Destroy (objetoTocado.gameObject, 0.05f);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D objetoTriggerTocado){
		string tagTocadaTrigger = objetoTriggerTocado.gameObject.tag;

		if (tagTocadaTrigger == "teletransporte") {
			transform.position = new Vector2 (58.47f, -1.55f);

			// Descomente a linha abaixo se quiser que o personagem
			// chegue parado no destino, sem carregar a velocidade anterior:
			// CorpoRigidoPersonagem.velocity = Vector2.zero;
		}
	}

	// Enquanto o personagem esta encostado em outro colisor
	void OnCollisionStay2D(Collision2D objetoTocado){
		string tagTocada = objetoTocado.gameObject.tag;

		// O evento so guarda QUAL objeto esta encostado.
		// Quem decide pegar e o Update, senao a tecla precisaria
		// coincidir com o frame exato do contato.
		if (tagTocada.Contains ("carregar") == true) {
			ObjetoEmContato = objetoTocado.gameObject;
		}
	}

	// Quando o personagem para de encostar
	void OnCollisionExit2D(Collision2D objetoParouTocar){
		string tagParouTocar = objetoParouTocar.gameObject.tag;

		if (tagParouTocar.Contains ("carregar") == true) {

			// So esquece a caixa se ela nao for a que esta sendo carregada.
			// A caixa carregada perde o contato de proposito.
			if (objetoParouTocar.gameObject != ObjetoCarregado) {
				ObjetoEmContato = null;
			}
		}
	}

	// "Objeto dentro de outro": o BlocoImpulso (destrutivel1) carrega
	// escondido dentro dele um NucleoImpulso (o item bonusPuloTemporario).
	// Ao ser tocado, o filho ganha fisica na hora, sai voando pra cima
	// (igual bloco de interrogacao) e o bloco em si e destruido.
	void VerificarDestrutivel(Collision2D objetoTocado){

		if (TagObjetoTocado.Contains ("destrutivel2")) {
			GameObject capa = objetoTocado.transform.GetChild (0).gameObject;   // filho zero
			GameObject item = objetoTocado.transform.GetChild (1).gameObject;   // filho um, escondido atras da capa

			capa.transform.parent = null;
			item.transform.parent = null;

			item.GetComponent<SpriteRenderer> ().enabled = true;
			item.AddComponent<BoxCollider2D> ();
			item.AddComponent<Rigidbody2D> ();
			item.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10);
			item.tag = "bonusPuloTemporario";

			Destroy (capa);
			Destroy (objetoTocado.gameObject);
		}

		if (TagObjetoTocado.Contains ("destrutivel1")) {
			GameObject item = objetoTocado.transform.GetChild (0).gameObject; // filho zero, escondido dentro do bloco

			item.transform.parent = null;

			item.GetComponent<SpriteRenderer> ().enabled = true;
			item.AddComponent<BoxCollider2D> ();
			item.AddComponent<Rigidbody2D> ();
			item.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10);
			item.tag = "bonusPuloTemporario";

			Destroy (objetoTocado.gameObject);
		}
	}

	void IniciarBoostPulo(float duracao, float multiplicador){
		// Se ja tem um boost rodando, cancela o antigo antes de comecar
		// o novo, senao os dois coroutines disputariam o valor final.
		if (BoostPuloEmAndamento != null) {
			StopCoroutine (BoostPuloEmAndamento);
		}

		BoostPuloEmAndamento = StartCoroutine (BoostPuloTemporario (duracao, multiplicador));
	}

	IEnumerator BoostPuloTemporario(float duracao, float multiplicador){
		VelocidadePuloSimples = VelocidadePuloBase * multiplicador;
		TempoRestanteBoost = duracao;
		print ("boost de pulo ativado por " + duracao + "s");

		// Conta regressivamente em vez de um WaitForSeconds unico,
		// pra dar pro HUD um valor de TempoRestanteBoost pra mostrar.
		while (TempoRestanteBoost > 0f) {
			TempoRestanteBoost -= Time.deltaTime;
			yield return null;
		}

		TempoRestanteBoost = 0f;
		VelocidadePuloSimples = VelocidadePuloBase;
		BoostPuloEmAndamento = null;
		print ("boost de pulo encerrado");
	}

	void ControlarCarga(){
		if (Input.GetKey (KeyCode.F) == true && ObjetoCarregado == null && ObjetoEmContato != null) {
			PegarObjeto (ObjetoEmContato);
		}

		if (Input.GetKey (KeyCode.F) == false && ObjetoCarregado != null) {
			SoltarObjeto ();
		}
	}

	void PegarObjeto(GameObject objeto){
		print ("carregando: " + objeto.tag);

		ObjetoCarregado = objeto;
		CorpoRigidoObjeto = objeto.GetComponent<Rigidbody2D> ();

		CorpoRigidoObjeto.velocity = new Vector2 (0, 0);

		// simulated = false desliga a fisica da caixa enquanto ela esta na mao:
		// sem gravidade, sem colisao e sem velocidade sobrando.
		// E o que impede a caixa de deslizar e de atravessar as plataformas.
		CorpoRigidoObjeto.simulated = false;

		// Propriedade parent: a caixa vira filha do personagem
		ObjetoCarregado.transform.parent = gameObject.transform;

		// Posiciona a caixa acima do personagem
		float xPersonagem = gameObject.transform.position.x;
		float yPersonagem = gameObject.transform.position.y;
		ObjetoCarregado.transform.position = new Vector2 (xPersonagem, yPersonagem + AlturaDaCarga);
	}

	void SoltarObjeto(){
		print ("soltou: " + ObjetoCarregado.tag);

		// Coloca a caixa na frente do personagem, e nao em cima da cabeca,
		// para ela nao cair sobre ele e empurra-lo.
		float xPersonagem = gameObject.transform.position.x;
		float yPersonagem = gameObject.transform.position.y;
		ObjetoCarregado.transform.position = new Vector2 (xPersonagem + (DistanciaAoSoltar * DirecaoOlhando), yPersonagem);

		// parent = null tira a caixa da hierarquia do personagem
		ObjetoCarregado.transform.parent = null;

		// Religa a fisica: a caixa volta a cair e a colidir
		CorpoRigidoObjeto.simulated = true;
		CorpoRigidoObjeto.velocity = new Vector2 (0, 0);

		ObjetoCarregado = null;
		CorpoRigidoObjeto = null;
		ObjetoEmContato = null;
	}
}
