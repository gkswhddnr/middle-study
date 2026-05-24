# 기초 10문제 (Basic)

C# 문법, Unity 기본기 위주. 각 문제 15~20분 안에 풀 수 있어야 함.
입력/출력은 그냥 함수 시그니처에 맞게 구현하면 됨 (콘솔 입출력 X).

---

## B1. 셀프 넘버 (넥슨 입사문제 유형)

자연수 n에 대해 `d(n) = n + (n의 각 자릿수의 합)`으로 정의한다.
예: `d(91) = 91 + 9 + 1 = 101`

n을 `d(n)`의 **제네레이터**라고 부른다. 제네레이터가 없는 수를 **셀프 넘버**라 한다.
(예: 1, 3, 5, 7, 9, 20, 31, 42, …)

**과제:** 1 이상 5000 미만의 모든 셀프 넘버의 합을 반환하는 함수를 작성하라.

```csharp
public static int SumOfSelfNumbersBelow(int limit);
```

---

## B2. 가장 긴 단어 찾기

문자열 `s`가 공백으로 구분된 여러 단어로 이루어져 있다.
가장 긴 단어를 반환하라. 길이가 같은 단어가 여러 개면 가장 먼저 나온 것을 반환.

```csharp
public static string LongestWord(string s);
// "I love unity engine very much" -> "engine"
```

---

## B3. 카드 덱 셔플

`1`부터 `n`까지의 카드를 담은 리스트가 있다.
**Fisher-Yates 셔플**을 직접 구현해서 무작위로 섞어라.
(Unity의 `Random.Range`나 `System.Random` 사용 가능. `LINQ OrderBy(Random)` 금지)

```csharp
public static List<int> ShuffleDeck(int n, int seed);
```

---

## B4. 인벤토리에서 아이템 개수 세기

플레이어 인벤토리가 `List<string>` (각 원소가 아이템 이름)으로 주어진다.
같은 아이템이 여러 번 등장할 수 있다.
각 아이템이 몇 개 있는지 `Dictionary<string,int>`로 반환하라.

```csharp
public static Dictionary<string,int> CountItems(List<string> inventory);
// ["sword","potion","sword","arrow","potion","potion"] -> {"sword":2,"potion":3,"arrow":1}
```

---

## B5. Update vs FixedUpdate 적합한 곳 고르기 (개념 + 코드)

다음 5개 동작 각각을 `Update()` 와 `FixedUpdate()` 중 어디에 넣어야 할지 고르고,
그 이유를 한 줄 주석으로 적은 뒤 실제 함수에 호출 코드를 작성하라.

1. 키보드 입력으로 점프 트리거
2. Rigidbody에 힘 가해서 이동
3. 카메라가 플레이어 따라가기 (LateUpdate가 더 적절한 게 있다면 그것도 표시)
4. 코인 회전 애니메이션 (`transform.Rotate`)
5. 적과의 충돌 검사 후 데미지 처리 (콜라이더 OnCollisionEnter는 제외)

```csharp
public class PlayerController : MonoBehaviour {
    void Update() { /* 여기에 들어갈 코드 */ }
    void FixedUpdate() { /* 여기에 들어갈 코드 */ }
    void LateUpdate() { /* 필요하면 */ }
}
```

---

## B6. Coroutine으로 N초 후 폭발

`Bomb` 컴포넌트를 만들어라.
- `public float fuseSeconds` 만큼 기다린 후 콘솔에 `"BOOM"`을 출력
- 폭발 직전 0.5초 동안 매 0.1초마다 `"tick"`을 출력
- 게임 오브젝트는 폭발 후 `Destroy(gameObject)`

```csharp
public class Bomb : MonoBehaviour {
    public float fuseSeconds = 3f;
    IEnumerator Start() { /* TODO */ }
}
```

---

## B7. Singleton 매니저 만들기

