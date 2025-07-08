using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Transform firePoint; // カメラ or 銃口

    private float touchStartTime = -1f;

    void Update()
    {
        // マウスクリック発射
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // タップ検出
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float duration = Time.time - touchStartTime;

                // 0.01〜0.1秒の短タップのみで発射
                if (duration >= 0.01f && duration <= 0.1f)
                {
                    Shoot();
                }

                touchStartTime = -1f;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // ★ Bulletタグをセット
        bullet.tag = "Bullet";

        // 速度を与える
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
    }
}
