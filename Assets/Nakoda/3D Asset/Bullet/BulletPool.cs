using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Membuat pool awal
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            // Jika pool kosong, buat bullet baru
            GameObject newBullet = Instantiate(bulletPrefab, position, rotation);
            return newBullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
