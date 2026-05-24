# 회사 스타일 12문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## C1. Nexon · NHN — STL 활용 RMQ

```csharp
public static List<int> ProcessQueries(int[] enemy, List<(int op, int a, int b)> queries) {
    // SegmentTree로 max 쿼리 O(log n). 단순 배열은 q*n = 4*10^10으로 시간초과
    int n = enemy.Length;
    int[] tree = new int[4 * n];
    void Build(int node, int l, int r) {
        if (l == r) { tree[node] = enemy[l]; return; }
        int m = (l + r) / 2;
        Build(node*2, l, m); Build(node*2+1, m+1, r);
        tree[node] = Math.Max(tree[node*2], tree[node*2+1]);
    }
    void Update(int node, int l, int r, int i, int v) {
        if (l == r) { tree[node] = v; return; }
        int m = (l + r) / 2;
        if (i <= m) Update(node*2, l, m, i, v);
        else Update(node*2+1, m+1, r, i, v);
        tree[node] = Math.Max(tree[node*2], tree[node*2+1]);
    }
    int Query(int node, int l, int r, int ql, int qr) {
        if (qr < l || r < ql) return int.MinValue;
        if (ql <= l && r <= qr) return tree[node];
        int m = (l + r) / 2;
        return Math.Max(Query(node*2, l, m, ql, qr), Query(node*2+1, m+1, r, ql, qr));
    }
    Build(1, 0, n-1);
    var result = new List<int>();
    foreach (var q in queries) {
        if (q.op == 1) Update(1, 0, n-1, q.a, q.b);
        else result.Add(Query(1, 0, n-1, q.a, q.b));
    }
    return result;
}
```

**핵심:** 점 갱신 + 구간 쿼리 = 세그먼트 트리 (`O(log n)` per op).
간단한 대안은 `SortedSet` 활용, 더 단순한 RMQ는 Sparse Table (정적이라면).

---

## C2. Netmarble · Line — 손코딩 + O(n)

```csharp
// 시간복잡도: O(n), 이유: HashSet은 평균 O(1) 검색·삽입, 배열 1회 순회
public static bool HasPairWithSum(int[] arr, int target) {
    var seen = new HashSet<int>();
    foreach (int x in arr) {
        if (seen.Contains(target - x)) return true;
        seen.Add(x);
    }
    return false;
}
```

**검증 흐름:** `[3,1,4,1,5]`, target=8 → seen={3}→{3,1}→{3,1,4}→...→4 만났을 때 (8-4=4) 본인 자신만 있어서 미스. 5 만났을 때 (8-5=3) 있음 → true.

**복잡도 함정:** 정렬+양쪽 포인터는 O(n log n). HashSet이 진짜 O(n).

---

## C3. Line — 디버깅

**세 가지 문제:**
1. **(a) `i < n`** → `i <= n` 이어야 함. 현재는 1부터 N-1까지만 합산.
2. **(b) 오버플로** → N이 약 65,000 이상이면 `int` 합이 2^31 넘음. `long`을 써야 함.
3. **반환 타입** → 함수 시그니처를 `long Sum(long n)`으로 바꿔야 큰 입력 받음.

```csharp
public static long Sum(long n) {
    long result = 0;
    for (long i = 1; i <= n; i++) result += i;
    return result;
}
// 또는 닫힌 형태: return n * (n + 1) / 2;
```

**팁:** "1부터 N까지" 문제는 즉시 `n(n+1)/2` 공식이 보여야 함. 반복문 코드 짠 사람이 1차 통과 못 받는 곳도 있음.

---

## C4. Kakao — 시뮬레이션

```csharp
public static int Simulate(int[,] map, int startR, int startC, int startDir, int K) {
    int N = map.GetLength(0);
    int[] dr = { -1, 0, 1, 0 };
    int[] dc = { 0, 1, 0, -1 };
    int r = startR, c = startC, dir = startDir;
    var grid = (int[,])map.Clone(); // 0=빈칸, 1=벽, 2=함정

    for (int turn = 1; turn <= K; turn++) {
        bool moved = false;
        for (int spin = 0; spin < 4; spin++) {
            int nr = r + dr[dir];
            int nc = c + dc[dir];
            if (nr >= 0 && nr < N && nc >= 0 && nc < N &&
                grid[nr, nc] != 1 && grid[nr, nc] != 2) {
                r = nr; c = nc;
                moved = true;
                break;
            }
            dir = (dir + 1) % 4; // 시계방향 90도
        }
        if (!moved) {
            return turn; // 캐릭터 죽음
        }
        grid[r, c] = 2; // 빈 칸으로 이동했으니 함정으로 변환
    }
    return -1;
}
```

