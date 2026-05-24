# 모바일 최적화 8문제 (Mobile Optimization)

Unity 모바일 게임(컴투스/넷마블/카카오게임즈/슈퍼캣 등) 면접 단골.
"이 코드의 문제점을 찾고 고쳐라" 유형이 많음. **고치는 이유**가 핵심.

각 문제 20~40분.

---

## O1. Update에서 GetComponent 호출 (찾고 고치기)

다음 코드의 문제점을 찾고 수정하라.

```csharp
public class PlayerHealth : MonoBehaviour {
    void Update() {
        var ui = GetComponent<HealthUI>();
        ui.SetValue(currentHp);
    }
    int currentHp = 100;
}
```

수정한 후, 왜 그게 더 좋은지 한 줄 주석. 추가로 `Camera.main` 사용 시 같은 함정이 있는데 그것도 언급.

---

## O2. Update에서 문자열 결합 (GC 압박)

이 점수 표시 코드는 모바일에서 프레임 드랍을 일으킨다. 두 가지를 고쳐라.

```csharp
public class ScoreText : MonoBehaviour {
    public Text text;
    public int score;
    void Update() {
        text.text = "Score: " + score + " / " + maxScore;
    }
    int maxScore = 9999;
}
```

힌트:
1. 매 프레임 문자열 결합 → 가비지 발생
2. score가 안 바뀌어도 매 프레임 갱신 → CPU 낭비

`StringBuilder`, 변경 감지, 또는 둘 다.

---

## O3. Draw Call 줄이기 — 어떤 방법을 쓸 것인가?

상황별로 적절한 배칭 방법(Static Batching / Dynamic Batching / GPU Instancing / SRP Batcher) 선택:

1. 움직이지 않는 100개의 나무, 모두 같은 머티리얼
2. 매 프레임 위치가 바뀌는 100개의 작은 총알 (300 vertex 이하)
3. 같은 메시·머티리얼인 1000개의 자갈
4. URP/HDRP를 쓰고 있고 서로 다른 머티리얼인 50개 캐릭터

각각의 답과 **이유** 한 줄.

---

## O4. 오브젝트 풀 vs Instantiate (측정 가정 + 비교)

다음 두 코드의 1000번 실행 시간을 비교한다고 가정하고, 어느 쪽이 얼마나 빠를지/왜인지 답하라.

```csharp
// A: 매번 새로
for (int i = 0; i < 1000; i++) {
    var go = Instantiate(bulletPrefab);
    Destroy(go, 1f);
}
// B: 풀에서
for (int i = 0; i < 1000; i++) {
    var go = pool.Get();
    StartCoroutine(ReturnAfter(go, 1f));
}
```

추가: GC.Collect 호출 횟수, Hierarchy 비용, Awake/OnEnable 호출 비용 관점에서 설명.

---

## O5. 텍스처 메모리 (실전 계산)

다음 텍스처가 비압축으로 메모리에 올라갈 때 차지하는 용량을 계산하라:

1. `2048 x 2048` RGBA32 (4byte/pixel)
2. 위와 같지만 **밉맵 포함** (밉맵 메모리 추가량)
3. 같은 텍스처를 ETC2 압축으로 변환했을 때 (Android, RGBA, 8bpp 기준)

추가: 모바일에서 큰 텍스처를 메모리에 안 올리고 쓸 수 있는 옵션 (Streaming, Crunch Compression).

---

## O6. Update를 줄이자 — 이벤트로 바꾸기

다음 매니저는 모든 적의 Update에서 자기 자신을 매니저에 등록한다.

```csharp
public class EnemyManager : MonoBehaviour {
    public List<Enemy> enemies = new();
    void Update() {
        foreach (var e in enemies)
            if (e.IsDead) enemies.Remove(e);
    }
}
public class Enemy : MonoBehaviour {
    public bool IsDead;
    void Update() {
        // 매 프레임 자기 상태 갱신
        if (currentHp <= 0) IsDead = true;
    }
}
```

문제 두 개:
1. 컬렉션 순회 중 수정
2. 매 프레임 폴링

**이벤트 기반**으로 리팩토링하라. Enemy가 죽을 때만 `OnDeath` 이벤트를 발생, 매니저는 구독해서 제거.

---

## O7. Find / FindObjectOfType 사용 (찾고 고치기)

다음 코드의 문제와 해결책:

```csharp
public class Bullet : MonoBehaviour {
    void Start() {
        var player = GameObject.Find("Player");
        var enemyManager = FindObjectOfType<EnemyManager>();
        // ...
    }
}
```

질문:
1. `Find`의 시간 복잡도?
2. 100개의 총알이 동시에 스폰될 때 영향?
3. 더 나은 방법 3가지 (Inject, Singleton, ScriptableObject) — 각각 언제 좋은지.

---

## O8. 메모리 누수 — 이벤트 구독 해제

다음 코드는 게임을 오래 돌리면 메모리가 계속 쌓인다. 이유와 해결.

```csharp
public class Achievement {
    public static event Action<int> OnScoreChanged;
}

public class Popup : MonoBehaviour {
    void Start() {
        Achievement.OnScoreChanged += HandleScore;
    }
    void HandleScore(int s) { /* ... */ }
}
```

질문:
1. `static event`에 인스턴스 메서드를 구독하면 무엇이 GC되지 못하는가?
2. 어디서 구독 해제해야 하는가?
3. `OnDestroy`에서 `-= HandleScore` 하면 진짜 해제되는가? (델리게이트 동등성)
