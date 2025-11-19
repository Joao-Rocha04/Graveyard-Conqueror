using System.Collections.Generic;
using UnityEngine;

public class PlayerSunstrike : MonoBehaviour
{
    public float sunstrikeOffsetY = 2f;
    public GameObject sunstrikePrefab;

    [Header("Configuração da Arma")]
    public float intervaloDeAtaque = 1.5f;   // tempo entre ataques
    public int quantidadeProjeteis = 1;      // quantos inimigos base por ataque
    public int dano = 1;                     // dano base por projétil

    [Header("Som do raio")]
    public AudioClip sfxRaio;
    public float sfxVolume = 1f;

    private List<Collider2D> inimigosNoAlcance = new List<Collider2D>();
    private float tempoDesdeUltimoAtaque = 0f;

    void Update()
    {
        tempoDesdeUltimoAtaque += Time.deltaTime;

        if (tempoDesdeUltimoAtaque >= intervaloDeAtaque)
        {
            AtacarInimigoMaisProximo();
            tempoDesdeUltimoAtaque = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !inimigosNoAlcance.Contains(other))
        {
            inimigosNoAlcance.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            inimigosNoAlcance.Remove(other);
        }
    }

    void AtacarInimigoMaisProximo()
    {
        // Remove inimigos que foram destruídos
        inimigosNoAlcance.RemoveAll(x => x == null);

        if (inimigosNoAlcance.Count == 0) return;

        Vector2 posJogador = transform.position;

        // Ordena inimigos pela distância ao jogador
        List<Collider2D> alvosOrdenados = new List<Collider2D>(inimigosNoAlcance);
        alvosOrdenados.Sort((a, b) =>
        {
            float da = Vector2.Distance(posJogador, a.transform.position);
            float db = Vector2.Distance(posJogador, b.transform.position);
            return da.CompareTo(db);
        });

        // ===== aplica upgrades =====

        // projéteis extras do upgrade
        int extraProj = (GameUpgrades.Instance != null)
            ? GameUpgrades.Instance.sunstrikeExtraProjectiles
            : 0;
        int totalProjeteis = quantidadeProjeteis + extraProj;

        int projeteisUsados = Mathf.Min(totalProjeteis, alvosOrdenados.Count);

        // multiplicador de dano do upgrade
        float dmgMul = (GameUpgrades.Instance != null)
            ? GameUpgrades.Instance.sunstrikeDamageMul
            : 1f;

        for (int i = 0; i < projeteisUsados; i++)
        {
            Transform alvo = alvosOrdenados[i].transform;
            Vector3 spawnPos = alvo.position + Vector3.up * sunstrikeOffsetY;

            GameObject instancia = Instantiate(sunstrikePrefab, spawnPos, Quaternion.identity);

            // Envia dano para o projétil
            Sunstrike sun = instancia.GetComponent<Sunstrike>();
            if (sun != null)
            {
                sun.dano = Mathf.RoundToInt(dano * dmgMul);
            }

            if (sfxRaio != null)
            {
                AudioSource.PlayClipAtPoint(sfxRaio, spawnPos, sfxVolume);
            }
        }
    }
}
