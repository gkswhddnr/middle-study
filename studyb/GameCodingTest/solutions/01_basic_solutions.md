# 기초 10문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## B1. 셀프 넘버

```csharp
public static int SumOfSelfNumbersBelow(int limit) {
    bool[] hasGenerator = new bool[limit];
    for (int n = 1; n < limit; n++) {
        int d = n;
        int t = n;
        while (t > 0) { d += t % 10; t /= 10; }
        if (d < limit) hasGenerator[d] = true;
    }
    int sum = 0;
    for (int i = 1; i < limit; i++) if (!hasGenerator[i]) sum += i;
    return sum;
}
```

**핵심:** 5000개 각각 제네레이터를 역추적하지 말고, 모든 n에 대해 d(n) 계산해서 마킹.
시간복잡도 O(N · log N), 셀프 넘버 정의를 그대로 따라가는 것보다 훨씬 빠름.

---

## B2. 가장 긴 단어 찾기

```csharp
public static string LongestWord(string s) {
    string[] words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    string best = "";
    foreach (var w in words) if (w.Length > best.Length) best = w;
    return best;
}
```

LINQ로 한 줄도 가능 (`words.OrderByDescending(w => w.Length).First()`)이지만 O(N log N).
위는 O(N).

---

## B3. Fisher-Yates 셔플

```csharp
public static List<int> ShuffleDeck(int n, int seed) {
    var deck = Enumerable.Range(1, n).ToList();
    var rng = new System.Random(seed);
    for (int i = deck.Count - 1; i > 0; i--) {
        int j = rng.Next(i + 1);
        (deck[i], deck[j]) = (deck[j], deck[i]);
    }
    return deck;
}
```

**왜 `OrderBy(Random)`가 안 좋은가:** 비교 기반 정렬이라 비교마다 새 랜덤이 호출되면 불안정한 비교가 되고
편향이 생긴다. Fisher-Yates는 균등 분포가 수학적으로 보장됨.

---

## B4. 인벤토리 카운트

```csharp
public static Dictionary<string,int> CountItems(List<string> inventory) {
    var dict = new Dictionary<string,int>();
    foreach (var item in inventory) {
        if (!dict.ContainsKey(item)) dict[item] = 0;
        dict[item]++;
    }
    return dict;
}
```

LINQ: `inventory.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count())`도 OK.

---

## B5. Update vs FixedUpdate

```csharp
public class PlayerController : MonoBehaviour {
    // 1. 키 입력은 Update — 프레임당 1번이지만 입력 누락 막으려면 Update
    // 4. 시각 효과(회전 애니메이션)는 Update — 매 프레임 부드러워야 함
    // 5. 거리 기반 데미지 체크도 Update면 충분
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { /* 점프 트리거 */ }
        coin.transform.Rotate(0, 90 * Time.deltaTime, 0);
        CheckEnemyHit();
    }
    // 2. Rigidbody 힘은 FixedUpdate — 물리 엔진과 동기화
    void FixedUpdate() {
        rb.AddForce(Vector3.forward * 10f);
    }
    // 3. 카메라 따라가기는 LateUpdate — 플레이어 이동이 끝난 뒤 카메라가 따라가야 떨림 없음
    void LateUpdate() {
        cam.position = transform.position + offset;
    }
}
```

---

## B6. Bomb 코루틴

```csharp
public class Bomb : MonoBehaviour {
    public float fuseSeconds = 3f;
    IEnumerator Start() {
        yield return new WaitForSeconds(fuseSeconds - 0.5f);
        for (int i = 0; i < 5; i++) {
            Debug.Log("tick");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("BOOM");
        Destroy(gameObject);
    }
}
```

**팁:** `MonoBehaviour.Start`는 반환형을 `IEnumerator`로 하면 자동으로 코루틴 시작됨. `StartCoroutine` 호출 안 해도 됨.

---

## B7. Singleton

```csharp
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string clipName) {
        Debug.Log($"SFX: {clipName}");
    }
}
```

**자주 묻는 함정:** `OnDestroy`에서 `Instance = null`로 안 해주면 씬 전환 시 좀비 참조 가능. 위는 DontDestroy라 큰 문제 없지만 일반적으로 해주는 게 안전.

---

## B8. 거리 / 범위

```csharp
public static float Distance(float px, float py, float ex, float ey) {
    float dx = px - ex, dy = py - ey;
    return Mathf.Sqrt(dx*dx + dy*dy);
}
public static bool InRange(float px, float py, float ex, float ey, float range) {
    float dx = px - ex, dy = py - ey;
    // Sqrt 호출 안 함: 양변을 제곱해서 비교 — sqrt는 비싼 연산
    return dx*dx + dy*dy <= range*range;
}
```

Unity의 `Vector3.sqrMagnitude`를 쓰는 이유와 동일.

---

## B9. 적이 플레이어 바라보기

