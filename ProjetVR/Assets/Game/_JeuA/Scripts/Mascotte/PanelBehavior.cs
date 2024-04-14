using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PanelBehavior : MonoBehaviour
{
    public Coroutine textCoroutine;

    public TextMeshPro textMesh;

    private List<Vector2Int> wobbleSections = new List<Vector2Int>();

    private void Update()
    {
        Wobble();
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

    public void SetDialog(SO_Dialogs newDialog)
    {
        ResetText();

        textMesh.text = newDialog.content;

        SetWoobleSections();
    }

    public void SetText(string newText, float time)
    {
        ResetText();

        textCoroutine = StartCoroutine(SetTextWithTime(newText, time));
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
            textMesh.transform.position = originalPos + Random.insideUnitSphere * 0.1f;
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

    IEnumerator AudioFade(AudioSource source)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
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

            Debug.Log(value);

            value.x -= 3;
            value.y -= 3;

            wobbleSections[i] = value;
        }

        textMesh.text = textMesh.text.Replace("[w]", "");
        textMesh.text = textMesh.text.Replace("[/w]", "");
    }
}
