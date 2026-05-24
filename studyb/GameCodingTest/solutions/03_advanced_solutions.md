# 고급 10문제 정답

**⚠️ 문제 먼저 풀고 보세요!**

---

## A1. A* 길찾기

```csharp
public static List<(int r, int c)> FindPath(int[,] grid, (int r, int c) start, (int r, int c) goal) {
    int N = grid.GetLength(0), M = grid.GetLength(1);
    int[] dr = { -1, 1, 0, 0 };
    int[] dc = { 0, 0, -1, 1 };
    int H((int r, int c) p) => Math.Abs(p.r - goal.r) + Math.Abs(p.c - goal.c);

    var open = new PriorityQueue<(int r, int c), int>();
    var gScore = new Dictionary<(int, int), int> { [start] = 0 };
    var came = new Dictionary<(int, int), (int, int)>();
    open.Enqueue(start, H(start));

    while (open.Count > 0) {
        var cur = open.Dequeue();
        if (cur == goal) {
            var path = new List<(int r, int c)>();
            for (var p = (ValueTuple<int,int>)goal; ; ) {
                path.Add(p);
                if (p == start) break;
                p = came[p];
            }
            path.Reverse();
            return path;
        }
        for (int i = 0; i < 4; i++) {
            var nb = (cur.r + dr[i], cur.c + dc[i]);
            if (nb.Item1 < 0 || nb.Item1 >= N || nb.Item2 < 0 || nb.Item2 >= M) continue;
            if (grid[nb.Item1, nb.Item2] == 1) continue;
            int tentative = gScore[cur] + 1;
            if (!gScore.TryGetValue(nb, out int g) || tentative < g) {
                gScore[nb] = tentative;
                came[nb] = cur;
                open.Enqueue(nb, tentative + H(nb));
            }
        }
    }
    return new List<(int, int)>();
}
```

**휴리스틱 선택:** 4방향 이동이면 Manhattan이 admissible(과대평가 X)이라 안전.
8방향이면 Chebyshev나 Octile distance.

---

## A2. 마법사 상어와 파이어볼

```csharp
public static int FireballSum(int N, int K, List<(int r,int c,int m,int s,int d)> balls) {
    int[] dr = { -1, -1, 0, 1, 1, 1, 0, -1 };
    int[] dc = { 0, 1, 1, 1, 0, -1, -1, -1 };
    var cur = balls.Select(b => (b.r, b.c, b.m, b.s, b.d)).ToList();
    for (int k = 0; k < K; k++) {
        var grid = new Dictionary<(int,int), List<(int m,int s,int d)>>();
        foreach (var b in cur) {
            int nr = ((b.r + dr[b.d] * b.s) % N + N) % N;
            int nc = ((b.c + dc[b.d] * b.s) % N + N) % N;
            if (!grid.ContainsKey((nr,nc))) grid[(nr,nc)] = new();
            grid[(nr,nc)].Add((b.m, b.s, b.d));
        }
        var next = new List<(int,int,int,int,int)>();
        foreach (var kv in grid) {
            var list = kv.Value;
            if (list.Count == 1) {
                var (m,s,d) = list[0];
                next.Add((kv.Key.Item1, kv.Key.Item2, m, s, d));
                continue;
            }
            int sumM = list.Sum(x => x.m);
            int sumS = list.Sum(x => x.s);
            int cnt = list.Count;
            int newM = sumM / 5;
            int newS = sumS / cnt;
            if (newM == 0) continue;
            bool allEven = list.All(x => x.d % 2 == 0);
            bool allOdd = list.All(x => x.d % 2 == 1);
            int[] newDirs = (allEven || allOdd) ? new[] {0,2,4,6} : new[] {1,3,5,7};
            foreach (var nd in newDirs)
                next.Add((kv.Key.Item1, kv.Key.Item2, newM, newS, nd));
        }
        cur = next;
    }
    return cur.Sum(x => x.Item3);
}
```

**핵심:** 토러스 좌표는 `((x % N) + N) % N`. 음수 나머지를 양수로 보정.
모이는 단계와 분열 단계를 분리하는 게 깔끔.

---

## A3. 카드 짝맞추기

전체 풀이는 100줄 넘어서 핵심만:

```
1. 카드 페어 위치를 1~6별로 모음
2. 모든 페어 방문 순서 순열 (6! = 720) × 각 페어 내 두 카드 방문 순서 (2^6 = 64)
3. 각 순열에 대해 비용 계산:
   - 현재 위치에서 첫 카드까지 BFS 최단 (이동 + 점프 둘 다 고려)
   - 첫 카드 위치에서 두 번째까지 BFS
   - 두 카드 모두 도착하면 Enter 2번 추가
   - 페어 처리 후 그 두 카드는 보드에서 제거된 상태로 다음 페어로
4. 최솟값 반환
```

BFS 한 번 = O(16 × 8(방향*점프)) 정도. 전체 720 × 64 × BFS = 충분히 시간 내.