```csharp
public static float AngleToFacePlayer(Vector2 enemyPos, Vector2 playerPos) {
    Vector2 dir = playerPos - enemyPos;
    // +Y가 forward라면, Atan2(x, y)로 해야 북쪽=0
    float rad = Mathf.Atan2(dir.x, dir.y);
    float deg = rad * Mathf.Rad2Deg;
    if (deg < 0) deg += 360f;
    return deg;
}
```

**주의:** `Atan2(y, x)`는 +X=0도 기준이고, `Atan2(x, y)`로 바꾸면 +Y=0도. 게임에서 "북쪽=정면" 기준이 흔함.

---

## B10. 격자 이동 시뮬레이션

```csharp
public static (int r, int c, int dir) Move(int N, int startR, int startC, int startDir, string commands) {
    int[] dr = { -1, 0, 1, 0 };
    int[] dc = { 0, 1, 0, -1 };
    int r = startR, c = startC, dir = startDir;
    foreach (char cmd in commands) {
        if (cmd == 'L') dir = (dir + 3) % 4;
        else if (cmd == 'R') dir = (dir + 1) % 4;
        else if (cmd == 'F') {
            int nr = r + dr[dir], nc = c + dc[dir];
            if (nr >= 0 && nr < N && nc >= 0 && nc < N) { r = nr; c = nc; }
        }
    }
    return (r, c, dir);
}
```

**관용구:** 4방향 배열 `dr/dc`는 무조건 외워둘 것. 게임/시뮬레이션 문제 80%에 등장.
좌회전은 `(dir + 3) % 4` (= `(dir - 1 + 4) % 4`와 동치, 음수 mod 안전).

---

## B11. 박싱/언박싱

```csharp
public static long SumWithObjectList(int n) {
    // List<object>에 int를 넣으면 매번 박싱 → 힙 할당 1000만 번 발생
    var list = new List<object>(n);
    for (int i = 0; i < n; i++) list.Add(i);
    long sum = 0;
    foreach (var o in list) sum += (int)o; // 언박싱
    return sum;
}
public static long SumWithIntList(int n) {
    // List<int>는 내부 배열이 int[], 박싱 없음
    var list = new List<int>(n);
    for (int i = 0; i < n; i++) list.Add(i);
    long sum = 0;
    foreach (var v in list) sum += v;
    return sum;
}
```

**결과:** `List<int>`가 5~10배 빠르고 메모리도 훨씬 적게 씀.
**왜:** `object`는 참조 타입이라 박싱 시 힙에 새 객체 생성 + GC 부담. 핫루프에 `object`/`ArrayList` 쓰지 말 것.

---

## B12. ref/out으로 다중 반환

```csharp
public static int MaxWithOut(int[] scores, out int index) {
    index = 0; int max = scores[0];
    for (int i = 1; i < scores.Length; i++)
        if (scores[i] > max) { max = scores[i]; index = i; }
    return max;
}
public static (int value, int index) MaxWithTuple(int[] scores) {
    int max = scores[0], idx = 0;
    for (int i = 1; i < scores.Length; i++)
        if (scores[i] > max) { max = scores[i]; idx = i; }
    return (max, idx);
}
public class MaxResult { public int Value; public int Index; }
public static MaxResult MaxWithClass(int[] scores) {
    var r = new MaxResult { Value = scores[0], Index = 0 };
    for (int i = 1; i < scores.Length; i++)
        if (scores[i] > r.Value) { r.Value = scores[i]; r.Index = i; }
    return r;
}
```

**언제 어떤 거?**
- `out`: 기존 API와 호환 필요, `Try*` 패턴(`int.TryParse`).
- 튜플: 최근 C# 스타일, 2~3개 값 반환에 가장 깔끔.
- 클래스: 필드 4개 이상이거나 미래에 확장될 가능성이 있을 때.

---

## B13. Time.deltaTime

```csharp
public class Player : MonoBehaviour {
    public float speed = 5f;
    void Update() {
        // 잘못된 버전: 프레임당 speed만큼 이동 → 144FPS는 30FPS의 5배 빠름
        transform.position += Vector3.right * speed;

        // 올바른 버전: 1초에 speed만큼 이동 (FPS 무관)
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
// 잘못된 버전: 60FPS에서 1초에 5*60=300 이동, 30FPS에서 5*30=150 이동
// 올바른 버전: 어떤 FPS에서도 1초에 5 이동
```

**관용구:** 프레임당 한 번 호출되는 곳에서 "속도 × 시간 = 거리"가 필요할 때마다 `* Time.deltaTime`.
물리(`FixedUpdate`)는 `Time.fixedDeltaTime`을 쓰지만 보통 그냥 `Time.deltaTime`도 같은 값 반환.

---

## B14. Lerp 체력바

```csharp
public class HealthBar : MonoBehaviour {
    public float targetHP;
    public float currentDisplay;
    public float smoothSpeed = 5f;
    void Update() {
        currentDisplay = Mathf.Lerp(currentDisplay, targetHP, smoothSpeed * Time.deltaTime);
    }
}
```

