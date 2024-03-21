using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Spawner Characteristics")]
    [SerializeField] public GameObject[] spawnerArray;
    [SerializeField] public GameObject[] mobArray;
    [SerializeField] float spawnInterval;

    [Header("Spawned Mob Parameters")]
    [SerializeField] GameObject target;
    [SerializeField] GameObject mobRotator;

    void Start()
    {
        StartCoroutine(SpawnCycle());
    } 

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            (GameObject, GameObject) selectedMobAndSpawner = SelectRandomMobAndSpawner();
            SpawnMob(selectedMobAndSpawner.Item1, selectedMobAndSpawner.Item2);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private (GameObject, GameObject) SelectRandomMobAndSpawner() => (spawnerArray[Random.Range(0, spawnerArray.Length)], mobArray[Random.Range(0, mobArray.Length)]); //Return random spawner and mob

    private void SpawnMob(GameObject _spawner, GameObject _mob)
    {
        Debug.Log(_mob);
        GameObject newMob = Instantiate(_mob, _spawner.transform.position, Quaternion.identity);
        Mob mobBehaviors = newMob.GetComponent<Mob>();

        if(mobBehaviors.canRotate) AddRotator(newMob);

        if (mobBehaviors != null) mobBehaviors.target = target.transform;
    }

    private void AddRotator(GameObject newMob)
    {
        GameObject newParent = Instantiate(mobRotator, Vector3.zero, Quaternion.identity);
        newMob.transform.parent = newParent.transform;
    }
}
