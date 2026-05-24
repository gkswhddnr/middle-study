# 중급 10문제 (Intermediate)

게임 시뮬레이션, 디자인 패턴, 기본 알고리즘 활용. 각 문제 30~45분.

---

## M1. 우선순위 큐로 데미지 처리

전투에서 여러 적이 동시에 데미지를 받는다. 매 프레임 처리할 수 있는 데미지 이벤트가 제한적이라
**다음 두 조건의 우선순위**로 처리하려고 한다:
1. 더 큰 데미지가 먼저
2. 같은 데미지면 더 먼저 들어온 이벤트가 먼저 (FIFO)

`DamageEvent`를 enqueue / dequeue 할 수 있는 큐를 직접 구현하라.
(`SortedSet`, `PriorityQueue`, `List + Sort` 다 가능. 시간 복잡도를 주석으로 적기)

```csharp
public class DamageEvent {
    public int targetId;
    public int damage;
    public long sequence; // 들어온 순서
}
public class DamageQueue {
    public void Enqueue(DamageEvent e);
    public DamageEvent Dequeue();
    public int Count { get; }
}
```

---

## M2. 점이 삼각형 안에 있는지 (게임 수학)

화면의 한 점 `P`가 삼각형 `ABC` 내부에 있는지 판별하라.
- 외적(cross product)의 부호로 판정
- 경계 위면 true로 처리

```csharp
public static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c);
```

---

## M3. 오브젝트 풀 (총알 매니저)

`BulletPool` 컴포넌트를 만들어라.
- 시작 시 prefab을 `initialSize`만큼 미리 생성해두고 비활성화
- `GetBullet()`: 비활성 총알 하나 활성화해서 반환. 풀이 비어있으면 새로 생성
- `ReturnBullet(GameObject)`: 비활성화하고 풀로 반납
- 풀 최대 크기 `maxSize`를 넘으면 새 요청은 `Destroy`해서 반환하지 말 것

```csharp
public class BulletPool : MonoBehaviour {
    public GameObject bulletPrefab;
    public int initialSize = 20;
    public int maxSize = 100;
    public GameObject GetBullet();
    public void ReturnBullet(GameObject bullet);
}
```

---

## M4. 회전하는 보스의 공격 패턴

원형 보스가 있고, 보스를 중심으로 8방향(0°, 45°, 90°, … 315°)으로 총알이 발사된다.
보스는 매 발사 후 시계 방향으로 `spinAngle` 도만큼 회전한다.

`shots` 회 발사한 뒤, 발사된 모든 총알의 진행 방향(degree, 0~360 미만으로 정규화)을 리스트로 반환.

```csharp
public static List<float> BossSpiralPattern(float spinAngle, int shots);
// spinAngle=10, shots=2 -> [0,45,90,135,180,225,270,315, 10,55,100,145,190,235,280,325]
```

---

## M5. BFS로 미로 최단 경로

`int[,] map`이 주어진다. 0=길, 1=벽. 시작 `(0,0)`에서 도착 `(N-1, M-1)`까지 4방향으로만 이동.
도달 가능하면 최단 거리(칸 수, 시작/끝 포함), 불가능하면 -1.

```csharp
public static int ShortestPath(int[,] map);
```

---

## M6. 회전하는 큐브의 윗면 (삼성 SW 역량테스트 스타일)

주사위가 격자 위에 놓여있다. 처음 상태:
- 윗면=1, 아랫면=6, 앞=2, 뒤=5, 좌=4, 우=3 (합이 마주보는 면 = 7)

명령어 `"NESW"` 순서로 굴린다 (`N`=북, `E`=동, `S`=남, `W`=서, 격자 안으로만).
명령어 끝난 뒤 **윗면의 숫자**를 반환.

```csharp
public static int DiceTopAfterRolls(int N, int M, int startR, int startC, string commands);
```

---

## M7. 카드 매칭 게임 (Pairs)

`4x4` 격자에 1~8까지 숫자가 2장씩 깔려있다. 매 턴 두 카드를 뒤집어:
- 같으면 둘 다 제거 (격자에서 0으로 표시)
- 다르면 다시 뒤집어 닫힘

격자가 주어지고, 카드를 뒤집는 순서 리스트(`(r,c)` 페어 — 항상 짝수개)가 주어진다.
모든 턴이 끝난 뒤 격자에 남은 카드의 합을 반환.

```csharp
public static int PairsRemaining(int[,] board, List<(int r, int c)> flipOrder);
```

---

## M8. Observer 패턴 — 체력 UI

`Health` 클래스(체력 변수)와 `HealthBarUI`, `LowHealthWarning` 두 구독자를 만들어라.
- `Health.TakeDamage(int)`/`Heal(int)` 호출 시 모든 구독자에게 알림
- `HealthBarUI`는 현재 체력 출력
- `LowHealthWarning`은 체력이 20% 이하일 때만 `"!!"`출력

C# `event` 또는 인터페이스 둘 다 가능. 의존성을 한쪽으로만 (Health → UI X) 유지하라.

```csharp
public class Health { public int Max; public int Current; public void TakeDamage(int amount); }
public class HealthBarUI { public void Subscribe(Health h); }
public class LowHealthWarning { public void Subscribe(Health h); }
```

---

## M9. 간단한 FSM — 적 AI

`Patrol → Chase → Attack → Patrol` 순환하는 적 AI를 FSM으로 구현하라.
- `Patrol`: 매 Update에 거리 확인. 플레이어 거리 < 5면 `Chase`로 전이
- `Chase`: 플레이어 추적. 거리 < 1.5면 `Attack`, 거리 > 8이면 `Patrol`로 전이
- `Attack`: 1초 간격으로 공격(콘솔 출력만). 플레이어 거리 > 2면 `Chase`로 전이

상태마다 `Enter / Update / Exit` 메서드를 가지는 구조로 만들어라.

```csharp
public interface IEnemyState {
    void Enter(Enemy e);
    void Update(Enemy e);
    void Exit(Enemy e);
}
public class Enemy : MonoBehaviour {
    public Transform player;
    public IEnemyState Current;
    public void ChangeState(IEnemyState next);
}
```

---

## M10. Command 패턴 — 입력 리바인딩 + Undo

`ICommand` 인터페이스로 `Move`, `Jump`, `Attack` 커맨드를 만들어라.
- 키 → 커맨드 매핑 `Dictionary<KeyCode, ICommand>`
- 매 입력시 커맨드 실행하고 스택에 push
- `Undo()` 호출하면 마지막 커맨드를 되돌림 (예: MoveLeft → MoveRight)

`Player.Position`(int) 하나만 있다고 가정하고 Move만이라도 완벽히 구현 (Jump/Attack은 출력만).

```csharp
public interface ICommand { void Execute(Player p); void Undo(Player p); }
public class Player { public int Position; }
public class InputHandler {
    public Dictionary<KeyCode, ICommand> Bindings;
    public void HandleInput(KeyCode key, Player p);
    public void UndoLast(Player p);
}
```