**왜 `smoothSpeed * Time.deltaTime`인가:**
- `Lerp(a, b, t)`는 a→b의 t 비율(0~1).
- 매 프레임 `Lerp(cur, tgt, 0.1)` 같이 고정값을 쓰면 60FPS에선 30FPS보다 부드러움이 두 배. 프레임 의존적.
- `smoothSpeed * Time.deltaTime`로 곱하면 단위시간당 진행률이 일정 → 프레임 독립적.
- 엄밀히 말하면 이건 지수 감쇠 근사라 완벽한 프레임 독립은 아니지만, 게임에선 충분.

**완전 프레임독립 원하면:** `1 - Mathf.Exp(-smoothSpeed * Time.deltaTime)`.

---

## B15. Instantiate

```csharp
public class Spawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform player;
    public float radius = 5f;

    public void SpawnEnemies(int n) {
        for (int i = 0; i < n; i++) {
            Vector2 offset2D = Random.insideUnitCircle * radius;
            Vector3 spawnPos = player.position + new Vector3(offset2D.x, 0, offset2D.y);
            // 적이 플레이어를 바라봄
            Quaternion lookRot = Quaternion.LookRotation(player.position - spawnPos);
            Instantiate(enemyPrefab, spawnPos, lookRot);
        }
    }
}
```

**팁:** `Quaternion.LookRotation`은 `Vector3.zero`를 주면 경고를 뱉음. 플레이어와 스폰위치가 일치할 일은 거의 없지만 방어적으로 체크하면 더 견고.

---

## B16. SerializeField vs public

```csharp
public class Health : MonoBehaviour {
    [SerializeField] private int maxHealth = 100;    // 인스펙터 O, 외부는 프로퍼티로
    public int MaxHealth => maxHealth;                // 읽기만

    public int CurrentHealth { get; private set; }   // 인스펙터 X, 외부 읽기 O

    [SerializeField] private AudioSource audioSource;// 인스펙터 O, 외부 X

    private const string playerTag = "Player";       // 상수, 외부 노출 불필요

    public event Action OnDeath;                      // 다른 스크립트가 구독
}
```

**왜 `public` 필드 대신 `[SerializeField] private`인가:** public 필드는 캡슐화를 깬다. 외부에서 마음대로 수정 가능. SerializeField는 "인스펙터엔 보여달라"는 의도만 표현.

---

## B17. enum + switch 데미지

```csharp
public enum WeaponType { Sword, Bow, Wand, Hammer }
public enum EnemyType { Normal, Armored, Flying }

public static int Damage(WeaponType w, EnemyType e, int baseDmg) {
    float mult = (w, e) switch {
        (_, EnemyType.Normal) => 1f,
        (WeaponType.Sword, EnemyType.Armored) => 0.5f,
        (WeaponType.Bow, EnemyType.Armored) => 0.7f,
        (WeaponType.Wand, EnemyType.Armored) => 1.0f,
        (WeaponType.Hammer, EnemyType.Armored) => 1.5f,
        (WeaponType.Sword, EnemyType.Flying) => 0.7f,
        (WeaponType.Bow, EnemyType.Flying) => 1.5f,
        (WeaponType.Wand, EnemyType.Flying) => 1.0f,
        (WeaponType.Hammer, EnemyType.Flying) => 0.3f,
        _ => 1f
    };
    return (int)(baseDmg * mult);
}
```

**스타일 메모:** 데미지표가 자주 바뀌면 `float[,] table = { ... }` 2차원 배열이 더 편함. 무기/적 종류 4개 이하면 switch가 가독성 좋고, 늘어나면 데이터 테이블로 빼는 게 정답.

---

## B18. Stack 괄호 짝

```csharp
public static bool IsBalanced(string s) {
    var stack = new Stack<char>();
    foreach (char c in s) {
        if (c == '(') stack.Push(c);
        else if (c == ')') {
            if (stack.Count == 0) return false;
            stack.Pop();
        }
    }
    return stack.Count == 0;
}

// 확장: 3종 괄호
public static bool IsBalancedMulti(string s) {
    var stack = new Stack<char>();
    var match = new Dictionary<char,char> { [')']='(', [']']='[', ['}']='{' };
    foreach (char c in s) {
        if (c == '(' || c == '[' || c == '{') stack.Push(c);
        else if (match.ContainsKey(c)) {
            if (stack.Count == 0 || stack.Pop() != match[c]) return false;
        }
    }
    return stack.Count == 0;
}
```

**왜 Stack:** "마지막에 연 괄호가 가장 먼저 닫혀야 함" = LIFO. 게임 콘솔 명령어 파서, 수식 평가, JSON 파서 등 어디서나 등장.

---

## B19. Queue 알림 매니저

