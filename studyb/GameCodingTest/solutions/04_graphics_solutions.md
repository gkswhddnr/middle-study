# 그래픽스 8문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## G1. 렌더링 파이프라인

```
[CPU 측]
1. Local Space:  모델 원점 기준 정점 좌표 (모델링한 그대로)
2. World Space:  Model Matrix 곱 → 씬 안에서의 위치
3. View Space:   View Matrix 곱 → 카메라 기준 좌표
4. Clip Space:   Projection Matrix 곱 → -1~1 정규화 직전 4D
[GPU 측 — Vertex Shader가 여기까지 책임]

5. Perspective Divide: w로 나눠 NDC (-1~1)
6. Screen Space: 뷰포트 변환 → 실제 픽셀 좌표

7. Rasterizer:   삼각형을 픽셀 단위 fragment로 쪼갬
8. Fragment(Pixel) Shader: 각 fragment의 색 결정
9. Output Merger: Depth Test, Stencil Test, Blending → 최종 픽셀
```

**처리 단위:**
- Vertex Shader = 정점 1개당 1번
- Fragment Shader = 픽셀 1개당 1번 (오버드로 있으면 같은 픽셀 여러 번)

**왜 중요:** 모바일에선 Fragment Shader가 훨씬 비싸다. 고해상도일수록 fragment 수가 폭발적.

---

## G2. MVP 행렬

```
Model Matrix:      Local → World (회전, 이동, 스케일)
View Matrix:       World → View (카메라의 역행렬)
Projection Matrix: View → Clip (원근투영, 또는 직교투영)

clipPos = Projection * View * Model * localPos
        = MVP * localPos
(행렬 곱은 오른쪽부터 적용된다)
```

**Unity 관련:**
- `Camera.worldToCameraMatrix` = View Matrix
- `Camera.projectionMatrix` = Projection Matrix
- `Renderer.localToWorldMatrix` = Model Matrix
- ShaderLab의 `UNITY_MATRIX_MVP` = 셋 다 곱한 것

---

## G3. 알파 블렌딩

**공식 (가장 일반적인 SrcAlpha · OneMinusSrcAlpha):**
```
finalColor = srcAlpha * srcColor + (1 - srcAlpha) * dstColor
```

**계산:**
- 빨강 (1,0,0,0.5) 위에 흰 (1,1,1,1):
  - R = 0.5*1 + 0.5*1 = 1.0
  - G = 0.5*0 + 0.5*1 = 0.5
  - B = 0.5*0 + 0.5*1 = 0.5
  - 결과: 핑크 (1, 0.5, 0.5)

- 빨강 (1,0,0,0.5) 위에 검정 (0,0,0,1):
  - R = 0.5*1 + 0.5*0 = 0.5
  - G = 0, B = 0
  - 결과: 어두운 빨강 (0.5, 0, 0)

**블렌딩 모드:**
- **Alpha Blend** = `SrcAlpha · OneMinusSrcAlpha` → 일반 반투명 (유리, UI)
- **Additive** = `One · One` → 색이 더해짐 (불, 빛, 폭발, 마법)
- **Multiply** = `DstColor · Zero` → 색이 곱해짐 (그림자, 색 필터)

---

## G4. Z-Buffer

1. **왜 필요:** 안 그러면 그리는 순서대로 덮어쓰기 → 멀리 있는 게 가까이 있는 걸 덮을 수 있음.
2. **저장값:** 각 픽셀의 깊이(카메라로부터 거리, 정확히는 NDC z, 0~1). 새 픽셀이 더 가까우면 통과, 더 멀면 폐기.
3. **Z-Fighting:** 두 면이 거의 같은 depth → 부동소수점 오차로 매 프레임 누가 앞인지 바뀌어 깜빡임.
   - 해결: 면 사이 거리 띄우기, near/far plane 비율 줄이기 (특히 near를 크게), depth bias 사용
4. **투명 + Z-Buffer 문제:**
   - 투명 오브젝트가 깊이를 써버리면 뒤에 있는 다른 투명이 컬링됨
   - 해결 1: 투명 오브젝트는 `ZWrite Off` (depth 쓰기 안 함)
   - 해결 2: 투명은 카메라에서 먼 것부터 뒤→앞 정렬해서 그리기
   - Unity URP는 자동으로 Transparent 큐를 뒤→앞 정렬

