using UnityEngine;
using System.Collections;

public class GeradorDeInimigos : MonoBehaviour
{
    [Header("Configurações do Gerador")]
    public GameObject inimigoPrefab; // O "molde" do inimigo
    public Transform[] pontosDeSpawn; // Locais onde eles podem nascer
    public float tempoEntreSpawns = 3f; // Tempo em segundos

    void Start()
    {
        // Começa o ciclo infinito de gerar inimigos
        StartCoroutine(GerarInimigoRotina());
    }

    IEnumerator GerarInimigoRotina()
    {
        while (true)
        {
            // Espera o tempo definido
            yield return new WaitForSeconds(tempoEntreSpawns);

            // Se você configurou os pontos corretamente...
            if (pontosDeSpawn.Length > 0 && inimigoPrefab != null)
            {
                // Sorteia um número de zero até a quantidade de pontos que você criou
                int pontoSorteado = Random.Range(0, pontosDeSpawn.Length);

                // Cria o inimigo no local sorteado
                Instantiate(inimigoPrefab, pontosDeSpawn[pontoSorteado].position, Quaternion.identity);
            }
        }
    }
}