```csharp
public class NotificationManager {
    private readonly Queue<string> queue = new();
    public int Pending => queue.Count;
    public void Notify(string msg) => queue.Enqueue(msg);
    public void Tick() {
        if (queue.Count > 0)
            Console.WriteLine(queue.Dequeue());
    }
}
```

**확장 아이디어:** `Notify(string msg, int priority)`로 우선순위 알림 받기 → 그땐 `PriorityQueue`로 바꾸면 됨.

---

## B20. 벡터 사이 각도

```csharp
public static float AngleBetween(Vector2 a, Vector2 b) {
    float dot = a.x * b.x + a.y * b.y;
    float magA = Mathf.Sqrt(a.x*a.x + a.y*a.y);
    float magB = Mathf.Sqrt(b.x*b.x + b.y*b.y);
    if (magA < 1e-6f || magB < 1e-6f) return 0f; // 영벡터 방어
    float cosT = dot / (magA * magB);
    cosT = Mathf.Clamp(cosT, -1f, 1f);            // 부동소수점 오차 방어
    return Mathf.Acos(cosT) * Mathf.Rad2Deg;
}
```

**왜 Clamp가 필요:** `cosT`가 `1.0000001` 같이 살짝 범위 밖이면 `Acos`이 `NaN` 반환. 영벡터 같은 코너 케이스도 항상 챙길 것.

---

## 기초 20문제 학습 후 체크리스트

- [ ] 박싱이 왜 비싼지 한 문장으로 설명할 수 있다
- [ ] `Update`/`FixedUpdate`/`LateUpdate` 차이를 즉시 구분한다
- [ ] `Time.deltaTime`을 어디 곱해야 할지 반사적으로 안다
- [ ] Singleton, Observer, Command 패턴을 코드로 즉시 쓴다
- [ ] 4방향 격자 탐색 `dr/dc` 관용구가 손에 익었다
- [ ] `out` / 튜플 / 클래스 반환 차이를 안다
- [ ] `SerializeField` vs `public` 선택 기준이 명확하다
- [ ] Stack/Queue를 언제 쓸지 즉답 가능

---

# 워밍업 정답 (B21~B40)

---

## B21. FizzBuzz 게임

```csharp
public static List<string> FizzBuzzGame(int n) {
    var result = new List<string>();
    for (int i = 1; i <= n; i++) {
        if (i % 15 == 0) result.Add("콤보");
        else if (i % 3 == 0) result.Add("공격");
        else if (i % 5 == 0) result.Add("방어");
        else result.Add(i.ToString());
    }
    return result;
}
```

**팁:** 15(=3×5)를 먼저 검사하는 게 핵심. 3·5 따로 검사하면 둘 다 만족할 때 잘못된 분기 탐.

---

## B22. 짝수의 합

```csharp
public static int SumOfEvens(int n) {
    int sum = 0;
    for (int i = 2; i <= n; i += 2) sum += i;
    return sum;
}
// 수학적으로 한 줄: n=2k → 2+4+…+2k = k(k+1)
```

---

## B23. 문자열 뒤집기

```csharp
public static string Reverse(string s) {
    var arr = s.ToCharArray();
    Array.Reverse(arr);
    return new string(arr);
}
// 정말 직접 만들고 싶다면:
// for (int i=0, j=arr.Length-1; i<j; i++, j--) (arr[i], arr[j]) = (arr[j], arr[i]);
```

---

## B24. 회문 검사

```csharp
public static bool IsPalindrome(string s) {
    int l = 0, r = s.Length - 1;
    while (l < r) {
        if (s[l] == ' ') { l++; continue; }
        if (s[r] == ' ') { r--; continue; }
        if (char.ToLower(s[l]) != char.ToLower(s[r])) return false;
        l++; r--;
    }
    return true;
}
```

**관용구:** 양쪽 포인터(`l`, `r`)로 안쪽으로 좁혀가는 패턴. 회문 / 정렬 배열에서 합 찾기 / 컨테이너 면적 등에 단골.

---

## B25. GCD (유클리드 호제법)

```csharp
public static int Gcd(int a, int b) {
    while (b != 0) {
        int t = b;
        b = a % b;
        a = t;
    }
    return a;
}
// 재귀: return b == 0 ? a : Gcd(b, a % b);
```

**왜:** a, b의 GCD는 (b, a%b)의 GCD와 같다. 매번 절반 이하로 줄어들어 O(log(min(a,b))).

---

## B26. 팩토리얼

```csharp
public static long FactorialRecursive(int n)
    => n <= 1 ? 1 : n * FactorialRecursive(n - 1);

public static long FactorialIterative(int n) {
    long r = 1;
    for (int i = 2; i <= n; i++) r *= i;
    return r;
}
```

**팁:** 게임 코드에서는 반복문이 안전 (재귀는 깊으면 StackOverflow). `n=20`이면 long도 넘침 — 큰 수는 BigInteger.

---

## B27. Min/Max 한 번에

