# 고급 10문제 (Advanced)

알고리즘 + 게임 시스템 종합. 각 문제 60~90분. 실제 채용 코딩테스트의 마지막 1~2문제 난이도.

---

## A1. A* 길찾기 구현

`int[,] grid` (0=길, 1=벽), 시작 `(sr, sc)`, 도착 `(er, ec)`.
A* 알고리즘으로 최단 경로를 좌표 리스트로 반환. 경로 없으면 빈 리스트.

- 휴리스틱: 맨해튼 거리
- 4방향 이동, 모든 이동 비용 1
- `PriorityQueue<TElement, TPriority>` (.NET 6+) 사용 가능

```csharp
public static List<(int r, int c)> FindPath(int[,] grid, (int r, int c) start, (int r, int c) goal);
```

검증: 직선 경로, 우회 경로, 막힌 경로 3가지 케이스로 확인.

---

## A2. 마법사 상어와 파이어볼 (삼성 SW 역량테스트 스타일)

`N x N` 격자(상하좌우가 이어진 토러스). `M`개의 파이어볼이 있고 각각 `(r, c, m, s, d)` —
위치, 질량, 속도, 방향(0=북, 1=북동, 2=동, … 7=북서).

`K`번 다음을 반복:
1. 모든 파이어볼이 자기 방향으로 `s`칸 이동 (격자가 토러스라 mod N)
2. 같은 칸에 2개 이상 있으면 모두 합쳐서:
   - 새 질량 = 합한 질량 / 5 (정수 나눗셈)
   - 새 속도 = 합한 속도 / (개수)
   - 모든 방향이 짝수거나 모두 홀수면 새 방향은 `{0,2,4,6}` 4개로 분리, 아니면 `{1,3,5,7}`
   - 새 질량이 0이면 소멸
   - 합쳐진 파이어볼은 모두 그 위치에서 4방향 모두로 1개씩 분열 (질량/속도/방향만 갱신)

`K`번 후 남은 파이어볼 질량의 총합을 반환.

```csharp
public static int FireballSum(int N, int K, List<(int r,int c,int m,int s,int d)> balls);
```

---

## A3. 카드 짝맞추기 최소 이동 (2021 카카오 블라인드 유형)

`4x4` 보드에 `1~6`의 카드가 각 2장씩, 나머지는 0. 커서가 시작 `(sr, sc)`에 있다.
- 방향키: 4방향 1칸 이동 (보드 밖 불가)
- `Ctrl+방향키`: 그 방향으로 가장 가까운 카드가 있는 칸 / 벽까지 점프
- `Enter`: 그 자리 카드 선택. 두 장 선택 시 같으면 둘 다 제거, 다르면 닫힘

모든 카드를 제거하는 데 필요한 최소 입력 횟수를 반환. (Enter도 1회)

힌트: 카드 페어 방문 순서 순열 + 각 카드 간 최단 이동(BFS)

```csharp
public static int MinInputsToClear(int[,] board, int startR, int startC);
```

---

## A4. 스킬 쿨다운 매니저

스킬 N개가 있다. 각 스킬은 `cooldown`(초), `cast`(시전시간 초)을 가진다.
플레이어가 시각 `t`초에 스킬 `id`를 누르면:
- 해당 스킬 쿨다운이 안 끝났으면 거절
- 다른 스킬을 시전 중이면 거절
- 둘 다 OK면 시전 시작, `t+cast`초에 스킬 발동 (콘솔에 `"casted id at t+cast"`)
- 발동 시각부터 `cooldown`초간 그 스킬은 다시 시전 불가

입력 이벤트 리스트가 시간 순서로 주어진다. 실제 발동된 스킬을 `(skillId, castEndTime)` 리스트로 반환.

```csharp
public class Skill { public int Id; public float Cooldown; public float Cast; }
public static List<(int id, float castEnd)> Simulate(
    List<Skill> skills,
    List<(float t, int id)> inputs);
```

---

## A5. 데미지 계산식 (속성 상성 + 크리티컬)

캐릭터:
```csharp
public class Unit {
    public int Atk;            // 공격력
    public int Def;            // 방어력
    public float CritRate;     // 0~1
    public float CritDmg;      // 1.5 = +50%
    public Element AtkElem;    // Fire/Water/Earth/Air
    public Element DefElem;
}
```

상성: Fire→Air 1.5배, Air→Earth 1.5배, Earth→Water 1.5배, Water→Fire 1.5배, 반대 방향이면 0.5배, 같으면 1배.