**문제 분석 팁:** "복잡한 조건의 시뮬레이션"은 조건을 작은 함수로 분리하지 말고 **순서를 그대로** 옮기는 게 디버깅 쉬움.

---

## C5. Krafton · EA — virtual / override / IDisposable

1. **호출되는 함수:** `Dragon.Attack`. **왜:** C# 객체는 객체 헤더에 메서드 테이블 포인터(MethodTable)를 가지고, 가상 메서드는 이 테이블의 슬롯을 통해 런타임에 디스패치된다. 정적 타입은 `Enemy`지만 실제 객체가 `Dragon`이므로 Dragon의 테이블 슬롯이 호출됨.

2. **`virtual` 없이 `new` 키워드:**
   ```csharp
   public class Enemy { public void Attack() { ... } }
   public class Dragon : Enemy { public new void Attack() { ... } }
   Enemy e = new Dragon();
   e.Attack(); // Enemy.Attack 호출됨! (정적 바인딩)
   ```
   `new`는 가상 디스패치가 아니라 단순히 베이스 메서드를 숨김. 변수의 컴파일 타임 타입으로 결정. **다형성 깨짐.**

3. **`sealed override`:** 더 이상 자식이 오버라이드 못 하게 막음. **JIT가 sealed임을 확신**하면 가상 디스패치 → 직접 호출(또는 인라이닝)로 변환 가능. 핫루프에서 의미 있음.

4. **C#엔 소멸자 없음 (정확히는 finalizer가 있지만 호출 시점 불확실):**
   ```csharp
   public class FileLogger : IDisposable {
       private StreamWriter sw;
       public void Dispose() { sw?.Dispose(); }
   }
   using (var log = new FileLogger()) { ... } // 블록 끝에서 자동 Dispose
   ```
   `using` 블록은 `try-finally`의 문법 설탕. 예외가 나도 Dispose 호출됨.

5. **기본:** **C#은 기본 non-virtual.** `virtual` 키워드를 명시해야 가상. Java는 정반대(기본 virtual, `final`로 막음). C++도 명시해야 virtual.
   이유: 가상 호출 비용 + 의도치 않은 오버라이드 방지.

---

## C6. Krafton — 짐벌락/쿼터니언

1. **짐벌락:** 3D 회전 중 한 축이 다른 축과 정렬되어 자유도가 3→2로 떨어지는 현상. 오일러 각 표현에서 발생.

2. **언제:** 보통 두 번째 축 회전이 ±90도일 때 (예: 오일러 Y가 90도면 X·Z축이 같은 방향이 됨).

3. **쿼터니언:** 회전을 4차원 단위 쿼터니언으로 단일 표현 → 축 분리 없음, 어떤 회전 조합도 짐벌락 없이 합성 가능. 또한 보간(Slerp)이 자연스러움.

4. **두 코드의 차이:**
- 첫 줄: Y 90° 후 Z 30°. 결과적으로 객체의 위쪽이 -X 방향으로 회전.
- 둘째 줄: Z 30° 후 Y 90°. 회전 순서가 다르므로 다른 자세.
- **쿼터니언 곱셈은 비가환:** `q1 * q2 ≠ q2 * q1`. 회전 순서 중요.

```csharp
Quaternion result = Quaternion.Slerp(qFrom, qTo, t); // t는 0~1
```

---

## C7. Smilegate · Comtus — 디자인 패턴

1. **Singleton** — 전역 1개 인스턴스, 어디서나 접근 (Service Locator도 가능하지만 Singleton이 더 직접적).
2. **State** — 상태별로 다른 동작, 상태 전이는 명확. 거대한 switch가 State로 쪼개짐.
3. **Observer** — 모델 변경을 구독자가 자동 감지. Player(주체) - HealthBar(관찰자).
4. **Object Pool** — 빈번한 생성·파괴 회피, GC 압박 제거.
5. **Command** — 입력을 객체로 캡슐화 → 매핑 교체 가능, Undo/매크로도 쉬워짐.
6. **Factory** — 타입 식별자로 다양한 객체 생성, 클라이언트는 구체 타입 모름.
7. **Strategy** — 알고리즘을 캡슐화하고 런타임에 교체. 비교 알고리즘 = 전형적 Strategy.
8. **Service Locator** (또는 DI) — 구체 의존성 대신 인터페이스, 테스트용 가짜 주입 가능.

---

## C8. Devsisters — 포물선 운동

