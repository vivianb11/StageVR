using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PanelBehavior : MonoBehaviour
{
    public Coroutine textCoroutine;

    public TextMeshPro textMesh;

    [Header("Color Gradient")]
    public Gradient colorGrad;

    [SerializeField] Transform background;

    private List<Vector2Int> wobbleSections = new List<Vector2Int>();

    private List<Vector2Int> rainbowSections = new List<Vector2Int>();

    [SerializeField] Vector2 padding;

    private void Update()
    {
        Rainbow();
        //Wobble();
    }

    private void Wobble()
    {
        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;

        foreach (var item in wobbleSections)
        {
            for (int i = item.x; i < item.y; i++)
            {
                var charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 2f) * 0.05f, 0);
                }
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textMesh.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private void Rainbow()
    {
        textMesh.ForceMeshUpdate();
        var mesh = textMesh.mesh;
        var vertices = mesh.vertices;
        var textInfo = textMesh.textInfo;

        Color[] colors = mesh.colors;

        foreach (var item in wobbleSections)
        {
            for (int w = item.x; w < item.y; w++)
            {
                colors[w] = colorGrad.Evaluate(Mathf.Repeat(Time.time + vertices[w].x * 0.001f, 1f));
                colors[w + 1] = colorGrad.Evaluate(Mathf.Repeat(Time.time + vertices[w + 1].x * 0.001f, 1f));
                colors[w + 2] = colorGrad.Evaluate(Mathf.Repeat(Time.time + vertices[w + 2].x * 0.001f, 1f));
                colors[w + 3] = colorGrad.Evaluate(Mathf.Repeat(Time.time + vertices[w + 3].x * 0.001f, 1f));
            }
        }
    }

    public void SetDialogue(string dialogue)
    {
        ResetText();

        textMesh.text = dialogue;

        StartCoroutine(ScaleBackground());

        //SetWoobleSections();
        //SetColorSections();
    }

    private IEnumerator ScaleBackground()
    {
        yield return new WaitForSeconds(0.01f);

        Vector3 scale = Vector2.zero;
        scale.x = textMesh.GetRenderedValues().x + padding.x;
        scale.y = textMesh.GetRenderedValues().y + padding.y;
        scale.z = 0.1f;

        background.localScale = scale;
    }

    IEnumerator SetTextWithTime(string newText, float time)
    {
        foreach (var letter in newText)
        {
            textMesh.text += letter;

            yield return new WaitForSeconds((time / newText.Length));
        }
        
        textCoroutine = null;
    }

    public void ShakeText() 
    {
        InvokeRepeating("ShakeTextCoroutine", 0, 0.01f);
    }

    IEnumerator ShakeTextCoroutine(float fortime)
    {
        Vector3 originalPos = textMesh.transform.position;
        float time = 0;
        while (time < fortime)
        {
            textMesh.transform.position = originalPos + UnityEngine.Random.insideUnitSphere * 0.1f;
            time += Time.deltaTime;
            yield return null;
        }
        textMesh.transform.position = originalPos;
    }

    public void ResetText()
    {
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        else
            textMesh.text = "";
    }

    private void SetWoobleSections()
    {
        wobbleSections.Clear();

        for (int i = 0; i < textMesh.text.SplitPattern("[w]", "[/w]").Length; i++)
        {
            if (textMesh.text.Contains("[w]") && textMesh.text.Contains("[/w]"))
            {
                int startIndex = wobbleSections.Count > 0 ? wobbleSections[wobbleSections.Count - 1].y + 3 : 0;

                wobbleSections.Add(new Vector2Int(textMesh.text.SearchPatternEnd(startIndex, "[w]"), textMesh.text.SearchPatternBegin(startIndex, "[/w]")));
            }
        }

        for (int i = 0; i < wobbleSections.Count; i++)
        {
            Vector2Int value = wobbleSections[i];

            int subValue = (i + 1) * 3 + i * 4;

            value -= new Vector2Int(subValue, subValue);

            wobbleSections[i] = value;
        }

        textMesh.text = textMesh.text.Replace("[w]", "");
        textMesh.text = textMesh.text.Replace("[/w]", "");
    }

    private void SetColorSections()
    {
        rainbowSections.Clear();

        for (int i = 0; i < textMesh.text.SplitPattern("[r]", "[/r]").Length; i++)
        {
            if (textMesh.text.Contains("[r]") && textMesh.text.Contains("[/r]"))
            {
                int startIndex = rainbowSections.Count > 0 ? rainbowSections[rainbowSections.Count - 1].y + 3 : 0;

                rainbowSections.Add(new Vector2Int(textMesh.text.SearchPatternEnd(startIndex, "[r]"), textMesh.text.SearchPatternBegin(startIndex, "[/r]")));
            }
        }

        for (int i = 0; i < rainbowSections.Count; i++)
        {
            Vector2Int value = rainbowSections[i];

            int subValue = (i + 1) * 3 + i * 4;

            value -= new Vector2Int(subValue, subValue);

            rainbowSections[i] = value;
        }

        textMesh.text = textMesh.text.Replace("[r]", "");
        textMesh.text = textMesh.text.Replace("[/r]", "");
    }
}