```csharp
public static (int min, int max) MinMax(int[] arr) {
    int mn = arr[0], mx = arr[0];
    for (int i = 1; i < arr.Length; i++) {
        if (arr[i] < mn) mn = arr[i];
        else if (arr[i] > mx) mx = arr[i];
    }
    return (mn, mx);
}
```

**팁:** `else if`로 비교 횟수 절반 절약 (한 원소가 min이면 max일 수 없음). 단, 첫 원소가 둘 다라 `else if` 트릭이 정확하려면 시작값을 `arr[0]`으로.

---

## B28. 평균

```csharp
public static double Average(int[] scores) {
    if (scores.Length == 0) return 0;
    long sum = 0;
    foreach (var s in scores) sum += s;
    return (double)sum / scores.Length;
}
```

**팁:** `int` 합산은 오버플로 위험 → `long` 으로. 마지막 나눗셈 전에 `double` 캐스팅 안 하면 정수 나눗셈 됨.

---

## B29. 중복 제거 (순서 유지)

```csharp
public static List<int> RemoveDuplicates(int[] arr) {
    var seen = new HashSet<int>();
    var result = new List<int>();
    foreach (var x in arr) {
        if (seen.Add(x)) result.Add(x); // Add는 새로 들어갔으면 true
    }
    return result;
}
```

**팁:** `HashSet.Add` 반환값이 "실제로 추가됐는지"라서 `Contains + Add` 두 번 호출 안 해도 됨. O(N).

---

## B30. 배열 왼쪽 회전

```csharp
public static int[] RotateLeft(int[] arr, int k) {
    int n = arr.Length;
    if (n == 0) return arr;
    k %= n; // 길이보다 큰 k 처리
    var result = new int[n];
    for (int i = 0; i < n; i++) result[i] = arr[(i + k) % n];
    return result;
}
```

**메모리 절약 버전:** "3번 reverse" 트릭 — 앞 k개 뒤집기, 나머지 뒤집기, 전체 뒤집기. in-place 가능.

---

## B31. GridMover

```csharp
public class GridMover : MonoBehaviour {
    void Update() {
        Vector3 d = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) d = Vector3.forward;
        else if (Input.GetKeyDown(KeyCode.S)) d = Vector3.back;
        else if (Input.GetKeyDown(KeyCode.A)) d = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) d = Vector3.right;
        transform.position += d;
    }
}
```

**왜 `GetKeyDown`:** 누르고 있어도 1번만 트리거. `GetKey`는 매 프레임. 격자 이동엔 `GetKeyDown`이 맞음.

---

## B32. 중점

```csharp
public static Vector3 Midpoint(Vector3 a, Vector3 b) => (a + b) * 0.5f;
```

**팁:** `/ 2`보다 `* 0.5f`가 살짝 빠름 (CPU의 곱셈이 나눗셈보다 빠름). 게임 핫루프에선 의미 있음.

---

## B33. 가장 가까운 적

```csharp
public static int NearestEnemy(Vector3 player, Vector3[] enemies) {
    if (enemies == null || enemies.Length == 0) return -1;
    int best = 0;
    float bestSqr = (enemies[0] - player).sqrMagnitude;
    for (int i = 1; i < enemies.Length; i++) {
        float d = (enemies[i] - player).sqrMagnitude;
        if (d < bestSqr) { best = i; bestSqr = d; }
    }
    return best;
}
```

**핵심:** `sqrMagnitude` 사용 (Sqrt 없이). 단순 비교 목적이면 거리 제곱으로 충분 (B8과 같은 원리).

---

## B34. 체력 안전 처리

```csharp
public static int ApplyDelta(int currentHp, int delta, int maxHp)
    => Mathf.Clamp(currentHp + delta, 0, maxHp);
```

**팁:** `Math.Clamp`(.NET 표준)도 있음. Unity 안이면 `Mathf.Clamp`가 일관적.

---

## B35. 경험치 → 레벨

```csharp
public static int LevelFromExp(int totalExp) {
    int level = 1;
    int need = 100; // 1→2 필요 경험치
    while (totalExp >= need) {
        totalExp -= need;
        level++;
        need = level * 100; // 다음 레벨업에 필요한 양
    }
    return level;
}
```

**팁:** 빠른 닫힌 형태도 있지만 (이차방정식) 게임에선 그냥 반복문이 가독성 좋음. 레벨 100 정도까진 충분히 빠름.

---

## B36. 쿨다운 체크

```csharp
public static bool CanUseSkill(float lastUsed, float cd, float now)
    => now - lastUsed >= cd;
```

**팁:** "마지막 사용 시각 + 쿨다운" 식이 직관적. `now >= lastUsed + cd`로 써도 OK.

---

## B37. 맨해튼 거리

```csharp
public static int Manhattan(int x1, int y1, int x2, int y2)
    => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
```

