using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;

    float radius;     // プレイヤーとの距離
    float latitude;   // 緯度（-90〜90）
    float longitude;  // 経度（0〜360）

    float latSpeed;
    float lonSpeed;

    Vector3 prevPos;

    [SerializeField] private GameObject hitEffectPrefab; // エフェクト用プレハブ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
            enabled = false;
            return;
        }

        // 現在位置 → プレイヤーとの方向・距離
        prevPos = transform.position;
        Vector3 dir = transform.position - player.transform.position;
        radius = dir.magnitude; // ★その時の距離を保持
        SphericalFromDirection(dir.normalized, out latitude, out longitude);

        StartCoroutine(ChangeDirectionLoop());
    }

    void Update()
    {
        if (player == null) return;

        // 経緯度の更新
        latitude += latSpeed * Time.deltaTime;
        longitude += lonSpeed * Time.deltaTime;
        latitude = Mathf.Clamp(latitude, -85f, 85f);

        // 球面上の次の位置
        Vector3 dir = DirectionFromSpherical(latitude, longitude);
        Vector3 newPos = player.transform.position + dir * radius;

        // 進行方向ベクトル
        Vector3 velocity = (newPos - prevPos).normalized;

        // 回転（進行方向を向く）
        if (velocity.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        // 位置更新
        transform.position = newPos;

        // 前位置更新
        prevPos = newPos;
    }

    IEnumerator ChangeDirectionLoop()
    {
        while (true)
        {
            latSpeed = Random.Range(-10f, 10f);
            lonSpeed = Random.Range(-20f, 20f);
            yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));
        }
    }

    Vector3 DirectionFromSpherical(float latDeg, float lonDeg)
    {
        float lat = latDeg * Mathf.Deg2Rad;
        float lon = lonDeg * Mathf.Deg2Rad;

        float x = Mathf.Cos(lat) * Mathf.Cos(lon);
        float y = Mathf.Sin(lat);
        float z = Mathf.Cos(lat) * Mathf.Sin(lon);
        return new Vector3(x, y, z);
    }

    void SphericalFromDirection(Vector3 dir, out float latDeg, out float lonDeg)
    {
        latDeg = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        lonDeg = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnemyController: Triggered with " + other.name);
        if (other.CompareTag("Bullet"))
        {
            // エフェクト再生
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // カウントアップ
            ScoreCounter killCounter = FindObjectOfType<ScoreCounter>();
            if (killCounter != null)
            {
                killCounter.AddKill();
            }

            // 弾と敵を削除
            Destroy(other.gameObject); // Bullet
            Destroy(gameObject);       // Enemy
        }
    }
}
