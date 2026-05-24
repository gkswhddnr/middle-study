# 회사 스타일 모의 문제 12문제 (Company Style)

회사 후기 광범위 조사 후 각 회사 특유의 출제 스타일을 모방한 모의 문제집.
실제 그 회사가 어떤 식으로 묻는지 미리 손에 익히는 용도. 회사 지원 직전에 골라 풀기 좋음.

각 문제 표제 옆에 모방한 회사 표시. 같은 스타일이면 묶음.

---

## C1. [Nexon · NHN 스타일] STL 활용 + 영어 문제

You are given an array of `n` enemy HP values. Process `q` queries:
- `1 i x` — set `enemy[i] = x` (set HP)
- `2 l r` — print the maximum HP among `enemy[l..r]` inclusive

Constraints: `n, q ≤ 200,000`. Choose appropriate STL/library to keep total runtime under 1 second.

```csharp
public static List<int> ProcessQueries(int[] enemy, List<(int op, int a, int b)> queries);
```

**Style:** 넥슨/NHN은 영어 문제 + 적절한 자료구조 선택을 봅니다 (set vs map, RB-tree vs heap 등).

---

## C2. [Netmarble · Line 스타일] 손코딩 + 시간복잡도 명시

다음 함수 시그니처를 보고 **종이에 쓴다고 생각하고** 작성하라.
- IDE 자동완성 없음, 디버거 없음
- 함수 위에 한 줄 주석으로 **최악 시간복잡도와 그 근거**를 적어야 함
- 손으로 짠 뒤 머릿속으로 입력 `[3, 1, 4, 1, 5]`를 흘려보내 검증

**문제:** `int[] arr`에서 합이 `target`인 두 원소가 있는지 (인덱스 i ≠ j) `O(n)`으로 판별.

```csharp
// 시간복잡도: ___ , 이유: ___
public static bool HasPairWithSum(int[] arr, int target);
```

---

## C3. [Line 스타일] 디버그 — 이 코드 뭐가 잘못됐어?

다음 코드는 1부터 N까지의 합을 구하려는 것인데 N이 큰 경우 잘못된 값을 반환한다. **세 가지** 문제점을 찾아 모두 적고, 고친 버전을 작성하라.

```csharp
public static int Sum(int n) {
    int result = 0;
    for (int i = 1; i < n; i++) {  // (a)
        result = result + i;        // (b)
    }
    return result;                  // (c)
}
```

**Style:** Line은 종이로 디버깅 — 본인이 짠 코드의 실수를 발견할 수 있어야 함.

---

## C4. [Kakao 스타일] 게임 시뮬레이션 (큰 문제)

`N x N` 격자에 캐릭터가 있다. 매 턴:
1. 캐릭터는 현재 방향으로 한 칸 전진. 격자 밖이거나 막힌 칸이면 **시계 방향으로 90도 회전**만 하고 같은 턴 안에 다시 전진 시도.
2. 4번 회전해도 전진 못 하면 **현재 자리가 함정**으로 변하고 캐릭터는 죽음.
3. 캐릭터가 빈 칸으로 이동하면 그 칸은 다음 턴에 이동 가능한 함정이 됨.

`map`, 시작 위치 `(r, c)`, 시작 방향(0=북, 1=동, 2=남, 3=서), 최대 턴 수 `K`가 주어진다.
캐릭터가 죽었을 때까지의 턴 수를 반환. K 턴 내 안 죽으면 -1.

```csharp
public static int Simulate(int[,] map, int startR, int startC, int startDir, int K);
```

**Style:** 카카오는 문제 설명이 길고 조건이 복잡한 시뮬레이션이 단골. 침착하게 조건을 옮기는 게 핵심.

---

## C5. [Krafton · EA 스타일] C# 깊이 — virtual/override + IDisposable

다음 C# 코드를 보고:

```csharp
public class Enemy {
    public virtual void Attack() => Console.WriteLine("Enemy attack");
}
public class Dragon : Enemy {
    public override void Attack() => Console.WriteLine("Dragon breath");
}
public class Boss : Dragon {
    public sealed override void Attack() => Console.WriteLine("Boss roar");
}

// 사용:
Enemy e = new Dragon();
e.Attack();
Enemy b = new Boss();
b.Attack();
```

