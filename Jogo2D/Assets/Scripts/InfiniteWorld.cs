using UnityEngine;
using UnityEngine.Tilemaps;              // <- necessário para TileBase
using System.Collections.Generic;

public class InfiniteWorld : MonoBehaviour
{
    [Header("Target & Prefab")]
    public Transform target;            // câmera ou player
    public Chunk chunkPrefab;           // prefab com Grid + Tilemaps + script Chunk

    [Header("World Settings")]
    [Min(1)] public int chunkSize = 32; // tiles por lado
    [Min(0)] public int viewRadius = 2; // raio em chunks
    public int seed = 12345;

    [Header("Tiles - Ground/Grass")]
    public TileBase[] groundTiles;
    public TileBase[] grassTiles;
    [Range(0,1)] public float grassChance = 0.25f;

    [Header("Tiles - Objects/Shadow")]
    public TileBase[] objectTiles;
    [Range(0,1)] public float objectChance = 0.08f;
    public TileBase[] shadowTiles;
    [Range(0,1)] public float shadowChance = 0.08f;

    private readonly Dictionary<Vector2Int, Chunk> loaded = new Dictionary<Vector2Int, Chunk>();
    private readonly List<Vector2Int> toRemove = new List<Vector2Int>();

    void Update()
    {
        if (!target || !chunkPrefab || chunkSize <= 0) return;

        Vector2Int center = WorldToChunk(target.position);

        // Criar/garantir os chunks ao redor
        for (int y = -viewRadius; y <= viewRadius; y++)
        for (int x = -viewRadius; x <= viewRadius; x++)
        {
            Vector2Int c = new Vector2Int(center.x + x, center.y + y);
            if (!loaded.ContainsKey(c))
                CreateChunk(c);
        }

        // Remover chunks distantes
        toRemove.Clear();
        foreach (var kv in loaded)
        {
            if (Mathf.Abs(kv.Key.x - center.x) > viewRadius ||
                Mathf.Abs(kv.Key.y - center.y) > viewRadius)
                toRemove.Add(kv.Key);
        }
        foreach (var c in toRemove)
        {
            if (loaded.TryGetValue(c, out var ch))
            {
                Destroy(ch.gameObject);
                loaded.Remove(c);
            }
        }
    }

    Vector2Int WorldToChunk(Vector3 pos)
    {
        int cx = Mathf.FloorToInt(pos.x / (float)chunkSize);
        int cy = Mathf.FloorToInt(pos.y / (float)chunkSize);
        return new Vector2Int(cx, cy);
    }

    void CreateChunk(Vector2Int c)
    {
        Vector3 pos = new Vector3(c.x * chunkSize, c.y * chunkSize, 0f);
        var chunk = Instantiate(chunkPrefab, pos, Quaternion.identity, transform);
        chunk.name = $"Chunk_{c.x}_{c.y}";

        // Geração do conteúdo do chunk
        chunk.Generate(
            c, chunkSize, seed,
            groundTiles,
            grassTiles, grassChance,
            objectTiles, objectChance,
            shadowTiles, shadowChance
        );

        loaded[c] = chunk;
    }
}
