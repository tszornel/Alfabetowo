using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Wobbler2 : MonoBehaviour
{
    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;
    List<int> wordIndexes;
    List<int> wordLengths;
    public float vect;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();
        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }
    // Update is called once per frame
    void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        for (int w = 0; w < wordIndexes.Count; w++)
        {
            int wordIndex = wordIndexes[w];
            vect = Random.Range(0.0f, 1.0f);
            Vector3 offset = Wobbling(Time.time + w * Mathf.PingPong(0.0f,Mathf.PI/2));  // w*Random.Range(0.0f,1.0f));
            for (int i = 0; i < wordLengths[w]; i++)
            {
                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];
                int index = c.vertexIndex;
                vertices[index]     += offset;
                offset = Wobbling(Time.time + w );
                //vertices[index + 1] += offset;
                offset = Wobbling(Time.time + w );
                vertices[index + 2] += offset;
                offset = Wobbling(Time.time + w );
                vertices[index + 3] += offset;
            }
        }
        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
    Vector2 Wobbling(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f)*Random.Range(0.0f,1.0f), Mathf.Cos(time * 2.5f)*Random.Range(0.0f, 1.0f));
    }
}
