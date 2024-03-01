using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject[] spawner;
    public GameObject[] mob;
    public GameObject mascotte;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            GameObject spawnerLocation = spawner[Random.Range(0, spawner.Length)];
            GameObject selectedMob = mob[Random.Range(0, mob.Length)];
            GameObject newMob = Instantiate(selectedMob, spawnerLocation.transform.position, Quaternion.identity);
            Mob ennemy = newMob.GetComponent<Mob>();
            if (ennemy != null)
                ennemy.target = mascotte.transform;
            yield return new WaitForSeconds(1.5f);
        }

    }
}
