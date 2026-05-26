# 게임업계 코딩테스트 문제집

게임사(넥슨/넷마블/엔씨/크래프톤/스마일게이트/펄어비스/카카오게임즈 + Riot/Blizzard/EA) 코딩테스트 실제 유형과 면접 단골 주제를 조합해서 만든 연습 문제집.

## 어떻게 쓰면 좋을까

1. 난이도 폴더(`01_basic` → `02_intermediate` → `03_advanced`)를 순서대로 푼다
2. 문제 파일만 보고 직접 코딩 (해답 먼저 보지 말 것!)
3. 다 풀면 같은 번호의 `solutions/` 파일과 비교
4. Claude한테 "GameCodingTest의 N번 문제 채점해줘" 라고 하면 풀이를 봐주는 식으로 활용 가능

## 폴더 구조

```
GameCodingTest/
  README.md                       # 이 파일 (인덱스)
  01_basic.md                    # 기초 60문제 (C# 문법, Unity 기본기, 워밍업, 확장 워밍업)
  02_intermediate.md             # 중급 10문제 (구현, 시뮬레이션, 패턴)
  03_advanced.md                 # 고급 10문제 (알고리즘 + 게임 시스템)
  04_graphics.md                 # 그래픽스 8문제 (셰이더, 파이프라인, 블렌딩)
  05_mobile_optimization.md      # 모바일 최적화 8문제 (Draw Call, GC, 메모리)
  06_company_style.md            # 회사 스타일 모의 12문제 (Nexon/Kakao/Krafton/Devsisters 등)
  07_cs_quiz.md                  # CS 객관식 25문제 (OS/네트워크/DB/그래픽스심화/알고리즘) — 컴투스 직격
  COMTUS_PLAN.md                 # 컴투스 8기 인턴 2주 학습 계획 (6/7 시험)
  solutions/
    01_basic_solutions.md
    02_intermediate_solutions.md
    03_advanced_solutions.md
    04_graphics_solutions.md
    05_mobile_optimization_solutions.md
    06_company_style_solutions.md
```

**클라이언트 직무 전용** 문제집입니다. 서버 카테고리는 제외했어요.

## 출제 영역 매트릭스

| 영역 | 워밍업·기초 | 중급·고급 | 도메인 특화 |
|------|------|------|------|
| C# 문법/문자열 | B1, B2, B11, B12, B17, B21~B26, B38, B40 | - | - |
| 자료구조 활용 | B3, B4, B18, B19, B27~B30 | M1, M5, A6 | - |
| Unity 기본 | B5, B6, B7, B13~B16, B31, B34 | M3 | - |
| 벡터/수학 | B8, B9, B20, B32, B33, B37, B39 | M2, A8 | G2, G7 |
| 게임 로직 기초 | B35, B36 | - | - |
| 시뮬레이션 | B10 | M4, M6, M7, A1, A2 | - |
| 디자인 패턴 | - | M8, M9, M10, A7 | - |
| 그래프 탐색 | - | M5, A1, A3 | - |
| 게임 시스템 | - | A4, A5, A9, A10 | - |
| 렌더링/셰이더 | - | - | G1~G8 |
| 모바일 최적화 | - | - | O1~O8 |

## 추천 학습 순서

01 → 02 → 03 → 05(모바일) → 04(그래픽스) → 06(회사 스타일)

기초 → 중급 → 고급 풀고, 모바일 최적화로 실무 감각 키운 뒤, 그래픽스로 깊이 더하고, 마지막에 지원하는 회사 스타일에 맞춰 06번에서 모의 훈련.

## 총 문제 수: 133문제

- 기초: **60문제** — B1~B20(정규, 각 15~20분), B21~B40(워밍업, 각 5~10분), B41~B60(확장 워밍업, 각 5~15분)
- 중급: 10문제 (각 30~45분 목표)
- 고급: 10문제 (각 60~90분 목표)
- 그래픽스: 8문제 (각 20~60분 목표 — 개념 답안은 글로)
- 모바일 최적화: 8문제 (각 20~40분 목표)
- 회사 스타일 모의: 12문제 (Nexon/NHN/Netmarble/Line/Kakao/Krafton/Smilegate/Comtus/Devsisters/Pearl Abyss/Joycity/Bungie/Riot 스타일)
- CS 객관식: **25문제** (OS 5 / 네트워크 5 / DB 4 / 그래픽스 심화 6 / 알고리즘 5) — 컴투스 8기 인턴 직격

> **B21~B40 워밍업 문제**는 매일 풀기 시작할 때 손 푸는 용도로 좋아요. 평균 5~10분, 다 풀어도 1~2시간.
> **06번 회사 스타일**은 지원할 회사가 정해진 직전에 그 스타일만 골라 풀면 효율적이에요.

## 참고한 출처 유형

**대기업 코테 후기:**
- 한국 게임사 공채 후기 (넥슨 넥토리얼, 넷마블, 엔씨, 크래프톤)
- 삼성 SW 역량테스트 게임 시뮬레이션 계열 (마법사 상어 시리즈 등)
- 카카오 코딩테스트 기출 (게임 시뮬레이션)
- Riot Games 인턴십 가이드 (N-Queen, 연결요소 등)

**중견/중소 게임사:**
- 데브시스터즈 클라이언트 코딩 면접 후기 (라이브 코딩, 프로그래머스 120분 3문제, 자료구조+알고리즘+물리/수학)
- 컴투스/게임빌 신입 공채 (정보처리기사 수준 + 객관식+코딩, 그래픽스 문제가 가장 많은 회사)
- 펄어비스 자체 IDE + 시뮬레이션/백트래킹/우선순위큐 (실제론 C++ 위주지만 본 문제집은 C# Unity로 번역)
- 스마일게이트 SGDT (객관식+주관식+마지막 코딩, 쉬운 편)
- NHN 구름IDE 90분 3문제 (정형화 X)
- 라인게임즈/라이온하트/데브캣/카카오게임즈 상시채용 패턴
- 슈퍼캣, 모비릭스, 액션스퀘어 중소

**글로벌 (원본은 C++ 회사 多, 본 문제집은 C# Unity로 번역):**
- Bungie: take-home C# 3-4시간 (production code 수준) ← C# 그대로
- Ubisoft: Codility (재귀+트리+수학), Unity prefab/SO, 그래픽스 파이프라인
- EA/Epic Games: 가상 메서드 디스패치 / 자원 관리 / 메모리 정렬 / 캐시 친화 자료구조 — 개념은 C#에도 동일하게 적용
- Naughty Dog/Riot: 코드 + 게임 사랑 질문
- miHoYo/HoYoverse, Tencent, FromSoftware, Square Enix, Nintendo, Capcom 채용 채널

**도메인별:**
- Unity ShaderLab 공식 문서 + 게임 그래픽스 면접 키워드 (짐벌락, 쿼터니언, MVP)
- Unity 모바일 최적화 공식 가이드 (SRP Batcher, Draw Call, GC Best Practices, Canvas Rebuild)
- Unity 고급 (IL2CPP vs Mono, Job System, Burst, Addressables, UniTask)
- 멀티플레이 클라이언트 (Lag Compensation, Client-side Prediction)
- 안티치트 / 메모리 변조 방지
- 게임 프로그래밍 패턴 (Object Pool, FSM, A*, AABB, Singleton, Observer, Command, ECS, Service Locator)
