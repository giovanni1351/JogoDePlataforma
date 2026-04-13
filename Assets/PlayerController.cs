using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 10f;

    [Header("Configurações de Ataque")]
    public float forcaImpulsoAtaque = 5f;
    public float tempoDeAtaque = 0.3f;
    public float raioDoAtaque = 0.8f; // Tamanho da área de dano
    public LayerMask camadaInimigo; // Define o que é inimigo

    [Header("Verificação de Chão")]
    public Transform verificadorChao;
    public float raioVerificacao = 0.2f;
    public LayerMask camadaChao;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float movimentoX;
    private bool noChao;
    private bool estaAtacando = false;
    private Animator animator; // Arraste o Animator aqui no Inspetor

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaAtacando) return;

        movimentoX = Input.GetAxisRaw("Horizontal");
        noChao = Physics2D.OverlapCircle(verificadorChao.position, raioVerificacao, camadaChao);
        animator.SetFloat("Velocidade", Mathf.Abs(movimentoX));
        if (Input.GetKeyDown(KeyCode.W) && noChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !estaAtacando)
        {
            StartCoroutine(EfeitoAtaqueProgramado());
        }

        if (movimentoX > 0) spriteRenderer.flipX = false;
        else if (movimentoX < 0) spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        if (!estaAtacando) rb.linearVelocity = new Vector2(movimentoX * velocidade, rb.linearVelocity.y);
    }

    IEnumerator EfeitoAtaqueProgramado()
    {
        estaAtacando = true;
        float direcao = spriteRenderer.flipX ? -1f : 1f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(direcao * forcaImpulsoAtaque, 0), ForceMode2D.Impulse);
        CriarRastroDeCorte(direcao);

        // --- NOVA LÓGICA DE DANO ---
        // Calcula a posição do golpe (na frente do player)
        Vector2 posicaoAtaque = (Vector2)transform.position + new Vector2(direcao * 0.8f, 0.2f);

        // Cria um círculo invisível e pega tudo que encostar nele que seja da CamadaInimigo
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(posicaoAtaque, raioDoAtaque, camadaInimigo);

        // Aplica dano em todos os inimigos pegos no círculo
        foreach (Collider2D inimigo in inimigosAtingidos)
        {
            inimigo.GetComponent<Inimigo>().TomarDano();
        }
        // -----------------------------

        yield return new WaitForSeconds(tempoDeAtaque);
        estaAtacando = false;
    }

    void CriarRastroDeCorte(float direcao)
    {
        GameObject rastro = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(rastro.GetComponent<BoxCollider>());
        rastro.transform.position = transform.position + new Vector3(direcao * 0.8f, 0.2f, 0);
        rastro.transform.localScale = new Vector3(1.5f, 0.1f, 0.1f);
        rastro.transform.rotation = Quaternion.Euler(0, 0, direcao == 1 ? -30f : 30f);
        Renderer rastroRenderer = rastro.GetComponent<Renderer>();
        if (rastroRenderer != null) rastroRenderer.material.color = Color.white;
        Destroy(rastro, 0.15f);
    }

    // Usamos isso para desenhar a área de ataque na tela da Unity (cor amarela)
    private void OnDrawGizmos()
    {
        if (verificadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(verificadorChao.position, raioVerificacao);
        }

        // Desenha o círculo de ataque na aba Scene para te ajudar a regular o tamanho
        float direcaoDaLinha = 1f;
        if (Application.isPlaying && spriteRenderer != null) direcaoDaLinha = spriteRenderer.flipX ? -1f : 1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(direcaoDaLinha * 0.8f, 0.2f), raioDoAtaque);
    }
}