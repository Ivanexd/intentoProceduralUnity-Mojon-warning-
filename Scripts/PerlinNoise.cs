using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 100.0f;

    public float offsetX = 100.0f;
    public float offsetY = 100.0f;
    private float treeDensity = .7f;

    public GameObject treePrefab; // Añade tu prefab de árbol aquí

    private void Start()
    {
        offsetX = Random.Range(0, 99999f);
        offsetY = Random.Range(0, 99999f);

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
        
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                UnityEngine.Color color = CalcularColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    UnityEngine.Color CalcularColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        float desertSample = Mathf.PerlinNoise(xCoord / 2f, yCoord / 2f); // Segunda capa de ruido de Perlin
        

        // Si el valor de sample es menor a 0.3, consideramos que es agua y devolvemos azul
        if (sample < 0.3f)
        {
            return UnityEngine.Color.blue;
        }
        // Si el valor de desertSample es mayor a 0.5, consideramos que es desierto y devolvemos amarillo
        else if (desertSample > 0.4f)
        {
            float v = Random.Range(0f, treeDensity);
            if (v > 0.69)
            {
                Vector3 position = new Vector3(x-(width/2), y-(height/2), 0); // Ajusta la posición según sea necesario
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);
                tree.transform.SetParent(this.transform);
                tree.transform.position = position;
            }
            return UnityEngine.Color.green;
            // Si el color es verde, instanciamos un árbol
            
        }
        // Si no se cumple ninguna de las condiciones anteriores, consideramos que es suelo y devolvemos verde
        else
        {
            // Usamos Lerp para suavizar la transición entre el verde y el amarillo
            float lerp = (sample - 0.3f) / (0.5f - 0.3f); // Normalizamos el valor de sample entre 0 y 1
            return UnityEngine.Color.Lerp(UnityEngine.Color.green, UnityEngine.Color.yellow, lerp);
        }
    }
}
