# 중급 10문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## M1. 데미지 우선순위 큐

```csharp
public class DamageEvent {
    public int targetId;
    public int damage;
    public long sequence;
}
public class DamageQueue {
    // .NET 6+ PriorityQueue 사용: (damage desc, sequence asc)
    // PriorityQueue는 min-heap이라 priority를 (-damage, sequence)로 인코딩
    private readonly PriorityQueue<DamageEvent, (int, long)> pq = new();
    public void Enqueue(DamageEvent e) => pq.Enqueue(e, (-e.damage, e.sequence));
    public DamageEvent Dequeue() => pq.Dequeue();
    public int Count => pq.Count;
}
// Enqueue/Dequeue: O(log N)
```

**대안:** `SortedSet<DamageEvent>`로 IComparer 정의 (단, 동일 damage/sequence는 중복 불가).

---

## M2. 점이 삼각형 안에 있는지 (외적 부호)

```csharp
public static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c) {
    float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        => (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    float d1 = Sign(p, a, b);
    float d2 = Sign(p, b, c);
    float d3 = Sign(p, c, a);
    bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
    bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
    return !(hasNeg && hasPos); // 모두 같은 부호 (또는 0) = 안쪽 or 경계
}
```

**원리:** 점이 삼각형 내부면 세 변에 대해 외적 부호가 모두 동일. 부호가 섞이면 외부.

---

## M3. BulletPool

```csharp
public class BulletPool : MonoBehaviour {
    public GameObject bulletPrefab;
    public int initialSize = 20;
    public int maxSize = 100;
    private readonly Stack<GameObject> pool = new();
    private int totalCreated = 0;

    void Awake() {
        for (int i = 0; i < initialSize; i++) {
            var b = Instantiate(bulletPrefab, transform);
            b.SetActive(false);
            pool.Push(b);
            totalCreated++;
        }
    }
    public GameObject GetBullet() {
        if (pool.Count > 0) {
            var b = pool.Pop();
            b.SetActive(true);
            return b;
        }
        if (totalCreated < maxSize) {
            totalCreated++;
            var b = Instantiate(bulletPrefab, transform);
            return b;
        }
        return null; // 풀 한계 도달
    }
    public void ReturnBullet(GameObject bullet) {
        if (totalCreated > maxSize) { Destroy(bullet); totalCreated--; return; }
        bullet.SetActive(false);
        pool.Push(bullet);
    }
}
```

---

## M4. 보스 나선 패턴

```csharp
public static List<float> BossSpiralPattern(float spinAngle, int shots) {
    var result = new List<float>();
    for (int s = 0; s < shots; s++) {
        for (int i = 0; i < 8; i++) {
            float ang = (i * 45f + s * spinAngle) % 360f;
            if (ang < 0) ang += 360f;
            result.Add(ang);
        }
    }
    return result;
}
```

---

## M5. BFS 미로 최단 경로

```csharp
public static int ShortestPath(int[,] map) {
    int N = map.GetLength(0), M = map.GetLength(1);
    if (map[0,0] == 1 || map[N-1,M-1] == 1) return -1;
    int[] dr = { -1, 1, 0, 0 };
    int[] dc = { 0, 0, -1, 1 };
    var visited = new bool[N, M];
    var q = new Queue<(int r, int c, int dist)>();
    q.Enqueue((0, 0, 1));
    visited[0, 0] = true;
    while (q.Count > 0) {
        var (r, c, d) = q.Dequeue();
        if (r == N-1 && c == M-1) return d;
        for (int i = 0; i < 4; i++) {
            int nr = r + dr[i], nc = c + dc[i];
            if (nr < 0 || nr >= N || nc < 0 || nc >= M) continue;
            if (visited[nr,nc] || map[nr,nc] == 1) continue;
            visited[nr,nc] = true;
            q.Enqueue((nr, nc, d + 1));
        }
    }
    return -1;
}
```

---

## M6. 주사위 굴리기

```csharp
public static int DiceTopAfterRolls(int N, int M, int startR, int startC, string commands) {
    // top, bottom, north, south, west, east
    int top=1, bot=6, n=5, s=2, w=4, e=3;
    int r = startR, c = startC;
    foreach (char cmd in commands) {
        int nr = r, nc = c;
        if (cmd == 'N') nr--;
        else if (cmd == 'S') nr++;
        else if (cmd == 'W') nc--;
        else if (cmd == 'E') nc++;
        if (nr < 0 || nr >= N || nc < 0 || nc >= M) continue;
        r = nr; c = nc;
        // 굴렸을 때 면 회전
        int t;
        switch (cmd) {
            case 'N': t = top; top = s; s = bot; bot = n; n = t; break;
            case 'S': t = top; top = n; n = bot; bot = s; s = t; break;
            case 'E': t = top; top = w; w = bot; bot = e; e = t; break;
            case 'W': t = top; top = e; e = bot; bot = w; w = t; break;
        }
    }
    return top;
}
```

**팁:** 주사위 면 회전은 6변수 swap 외워두면 좋음. 4면(전후좌우 중 회전축 기준 4면)이 순환.

