using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Random Mobs & Spawners")]
    [SerializeField] GameObject[] spawnerArray;
    [SerializeField] GameObject[] mobArray;

    [Header("Spawner Characteristics")]
    [SerializeField] GameObject target;
    [SerializeField] float spawnInterval;

    private void Start() => StartCoroutine(Spawn());
    
    private IEnumerator Spawn()
    {
        while (true)
        {
            var randomMobAndSpawn = SelectRandomMobAndSpawner();
            SpawnMob(randomMobAndSpawn.Item1, randomMobAndSpawn.Item2);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private (GameObject, GameObject) SelectRandomMobAndSpawner() => (spawnerArray[Random.Range(0, spawnerArray.Length)], mobArray[Random.Range(0, mobArray.Length)]); //Return random spawner and mob

    private void SpawnMob(GameObject _mob, GameObject _spawner)
    /*Instanitate given mob at given spawner location, and set target position to given target*/
    {
        GameObject newMob = Instantiate(_mob, _spawner.transform.position, Quaternion.identity);
        Mob mobBehaviors = newMob.GetComponent<Mob>();
        if (mobBehaviors != null) mobBehaviors.target = target.transform;
    }
}