답하라:
1. `e.Attack()`은 어느 함수를 호출하나? **왜** 그게 가능한지 (가상 메서드 디스패치 / 메서드 테이블 단어 사용).
2. `virtual` 없이 그냥 `public void Attack()`이고 자식이 `new` 키워드로 가렸다면 어떻게 다를까? (정적 바인딩 vs 동적 바인딩)
3. `sealed override`는 무엇을 막는가? 왜 그게 성능에 도움이 될 수 있나? (인라이닝 관점)
4. C#엔 C++의 소멸자가 없는데, "사용 끝났을 때 자원 정리" 하려면 어떤 패턴을 쓰나? (`IDisposable` + `using`)
5. **C# 가상 메서드는 기본 virtual인가 non-virtual인가?** Java와 비교해서.

**Style:** 크래프톤·EA는 C++ 위주지만, 동일한 개념을 C#으로 묻는 회사도 많음 (NHN/Netmarble/Devsisters 등). C#의 가상 디스패치 메커니즘은 C++ vtable과 거의 동일하다.

---

## C6. [Krafton 스타일] 짐벌락/쿼터니언 (개념 + 코드)

답하라:
1. 짐벌락(Gimbal Lock)이 무엇인가? 어느 회전 표현에서 생기나?
2. 어떤 상황에서 발생하나? (Pitch가 ___ 일 때)
3. 쿼터니언은 왜 짐벌락이 안 생기나? (자유도 / 축 분리 관점)
4. 다음 Unity 코드의 결과를 예측: 두 코드의 차이는?

```csharp
transform.rotation = Quaternion.Euler(0, 90, 0) * Quaternion.Euler(0, 0, 30);
transform.rotation = Quaternion.Euler(0, 0, 30) * Quaternion.Euler(0, 90, 0);
```

추가 코드: 두 쿼터니언 사이 **구면 보간(Slerp)** 한 줄 호출.

**Style:** 크래프톤은 게임 수학을 깊이 묻는 회사로 유명.

---

## C7. [Smilegate · Comtus 스타일] 객관식 — 디자인 패턴

다음 상황 각각에 가장 적절한 패턴을 고르고 한 줄 근거.

선택지: Singleton, Observer, Command, State, Factory, Object Pool, Service Locator, Strategy

1. 게임 전역에서 접근해야 하는 SoundManager → ___
2. 캐릭터가 `Idle / Walk / Run / Attack` 중 한 상태에 있고 각 상태마다 다른 Update 로직 → ___
3. UI 체력바가 Player의 체력 변경에 자동 반응 → ___
4. 매 프레임 100발 발사되는 총알을 GC 부담 없이 관리 → ___
5. 키 입력에 따라 점프/공격이 매핑되고, 게임 옵션에서 키 바인딩 변경 가능 → ___
6. 적 타입(Goblin / Orc / Dragon)을 데이터에서 읽어 동적으로 생성 → ___
7. 같은 조건 비교 알고리즘(가장 가까운 적, 가장 약한 적, 가장 강한 적)을 런타임에 교체 → ___
8. AudioManager를 직접 참조하지 않고 인터페이스로 받아서 테스트 가능하게 → ___

**Style:** 스마일게이트·컴투스는 객관식 + 디자인 패턴 + 그래픽스 개념 위주.

---

## C8. [Devsisters 스타일] 물리/수학 게임 문제

쿠키 캐릭터가 점프 발사대에 올라타 포물선으로 날아간다.
초기 속도 `v0` (m/s), 발사 각도 `angleDeg` (도, 수평선 기준), 중력 `g = 9.8 m/s²` (아래쪽으로).

1. **수평 도달 거리**를 반환 (착륙 = 발사 높이로 돌아온 시점)
2. **최고 도달 높이**를 반환
3. **착륙까지 걸리는 시간**을 반환

```csharp
public static (float range, float maxHeight, float airTime)
    Projectile(float v0, float angleDeg);
```

추가: `t`초 후 위치 `(x, y)`를 반환하는 함수도 작성.

**Style:** 데브시스터즈 코딩 면접에서 "물리/수학 기반 게임 문제"가 자주 나옴. 정확성과 효율성 둘 다 평가.

---

## C9. [Pearl Abyss 스타일] 백트래킹 시뮬레이션