`CalcDamage(attacker, defender, rng)`:
- 기본 데미지 = `max(1, Atk - Def)`
- 상성 배율 적용
- `rng.NextDouble() < CritRate` 면 크리티컬, 결과에 `CritDmg` 곱
- 결과를 int로 (반올림)

```csharp
public static int CalcDamage(Unit attacker, Unit defender, Random rng);
```

`rng`를 주입받게 만들어서 결정론적 테스트가 가능하게 할 것.

---

## A6. LRU 캐시 — 텍스처 로더

크기 `capacity`의 LRU 캐시. `Get(string key)` 호출 시:
- 캐시에 있으면 반환 + 최근 사용으로 갱신
- 없으면 디스크 로드(`Load(key)` 라고 가정, 그냥 더미 객체 반환), 캐시 풀이면 가장 오래된 것 제거

`O(1)` 접근/갱신이 가능해야 함 (`Dictionary` + 이중 연결 리스트).

```csharp
public class TextureCache {
    public TextureCache(int capacity);
    public Texture Get(string key);
}
```

---

## A7. ECS-lite — 컴포넌트 쿼리

엔티티 ID(int)에 여러 컴포넌트(클래스)를 붙이고 뺄 수 있는 간단한 ECS를 만들어라.
- `AddComponent<T>(int entityId, T comp)`
- `RemoveComponent<T>(int entityId)`
- `Query<T1, T2>()` → `T1`과 `T2`를 **둘 다** 가진 모든 엔티티의 `(id, T1, T2)` 시퀀스 반환

```csharp
public class World {
    public void AddComponent<T>(int eid, T comp);
    public void RemoveComponent<T>(int eid);
    public IEnumerable<(int eid, T1 a, T2 b)> Query<T1, T2>();
}
```

힌트: `Dictionary<Type, Dictionary<int, object>>` 한 단계 인덱스만으로 충분.

---

## A8. 두 원의 충돌 + 충돌 응답 (반사)

원 A는 위치 `posA`, 반지름 `rA`, 속도 `velA`로 움직인다. 원 B도 마찬가지.
**현재 프레임에 충돌**(중심 거리 ≤ rA + rB)이면:
1. 두 원의 중심을 잇는 충돌 노멀을 구하고
2. 각 속도를 그 노멀에 대해 반사 (탄성 충돌, 질량은 둘 다 1로 가정)
3. 두 원이 겹쳐있으면 살짝 밀어내서 정확히 닿는 거리로 분리

`(newVelA, newVelB, newPosA, newPosB)`를 반환.

```csharp
public static (Vector2 vA, Vector2 vB, Vector2 pA, Vector2 pB)
    ResolveCircleCollision(Vector2 posA, Vector2 velA, float rA,
                           Vector2 posB, Vector2 velB, float rB);
```

---

## A9. 가챠 시스템 (가중 확률 + 천장)

뽑기 풀:
```csharp
public class GachaItem { public string Name; public int Weight; public string Tier; } // Tier: "SR"/"R"/"N"
```

규칙:
- 매 뽑기마다 `Weight` 비례 확률
- **천장**: SR이 90연속으로 한 번도 안 뽑히면 91번째에 SR 풀에서만 가중치 비례로 강제 SR
- 강제 SR이든 정상 SR이든 SR이 뽑힌 그 순간 카운트는 0으로 리셋

`Draw(int count, Random rng)` 호출시 결과 리스트를 순서대로 반환.

```csharp
public class Gacha {
    public Gacha(List<GachaItem> pool);
    public List<GachaItem> Draw(int count, Random rng);
}
```

---

## A10. 멀티플레이 로비 매칭

플레이어들이 매칭 요청을 보낸다. 각 플레이어: `(id, mmr, waitStart)`.
매칭 규칙:
- 4인 한 팀 매칭
- 한 방의 mmr 차이는 100 이하여야 함
- 단, 대기 시간 1초당 허용 mmr 차이가 50씩 증가 (10초 대기 = 600)
- 매칭은 FIFO 우선 (먼저 들어온 사람 우선 매칭)

`Tick(float currentTime, List<Player> queue)` 호출 시 매칭된 팀들의 리스트를 반환하고, 큐에서 제거된 플레이어는 제외하라.

```csharp
public class Player { public int Id; public int Mmr; public float WaitStart; }
public class Matchmaker {
    public List<List<Player>> Tick(float currentTime, List<Player> queue);
}
```

테스트: 비슷한 mmr 4명이 동시에 들어왔을 때 매칭되는지, mmr 차이가 큰 사람이 오래 기다리면 결국 매칭되는지.