---

## M7. 카드 매칭

```csharp
public static int PairsRemaining(int[,] board, List<(int r, int c)> flipOrder) {
    int N = board.GetLength(0), M = board.GetLength(1);
    var copy = (int[,])board.Clone();
    for (int i = 0; i < flipOrder.Count; i += 2) {
        var (r1, c1) = flipOrder[i];
        var (r2, c2) = flipOrder[i+1];
        if (copy[r1,c1] != 0 && copy[r1,c1] == copy[r2,c2]) {
            copy[r1,c1] = 0;
            copy[r2,c2] = 0;
        }
    }
    int sum = 0;
    for (int r = 0; r < N; r++)
        for (int c = 0; c < M; c++)
            sum += copy[r,c];
    return sum;
}
```

---

## M8. Observer (이벤트)

```csharp
public class Health {
    public int Max, Current;
    public event Action<int, int> OnChanged; // (current, max)
    public Health(int max) { Max = Current = max; }
    public void TakeDamage(int amount) {
        Current = Math.Max(0, Current - amount);
        OnChanged?.Invoke(Current, Max);
    }
    public void Heal(int amount) {
        Current = Math.Min(Max, Current + amount);
        OnChanged?.Invoke(Current, Max);
    }
}
public class HealthBarUI {
    public void Subscribe(Health h) => h.OnChanged += (cur, max) => Console.WriteLine($"HP: {cur}/{max}");
}
public class LowHealthWarning {
    public void Subscribe(Health h) => h.OnChanged += (cur, max) => {
        if (cur <= max * 0.2f) Console.WriteLine("!!");
    };
}
```

**왜 event를 쓰나:** `Health`는 UI를 모름. 의존성 한 방향 (UI → Health). 테스트하기 좋음.

---

## M9. 적 FSM

```csharp
public interface IEnemyState { void Enter(Enemy e); void Update(Enemy e); void Exit(Enemy e); }

public class Enemy : MonoBehaviour {
    public Transform player;
    public IEnemyState Current;
    void Start() { ChangeState(new PatrolState()); }
    void Update() { Current?.Update(this); }
    public void ChangeState(IEnemyState next) {
        Current?.Exit(this);
        Current = next;
        Current.Enter(this);
    }
    public float DistanceToPlayer() => Vector3.Distance(transform.position, player.position);
}

public class PatrolState : IEnemyState {
    public void Enter(Enemy e) { Debug.Log("Patrol"); }
    public void Update(Enemy e) {
        if (e.DistanceToPlayer() < 5f) e.ChangeState(new ChaseState());
    }
    public void Exit(Enemy e) { }
}
public class ChaseState : IEnemyState {
    public void Enter(Enemy e) { Debug.Log("Chase"); }
    public void Update(Enemy e) {
        float d = e.DistanceToPlayer();
        if (d < 1.5f) e.ChangeState(new AttackState());
        else if (d > 8f) e.ChangeState(new PatrolState());
        // 추적 이동: e.transform.position = Vector3.MoveTowards(...)
    }
    public void Exit(Enemy e) { }
}
public class AttackState : IEnemyState {
    private float nextAttack;
    public void Enter(Enemy e) { nextAttack = Time.time; Debug.Log("Attack"); }
    public void Update(Enemy e) {
        if (e.DistanceToPlayer() > 2f) { e.ChangeState(new ChaseState()); return; }
        if (Time.time >= nextAttack) {
            Debug.Log("hit!");
            nextAttack = Time.time + 1f;
        }
    }
    public void Exit(Enemy e) { }
}
```

**왜 클래스 분리:** 상태 추가/변경이 쉬워짐. 거대한 `switch`보다 유지보수가 압도적으로 좋음.

---

## M10. Command 패턴 + Undo

```csharp
public interface ICommand { void Execute(Player p); void Undo(Player p); }
public class Player { public int Position; }

public class MoveLeftCommand : ICommand {
    public void Execute(Player p) => p.Position--;
    public void Undo(Player p) => p.Position++;
}
public class MoveRightCommand : ICommand {
    public void Execute(Player p) => p.Position++;
    public void Undo(Player p) => p.Position--;
}
public class JumpCommand : ICommand {
    public void Execute(Player p) => Console.WriteLine("Jump");
    public void Undo(Player p) => Console.WriteLine("Unjump");
}

public class InputHandler {
    public Dictionary<KeyCode, ICommand> Bindings = new();
    private readonly Stack<ICommand> history = new();
    public void HandleInput(KeyCode key, Player p) {
        if (Bindings.TryGetValue(key, out var cmd)) {
            cmd.Execute(p);
            history.Push(cmd);
        }
    }
    public void UndoLast(Player p) {
        if (history.Count > 0) history.Pop().Undo(p);
    }
}
```

**왜 좋은가:** 키 리바인딩이 `Bindings[KeyCode.A] = new JumpCommand()` 한 줄. 매크로 녹화/리플레이도 history 저장만으로 가능.