**관용구:** 4방향 격자에서 두 점 최단거리(장애물 무시). A*의 휴리스틱 기본값. 8방향이면 Chebyshev: `Max(|dx|, |dy|)`.

---

## B38. 정수 거듭제곱

```csharp
public static long IntPow(int baseValue, int exp) {
    long result = 1;
    for (int i = 0; i < exp; i++) result *= baseValue;
    return result;
}
// O(log exp) 빠른 거듭제곱:
public static long FastPow(long b, int e) {
    long r = 1;
    while (e > 0) {
        if ((e & 1) == 1) r *= b;
        b *= b;
        e >>= 1;
    }
    return r;
}
```

**왜 O(log):** `2^10 = (2^5)^2 = ((2^2)^2 * 2)^2`. 지수를 이진으로 보고 매번 절반.

---

## B39. swap

```csharp
public static (int, int) SwapTuple(int a, int b) => (b, a);

public static (int, int) SwapTemp(int a, int b) {
    int t = a; a = b; b = t;
    return (a, b);
}
```

**팁:** C# 7+ 튜플 분해는 swap을 한 줄로. 함수 안에서도 `(a, b) = (b, a)` 식으로 쓰면 임시 변수 없음.

---

## B40. 소수 판별

```csharp
public static bool IsPrime(int n) {
    if (n < 2) return false;
    if (n < 4) return true;          // 2, 3
    if (n % 2 == 0) return false;
    for (int i = 3; (long)i * i <= n; i += 2) {
        if (n % i == 0) return false;
    }
    return true;
}
```

**왜 `i*i <= n`:** `n`의 약수 쌍은 `(a, b)`로 `a*b = n`. 둘 중 작은 쪽은 `√n` 이하. 그러니 `√n`까지만 검사하면 충분. `i*i`는 sqrt 호출 없이 같은 효과 — 더 빠르고 정확.

**팁:** `(long)i * i` 캐스팅 — `int` 곱셈 오버플로 방지.

---

# 확장 워밍업 정답 (B41~B60)

---

## B41. Lerp vs Slerp

1. **카메라 위치 추격** → `Vector3.Lerp(현재, 목표, t * Time.deltaTime)` — 위치는 직선 보간이 자연스러움
2. **적이 플레이어 방향 회전 (방향 벡터)** → `Vector3.Slerp(현재방향, 목표방향, t)` — 방향 벡터는 구면 보간이 자연스러움 (각속도 일정)
3. **체력바 게이지** → `Mathf.Lerp(현재값, 목표값, t * Time.deltaTime)` — float 값은 Mathf.Lerp

```csharp
cam.position = Vector3.Lerp(cam.position, player.position, 5f * Time.deltaTime);
forward = Vector3.Slerp(forward, dirToPlayer, 5f * Time.deltaTime);
hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, hpRatio, 5f * Time.deltaTime);
```

**핵심 차이:** Lerp는 직선, Slerp는 구의 호. 회전·방향에는 Slerp, 위치·값에는 Lerp.

---

## B42. LookAt 없이 회전

```csharp
public static void FaceTarget(Transform self, Vector3 target) {
    Vector3 dir = target - self.position;
    if (dir.sqrMagnitude < 1e-6f) return;        // 같은 위치면 무시
    self.rotation = Quaternion.LookRotation(dir);
}
```

**보너스 — 부드러운 회전:**
```csharp
Quaternion targetRot = Quaternion.LookRotation(dir);
self.rotation = Quaternion.Slerp(self.rotation, targetRot, 5f * Time.deltaTime);
```

---

## B43. Random.Range 함정

- `Random.Range(int, int)` — **max 미포함** (`[1, 10)` = 1~9)
- `Random.Range(float, float)` — **max 포함** (`[1, 10]` = 1~10)

**왜 다를까:** int는 배열 인덱스 용도가 많아서 exclusive max가 더 자연스러움. float는 범위 자체가 의미여서 inclusive.

```csharp
public static List<int> RollIntDice(int min, int max, int count) {
    var result = new List<int>(count);
    for (int i = 0; i < count; i++)
        result.Add(Random.Range(min, max + 1)); // max 포함하고 싶으면 +1
    return result;
}
```

**함정:** "1~6 주사위" 만들고 싶으면 `Random.Range(1, 7)`이지 `Random.Range(1, 6)` 아님.

---

## B44. PlayerPrefs 최고점

```csharp
public static class HighScore {
    private const string Key = "highScore";

    public static void SaveHighScore(int score) {
        PlayerPrefs.SetInt(Key, score);
        PlayerPrefs.Save(); // 디스크에 즉시 강제 저장
    }
    public static int LoadHighScore() {
        return PlayerPrefs.GetInt(Key, 0); // 기본값 0
    }
    public static void ResetHighScore() {
        PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
    }
}
```

