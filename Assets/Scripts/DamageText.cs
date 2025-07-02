using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float lifetime = 0.6f;          // �ؽ�Ʈ�� ������ �ð�
    private float elapsedTime = 0f;         // ��� �ð�
    private float gravity = -9.8f;          // �߷� ���ӵ� (Y�࿡�� ����)

    private Vector3 startPos;               // ���� ��ġ
    private Vector3 velocity;               // �ʱ� �ӵ�

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

        // �� ������ �� �������� Ƣ�� �ʱ� �ӵ� ����
        float xPower = Random.Range(1.0f, 4.0f);   // �������� �� �а� Ƣ����
        float yPower = Random.Range(2.0f, 5.0f);   // ���� �� �پ��ϰ� Ƣ����
        velocity = new Vector3(xPower, yPower, 0f); // �ʱ� �ӵ� ����

        // �� ũ�� ���� Ƣ�� ����
        transform.localScale = Vector3.one * Random.Range(1.0f, 1.3f);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // �� ������ �˵� ��� (�⺻ ���� ������ ���)
        float dt = elapsedTime;
        Vector3 offset = velocity * dt + 0.5f * new Vector3(0, gravity, 0) * dt * dt;
        transform.position = startPos + offset;

        // �� ���� �����ϰ�
        alpha.a = Mathf.Lerp(1, 0, elapsedTime / lifetime);
        text.color = alpha;

        // �� ������ ���ϸ� ��Ȱ��ȭ
        if (elapsedTime >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