`N x N` 던전에 보스 1마리, 보물 K개가 있다. 시작 `(0,0)`에서 출발해서:
- 4방향 이동 가능, 벽 통과 X (`map[r,c] == 1` 이면 벽)
- 보물 모두 먹은 뒤 보스에 도달해야 클리어
- 가능한 모든 경로 중 **이동 횟수 최소**값을 반환. 불가능하면 -1

```csharp
public static int MinPathToClear(int[,] map, (int r, int c)[] treasures, (int r, int c) boss);
```

힌트:
- 보물 K개의 방문 순서가 자유 → K!개의 순열 (K ≤ 6 정도 가정)
- 두 점 사이 최단 경로는 BFS
- 메모이제이션이나 비트마스킹 DP로 더 빠르게도 가능 (도전과제)

**Style:** 펄어비스는 백트래킹/시뮬레이션/우선순위큐 위주의 정통 알고리즘.

---

## C10. [Joycity 스타일] 빠른 객관식 — 20문제 40분 컨셉

답을 종이에 빠르게 적되, 헷갈리는 건 표시하고 넘어가는 연습. 각 문제 1~2분.

1. `int[] a = new int[5];` 의 메모리 크기는? (a) 5 bytes (b) 20 bytes (c) 20 bytes + 헤더
2. `List<int>.Add`의 평균 시간복잡도? (a) O(1) 분할상환 (b) O(n) (c) O(log n)
3. `Dictionary<K,V>.TryGetValue`의 최악 시간복잡도? (a) O(1) (b) O(n) (c) O(log n)
4. C# 가비지 컬렉터의 세대 수? (a) 2개 (b) 3개 (c) 4개
5. Unity `Update`는 언제 호출? (a) 매 프레임 (b) 매 0.02초 (c) 매 1초
6. Unity `FixedUpdate`의 기본 호출 주기? (a) 매 프레임 (b) 매 0.02초 (c) 매 0.1초
7. `Quaternion.identity`는? (a) (0,0,0,0) (b) (0,0,0,1) (c) (1,1,1,1)
8. C# `IDisposable` 패턴을 쓰는 이유는? `using` 키워드와의 관계 한 줄.
9. C# `class vs struct` — 어디 할당? 박싱은 언제 일어나나?
10. `SortedDictionary<K,V>` vs `Dictionary<K,V>` 차이 한 줄?

**Style:** 조이시티는 짧은 시간에 객관식 20문제. 깊이보다 폭과 속도.

---

## C11. [Bungie · 데브시스터즈 스타일] take-home C# 미니 과제

다음을 구현 (제한시간 1~2시간 가정):

`Inventory` 클래스 — 게임 인벤토리 시스템
- 최대 칸 수 `capacity`
- `Add(string itemId, int count)`: 같은 아이템은 스택. 다른 아이템은 빈 칸에. 칸이 부족하면 false.
- `Remove(string itemId, int count)`: 부족하면 false (부분 제거 X)
- `GetCount(string itemId)`: 보유 개수
- `Move(int fromSlot, int toSlot)`: 슬롯 간 이동/합치기

C#로 작성. 가독성, 예외 처리(`ArgumentException` 등), 엣지 케이스 처리, XML 주석 모두 평가됨.

```csharp
public class Inventory {
    public Inventory(int capacity);
    public bool Add(string itemId, int count);
    public bool Remove(string itemId, int count);
    public int GetCount(string itemId);
    public bool Move(int fromSlot, int toSlot);
}
```

**Style:** Bungie는 take-home C# 3~4시간 (production code 수준). 데브시스터즈도 비슷한 패턴 (자료구조 + 정확성 + 예외처리).

---

## C12. [Riot · Naughty Dog 스타일] 게임 사랑 + 코드

답하라:
1. 본인이 가장 좋아하는 게임은? **왜?** (게임 디자인 관점에서 한 가지 요소)
2. 그 게임을 만든 회사의 핵심 IP를 하나 더 말해보라.
3. 그 게임의 어떤 시스템(예: 인벤토리, 전투, 매칭)을 본인이 다시 만든다면 어떻게 만들 것인가? 의사코드로 핵심 자료구조 한 가지를 설계.

**Style:** 라이엇/너티독 인터뷰는 코드만큼 게임 자체에 대한 애정·이해도를 봅니다. 답을 미리 준비해두면 큰 차이.
