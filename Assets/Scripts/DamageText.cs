using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    TextMeshPro text;
    Color alpha;

    private void Awake()
    {
        moveSpeed = 1.5f;
        alphaSpeed = 1.0f;
        destroyTime = 1.5f;

        text = GetComponent<TextMeshPro>();
    }

    public void Initalize(int _dmg)
    {
        alpha = new Color(1, 1, 1, 1);
        text.text = _dmg.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // �ؽ�Ʈ ��ġ

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // �ؽ�Ʈ ���İ�
        text.color = alpha;
    }

    private void DestroyObject()
    {
        this.gameObject.SetActive(false);
    }
}