**팁:** Ctrl+방향 점프는 보드 끝까지 또는 다른 카드까지. BFS의 인접 노드 정의에 그 점프를 4개 더 추가하면 한 번의 BFS로 처리됨.

---

## A4. 스킬 쿨다운

```csharp
public class Skill { public int Id; public float Cooldown; public float Cast; }

public static List<(int id, float castEnd)> Simulate(
    List<Skill> skills,
    List<(float t, int id)> inputs)
{
    var bySkillId = skills.ToDictionary(s => s.Id);
    var nextAvailable = skills.ToDictionary(s => s.Id, _ => 0f);
    float castingUntil = -1f;
    var result = new List<(int, float)>();
    foreach (var (t, id) in inputs) {
        if (!bySkillId.ContainsKey(id)) continue;
        if (t < castingUntil) continue;          // 다른 스킬 시전 중
        if (t < nextAvailable[id]) continue;     // 이 스킬 쿨다운
        var sk = bySkillId[id];
        float castEnd = t + sk.Cast;
        castingUntil = castEnd;
        nextAvailable[id] = castEnd + sk.Cooldown;
        result.Add((id, castEnd));
    }
    return result;
}
```

**디자인 메모:** 시전 중 추가 입력 큐잉을 하고 싶다면 별도 처리. 위는 단순 거절 모델.

---

## A5. 데미지 계산식

```csharp
public enum Element { Fire, Water, Earth, Air }
public class Unit {
    public int Atk, Def;
    public float CritRate;
    public float CritDmg;
    public Element AtkElem, DefElem;
}

public static int CalcDamage(Unit atk, Unit def, Random rng) {
    int baseDmg = Math.Max(1, atk.Atk - def.Def);
    float mult = ElementMultiplier(atk.AtkElem, def.DefElem);
    float dmg = baseDmg * mult;
    if (rng.NextDouble() < atk.CritRate) dmg *= atk.CritDmg;
    return (int)Math.Round(dmg);
}

private static float ElementMultiplier(Element a, Element d) {
    // Fire→Air, Air→Earth, Earth→Water, Water→Fire 가 1.5
    var strong = new Dictionary<Element, Element> {
        [Element.Fire] = Element.Air,
        [Element.Air] = Element.Earth,
        [Element.Earth] = Element.Water,
        [Element.Water] = Element.Fire,
    };
    if (a == d) return 1f;
    if (strong[a] == d) return 1.5f;
    if (strong[d] == a) return 0.5f;
    return 1f;
}
```

**테스트 가능성:** `Random rng`를 주입받으니 시드 고정하면 결정론적. `if (rng == null) rng = new Random()` 안 해도 됨 — 호출자가 책임.

---

## A6. LRU 캐시

```csharp
public class Texture { public string Key; }

public class TextureCache {
    private readonly int capacity;
    private readonly Dictionary<string, LinkedListNode<(string k, Texture v)>> map = new();
    private readonly LinkedList<(string k, Texture v)> order = new(); // front=most recent

    public TextureCache(int capacity) { this.capacity = capacity; }

    public Texture Get(string key) {
        if (map.TryGetValue(key, out var node)) {
            order.Remove(node);
            order.AddFirst(node);
            return node.Value.v;
        }
        var tex = Load(key);
        if (map.Count >= capacity) {
            var last = order.Last;
            order.RemoveLast();
            map.Remove(last.Value.k);
        }
        var newNode = new LinkedListNode<(string, Texture)>((key, tex));
        order.AddFirst(newNode);
        map[key] = newNode;
        return tex;
    }
    private Texture Load(string key) => new Texture { Key = key };
}
```

**왜 LinkedList + Dictionary:** `LinkedList`만 쓰면 검색 O(N). `Dictionary`만 쓰면 순서 추적 불가. 둘을 합치면 O(1) 갱신.

---

## A7. ECS-lite

```csharp
public class World {
    private readonly Dictionary<Type, Dictionary<int, object>> store = new();

    public void AddComponent<T>(int eid, T comp) {
        if (!store.TryGetValue(typeof(T), out var bucket)) {
            bucket = new Dictionary<int, object>();
            store[typeof(T)] = bucket;
        }
        bucket[eid] = comp;
    }
    public void RemoveComponent<T>(int eid) {
        if (store.TryGetValue(typeof(T), out var bucket)) bucket.Remove(eid);
    }
    public IEnumerable<(int eid, T1 a, T2 b)> Query<T1, T2>() {
        if (!store.TryGetValue(typeof(T1), out var b1)) yield break;
        if (!store.TryGetValue(typeof(T2), out var b2)) yield break;
        // 작은 쪽을 기준으로 순회
        var smaller = b1.Count <= b2.Count ? b1 : b2;
        var other = smaller == b1 ? b2 : b1;
        foreach (var kv in smaller) {
            if (other.TryGetValue(kv.Key, out var o)) {
                if (smaller == b1) yield return (kv.Key, (T1)kv.Value, (T2)o);
                else yield return (kv.Key, (T1)o, (T2)kv.Value);
            }
        }
    }
}
```

