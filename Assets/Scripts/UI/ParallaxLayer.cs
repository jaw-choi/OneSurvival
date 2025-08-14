using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;                // (�ѱ� �ּ�) ī�޶�
    public float parallaxFactor = 0.3f;  // (�ѱ� �ּ�) 0~1, �������� �� �� ����
    private Renderer rend;
    private Vector3 startCamPos;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        startCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - startCamPos;
        // (�ѱ� �ּ�) ��Ƽ���� ���������� ���� ��ũ�� ����
        Vector2 uv = new Vector2(delta.x, delta.y) * parallaxFactor * 0.01f;
        // (�ѱ� �ּ�) Instancing ����: sharedMaterial ��� MaterialPropertyBlock ����
        rend.material.mainTextureOffset = uv;
    }
}
