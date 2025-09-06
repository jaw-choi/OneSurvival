# OneSurvival

OneSurvival game pojects

# 🐍 2D 로그라이크 자동공격 서바이벌 게임

Vampire Survivors 스타일의 자동 공격 생존 게임입니다.  
다양한 무기를 조합해 몰려오는 적들을 처치하며 최대한 오래 생존하세요!

---

## 📌 프로젝트 개요

이 프로젝트는 Unity로 제작된 2D 로그라이크 자동공격 서바이벌 게임입니다.  
플레이어는 이동만 조작하며, 무기는 자동으로 공격합니다.  
경험치를 수집해 레벨업하고 다양한 무기와 스킬을 선택하며 성장합니다.

---

## ⚙️ 주요 기능

- 자동 공격 무기 시스템 (ScriptableObject 기반)
- 무기 진화 및 강화 시스템
- 레벨업 시 무기/스킬 선택 UI
- 적 AI 및 웨이브 스폰 시스템
- 경험치 수집 및 성장 시스템
- Object Pooling 기반 최적화
- 모바일 대응 UGUI UI
- 로그라이크 요소 (무작위 선택, 매판 다른 조합)
- 메타 성장 및 저장 시스템 (준비 중)

---

## 🧱 사용 기술

- Unity 6.0 (6000.0.52f1)
- C#
- UGUI (Unity UI)
- ScriptableObject
- Object Pooling
- Git & GitHub

---

## 📱 플랫폼

- Android (출시 예정)
- PC (개발용 테스트)

---

## 🕹️ 플레이 방법

- **이동**: 터치 또는 키보드 방향키
- **공격**: 자동 발사
- **레벨업**: 경험치 획득 시 스킬/무기 선택
- **목표**: 최대한 오래 생존하고 더 강해지세요!

---

## 🗂️ 폴더 구조