씬에 하나만 존재해야 하는 `AudioManager` 싱글톤을 작성하라.
- 어디서든 `AudioManager.Instance.PlaySFX(string clipName)` 호출 가능해야 함
- 씬이 바뀌어도 파괴되지 않아야 함 (DontDestroyOnLoad)
- 두 번째 인스턴스가 생기면 자기 자신을 파괴해야 함

```csharp
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }
    public void PlaySFX(string clipName) { /* 출력만 해도 됨 */ }
}
```

---

## B8. 두 점 사이 거리 / 정사각형 안인지

플레이어 위치 `(px, py)`와 적 위치 `(ex, ey)`가 주어진다.
1. 두 점 사이 거리를 반환 (제곱근 사용 OK)
2. 거리가 `range` 이내면 true, 아니면 false (단, `Mathf.Sqrt` 호출 없이 비교만 — 왜 더 빠른지 주석)

```csharp
public static float Distance(float px, float py, float ex, float ey);
public static bool InRange(float px, float py, float ex, float ey, float range);
```

---

## B9. 적이 플레이어를 바라보게 회전

플레이어 위치 `playerPos` (Vector2)와 적 위치 `enemyPos`가 주어진다.
적이 플레이어를 향해 바라봐야 할 각도(도, degree)를 반환하라.
(2D 탑다운 기준, 적의 forward는 `+Y` 방향)

힌트: `Mathf.Atan2`

```csharp
public static float AngleToFacePlayer(Vector2 enemyPos, Vector2 playerPos);
```

---

## B10. 격자 위에서 명령어대로 이동 (이코테 게임개발 유형)

`N x N` 격자에서 캐릭터가 시작 위치 `(r, c)`와 방향(0=북, 1=동, 2=남, 3=서)을 가진다.
명령어 문자열이 주어진다:
- `'L'` : 왼쪽으로 90도 회전 (이동 안 함)
- `'R'` : 오른쪽으로 90도 회전 (이동 안 함)
- `'F'` : 바라보는 방향으로 한 칸 전진 (격자 밖으로는 못 나감, 무시)

모든 명령어 수행 후 최종 위치 `(r, c)`와 방향을 반환.

```csharp
public static (int r, int c, int dir) Move(int N, int startR, int startC, int startDir, string commands);
// N=5, start=(2,2,0), commands="FFRFF" -> (0,4,1)
```

---

## B11. 박싱/언박싱 비용 (C# 면접 단골)

다음 두 함수가 정수 1000만개의 합을 구한다. 어느 쪽이 빠를지, **왜** 그런지 한 줄 주석으로 적고
두 버전 모두 동작하도록 구현하라.

```csharp
public static long SumWithObjectList(int n);   // List<object>에 i를 넣어서 합산
public static long SumWithIntList(int n);      // List<int>에 i를 넣어서 합산
```

힌트: 박싱은 힙 할당이 발생. 가능하면 `Stopwatch`로 직접 측정해보면 좋음.

---

## B12. ref/out으로 다중 반환

`int[] scores` 에서 **최댓값**과 **그 인덱스**를 동시에 반환하라. 한 번의 순회로.

세 가지 방법을 모두 작성해보고, 어느 게 어떤 상황에 좋은지 주석:
1. `out` 매개변수
2. 튜플 반환
3. 클래스 반환 (`MaxResult`)

```csharp
public static int MaxWithOut(int[] scores, out int index);
public static (int value, int index) MaxWithTuple(int[] scores);
public class MaxResult { public int Value; public int Index; }
public static MaxResult MaxWithClass(int[] scores);
```

---

## B13. Time.deltaTime — 프레임 독립적 이동

`Player.Update()` 안에서 매 프레임 오른쪽으로 `speed` 단위/초 만큼 이동시키고 싶다.
잘못된 코드와 올바른 코드를 모두 적고, 잘못된 쪽이 어떤 상황에서 문제가 되는지 설명.

```csharp
public class Player : MonoBehaviour {
    public float speed = 5f;
    void Update() {
        // 잘못된 버전:

        // 올바른 버전:
    }
}
```