---

## G5. Vertex Shader (흔들리는 효과)

```hlsl
struct appdata { float4 vertex : POSITION; };
struct v2f { float4 pos : SV_POSITION; };

v2f vert(appdata v) {
    v2f o;
    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
    worldPos.y += sin(_Time.y) * 0.3;
    o.pos = mul(UNITY_MATRIX_VP, worldPos);
    // 또는 한 줄로: o.pos = UnityObjectToClipPos(v.vertex + float4(0, sin(_Time.y)*0.3, 0, 0));
    return o;
}
```

**주의:**
- `_Time.y` = `t` (초). `_Time.x` = `t/20`, `_Time.z` = `t*2`, `_Time.w` = `t*3`
- 위에서 흔드는 양은 월드 공간에서 더해야 회전 후에도 위쪽이 일관됨. Local에서 더하면 회전된 모델의 "위"가 다른 방향이 됨.

---

## G6. Fragment Shader (그라데이션)

```hlsl
fixed4 frag(v2f i) : SV_Target {
    fixed4 col;
    col.rgb = lerp(fixed3(1,0,0), fixed3(0,0,1), i.uv.x); // R→B
    if (i.uv.y < 0.5) col.rgb *= 0.5;                     // 아래 절반 어둡게
    col.a = 1;
    return col;
}
```

**팁:** `step(0.5, i.uv.y)`로 if 없이 분기 없는 코드 가능. GPU는 분기에 약하므로 무분기 쪽이 더 성능 좋음.

```hlsl
float dark = lerp(0.5, 1.0, step(0.5, i.uv.y));
col.rgb *= dark;
```

---

## G7. Back-Face Culling

1. **왜 도움:** 뒤집힌 면은 보이지 않으므로 fragment shader 호출 자체를 스킵 → fillrate 절약. 보통 50%가량 fragment 제거.
2. **누가 결정:** CPU에서 메시 생성 시 정점 순서(winding order). Unity는 기본 CW가 앞면 (그래픽스 API 설정에 따라 바뀌기도).
3. **계산:**
   ```csharp
   public static bool IsFrontFace(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 cameraForward) {
       Vector3 edge1 = v1 - v0;
       Vector3 edge2 = v2 - v0;
       Vector3 normal = Vector3.Cross(edge1, edge2); // CCW면 노멀이 +Z
       return Vector3.Dot(normal, cameraForward) < 0; // 노멀이 카메라 반대편 = 앞면
   }
   ```
   주어진 삼각형:
   - edge1 = (1, -1, 0), edge2 = (-1, -1, 0)
   - normal = edge1 × edge2 = (((-1)(0) - (0)(-1)), ((0)(-1) - (1)(0)), ((1)(-1) - (-1)(-1))) = (0, 0, -2)
   - 카메라 forward = (0, 0, 1)
   - dot(normal, forward) = -2 < 0 → 앞면 ✓

---

## G8. LOD 시스템

```csharp
public class LODGroup : MonoBehaviour {
    public GameObject[] meshes;
    public float[] distanceThresholds;
    public Transform cameraTransform;
    private float nextUpdate = 0f;

    void Update() {
        // 매 프레임 X, 0.5초마다
        if (Time.time < nextUpdate) return;
        nextUpdate = Time.time + 0.5f;

        float dist = Vector3.Distance(transform.position, cameraTransform.position);
        int active = meshes.Length - 1; // 기본은 가장 단순한 것
        for (int i = 0; i < distanceThresholds.Length; i++) {
            if (dist <= distanceThresholds[i]) {
                active = i;
                break;
            }
        }
        for (int i = 0; i < meshes.Length; i++)
            meshes[i].SetActive(i == active);
    }
}
```

**최적화 더:**
- `sqrMagnitude` 사용으로 Sqrt 회피, threshold도 미리 제곱해두기
- 거리에 따라 갱신 주기 변경: 가까울수록 자주, 멀수록 드물게
- 가장 단순한 LOD조차 안 보일 거리면 통째로 비활성화 (오클루전과 결합)

**참고:** Unity에 빌트인 `LODGroup` 컴포넌트가 있다 (혼동 주의 — 위 코드는 직접 구현 버전). 실무에선 빌트인 쓰는 게 보통.
