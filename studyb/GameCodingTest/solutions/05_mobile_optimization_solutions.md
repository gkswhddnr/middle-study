# 모바일 최적화 8문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## O1. Update에서 GetComponent

```csharp
public class PlayerHealth : MonoBehaviour {
    private HealthUI ui; // 캐싱
    int currentHp = 100;

    void Awake() {
        ui = GetComponent<HealthUI>();
    }
    void Update() {
        ui.SetValue(currentHp);
    }
}
```

**왜:** `GetComponent`는 내부적으로 컴포넌트 배열 순회 = O(컴포넌트 개수). 매 프레임 호출 시 60FPS 기준 초당 60번 검색.
Awake/Start에서 한 번만 잡아두면 O(1) 필드 접근.

**Camera.main 함정:** 내부적으로 `FindGameObjectsWithTag("MainCamera")` 호출 = O(N). Unity 2020.2+에선 캐싱돼서 좀 낫지만 직접 캐싱이 안전.

---

## O2. 문자열 결합 GC 압박

```csharp
public class ScoreText : MonoBehaviour {
    public Text text;
    public int score;
    private int lastScore = -1;
    private readonly StringBuilder sb = new(32);
    int maxScore = 9999;

    void Update() {
        if (score == lastScore) return;       // 변경 감지
        lastScore = score;
        sb.Clear();
        sb.Append("Score: ").Append(score).Append(" / ").Append(maxScore);
        text.text = sb.ToString();
    }
}
```

**두 개선:**
1. **변경 감지:** 점수 안 바뀌면 UI 갱신 자체 스킵
2. **StringBuilder:** `+` 연결은 매번 새 string 할당 → GC 압박. SB는 내부 버퍼 재사용.

추가 팁: `text.text = sb.ToString()` 도 string 할당이라 완전 zero-alloc은 아님. 정말 극단적이면 TextMeshPro `SetText(StringBuilder)` 오버로드 사용.

---

## O3. 배칭 방법 선택

1. **움직이지 않는 100개 나무, 같은 머티리얼** → **Static Batching**
   - 정점을 메모리에 합쳐서 한 번에 그림. 메모리 더 쓰지만 draw call 1번.

2. **매 프레임 위치 바뀌는 100개 작은 총알 (300 vertex 이하)** → **Dynamic Batching**
   - Unity가 CPU에서 매 프레임 합쳐줌. 300 vertex 제한이라 작은 메시만.
   - 사실 요즘은 GPU Instancing이 더 나을 때 많음.

3. **같은 메시·머티리얼 1000개 자갈** → **GPU Instancing**
   - 같은 메시면 행렬만 다르게 한 번의 draw call로 N개 그림. 가장 효율적.

4. **URP/HDRP에서 50개 캐릭터 (다른 머티리얼)** → **SRP Batcher**
   - 같은 쉐이더만 쓰면 머티리얼이 달라도 batch. SetPassCalls 줄임. Built-in엔 없음.

---

## O4. 풀 vs Instantiate

| 항목 | A (Instantiate/Destroy) | B (Pool) |
|------|------------------------|----------|
| GC 발생 | 1000번 객체 생성/파괴 → GC 다수 호출 | 거의 0 (오브젝트 재사용) |
| Awake/OnEnable | 1000번 호출 | OnEnable만 호출 |
| 메모리 사용 | 변동 (생성/파괴 반복) | 고정 |
| 1000번 소요 | 수십~수백 ms (GC 스파이크 포함) | 수 ms |

**속도 차이:** 대체로 B가 **10~50배 빠름**. 더 중요한 건 GC 스파이크가 없어 프레임 드랍이 안 생긴다는 것.

**언제 풀 안 써도 되나:** 진짜 드물게(레벨 시작/끝) 생성/파괴되는 것은 굳이 풀 필요 X. 빈번한 것만 풀.

---

## O5. 텍스처 메모리

1. **2048×2048 RGBA32:**
   - 2048 × 2048 × 4 byte = 16,777,216 byte = **16 MB**

2. **밉맵 포함:**
   - 밉맵 시리즈: 2048², 1024², 512², …, 1²
   - 추가량 = 2048²/4 + 2048²/16 + … ≈ 2048² × 1/3
   - 즉 원본의 약 33% 추가 → 총 **약 21.3 MB**
   - 공식: 밉맵 포함 시 메모리 = 원본 × 4/3

3. **ETC2 RGBA (Android, 8bpp):**
   - 2048 × 2048 × 1 byte = **4 MB** (1/4)
   - 밉맵 포함 ≈ 5.3 MB