Assets/Scripts/
├─ Animator/
│ └─ AtlasFrameAnimator.cs // 스프라이트 아틀라스 프레임 애니메이터(상태/콜백 onCompleted)
│
├─ BackEnd/ // 백엔드 연동(로그인/프로필/진행)
│ ├─ BackEndManager.cs // 백엔드 SDK 초기화/세션 관리
│ ├─ FindID.cs // 아이디 찾기 UI 로직
│ ├─ FindPW.cs // 비밀번호 찾기/재설정 UI
│ ├─ LobbyScenario.cs // 로비 흐름 시나리오(샘플)
│ ├─ Login.cs // 로그인 구현(입력→검증→콜백)
│ ├─ LoginBase.cs // 로그인 공통 베이스(상속 포인트)
│ ├─ LoginSample.cs // 로그인 예시 흐름
│ ├─ LogoScenario.cs // 부트/로고 시퀀스
│ ├─ Nickname.cs // 닉네임 설정/변경
│ ├─ PopupUpdateProfileViewer.cs // 프로필 갱신 팝업 뷰
│ ├─ Progress.cs // 로딩/진행 표시
│ ├─ RegisterAccount.cs // 회원가입 처리
│ ├─ TopPanelViewer.cs // 상단 패널(닉네임/통계) 바인딩
│ ├─ UITextInteraction.cs // 텍스트 입력/상호작용 핸들러
│ └─ UserInfo.cs // GetUserInfo + onUserInfoEvent 브로드캐스트
│
├─ Enemy/ // 적 데이터/상태/스폰/풀링
│ ├─ EnemyBase.cs // HP/피격/사망·드랍/콜라이더·애니 상태 전환
│ ├─ EnemyData.cs // 적 ScriptableObject(체력/속도/드랍 등)
│ ├─ EnemyKillCounter.cs // 적 처치 수 집계(전역 카운터)
│ ├─ EnemyMovement.cs // 플레이어 추적 이동 및 flip 처리
│ ├─ EnemyPooler.cs // 개별 적 풀(사전 생성/대여/반납)
│ ├─ EnemyPoolHub.cs // EnemyData→Pooler 매핑, 스폰 허브
│ ├─ EnemySpawner.cs // 스폰 타이밍/위치 제어
│ ├─ EnemyType.cs // 적 타입 열거/정의
│ ├─ HitFlashKnockback.cs // 피격 플래시/넉백/히트스톱 연출
│ └─ SpawnProgressionData.cs // 시간·웨이브에 따른 스폰 곡선/테이블
│
├─ Item/
│ └─ ExpGem.cs // 경험치 젬(드랍/흡수/가치)
│
├─ Managers/ // 전역 시스템/상태 머신
│ ├─ AudioManager.cs // BGM/SFX 재생·풀링
│ ├─ GameManager.cs // 게임 전역 상태/참조 허브
│ ├─ GameOverState.cs // 상태 머신: 게임오버 상태
│ ├─ GameStateManager.cs // 상태 전환 관리자(IGameState 기반)
│ ├─ GameStateType.cs // 상태 타입 열거
│ ├─ IGameState.cs // 상태 인터페이스
│ ├─ InGameState.cs // 상태 머신: 인게임 상태
│ ├─ MainMenuManager.cs // 메인 메뉴 흐름
│ ├─ PermanentStatManager.cs // 메타 영구 스탯 저장/적용
│ ├─ PlayerExpManager.cs // 플레이어 경험치/레벨 관리
│ ├─ ResultSceneManager.cs // 결과 화면(보상/통계) 표시
│ ├─ SettingsManager.cs // 옵션(볼륨/그래픽 등) 저장/로드
│ ├─ StatUpgradeManager.cs // 레벨업 업그레이드 선택/적용
│ ├─ UpgradeRoller.cs // 업그레이드 후보 롤링/가중치
│ └─ WeaponManager.cs // 무기 등록/장착/업그레이드 파이프라인
│
├─ Player/
│ ├─ PlayerAutoAttack.cs // 자동 공격 트리거/쿨다운
│ ├─ PlayerCollision.cs // 충돌 처리(피해/획득)
│ ├─ PlayerHealth.cs // 플레이어 체력/피격/사망
│ └─ PlayerMovement.cs // 이동 입력/속도/경계 처리
│
├─ Projectiles/ // 투사체 시스템
│ ├─ Projectile.cs // 투사체 수명/충돌/피해
│ ├─ ProjectileData.cs // 투사체 SO(속도/관통/피해 등)
│ ├─ ProjectileHitType.cs // 히트 타입 정의
│ ├─ ProjectileMoveType.cs // 이동 타입 정의(직선/곡선 등)
│ └─ WeaponFireType.cs // 발사 방식 열거(단발/연사/AoE 등)
│
├─ Stats/ // 스탯/업그레이드 데이터
│ ├─ PermanentStatType.cs // 영구 스탯 종류
│ ├─ PlayerStats.cs // 런타임 스탯 집계(공격력/공속/이속 등)
│ ├─ StatType.cs // 일시 업그레이드 스탯 종류
│ ├─ StatUpgradeData.cs // 업그레이드 수치/곡선 데이터
│ ├─ StatUpgradeOptionSO.cs // 업그레이드 옵션 SO(선택지)
│ └─ UpgradeOptionSO.cs // 업그레이드 공통 SO
│
├─ UI/ // HUD/메뉴/보상 화면
│ ├─ CameraFollow.cs // 카메라 추적(목표 따라가기)
│ ├─ DebugUI.cs // FPS/카운트 등 디버그 HUD
│ ├─ EnemyKillsManager.cs // 처치 수 UI 바인딩
│ ├─ GoldUI.cs // 골드 표시 UI
│ ├─ GridSnapManager.cs // 2×2 청크 스냅 이동(무한 맵 배경)
│ ├─ HealthBar.cs // 체력바 표시 로직
│ ├─ LevelUpSystem.cs // 레벨업/업그레이드 선택 UI 플로우
│ ├─ ParallaxLayer.cs // 배경 패럴럭스
│ ├─ PlayTimeDisplay.cs // 플레이타임 표시
│ ├─ Reposition.cs // 반복 배경·오브젝트 재배치 헬퍼
│ ├─ SafeAreaFitter.cs // 모바일 세이프에어리아 대응
│ ├─ SettingsUI.cs // 옵션 UI(볼륨/그래픽)
│ ├─ UIExpDisplay.cs // 경험치 게이지 표시
│ ├─ UIManager.cs // UI 화면 전환/참조
│ └─ WeaponChoiceUI.cs // 업그레이드/무기 선택 패널
│
└─ Weapon/ // 무기·발사 로직
├─ AoeFireBehaviour.cs // 광역형 발사 동작
├─ BurstFireBehaviour.cs // 연사(버스트) 발사 동작
├─ FlamethrowerBehaviour.cs // 화염방사 동작
├─ GarlicWeapon.cs // 고유 근접/오라형 무기 구현
├─ IWeaponFireBehaviour.cs // 발사 동작 인터페이스
├─ SingleShotBehaviour.cs // 단발 발사 동작
├─ Weapon.cs // 공통 무기 컴포넌트(상태/쿨다운)
├─ WeaponData.cs // 무기 SO(스탯/이펙트/발사 방식)
├─ WeaponDatabaseLoader.cs // 무기 데이터 로더/등록
├─ WeaponInstance.cs // 장착 인스턴스/레벨/시너지
└─ WeaponUpgradeOptionSO.cs // 무기 업그레이드 옵션 SO

## 🛠️ 설치 및 실행 방법

...

📈 개발 로드맵
기본 무기 시스템 구축

경험치 및 레벨업 시스템

적 웨이브 및 AI 구현

무기 진화 시스템

메타 성장 시스템

UI 사운드 및 연출 추가

Google Play 출시

✨ 스크린샷
(추후 게임플레이, 레벨업 UI, 무기 이펙트 등 이미지 첨부 예정)

🙌 크레딧
개발자: jaw-choi

연락처: ...

저작권:
BGM
Royalty Free Music: Bensound.com/royalty-free-music
License code: ZX1JCESJEAP9QBXN
Artist: : Benjamin Tissot

SFX
Sound Effect by <a href="https://pixabay.com/ko/users/zennnsounds-35538808/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=163073">zennnsounds</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=163073">Pixabay</a>

Grid,icon in game
언데드 서바이버 에셋 팩 by 골드메탈

App Icon
<a href="https://www.flaticon.com/free-icons/dungeon" title="dungeon icons">Dungeon icons created by Freepik - Flaticon</a>
