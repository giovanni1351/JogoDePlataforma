using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public float velocidade = 2f; // Velocidade de perseguição

    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Pega o componente visual para podermos virar o rosto dele
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Procura automaticamente quem é o Player na tela
        // IMPORTANTE: O seu Player precisa ter a Tag "Player" na Unity!
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");

        if (objPlayer != null)
        {
            player = objPlayer.transform;
        }
    }

    void Update()
    {
        // Se ele encontrou o player, vai atrás dele o tempo todo
        if (player != null)
        {
            // A mágica acontece aqui: move o inimigo gradualmente até a posição do player
            transform.position = Vector2.MoveTowards(transform.position, player.position, velocidade * Time.deltaTime);

            // Faz o inimigo olhar para o lado certo (espelha a imagem igual fizemos com o player)
            if (player.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true; // Olha para a direita
            }
            else if (player.position.x < transform.position.x)
            {
                spriteRenderer.flipX = false; // Olha para a esquerda
            }
        }
    }

    // A função que já tínhamos feito para ele morrer com a espadada
    public void TomarDano()
    {
        if (GameManager.instancia != null)
        {
            GameManager.instancia.AdicionarKill();
        }
        Destroy(gameObject);
    }
}