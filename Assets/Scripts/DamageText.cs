using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float lifetime = 0.6f;          // 텍스트가 존재할 시간
    private float elapsedTime = 0f;         // 경과 시간
    private float gravity = -9.8f;          // 중력 가속도 (Y축에만 적용)

    private Vector3 startPos;               // 시작 위치
    private Vector3 velocity;               // 초기 속도

    private TextMeshPro text;
    private Color alpha;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Initalize(int _dmg)
    {
        startPos = transform.position;
        elapsedTime = 0f;

        alpha = new Color(1, 1, 1, 1);
        text.text = _dmg.ToString();
        text.color = alpha;

        // ▶ 오른쪽 위 방향으로 튀는 초기 속도 설정
        float xPower = Random.Range(1.0f, 4.0f);   // 우측으로 더 넓게 튀도록
        float yPower = Random.Range(2.0f, 5.0f);   // 위로 더 다양하게 튀도록
        velocity = new Vector3(xPower, yPower, 0f); // 초기 속도 벡터

        // ▶ 크기 랜덤 튀는 느낌
        transform.localScale = Vector3.one * Random.Range(1.0f, 1.3f);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // ▶ 포물선 궤도 계산 (기본 물리 공식을 사용)
        float dt = elapsedTime;
        Vector3 offset = velocity * dt + 0.5f * new Vector3(0, gravity, 0) * dt * dt;
        transform.position = startPos + offset;

        // ▶ 점점 투명하게
        alpha.a = Mathf.Lerp(1, 0, elapsedTime / lifetime);
        text.color = alpha;

        // ▶ 수명이 다하면 비활성화
        if (elapsedTime >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