**팁:** `PlayerPrefs.Save()` 안 부르면 앱이 강제 종료될 때 데이터 손실 가능. 중요한 값은 저장 직후 호출.
**한계:** PlayerPrefs는 평문 저장이라 조작 쉬움. 보안 중요한 값(인앱 결제, 진행도)은 서버에. 클라 게임이면 적어도 암호화.

---

## B45. Pause/Resume

```csharp
public class PauseManager {
    private bool isPaused;
    public void Pause() { Time.timeScale = 0f; isPaused = true; }
    public void Resume() { Time.timeScale = 1f; isPaused = false; }
}
```

**UI 애니메이션이 안 멈추게:**
- Animator 컴포넌트의 `Update Mode = Unscaled Time`으로 설정
- 코드에서 `animator.updateMode = AnimatorUpdateMode.UnscaledTime;`
- 코루틴은 `WaitForSecondsRealtime` 사용 (vs `WaitForSeconds`는 timeScale 영향 받음)
- 직접 시간 다룰 때는 `Time.unscaledDeltaTime`

**왜:** `Time.timeScale = 0`이면 `deltaTime`도 0이라 일반 Update에서 시간 진행 안 함. UI는 별도 시계를 써야 함.

---

## B46. ScreenToWorldPoint (2D)

```csharp
public class ClickToSpawn : MonoBehaviour {
    public Camera cam;
    public GameObject explosionPrefab;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z; // 2D: 카메라까지 거리
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            Instantiate(explosionPrefab, worldPos, Quaternion.identity);
        }
    }
}
```

**왜 z 설정이 필요한가:** `Input.mousePosition.z = 0`이라 카메라 정확히 같은 평면이 됨. 카메라에서 떨어진 거리만큼 z를 설정해야 정확한 월드 좌표.

**3D 게임이면:** 보통 `Camera.ScreenPointToRay` + `Raycast`로 처리 (B47 참조).

---

## B47. Raycast 적 클릭

```csharp
public class ClickDetector : MonoBehaviour {
    public Camera cam;
    void Update() {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) {
            if (hit.collider.CompareTag("Enemy"))
                Debug.Log($"Hit: {hit.collider.name}");
        }
    }
}
```

**팁:** `CompareTag("Enemy")`가 `tag == "Enemy"`보다 빠름 (문자열 비교 회피).
**2D:** `Physics2D.Raycast` 또는 `OverlapPoint`로.

---

## B48. AudioSource

```csharp
public class SoundController : MonoBehaviour {
    private AudioSource source;
    void Awake() { source = GetComponent<AudioSource>(); }
    public void PlaySFX(AudioClip clip, float volume) {
        if (clip == null) return;
        source.PlayOneShot(clip, volume);
    }
    public void MuteAll(bool mute) { source.mute = mute; }
}
```

**팁:** `PlayOneShot`은 현재 재생 중인 것과 별도로 중첩 재생. `Play()`는 기존 것을 끊고 재생. 효과음은 PlayOneShot.

---

## B49. 데미지 계산

```csharp
public static int CalcDamage(int atk, int def, bool isCritical) {
    int dmg = Math.Max(1, atk - def);
    if (isCritical) dmg *= 2;
    return dmg;
}

public static int CalcDamageWithPen(int atk, int def, float pen, bool isCritical) {
    int effectiveDef = (int)(def * (1f - Mathf.Clamp01(pen)));
    int dmg = Math.Max(1, atk - effectiveDef);
    if (isCritical) dmg *= 2;
    return dmg;
}
```

**팁:** `Mathf.Clamp01`로 pen이 0~1 범위 벗어나도 안전. 음수 관통이나 100% 초과 관통을 의도적으로 허용하려면 빼면 됨.

---

## B50. 카운트다운 포맷

```csharp
public static string FormatCountdown(int seconds) {
    if (seconds < 0) return "00:00";
    int m = seconds / 60;
    int s = seconds % 60;
    return $"{m:D2}:{s:D2}"; // D2 = 2자리 0 패딩
}
```

**팁:** `$"{m:D2}"` = `m.ToString("D2")`. 음수 입력 방어가 자주 빠지는 함정.

---

## B51. 점수 콤마 포맷

```csharp
public static string FormatScore(int score) {
    string s = score.ToString();
    if (s.Length <= 3) return s;
    var sb = new StringBuilder();
    bool neg = s.StartsWith("-");
    if (neg) { sb.Append('-'); s = s.Substring(1); }
    int firstGroup = s.Length % 3;
    if (firstGroup > 0) sb.Append(s, 0, firstGroup);
    for (int i = firstGroup; i < s.Length; i += 3) {
        if (sb.Length > (neg ? 1 : 0)) sb.Append(',');
        sb.Append(s, i, 3);
    }
    return sb.ToString();
}

public static string FormatScoreBuiltin(int score) => score.ToString("N0");
```

**실무:** 거의 항상 `ToString("N0")` 사용. 자체 구현은 면접에서 "포맷 직접 짤 수 있나" 보려는 용도.

---

## B52. 가중 확률 드롭

