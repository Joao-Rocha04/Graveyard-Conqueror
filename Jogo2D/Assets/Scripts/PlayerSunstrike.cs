using System.Collections.Generic;
using UnityEngine;

public class PlayerSunstrike : MonoBehaviour
{
    public float sunstrikeOffsetY = 2f;
    public GameObject sunstrikePrefab;
    public float intervaloDeAtaque = 1.5f; // tempo entre ataques (em segundos)
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
        if (inimigosNoAlcance.Count == 0) return;

        Transform alvoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;
        Vector2 posJogador = transform.position;

        foreach (Collider2D col in inimigosNoAlcance)
        {
            if (col == null) continue;
            float dist = Vector2.Distance(posJogador, col.transform.position);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                alvoMaisProximo = col.transform;
            }
        }

        if (alvoMaisProximo != null)
        {
            Vector3 spawnPos = alvoMaisProximo.position + Vector3.up * sunstrikeOffsetY;
            Instantiate(sunstrikePrefab, spawnPos, Quaternion.identity);
            AudioSource.PlayClipAtPoint(sfxRaio, spawnPos, sfxVolume);

        }
    }
}
