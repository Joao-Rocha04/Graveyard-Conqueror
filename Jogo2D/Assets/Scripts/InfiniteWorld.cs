using UnityEngine;
using System.Collections.Generic;

public class EndlessPrefabWorld2D : MonoBehaviour
{
    [Header("Target (câmera ou player)")]
    public Transform target;

    [Header("Tamanho do CHUNK em tiles")]
    [Tooltip("Largura em tiles (X). Ex: 18")]
    public int tilesX = 18;
    [Tooltip("Altura em tiles (Y). Ex: 12")]
    public int tilesY = 12;

    [Header("Conversão tile->unidade (PPU)")]
    [Tooltip("Se 32px = 1 unidade, use 1.0f")]
    public float tileUnitSize = 1f;

    [Header("Carregamento")]
    [Tooltip("Quantos chunks manter ao redor do alvo")]
    public int viewRadius = 2;
    [Tooltip("Mantém sorteio estável por coordenada")]
    public int seed = 12345;

    [Header("Prefabs (seus 6 mapas)")]
    public GameObject[] chunkPrefabs;

    // Dicionário de chunks carregados pela coordenada de grade
    private readonly Dictionary<Vector2Int, GameObject> loaded = new();

    // Stride mundial (passo) em unidades
    float StrideX => tilesX * tileUnitSize;
    float StrideY => tilesY * tileUnitSize;

    void Update()
    {
        if (!target || tilesX <= 0 || tilesY <= 0 ||
            chunkPrefabs == null || chunkPrefabs.Length == 0 ||
            tileUnitSize <= 0f)
            return;

        // Coordenada do chunk em que o target está (grade em stride retangular)
        var cx = Mathf.FloorToInt(target.position.x / StrideX);
        var cy = Mathf.FloorToInt(target.position.y / StrideY);
        var center = new Vector2Int(cx, cy);

        // Garante vizinhança carregada
        for (int y = -viewRadius; y <= viewRadius; y++)
        {
            for (int x = -viewRadius; x <= viewRadius; x++)
            {
                var c = new Vector2Int(center.x + x, center.y + y);
                if (!loaded.ContainsKey(c))
                    CreateChunk(c);
            }
        }

        // Remove chunks muito distantes
        var rm = new List<Vector2Int>();
        foreach (var kv in loaded)
        {
            if (Mathf.Abs(kv.Key.x - center.x) > viewRadius ||
                Mathf.Abs(kv.Key.y - center.y) > viewRadius)
                rm.Add(kv.Key);
        }
        foreach (var c in rm)
        {
            if (loaded.TryGetValue(c, out var go) && go)
                Destroy(go);
            loaded.Remove(c);
        }
    }

    void CreateChunk(Vector2Int c)
    {
        // Escolha determinística de prefab por coordenada
        int h = Hash3(c.x, c.y, seed);
        int idx = Mathf.Abs(h) % chunkPrefabs.Length;
        GameObject prefab = chunkPrefabs[idx];

        // Instancia no zero para medir bounds
        var go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

        // Bounds mundiais do prefab (todos os Renderers)
        Bounds b = GetWorldBounds(go);

        // Posição de grade do canto inferior esquerdo desejado
        Vector3 gridPos = new Vector3(c.x * StrideX, c.y * StrideY, 0f);

        // Move para alinhar b.min ao grid (encosta pelo canto inferior esquerdo)
        Vector3 offset = gridPos - b.min;
        go.transform.position += offset;

        // Verifica tamanho real vs stride esperado (18x12 tiles)
        b = GetWorldBounds(go);
        Vector2 expected = new Vector2(StrideX, StrideY);
        Vector2 actual = new Vector2(b.size.x, b.size.y);

        // Avisa se houver divergência relevante
        if (Mathf.Abs(actual.x - expected.x) > 0.01f || Mathf.Abs(actual.y - expected.y) > 0.01f)
        {
            Debug.LogWarning(
                $"[EndlessPrefabWorld2D] O prefab '{prefab.name}' mede {actual} unidades," +
                $" mas o stride esperado é {expected}. Ajuste o prefab ou tilesX/tilesY/tileUnitSize."
            );
        }

        go.name = $"Chunk_{c.x}_{c.y}_{prefab.name}";
        loaded[c] = go;
    }

    static Bounds GetWorldBounds(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(go.transform.position, Vector3.zero);

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
        return b;
    }

    static int Hash3(int a, int b, int c)
    {
        unchecked
        {
            int h = 17; h = h * 31 + a; h = h * 31 + b; h = h * 31 + c;
            h ^= (h << 13); h ^= (h >> 17); h ^= (h << 5);
            return h;
        }
    }

    void OnDrawGizmosSelected()
    {
        // desenha as caixas dos chunks carregados
        Gizmos.color = Color.yellow;
        foreach (var kv in loaded)
        {
            var center = new Vector3(
                kv.Key.x * StrideX + StrideX / 2f,
                kv.Key.y * StrideY + StrideY / 2f,
                0f
            );
            Gizmos.DrawWireCube(center, new Vector3(StrideX, StrideY, 0f));
        }
    }
}
