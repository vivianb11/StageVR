using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Behavior : MonoBehaviour
{
    public void OnTPSelected()
    {
        StartCoroutine(SelectedCoroutine());
    }

    public void OnTPDeselected()
    {
        Debug.Log("TP Deselected");

        StopCoroutine(SelectedCoroutine());
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private IEnumerator SelectedCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < 5; i++)
        {
            this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(SelectedCoroutine());
    }

    public void TPplayer()
    {
        GameObject player = GameManager.Instance.player;
        player.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