```csharp
public static (float range, float maxHeight, float airTime)
    Projectile(float v0, float angleDeg)
{
    const float g = 9.8f;
    float rad = angleDeg * MathF.PI / 180f;
    float vx = v0 * MathF.Cos(rad);
    float vy = v0 * MathF.Sin(rad);

    float airTime = 2 * vy / g;
    float range = vx * airTime;            // = v0² sin(2θ) / g
    float maxHeight = (vy * vy) / (2 * g); // = v0² sin²(θ) / (2g)
    return (range, maxHeight, airTime);
}

public static (float x, float y) PositionAt(float v0, float angleDeg, float t) {
    const float g = 9.8f;
    float rad = angleDeg * MathF.PI / 180f;
    float x = v0 * MathF.Cos(rad) * t;
    float y = v0 * MathF.Sin(rad) * t - 0.5f * g * t * t;
    return (x, y);
}
```

**자주 묻는 추가:** 발사 높이가 0이 아니면 (절벽 위)? 최대 도달 거리 각도(45°)는 같지만 시간/거리 공식이 바뀜.

---

## C9. Pearl Abyss — 백트래킹

```csharp
public static int MinPathToClear(int[,] map, (int r, int c)[] treasures, (int r, int c) boss) {
    int K = treasures.Length;
    if (K == 0) return BfsDist(map, (0, 0), boss);

    // 모든 두 점 사이 거리 미리 계산 (0=시작, 1..K=보물, K+1=보스)
    var points = new List<(int r, int c)> { (0, 0) };
    points.AddRange(treasures);
    points.Add(boss);
    int N = points.Count;
    int[,] dist = new int[N, N];
    for (int i = 0; i < N; i++)
        for (int j = i + 1; j < N; j++) {
            dist[i, j] = BfsDist(map, points[i], points[j]);
            dist[j, i] = dist[i, j];
            if (dist[i, j] == -1) return -1;
        }

    // 보물 순열 모두 시도
    int best = int.MaxValue;
    var perm = Enumerable.Range(1, K).ToArray();
    foreach (var p in Permutations(perm)) {
        int total = dist[0, p[0]];
        for (int i = 0; i + 1 < p.Length; i++) total += dist[p[i], p[i+1]];
        total += dist[p[^1], N - 1]; // 마지막 보물 → 보스
        best = Math.Min(best, total);
    }
    return best;
}
// BfsDist와 Permutations는 표준 구현 — 생략
```

**최적화:** Bitmask DP로 `O(2^K · K)` — 외판원 문제 변형. K가 10 이상이면 순열은 못 씀.

---

## C10. Joycity — 객관식 답

1. **(c) 20 bytes + 헤더** — C# `int[]`는 참조 타입이라 힙에 할당, 데이터 20byte + 객체 헤더 + 길이 필드 = 총 약 32byte 이상 (64bit)
2. **(a) O(1) 분할상환** — 내부 배열이 가득 차면 두 배로 늘리지만 평균은 상수
3. **(b) O(n)** — 해시 충돌 최악 시. 평균은 O(1)
4. **(b) 3개** — Gen 0(짧은 수명), Gen 1, Gen 2(긴 수명)
5. **(a) 매 프레임**
6. **(b) 매 0.02초** (= 50Hz, `Time.fixedDeltaTime`)
7. **(b) (0,0,0,1)** — w=1, x=y=z=0인 단위 쿼터니언
8. `IDisposable`은 GC가 못 정리하는 자원(파일 핸들, 소켓, 네이티브 메모리)을 명시적으로 해제. `using` 블록은 블록 끝에서 `Dispose()` 자동 호출 (try-finally의 문법 설탕)
9. `class`는 힙(참조 타입), `struct`는 스택 또는 인라인(값 타입). 박싱은 값 타입을 `object`/인터페이스로 변환할 때 힙에 복제. 핫루프에서 박싱 회피해야 함
10. `SortedDictionary`는 RB-tree 기반 O(log n) **키 정렬 순서 보장**, `Dictionary`는 해시 평균 O(1) 정렬 X. 키 순서대로 순회 필요하면 SortedDictionary

**스피드 팁:** 헷갈리는 건 표시하고 넘어가기 → 마지막 5분에 돌아와서 정리.

---

## C11. Bungie · 데브시스터즈 — Inventory (C#)

