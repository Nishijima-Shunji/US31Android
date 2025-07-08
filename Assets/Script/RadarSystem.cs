using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarSystem2 : MonoBehaviour
{
    [Header("���[�_�[�ݒ�")]
    public Transform player;
    public float radarRange = 50f;
    public RectTransform radarPanel;
    public GameObject blipPrefab;

    private List<GameObject> blips = new List<GameObject>();
    private string enemyTag = "Enemy";

    void Update()
    {
        // �G�̍Ď擾�i���I�ɑΉ��j
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);

        // �����u���b�v���폜
        foreach (var blip in blips)
        {
            Destroy(blip);
        }
        blips.Clear();

        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy == null) continue;

            Vector3 relativePos = enemy.transform.position - player.position;

            if (relativePos.magnitude > radarRange) continue;

            Vector2 radarPos = new Vector2(relativePos.x, relativePos.z);
            radarPos = radarPos / radarRange * (radarPanel.rect.width / 2f);

            GameObject blip = Instantiate(blipPrefab, radarPanel);
            blip.GetComponent<RectTransform>().anchoredPosition = radarPos;
            blips.Add(blip);
        }
    }
}
