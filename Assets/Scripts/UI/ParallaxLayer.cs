using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;                // (한글 주석) 카메라
    public float parallaxFactor = 0.3f;  // (한글 주석) 0~1, 작을수록 더 먼 느낌
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
        // (한글 주석) 머티리얼 오프셋으로 무한 스크롤 느낌
        Vector2 uv = new Vector2(delta.x, delta.y) * parallaxFactor * 0.01f;
        // (한글 주석) Instancing 방지: sharedMaterial 대신 MaterialPropertyBlock 권장
        rend.material.mainTextureOffset = uv;
    }
}
