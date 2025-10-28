using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    public Tilemap ground;   // TileBackGround
    public Tilemap grass;    // TileBackGroundGrass
    public Tilemap objects;  // ForeGround (objetos: árvores/rochas)
    public Tilemap shadow;   // Shadow layer para objetos

    public void ClearAll()
    {
        if (ground) ground.ClearAllTiles();
        if (grass)  grass.ClearAllTiles();
        if (objects) objects.ClearAllTiles();
        if (shadow)  shadow.ClearAllTiles();
    }

    public void Generate(
        Vector2Int c, int size, int seed,
        TileBase[] groundTiles,
        TileBase[] grassTiles, float grassChance,
        TileBase[] objectTiles, float objectChance,
        TileBase[] shadowTiles, float shadowChance,
        float noiseScale = 18f)
    {
        ClearAll();

        int ox = c.x * size, oy = c.y * size;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            int wx = ox + x, wy = oy + y;

            // tile de chão
            float n = Mathf.PerlinNoise((wx + seed) / noiseScale, (wy - seed) / noiseScale);
            int gi = Mathf.Clamp(Mathf.FloorToInt(n * groundTiles.Length), 0, groundTiles.Length - 1);
            ground.SetTile(new Vector3Int(x, y, 0), groundTiles[gi]);

            // grama (overlay)
            if (grass && grassTiles != null && grassTiles.Length > 0 && grassChance > 0f)
            {
                float g = Mathf.PerlinNoise((wx + seed * 2) / (noiseScale * 0.8f), (wy + seed * 3) / (noiseScale * 0.8f));
                if (g > 1f - grassChance)
                {
                    int idx = Mathf.Clamp(Mathf.FloorToInt(g * grassTiles.Length), 0, grassTiles.Length - 1);
                    grass.SetTile(new Vector3Int(x, y, 0), grassTiles[idx]);
                }
            }

            // objetos (árvores/rochas)
            bool placedObject = false;
            if (objects && objectTiles != null && objectTiles.Length > 0 && objectChance > 0f)
            {
                float d = Mathf.PerlinNoise((wx - seed * 5) / (noiseScale * 0.5f), (wy + seed * 7) / (noiseScale * 0.5f));
                if (d > 1f - objectChance)
                {
                    int di = Mathf.Clamp(Mathf.FloorToInt(d * objectTiles.Length), 0, objectTiles.Length - 1);
                    objects.SetTile(new Vector3Int(x, y, 0), objectTiles[di]);
                    placedObject = true;
                }
            }

            // sombras para objetos (layer separada)
            if (shadow && shadowTiles != null && shadowTiles.Length > 0 && shadowChance > 0f)
            {
                // preferência: usar ruído próprio para sombras para variar, mas geralmente correlacionar com presença de objeto
                float s = Mathf.PerlinNoise((wx - seed * 11) / (noiseScale * 0.6f), (wy + seed * 13) / (noiseScale * 0.6f));
                if ( (placedObject && s > 0f) && s > 1f - shadowChance ) // mais propenso quando há objeto
                {
                    int si = Mathf.Clamp(Mathf.FloorToInt(s * shadowTiles.Length), 0, shadowTiles.Length - 1);
                    shadow.SetTile(new Vector3Int(x, y, 0), shadowTiles[si]);
                }
            }
        }
    }
}