```csharp
public class Inventory {
    private struct Slot { public string ItemId; public int Count; }
    private readonly Slot[] slots;
    private readonly int capacity;

    public Inventory(int capacity) {
        if (capacity <= 0) throw new ArgumentException("capacity must be positive");
        this.capacity = capacity;
        this.slots = new Slot[capacity];
    }

    public bool Add(string itemId, int count) {
        if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
        // 1) 같은 아이템 슬롯에 스택
        for (int i = 0; i < capacity; i++) {
            if (slots[i].ItemId == itemId) {
                slots[i].Count += count;
                return true;
            }
        }
        // 2) 빈 슬롯에 새로
        for (int i = 0; i < capacity; i++) {
            if (slots[i].Count == 0) {
                slots[i].ItemId = itemId;
                slots[i].Count = count;
                return true;
            }
        }
        return false; // 가득
    }

    public bool Remove(string itemId, int count) {
        if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
        if (GetCount(itemId) < count) return false;
        for (int i = 0; i < capacity && count > 0; i++) {
            if (slots[i].ItemId == itemId) {
                int take = Math.Min(slots[i].Count, count);
                slots[i].Count -= take;
                count -= take;
                if (slots[i].Count == 0) slots[i].ItemId = null;
            }
        }
        return true;
    }

    public int GetCount(string itemId) {
        int total = 0;
        for (int i = 0; i < capacity; i++)
            if (slots[i].ItemId == itemId) total += slots[i].Count;
        return total;
    }

    public bool Move(int from, int to) {
        if (from < 0 || from >= capacity || to < 0 || to >= capacity) return false;
        if (from == to) return true;
        if (slots[from].Count == 0) return false;
        if (slots[to].Count == 0) {
            slots[to] = slots[from];
            slots[from] = default;
            return true;
        }
        if (slots[to].ItemId == slots[from].ItemId) {
            slots[to].Count += slots[from].Count;
            slots[from] = default;
            return true;
        }
        // 다른 아이템이면 swap
        (slots[from], slots[to]) = (slots[to], slots[from]);
        return true;
    }
}
```

**평가 포인트 (Bungie/데브시스터즈가 보는 것):**
- 엣지 케이스: null/빈 문자열, count ≤ 0, 슬롯 인덱스 범위 외, from == to, 다른 아이템 swap
- 자료구조 선택: 슬롯 기반이라 `Slot[]` (값 타입 struct로 박싱 회피)
- 정확성: `Remove`는 부분 제거 안 함 — 총합 부족하면 즉시 false
- 가독성: 메서드 분리, 명확한 변수명, `default`로 슬롯 비우기
- 추가 개선 여지: 아이템별 총합 조회가 잦으면 `Dictionary<string,int>` 캐시 병행

---

## C12. Riot · Naughty Dog — 게임 사랑

**예시 답 (각자 자신만의 답을 준비)**:

1. **좋아하는 게임:** *Hollow Knight*. 이유 — "탐험 보상 디자인". 새 능력을 얻을 때마다 이전 지도가 새로 열림. 메트로배니아 장르의 모범.

2. **같은 회사 다른 IP:** Team Cherry는 Hollow Knight + 후속작 Silksong. (회사가 작아서 IP 하나뿐이면 그렇게 답해도 됨.)

3. **시스템 재설계 예시:** "능력 잠금 해제 시스템".
```csharp
public class AbilityGate {
    [Flags] public enum Ability { None = 0, Dash = 1, WallJump = 2, DoubleJump = 4, ... }
    public static Ability Acquired { get; private set; }
    public static bool Has(Ability a) => (Acquired & a) == a;
    public static void Unlock(Ability a) {
        Acquired |= a;
        OnUnlocked?.Invoke(a);
    }
    public static event Action<Ability> OnUnlocked;
}
```
- 비트마스크로 능력 보유 표현 (확장 쉬움)
- 이벤트로 능력 해금 시 UI/사운드/맵 갱신을 한 곳에 안 묶음
- `Has(Ability.Dash | Ability.WallJump)` 같은 복합 체크 자연스러움

**Style 메모:** 라이엇/너티독은 "당신이 게임을 사랑하는가"를 봅니다. 후보가 코드 능력 동등하면 게임 디테일을 더 잘 아는 사람을 뽑음.

---

## 회사 스타일 학습 후 체크리스트

- [ ] 카카오 시뮬레이션 큰 문제를 침착하게 조건 옮길 수 있다
- [ ] 손코딩으로 O(n) 해법을 즉시 적을 수 있다
- [ ] C# 가상 메서드 디스패치 / `IDisposable + using` 패턴을 즉답
- [ ] 짐벌락이 왜 생기고 쿼터니언이 어떻게 해결하는지 설명 가능
- [ ] 8가지 핵심 디자인 패턴(Singleton/Observer/Command/State/Factory/Pool/Service Locator/Strategy)을 상황별로 즉시 선택
- [ ] 포물선 운동 공식(range, maxHeight, t) 외워둠
- [ ] C# take-home 미니 과제(Inventory 클래스 등)를 1~2시간 안에 구조 잡고 작성
- [ ] 좋아하는 게임의 시스템을 코드로 설계해보는 연습
