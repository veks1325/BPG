//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaserController : MonoBehaviour
//{
//    public GameObject L1;
//    public GameObject L2; // The laser prefab to instantiate
//    public Vector3 laserSpawnPoint; // The point where the laser will appear

//    void Start()
//    {
//        L1 = GameObject.FindGameObjectWithTag("L1");
//        L2 = GameObject.FindGameObjectWithTag("L2");
//        StartCoroutine(SpawnLaser());
//    }

//    IEnumerator SpawnLaser()
//    {
//        while (true)
//        {
//            float randomx = Random.Range(-15f, 15.0f);
//            float randomy = Random.Range(-5f, 5.0f);

//            // Random rotation angle
//            float randomRotationZ = Random.Range(0f, 360f);
//            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomRotationZ);

//            // Random spawn point
//            laserSpawnPoint = new Vector3(randomx, randomy, 0f);

//            yield return new WaitForSeconds(4f); // Wait for 4 seconds

//            // Instantiate L1 with the same random rotation
//            GameObject laser = Instantiate(L1, laserSpawnPoint, randomRotation);
//            Destroy(laser, 0.2f); // Destroy the laser after 0.2 seconds

//            yield return new WaitForSeconds(1f); // Wait for 1 second

//            // Instantiate L2 with the same random rotation
//            GameObject laser2 = Instantiate(L2, laserSpawnPoint, randomRotation);
//            Destroy(laser2, 1f); // Destroy the laser after 1 second
//        }
//    }
//}
