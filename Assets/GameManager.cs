using UnityEngine;
using TMPro; // Necessário para mexer com Textos modernos na Unity
using UnityEngine.SceneManagement; // Necessário para trocar de fase

public class GameManager : MonoBehaviour
{
    // A "instancia" permite que outros scripts achem o GameManager facilmente
    public static GameManager instancia;

    [Header("Progresso da Fase")]
    public int killsNecessarias = 10; // Quantos inimigos precisa matar
    private int killsAtuais = 0;
    public string nomeProximaFase = "Fase2"; // O nome exato da próxima cena

    [Header("Interface Visual")]
    public TextMeshProUGUI textoContador; // O texto que vai ficar na tela

    void Awake()
    {
        // Garante que só exista um GameManager na tela
        if (instancia == null) { instancia = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        AtualizarTexto();
    }

    // Essa função será chamada pelo inimigo quando ele morrer
    public void AdicionarKill()
    {
        killsAtuais++;
        AtualizarTexto();

        // Verifica se bateu a meta
        if (killsAtuais >= killsNecessarias)
        {
            PassarDeFase();
        }
    }

    private void AtualizarTexto()
    {
        if (textoContador != null)
        {
            textoContador.text = "Almas Coletadas: " + killsAtuais + " / " + killsNecessarias;
        }
    }

    private void PassarDeFase()
    {
        Debug.Log("Passando para a próxima fase...");
        // Carrega a cena que você digitou no painel da Unity
        SceneManager.LoadScene(nomeProximaFase);
    }
}