**최적화 포인트:** 작은 버킷부터 순회하면 큰 차이. 진짜 ECS는 archetype/chunk로 cache locality까지 챙기지만 lite버전은 여기까지.

---

## A8. 원-원 충돌 응답

```csharp
public static (Vector2 vA, Vector2 vB, Vector2 pA, Vector2 pB)
    ResolveCircleCollision(Vector2 posA, Vector2 velA, float rA,
                           Vector2 posB, Vector2 velB, float rB)
{
    Vector2 delta = posB - posA;
    float dist = delta.magnitude;
    float sumR = rA + rB;
    if (dist > sumR) return (velA, velB, posA, posB); // 충돌 X

    Vector2 normal = dist > 1e-6f ? delta / dist : new Vector2(1, 0);
    // 분리: 겹친 만큼 절반씩 밀기
    float overlap = sumR - dist;
    Vector2 newPosA = posA - normal * overlap * 0.5f;
    Vector2 newPosB = posB + normal * overlap * 0.5f;

    // 탄성 충돌 (질량 동일): 노멀 방향 속도 성분만 스왑
    float vAn = Vector2.Dot(velA, normal);
    float vBn = Vector2.Dot(velB, normal);
    Vector2 newVelA = velA + (vBn - vAn) * normal;
    Vector2 newVelB = velB + (vAn - vBn) * normal;
    return (newVelA, newVelB, newPosA, newPosB);
}
```

**물리 메모:** 일반화하면 `Δv = (1 + e) * (v_rel · n) * (m_other / (mA + mB))`. `e`=반발계수, 1이면 완전탄성.

---

## A9. 가챠 (가중 + 천장)

```csharp
public class GachaItem { public string Name; public int Weight; public string Tier; }

public class Gacha {
    private readonly List<GachaItem> pool;
    private readonly int totalWeight;
    private readonly List<GachaItem> srPool;
    private readonly int srTotalWeight;
    private int sinceLastSR = 0;

    public Gacha(List<GachaItem> pool) {
        this.pool = pool;
        totalWeight = pool.Sum(p => p.Weight);
        srPool = pool.Where(p => p.Tier == "SR").ToList();
        srTotalWeight = srPool.Sum(p => p.Weight);
    }

    public List<GachaItem> Draw(int count, Random rng) {
        var results = new List<GachaItem>();
        for (int i = 0; i < count; i++) {
            GachaItem picked;
            if (sinceLastSR >= 90) {
                int roll = rng.Next(srTotalWeight);
                picked = PickWeighted(srPool, roll);
            } else {
                int roll = rng.Next(totalWeight);
                picked = PickWeighted(pool, roll);
            }
            results.Add(picked);
            if (picked.Tier == "SR") sinceLastSR = 0;
            else sinceLastSR++;
        }
        return results;
    }
    private static GachaItem PickWeighted(List<GachaItem> list, int roll) {
        int acc = 0;
        foreach (var item in list) {
            acc += item.Weight;
            if (roll < acc) return item;
        }
        return list[^1];
    }
}
```

**확률 검증:** 만 번 돌려서 각 아이템 비율이 Weight 비율과 일치하는지 테스트.

---

## A10. 매칭메이커

```csharp
public class Player { public int Id; public int Mmr; public float WaitStart; }

public class Matchmaker {
    public List<List<Player>> Tick(float currentTime, List<Player> queue) {
        var teams = new List<List<Player>>();
        // FIFO 정렬
        queue.Sort((a, b) => a.WaitStart.CompareTo(b.WaitStart));
        var matched = new HashSet<int>();
        for (int i = 0; i < queue.Count; i++) {
            if (matched.Contains(queue[i].Id)) continue;
            var anchor = queue[i];
            float anchorTol = 100f + (currentTime - anchor.WaitStart) * 50f;
            var team = new List<Player> { anchor };
            for (int j = i + 1; j < queue.Count && team.Count < 4; j++) {
                if (matched.Contains(queue[j].Id)) continue;
                var cand = queue[j];
                float candTol = 100f + (currentTime - cand.WaitStart) * 50f;
                float tol = Math.Min(anchorTol, candTol);
                // 팀 내 누구와도 tol 이내여야 함
                if (team.All(p => Math.Abs(p.Mmr - cand.Mmr) <= tol))
                    team.Add(cand);
            }
            if (team.Count == 4) {
                foreach (var p in team) matched.Add(p.Id);
                teams.Add(team);
            }
        }
        queue.RemoveAll(p => matched.Contains(p.Id));
        return teams;
    }
}
```

**디자인 트레이드오프:** 그리디 매칭은 빠르지만 최적이 아님. 더 좋은 매칭을 원하면 mmr 버킷팅 + 우선순위큐 + 주기적 재시도.
실제 라이엇/블리자드도 비슷한 휴리스틱.