**메모리 절약 옵션:**
- **Streaming**: 멀리 있는 텍스처는 저해상도만 메모리에 (Unity Texture Streaming)
- **Crunch Compression**: 디스크 + 메모리 둘 다 줄임 (해제는 CPU)
- **Atlas + Sprite**: 작은 텍스처들은 합쳐서 draw call까지 줄임

---

## O6. 이벤트로 변경

```csharp
public class EnemyManager : MonoBehaviour {
    public List<Enemy> enemies = new();
    public void Register(Enemy e) {
        enemies.Add(e);
        e.OnDeath += HandleDeath;
    }
    private void HandleDeath(Enemy e) {
        enemies.Remove(e);
        e.OnDeath -= HandleDeath; // 누수 방지
    }
}

public class Enemy : MonoBehaviour {
    public event Action<Enemy> OnDeath;
    private int currentHp = 100;

    public void TakeDamage(int amount) {
        currentHp -= amount;
        if (currentHp <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
    // Update 제거됨!
}
```

**개선 포인트:**
- 컬렉션 순회 중 수정 → 해결: 매니저가 이벤트 핸들러에서만 제거
- 매 프레임 폴링 → 해결: 데미지 받았을 때만 체크
- 1000마리 적이라도 매니저 Update 비용 0

---

## O7. Find / FindObjectOfType

1. **시간 복잡도:**
   - `GameObject.Find`: 모든 활성 GameObject 이름 비교 = **O(N)**
   - `FindObjectOfType`: 모든 객체 순회 후 타입 매칭 = **O(N)**
   - N = 씬의 전체 오브젝트 수. 큰 씬은 수만 개

2. **100개 총알 동시 스폰 시:** 100 × O(N) = 큰 스파이크. 특히 모바일에선 즉시 프레임 드랍.

3. **더 나은 방법:**
   - **Inject (Inspector 드래그)**: 가장 단순/명시적. 단점은 프리팹에 직접 못 연결.
   - **Singleton**: `Player.Instance`처럼. 단순하지만 테스트하기 어려움, 전역 상태 증가.
   - **ScriptableObject 채널/Reference**: 프리팹과 씬 오브젝트 간 참조 깔끔. 학습 곡선 있음.
   - 보너스: 스폰할 때 스포너가 직접 참조 주입 (`bullet.Init(player)`).

---

## O8. 메모리 누수 — 이벤트 구독 해제

```csharp
public class Popup : MonoBehaviour {
    void OnEnable() { Achievement.OnScoreChanged += HandleScore; }
    void OnDisable() { Achievement.OnScoreChanged -= HandleScore; } // 또는 OnDestroy
    void HandleScore(int s) { /* ... */ }
}
```

1. **무엇이 GC 안 되는가:**
   - `static event`가 Popup 인스턴스의 메서드를 참조 → Popup 인스턴스가 `Destroy`돼도 델리게이트가 강참조 유지
   - Unity의 "fake null"(파괴됐지만 C# 객체는 살아있음) 상태로 영원히 메모리 점유

2. **어디서 해제:** `OnDisable` 또는 `OnDestroy`. 한 번에 둘 다 하면 중복 호출 위험.

3. **`-= HandleScore` 동작:** 같은 인스턴스의 같은 메서드면 `Delegate.Equals`로 매칭되어 정확히 제거됨. 익명 람다는 매번 새 인스턴스라 `-=`로 못 뗌 — 그래서 람다 구독 시엔 변수에 담아둬야 함.

```csharp
// 나쁜 예
Achievement.OnScoreChanged += s => Debug.Log(s);   // 못 뗀다
// 좋은 예
Action<int> handler = s => Debug.Log(s);
Achievement.OnScoreChanged += handler;
Achievement.OnScoreChanged -= handler; // OK
```

---

## 모바일 최적화 학습 후 체크리스트

- [ ] GetComponent/Find는 캐싱한다는 본능이 있다
- [ ] 매 프레임 GC 알로케이션을 발생시키는 패턴(string +, foreach with boxing 등)을 안다
- [ ] 4가지 배칭(Static/Dynamic/GPU Instancing/SRP Batcher) 차이를 안다
- [ ] 텍스처 메모리 계산 (해상도 × bpp × 1.33)을 즉시 한다
- [ ] 이벤트 구독 해제를 OnDisable에서 한다
- [ ] Update 폴링을 이벤트로 바꿀 줄 안다
