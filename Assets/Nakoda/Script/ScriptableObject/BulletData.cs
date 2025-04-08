using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletData", menuName = "Game/Bullet Data")]
public class BulletData : ScriptableObject
{
    public GameObject bulletPrefab; // Prefab peluru
    public float bulletForce = 250; // Kekuatan peluru
    public float bulletDamage = 20f; // Damage peluru
}