```csharp
public static string RollDrop(Dictionary<string, int> dropTable) {
    int total = 0;
    foreach (var kv in dropTable) total += kv.Value;
    int roll = Random.Range(0, total);
    int acc = 0;
    foreach (var kv in dropTable) {
        acc += kv.Value;
        if (roll < acc) return kv.Key;
    }
    return null; // 도달 불가 (테이블이 비었을 때만)
}
```

**복잡도:** O(N). N이 작으면(드롭 테이블 보통 10개 이하) 충분.
**최적화:** N이 크면 누적 가중치 배열 + 이진 탐색 O(log N).

---

## B53. 최단 회전

```csharp
public static float ShortestRotation(float from, float to) {
    float diff = ((to - from + 540f) % 360f) - 180f;
    return diff;
}
```

**왜 `+540 % 360 - 180`:** `(to - from)`이 -360~+360 범위일 수 있음. `+540`을 더하면 양수가 되고 `% 360`으로 0~360에 들어옴. `-180`으로 -180~180 범위로 옮김.

**활용:** 회전 보간 시 "10도에서 350도로 가야 한다"면 -340도가 아니라 +20도가 맞음.

---

## B54. 각도 정규화

```csharp
public static float Normalize360(float angle) {
    angle = angle % 360f;
    if (angle < 0) angle += 360f;
    return angle;
}
```

**팁:** C# `%`는 부호가 피제수 따라감 (-10 % 360 = -10). 양수로 보정하려면 한 번 더 더하기.

---

## B55. 라디안 ↔ 도

```csharp
public static float DegToRad(float deg) => deg * (MathF.PI / 180f);
public static float RadToDeg(float rad) => rad * (180f / MathF.PI);
```

**관용구 (Unity):** `Mathf.Deg2Rad ≈ 0.01745`, `Mathf.Rad2Deg ≈ 57.2958`. 둘 다 미리 계산된 상수라 곱하기만 함.

---

## B56. AABB 충돌

```csharp
public static bool AABBOverlap(
    float ax, float ay, float aw, float ah,
    float bx, float by, float bw, float bh)
{
    return ax < bx + bw && bx < ax + aw &&
           ay < by + bh && by < ay + ah;
}
```

**원리:** "안 겹치는 조건"의 부정. A가 B 완전 왼쪽에 있거나, 완전 오른쪽, 완전 위, 완전 아래 중 하나라도 만족하면 안 겹침. 그 외는 겹침.

**경계 포함하려면:** `<` 대신 `<=` 사용 (문제 정의에 따라).

---

## B57. 원 안 점

```csharp
public static bool PointInCircle(float px, float py, float cx, float cy, float r) {
    float dx = px - cx, dy = py - cy;
    return dx*dx + dy*dy <= r*r; // 양변 제곱
}
```

**관용구:** Sqrt는 비싸니까 비교는 제곱으로. B8과 같은 원리.

---

## B58. 발사 간격 제한

```csharp
public class Gun {
    public float fireInterval = 0.2f;
    private float lastFired = float.NegativeInfinity; // 첫 발은 항상 가능
    public bool TryFire(float now) {
        if (now - lastFired < fireInterval) return false;
        lastFired = now;
        return true;
    }
}
```

**관용구:** `NegativeInfinity` 초기값으로 "처음엔 무조건 가능" 표현. `0`으로 하면 게임 시작 직후 0초에 발사 시도 시 위험.

---

## B59. 이중 점프

```csharp
public class JumpController {
    public int maxJumps = 2;
    private int jumpCount = 0;
    public bool TryJump() {
        if (jumpCount >= maxJumps) return false;
        jumpCount++;
        return true;
    }
    public void Land() { jumpCount = 0; }
}
```

**확장 아이디어:** `OnDeath`/`OnAirJumpUsed` 같은 이벤트로 효과 트리거. 코요테 점프(절벽에서 떨어진 후 짧은 시간 점프 가능)도 자주 묻는 주제.

---

## B60. 콤보 점수

```csharp
public static int ComboScore(int comboCount, int baseScore) {
    float mult;
    if (comboCount >= 50) mult = 5f;
    else if (comboCount >= 20) mult = 3f;
    else if (comboCount >= 10) mult = 2f;
    else if (comboCount >= 5) mult = 1.5f;
    else mult = 1f;
    return (int)(baseScore * mult);
}
```

**더 깔끔하게 — 테이블 기반:**
```csharp
private static readonly (int threshold, float mult)[] tiers = {
    (50, 5f), (20, 3f), (10, 2f), (5, 1.5f), (0, 1f)
};
public static int ComboScore(int comboCount, int baseScore) {
    foreach (var (t, m) in tiers)
        if (comboCount >= t) return (int)(baseScore * m);
    return baseScore;
}
```

**왜 좋은가:** 콤보 구간 추가/조정이 한 줄. 디자이너에게 ScriptableObject로 노출하기도 쉬움.