추가: 60FPS와 30FPS에서 1초 후 위치가 어떻게 달라지는지 두 줄 주석으로 표기.

---

## B14. Lerp로 부드러운 체력바

체력바 UI가 있다. `currentDisplay`가 실제 체력 `targetHP`를 향해 매 프레임 부드럽게 따라가야 한다.
프레임 독립적으로(=프레임레이트 달라도 비슷한 속도로) 작동해야 함.

```csharp
public class HealthBar : MonoBehaviour {
    public float targetHP;        // 실제 체력
    public float currentDisplay;  // 표시 중인 값
    public float smoothSpeed = 5f;
    void Update() {
        // currentDisplay를 targetHP로 부드럽게 보간
    }
}
```

힌트: `Mathf.Lerp(a, b, t * Time.deltaTime)` — 왜 `t * Time.deltaTime`이 정답에 가까운지 주석으로.

---

## B15. Instantiate로 적 생성

플레이어 위치 기준 반경 5m의 무작위 위치에 적 프리팹 N마리를 스폰하는 메서드를 작성하라.
적은 플레이어를 바라봐야 함 (회전).

```csharp
public class Spawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform player;
    public void SpawnEnemies(int n) {
        // TODO
    }
}
```

힌트: `Random.insideUnitCircle * radius` 또는 직접 각도 무작위.

---

## B16. SerializeField vs public

다음 5개 필드 중 **인스펙터에 보여야 하는 것**과 **다른 스크립트에서 접근 가능해야 하는 것**을 구분하라.
각 필드에 알맞은 접근 한정자/어트리뷰트를 붙여서 작성.

1. `maxHealth` (인스펙터에서 디자이너가 조정, 다른 스크립트에서 읽기만 가능해야 함)
2. `currentHealth` (런타임에 변동, 다른 스크립트에서 접근 가능해야 하지만 인스펙터엔 굳이 X)
3. `audioSource` (인스펙터에서 드래그로 연결, 외부 접근 불필요)
4. `playerTag` (상수, 코드에서만 씀)
5. `OnDeath` 이벤트 (다른 스크립트에서 구독 가능해야 함)

```csharp
public class Health : MonoBehaviour {
    // 5개 필드 — 적절한 한정자/속성으로
}
```

---

## B17. enum + switch 무기 데미지

`enum WeaponType { Sword, Bow, Wand, Hammer }` 가 있다.
무기 타입에 따라 기본 데미지가 다르고, 적의 타입(`EnemyType { Normal, Armored, Flying }`)에 따라 배율이 다르다.

배율표:
```
        Sword  Bow   Wand  Hammer
Normal   1.0   1.0   1.0   1.0
Armored  0.5   0.7   1.0   1.5
Flying   0.7   1.5   1.0   0.3
```

