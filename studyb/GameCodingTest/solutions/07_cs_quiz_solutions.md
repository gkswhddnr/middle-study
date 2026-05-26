# CS 객관식 25문제 정답 (컴투스 직격)

---

## 운영체제

**Q1.** (c) — 스레드 컨텍스트 스위칭이 프로세스 간보다 **싸다** (메모리 맵 안 바꿈). 진술이 반대.

**Q2. 데드락 4 필요조건:**
1. **상호 배제** (Mutual Exclusion) — 자원은 한 번에 하나만 사용
2. **점유와 대기** (Hold and Wait) — 자원 가진 채 다른 자원 대기
3. **비선점** (No Preemption) — 다른 프로세스가 강제로 뺏을 수 없음
4. **순환 대기** (Circular Wait) — P1→P2→P3→P1 식으로 순환

**Q3.** (c) — Mutex는 소유 개념 있음 (잠근 스레드만 풀 수 있음). Binary Semaphore는 소유 없어서 다른 스레드도 풀 수 있음.

**Q4.** (d) — 스와핑은 디스크 I/O라 매우 느림. 나머지는 맞음.

**Q5.** (b) — Non-preemptive SJF는 비선점. 한 번 시작하면 끝까지.

---

## 네트워크

**Q6.**
1. 채팅 → **TCP** (메시지 누락 안 됨)
2. FPS 위치 → **UDP** (실시간성 > 신뢰성, 누락돼도 다음 패킷)
3. 인앱 결제 → **TCP** (트랜잭션 보장)
4. 음성 채팅 → **UDP** (지연 최소화)

**Q7.** 데이터링크 → ___ → 전송 → 표현
- 1.물리 / 2.**데이터링크** / 3.네트워크 / 4.**전송** / 5.세션 / 6.**표현** / 7.응용

**Q8.**
- 200 OK / 201 Created / 204 No Content
- 301 Moved Permanently
- 400 Bad Request / 401 Unauthorized / 403 Forbidden / 404 Not Found
- 500 Internal Server Error / 503 Service Unavailable

**암기 팁:** 2xx 성공, 3xx 리다이렉트, 4xx 클라잘못, 5xx 서버잘못.

**Q9.**
1. SYN
2. SYN + ACK
3. ACK

**Q10.** (c) — REST는 **Stateless**가 원칙. 서버가 세션 유지하면 RESTful 아님.

---

## 데이터베이스

**Q11. ACID:**
- **A**tomicity (원자성): 트랜잭션은 전부 성공 or 전부 실패 (중간 X)
- **C**onsistency (일관성): 트랜잭션 전후 DB 무결성 유지
- **I**solation (격리성): 동시 트랜잭션이 서로 영향 X
- **D**urability (지속성): 커밋된 결과는 시스템 장애에도 보존

**Q12.** (b) — 2NF = 1NF + 부분 함수 종속 제거. (c)는 3NF.

**Q13.** (c) — 인덱스 많으면 INSERT/UPDATE/DELETE 비용 증가 + 디스크 공간 증가. 적정선 필요.

**Q14.** (c) 5행 — LEFT JOIN은 왼쪽 테이블 모든 행 보존, 매칭 없으면 NULL.

---

## 그래픽스 심화

**Q15. Phong 조명:**
1. **Ambient** (주변광): 모든 방향에서 오는 균일한 환경광. 그림자 영역도 완전 검정 아니게.
2. **Diffuse** (난반사): `max(0, N·L)`. 표면 법선과 광선 방향의 각도에 따른 밝기.
3. **Specular** (정반사): `max(0, R·V)^shininess`. 반사벡터와 시점벡터로 하이라이트.

**Lambert 모델 = Ambient + Diffuse만 사용** (Specular X).

**Q16.** (a) **R = I - 2(N·I)N**
- 입사벡터 I를 법선 N에 대해 반사. 광선이 표면을 향하므로 부호 주의.
- 외워두기: "반사 = 입사 - 2*(법선·입사)*법선"

**Q17.** (d) — Z-Buffer 정밀도를 **낮추면** Z-Fighting이 더 심해짐. 나머지는 완화책.

**Q18.** (c) — Trilinear = Bilinear × 2 (두 밉맵 레벨에서 각각) + 두 결과를 다시 보간.
- (a) Point/Nearest
- (b) Bilinear
- (d) Anisotropic

**Q19.** (a), (c), (e) — 빛이 더해지는 효과 (불, 광원, 마법). (b) 반투명 유리는 Alpha Blend, (d) 그림자는 Multiply.

**Q20.**
1. Local → World: **Model Matrix** (또는 World Matrix)
2. World → View: **View Matrix**
3. View → Clip: **Projection Matrix**

합친 약칭: **MVP** (Model × View × Projection, 적용 순서는 오른쪽부터).

---

## 알고리즘·자료구조

**Q21. 최악 시간복잡도:**
- Bubble: O(n²)
- Insertion: O(n²)
- Merge: **O(n log n)**
- Quick: O(n²) ⚠️ (피벗 운 나쁘면. 평균은 O(n log n))
- Heap: O(n log n)
- Counting: O(n + k)

**Q22. 자료구조 선택:**
1. 가장 위험한 운석 → **우선순위 큐 (Max-Heap)**
2. 괄호 짝 → **Stack** (LIFO)
3. 메시지 순서 → **Queue** (FIFO)
4. 닉네임 검색 → **HashSet / Dictionary** (O(1) 평균)
5. 던전 DFS → **Stack** (또는 재귀)

**Q23. 느린 순 (최악 → 최선):**
`O(n!) > O(2^n) > O(n²) > O(n log n) > O(n) > O(log n) > O(1)`

**암기:** "팩토리얼 → 지수 → 다항식 → 로그선형 → 선형 → 로그 → 상수"

**Q24.** (d) Quick Select — 이건 K번째 원소 찾는 알고리즘. 해시 충돌과 무관.

**Q25.** (b) — DP의 핵심은 **부분문제 결과 저장**. (a)는 분할정복의 특징. (c) 반복문으로도 가능 (bottom-up). (d) 그리디와 다름.

---

## 학습 후 점검 (컴투스 직격)

- [ ] 데드락 4 조건을 즉시 답할 수 있다
- [ ] OSI 7계층 다 외움
- [ ] HTTP 상태 코드 10개 의미 즉답
- [ ] ACID 4가지 풀이 가능
- [ ] Phong 조명 3성분 + Lambert 차이
- [ ] 반사벡터 공식 `R = I - 2(N·I)N` 외움
- [ ] 정렬 알고리즘 6개의 시간복잡도 즉답
- [ ] Big-O 7개 순서 즉답
- [ ] 자료구조 5개를 상황별로 즉시 선택
- [ ] MVP 행렬 순서와 의미

이 10개 즉답되면 컴투스 객관식 80% 이상 잡힘.
