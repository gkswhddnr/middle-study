# 그래픽스/셰이더 8문제 (Graphics)

게임 그래픽스 프로그래머 면접 단골 + 클라이언트 면접에서 자주 묻는 그래픽스 기초.
순수 코드 문제 + 개념 설명 문제가 섞여있음. **개념 문제는 글로 답을 작성해보고 정답과 비교.**

각 문제 20~60분.

---

## G1. 렌더링 파이프라인 단계 나열 (개념)

3D 모델 하나가 화면 픽셀로 그려지기까지 거치는 단계를 순서대로 나열하고,
각 단계에서 무슨 일이 일어나는지 한 줄로 설명하라.

다음 키워드를 모두 사용:
- Local Space → World Space → View Space → Clip Space → Screen Space
- Vertex Shader, Rasterizer, Fragment(Pixel) Shader, Output Merger
- Culling, Depth Test, Blending

추가 질문: Vertex Shader와 Fragment Shader가 각각 처리하는 "단위"는 무엇인가?

---

## G2. 좌표 변환 — Model/View/Projection 행렬 (개념 + 코드)

다음 3개의 변환 행렬이 하는 일을 한 줄씩 설명하고, 곱하는 **순서**를 적어라.

```
Model Matrix:
View Matrix:
Projection Matrix:

clipPos = (어떤 순서로 곱?) * localPos
```

추가: Unity의 `Camera.worldToCameraMatrix`는 위 3개 중 무엇에 해당하는가?

---

## G3. 알파 블렌딩 공식 (개념 + 계산)

알파 블렌딩의 기본 공식을 적어라:
```
finalColor = ??? * srcColor + ??? * dstColor
```

다음 케이스를 손으로 계산:
- 빨강 `(1, 0, 0, 0.5)`를 흰색 `(1, 1, 1, 1)` 배경 위에 그릴 때 결과 색?
- 같은 빨강을 검정 `(0, 0, 0, 1)` 배경 위에 그릴 때?

블렌딩 모드 종류:
- Alpha Blend (가장 일반적)
- Additive (라이트, 파티클)
- Multiply (그림자, 어두운 오버레이)

각 모드의 공식과 게임에서 어떤 효과에 쓰는지 한 줄.

---

## G4. Z-Buffer (Depth Buffer) 원리 (개념)

다음 질문에 답하라:
1. Z-Buffer는 왜 필요한가? 없으면 어떤 문제가 생기는가?
2. Z-Buffer는 무엇을 저장하는가? (값의 의미)
3. **Z-Fighting**이 무엇이고 왜 생기며 어떻게 줄이는가?
4. 투명한 오브젝트가 Z-Buffer와 함께 쓰일 때 생기는 문제와 해결법 (정렬?)

---

## G5. 간단한 Vertex Shader 작성 (HLSL/ShaderLab)

Unity의 ShaderLab 문법으로 다음 셰이더의 Vertex 함수를 작성하라.
- 입력: 모델 공간의 정점 위치 `float4 pos : POSITION`
- 출력: 클립 공간 위치
- 추가로 시간(`_Time.y`)에 따라 위쪽으로 `sin(t)` 만큼 흔들리는 효과

```hlsl
v2f vert(appdata v) {
    v2f o;
    // TODO
    return o;
}
```

`UnityObjectToClipPos`를 모를 경우 어떤 행렬을 곱해야 하는지 주석.

---

## G6. 간단한 Fragment Shader — 그라데이션

UV 좌표 `float2 uv`가 주어진다 (0~1, 0~1).
- `uv.x`에 따라 빨강 → 파랑으로 가로 그라데이션
- `uv.y < 0.5`인 영역은 절반 어둡게

```hlsl
fixed4 frag(v2f i) : SV_Target {
    // TODO
    return col;
}
```

---

## G7. Back-Face Culling (개념 + 계산)

1. 백페이스 컬링이 왜 성능에 도움이 되는가?
2. 어떤 정점 winding order(CW/CCW)가 앞면인지는 누가 결정하는가?
3. 다음 삼각형이 카메라(원점에 있고 +Z를 봄)에서 봤을 때 앞면인지 뒷면인지 판정하라:
   - V0 = (0, 1, 5)
   - V1 = (1, 0, 5)
   - V2 = (-1, 0, 5)
   - CCW가 앞면이라고 가정. 노멀 벡터를 외적으로 구해서 부호로 판정.

```csharp
public static bool IsFrontFace(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 cameraForward);
```

---

## G8. LOD (Level of Detail) 시스템 — 간단 구현

오브젝트가 카메라에서 멀어질수록 더 단순한 메시를 보여주고 싶다.

`LODGroup` 컴포넌트를 만들어라:
- `meshes[]`: 인덱스 0이 가장 디테일, 큰 인덱스가 단순
- `distanceThresholds[]`: 각 LOD의 최대 거리
- `Update()`에서 카메라와 거리 계산해서 적절한 LOD만 렌더 (나머지는 SetActive false)

```csharp
public class LODGroup : MonoBehaviour {
    public GameObject[] meshes;
    public float[] distanceThresholds;
    public Transform cameraTransform;
    void Update() { /* TODO */ }
}
```

생각해볼 점: 매 프레임 거리 계산이 비싸다면 어떻게 줄일까? (힌트: 0.5초마다, 또는 거리에 따라 갱신 주기 변경)