`Damage(WeaponType, EnemyType, int baseDmg)` 를 구현. `switch` 표현식(C# 8+) 권장.

```csharp
public static int Damage(WeaponType w, EnemyType e, int baseDmg);
```

---

## B18. Stack으로 괄호 짝 검사 (콘솔 명령어 파서)

게임 콘솔 명령어 `"give (sword 3) (potion (red 2))"` 같은 식이 들어온다.
괄호 `(`, `)` 쌍이 올바르게 닫혀있는지 검사하는 함수를 작성.

```csharp
public static bool IsBalanced(string s);
// "(())" true, "(()" false, "())(" false
```

추가: `[`, `]`, `{`, `}` 도 같이 받는 확장판도 작성.

---

## B19. Queue로 알림 메시지 순서

게임에서 알림(예: "퀘스트 완료!", "아이템 획득!")이 동시에 여러 개 발생해도
**먼저 들어온 순서대로** 한 번에 하나씩 표시되어야 한다.

`Notify(string msg)`로 메시지를 큐에 넣고,
`Tick()`이 호출될 때마다 큐의 맨 앞 메시지 하나만 표시(콘솔 출력)하고 큐에서 제거.

```csharp
public class NotificationManager {
    public void Notify(string msg);
    public void Tick(); // 한 번에 한 개씩 출력
    public int Pending { get; }
}
```

---

## B20. 두 벡터 사이의 각도 (Vector2.Angle 없이)

두 2D 벡터 `a`, `b`의 사이각(0~180도)을 직접 계산하라. `Vector2.Angle` 호출 금지.

```csharp
public static float AngleBetween(Vector2 a, Vector2 b);
```

힌트: `cos θ = (a · b) / (|a| |b|)`. 부동소수점 오차로 acos 인자가 `[-1, 1]`을 벗어날 수 있다 — clamping 필요.

---

# 워밍업 문제 (B21~B40)

5~10분 안에 풀 수 있는 진짜 기초. 손 푸는 용도 / 매일 워밍업으로 쓰기 좋음.

---

## B21. FizzBuzz 게임 버전

1부터 N까지 출력하되:
- 3의 배수: `"공격"`
- 5의 배수: `"방어"`
- 둘 다: `"콤보"`
- 그 외: 숫자 그대로

`List<string>`으로 반환.

```csharp
public static List<string> FizzBuzzGame(int n);
```

---

## B22. 짝수의 합

1 ~ N 범위의 모든 짝수의 합을 반환.

```csharp
public static int SumOfEvens(int n);
// n=10 -> 2+4+6+8+10 = 30
```

---

## B23. 문자열 뒤집기

문자열 `s`를 뒤집어 반환. `string.Reverse()`나 LINQ 금지.

```csharp
public static string Reverse(string s);
// "hello" -> "olleh"
```

---

## B24. 회문 검사

대소문자 무시, 공백 무시하고 회문(palindrome)인지 판별.

```csharp
public static bool IsPalindrome(string s);
// "A man a plan a canal Panama" -> true
```

---

## B25. 최대공약수 (GCD)

두 정수의 최대공약수를 반환. 유클리드 호제법.

```csharp
public static int Gcd(int a, int b);
// (12, 18) -> 6
```

---

## B26. 팩토리얼

`n!`을 반환. 재귀 / 반복문 둘 다 작성해보기.

```csharp
public static long FactorialRecursive(int n);
public static long FactorialIterative(int n);
```

---

## B27. 배열 최댓값/최솟값

`int[] arr` 에서 최댓값과 최솟값을 동시에 반환.

```csharp
public static (int min, int max) MinMax(int[] arr);
```

---

## B28. 배열 평균

`int[] scores` 평균을 `double`로 반환. 빈 배열은 0.

```csharp
public static double Average(int[] scores);
```

---

## B29. 중복 제거 (순서 유지)

배열에서 중복 원소를 제거하되 **원래 등장 순서**를 유지. LINQ Distinct 금지.

```csharp
public static List<int> RemoveDuplicates(int[] arr);
// {3, 1, 4, 1, 5, 9, 2, 6, 5} -> {3, 1, 4, 5, 9, 2, 6}
```

---

## B30. 배열 왼쪽으로 k번 회전

`{1,2,3,4,5}`를 왼쪽으로 2번 회전하면 `{3,4,5,1,2}`.
`k`가 배열 길이보다 클 수도 있음 (mod 처리).

```csharp
public static int[] RotateLeft(int[] arr, int k);
```

---

## B31. transform 한 칸 이동

키 입력(`KeyCode` 또는 문자)을 받아 GameObject를 4방향으로 1칸씩 이동.

```csharp
public class GridMover : MonoBehaviour {
    void Update() {
        // W/A/S/D 또는 화살표키로 transform.position을 1칸씩 이동
    }
}
```

---

## B32. 두 점의 중점

`Vector3` 두 개의 중점 반환.

```csharp
public static Vector3 Midpoint(Vector3 a, Vector3 b);
```

---

## B33. 가장 가까운 적 찾기

플레이어 위치 `Vector3 player`와 `Vector3[] enemies`가 주어진다. 가장 가까운 적의 **인덱스** 반환. 적이 없으면 -1.

```csharp
public static int NearestEnemy(Vector3 player, Vector3[] enemies);
```

---

## B34. Mathf.Clamp로 체력 안전 처리

`currentHp`에 `delta`(양수=회복, 음수=데미지)를 적용하되 `0 <= hp <= maxHp` 범위를 벗어나지 않게.

```csharp
public static int ApplyDelta(int currentHp, int delta, int maxHp);
```

---

## B35. 경험치 → 레벨 계산

레벨업에 필요한 경험치 표가 있다: 1→2 는 100, 2→3 은 200, … N→N+1 은 N*100.
누적 경험치 `totalExp`가 주어지면 현재 레벨을 반환 (시작 레벨 1).

```csharp
public static int LevelFromExp(int totalExp);
// totalExp=100 -> Lv 2 (1→2 100 필요)
// totalExp=300 -> Lv 3 (100 + 200)
// totalExp=250 -> Lv 2 (300까진 못 가니까)
```

---

## B36. 쿨다운 타이머 체크

스킬을 마지막으로 쓴 시각 `lastUsed`, 쿨다운 `cd`, 현재 시각 `now`가 주어진다.
지금 쓸 수 있으면 `true`, 아니면 남은 초를 음수로 알려주면 더 좋음 — 그냥 bool로.

```csharp
public static bool CanUseSkill(float lastUsed, float cd, float now);
```

---

## B37. 맨해튼 거리

격자 게임에서 자주 쓰는 맨해튼 거리 `|x1-x2| + |y1-y2|`.

```csharp
public static int Manhattan(int x1, int y1, int x2, int y2);
```

---

## B38. 정수 거듭제곱 (Pow 없이)

`Mathf.Pow` / `Math.Pow` 호출 금지. `base^exp`를 반환. exp는 0 이상의 정수.

```csharp
public static long IntPow(int baseValue, int exp);
// IntPow(2, 10) -> 1024
```

힌트: 단순 반복으로 충분 (O(exp)). 도전과제 — O(log exp) 버전.

---

## B39. 두 수 swap

`int a, int b`를 받아 서로 바꿔서 반환. C# 7+ 튜플 분해를 쓰는 버전과 임시 변수 쓰는 버전 둘 다 작성.

```csharp
public static (int, int) SwapTuple(int a, int b);
public static (int, int) SwapTemp(int a, int b);
```

---

## B40. 소수 판별

정수 `n`이 소수인지 판별. 2 미만은 false.

```csharp
public static bool IsPrime(int n);
```

힌트: `i*i <= n`까지만 나누면 충분. 왜?

---

# 확장 워밍업 (B41~B60)

Unity API, 게임 로직, 수학·벡터, 포맷팅. 각 5~15분.

---

## B41. Vector3.Lerp vs Slerp — 어느 걸 쓸까

세 가지 상황에 적절한 보간 함수를 고르고 한 줄 이유:
1. 카메라가 플레이어 위치로 부드럽게 따라감
2. 적이 플레이어 방향으로 부드럽게 회전 (방향 벡터 보간)
3. 체력바 게이지가 줄어드는 애니메이션

선택지: `Vector3.Lerp`, `Vector3.Slerp`, `Mathf.Lerp`, `Quaternion.Slerp`

각 상황마다 짧은 코드 1줄로 호출 예시도.

---

## B42. transform.LookAt 없이 회전

`Vector3 target` 방향을 바라보게 `transform.rotation`을 설정하라.
`transform.LookAt(target)` 호출 금지 — `Quaternion.LookRotation`만 사용.

```csharp
public static void FaceTarget(Transform self, Vector3 target);
```

---

## B43. Random.Range 함정

다음 두 코드의 차이를 설명하라:
```csharp
int a = Random.Range(1, 10);     // int 버전
float b = Random.Range(1f, 10f); // float 버전
```

특히 **10이 결과로 나올 수 있나?** 각각 답하라.

```csharp
public static List<int> RollIntDice(int min, int max, int count); // count 번 굴린 결과 리스트
```

---

## B44. PlayerPrefs로 최고점 저장/불러오기

게임 최고점을 저장하고 불러오는 메서드. 한 번도 저장 안 했으면 0 반환.

```csharp
public static void SaveHighScore(int score);
public static int LoadHighScore();
public static void ResetHighScore();
```

힌트: `PlayerPrefs.SetInt`, `GetInt`, `DeleteKey`. `Save()`도 호출해야 디스크에 강제 저장.

---

## B45. Time.timeScale로 일시정지

`Pause()`, `Resume()`을 호출하면 게임이 멈추고 재개되어야 한다. 단, **UI 애니메이션은 일시정지 중에도 작동**해야 한다.

```csharp
public class PauseManager {
    public void Pause();
    public void Resume();
}
// UI 애니메이션이 timeScale 영향 안 받게 하는 방법도 답하라
```

힌트: `Animator.updateMode = AnimatorUpdateMode.UnscaledTime`. 또는 `Time.unscaledDeltaTime` 사용.

---

## B46. Camera.ScreenToWorldPoint 활용

마우스 클릭한 화면 좌표를 월드 좌표로 변환해서 그 위치에 폭발 이펙트 프리팹을 생성하라.
**2D 게임 기준** (z = 0).

```csharp
public class ClickToSpawn : MonoBehaviour {
    public Camera cam;
    public GameObject explosionPrefab;
    void Update() { /* TODO */ }
}
```

---

## B47. Raycast로 적 클릭 감지

마우스 왼쪽 클릭 시 카메라에서 마우스 방향으로 Raycast를 쏴서 맞은 오브젝트가 `"Enemy"` 태그면 콘솔에 적 이름 출력.

```csharp
public class ClickDetector : MonoBehaviour {
    public Camera cam;
    void Update() { /* TODO */ }
}
```

힌트: `Camera.ScreenPointToRay`, `Physics.Raycast`.

---

## B48. AudioSource 재생 + 볼륨

`AudioSource` 컴포넌트가 붙은 오브젝트에서:
- `PlaySFX(AudioClip clip, float volume)`: 1회 재생 (다른 사운드 중단 X)
- `MuteAll(bool mute)`: 모든 사운드 음소거

```csharp
public class SoundController : MonoBehaviour {
    private AudioSource source;
    void Awake() { /* TODO */ }
    public void PlaySFX(AudioClip clip, float volume);
    public void MuteAll(bool mute);
}
```

힌트: `PlayOneShot`은 중첩 재생됨.

---

## B49. 데미지 = 공격력 - 방어력 (단, 최소 1)

`Atk - Def`로 데미지를 계산하되, 결과가 0 이하면 1로. 크리티컬이면 두 배.

```csharp
public static int CalcDamage(int atk, int def, bool isCritical);
```

확장: 방어 관통 `pen`(0~1)을 받아 `Def`를 그만큼 무시.
```csharp
public static int CalcDamageWithPen(int atk, int def, float pen, bool isCritical);
// pen=0.3이면 방어력의 30%는 무시
```

---

## B50. 카운트다운 "MM:SS" 포맷

남은 초 `seconds`를 받아 `"03:25"` 같은 문자열로 변환. 음수면 `"00:00"`.

```csharp
public static string FormatCountdown(int seconds);
// 205 -> "03:25"
// 0   -> "00:00"
// -5  -> "00:00"
// 3600 -> "60:00" (시간 단위 안 함)
```

---

## B51. 점수 천 단위 콤마

`12345678` → `"12,345,678"`. 자체 구현 (LINQ 가능, `ToString("N0")` 가능 — 둘 다 작성).

```csharp
public static string FormatScore(int score);                // 직접 구현
public static string FormatScoreBuiltin(int score);         // ToString("N0") 사용
```

---

## B52. 가중 확률 드롭

아이템 드롭 테이블:
```
{ "diamond": 1, "gold": 10, "iron": 30, "stone": 59 }  // 합 100
```

`Random.Range(0, 100)` 한 번 굴려서 가중치에 따라 어떤 아이템을 드롭할지 반환.

```csharp
public static string RollDrop(Dictionary<string, int> dropTable);
```

---

## B53. 두 각도 사이 최단 회전

현재 각도 `from`에서 목표 각도 `to`까지 회전할 때 **시계/반시계 중 짧은 쪽**으로 가야 한다.
회전량(부호 있는 도, -180 ~ 180)을 반환.

```csharp
public static float ShortestRotation(float from, float to);
// (350, 10) -> 20 (반시계, 짧음. 큰 길은 -340)
// (10, 350) -> -20
// (0, 180)  -> 180 또는 -180 (둘 다 OK)
```

힌트: `((to - from + 540) % 360) - 180`.

---

## B54. 각도 정규화 (0~360)

임의의 각도(음수도 가능)를 `[0, 360)` 범위로 정규화.

```csharp
public static float Normalize360(float angle);
// 370   -> 10
// -10   -> 350
// 720.5 -> 0.5
```

---

## B55. 라디안 ↔ 도 변환

`Mathf.Rad2Deg` / `Mathf.Deg2Rad` 안 쓰고 직접 작성.

```csharp
public static float DegToRad(float deg);
public static float RadToDeg(float rad);
```

힌트: π = 3.14159... 한 바퀴는 2π = 360도.

---

## B56. AABB 충돌 판정 (2D)

두 사각형 (x, y, width, height)이 겹치는지 판별. 한 점만 닿아도 충돌로 본다.

```csharp
public static bool AABBOverlap(
    float ax, float ay, float aw, float ah,
    float bx, float by, float bw, float bh);
```

힌트: "겹치지 않는 조건"의 부정이 더 쉬움.

---

## B57. 원 안에 점이 있는지

점 `(px, py)`가 중심 `(cx, cy)` 반지름 `r`인 원 내부에 있는지 (경계 포함).
`Mathf.Sqrt` 호출 금지.

```csharp
public static bool PointInCircle(float px, float py, float cx, float cy, float r);
```

---

## B58. 발사 간격 제한 (Rate Limit)

플레이어가 총을 너무 빨리 쏘지 못하게 최소 간격을 둔다.
`CanFire(float now)`: 마지막 발사 시각으로부터 `fireInterval` 이상 지났으면 true (그리고 내부적으로 마지막 시각 갱신).

```csharp
public class Gun {
    public float fireInterval = 0.2f;
    private float lastFired = float.NegativeInfinity;
    public bool TryFire(float now);
}
```

---

## B59. 이중 점프 카운트

`Jump()` 호출 시 점프. 단, 공중에서 추가 1번만 더 가능 (총 2번).
`Land()`가 호출되면 카운트 리셋.

```csharp
public class JumpController {
    public int maxJumps = 2;
    private int jumpCount = 0;
    public bool TryJump();   // 가능하면 true 반환 + 점프 카운트++
    public void Land();      // 착지 시 호출
}
```

---

## B60. 콤보 점수 — 콤보 횟수 × 배율

콤보가 이어지면 점수에 배율이 곱해진다:
- 1콤보: ×1
- 5콤보: ×1.5
- 10콤보: ×2
- 20콤보: ×3
- 50콤보 이상: ×5

`comboCount`와 `baseScore`(=한 번에 얻는 기본 점수, 보통 100)를 받아 실제 획득 점수 반환.

```csharp
public static int ComboScore(int comboCount, int baseScore);
// comboCount=7, baseScore=100 -> 150 (5콤보 구간이라 ×1.5)
```
