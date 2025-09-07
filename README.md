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

<details>
<summary>클릭해서 보기</summary>

Assets/Scripts/<br>
├─ Animator/<br>
│ └─ AtlasFrameAnimator.cs // 스프라이트 아틀라스 프레임 애니메이터(상태/콜백 onCompleted)<br>
│<br>
├─ BackEnd/ // 백엔드 연동(로그인/프로필/진행)<br>
│ ├─ BackEndManager.cs // 백엔드 SDK 초기화/세션 관리<br>
│ ├─ FindID.cs // 아이디 찾기 UI 로직<br>
│ ├─ FindPW.cs // 비밀번호 찾기/재설정 UI<br>
│ ├─ LobbyScenario.cs // 로비 흐름 시나리오(샘플)<br>
│ ├─ Login.cs // 로그인 구현(입력→검증→콜백)<br>
│ ├─ LoginBase.cs // 로그인 공통 베이스(상속 포인트)<br>
│ ├─ LoginSample.cs // 로그인 예시 흐름<br>
│ ├─ LogoScenario.cs // 부트/로고 시퀀스<br>
│ ├─ Nickname.cs // 닉네임 설정/변경<br>
│ ├─ PopupUpdateProfileViewer.cs // 프로필 갱신 팝업 뷰<br>
│ ├─ Progress.cs // 로딩/진행 표시<br>
│ ├─ RegisterAccount.cs // 회원가입 처리<br>
│ ├─ TopPanelViewer.cs // 상단 패널(닉네임/통계) 바인딩<br>
│ ├─ UITextInteraction.cs // 텍스트 입력/상호작용 핸들러<br>
│ └─ UserInfo.cs // GetUserInfo + onUserInfoEvent 브로드캐스트<br>
│<br>
├─ Enemy/ // 적 데이터/상태/스폰/풀링<br>
│ ├─ EnemyBase.cs // HP/피격/사망·드랍/콜라이더·애니 상태 전환<br>
│ ├─ EnemyData.cs // 적 ScriptableObject(체력/속도/드랍 등)<br>
│ ├─ EnemyKillCounter.cs // 적 처치 수 집계(전역 카운터)<br>
│ ├─ EnemyMovement.cs // 플레이어 추적 이동 및 flip 처리<br>
│ ├─ EnemyPooler.cs // 개별 적 풀(사전 생성/대여/반납)<br>
│ ├─ EnemyPoolHub.cs // EnemyData→Pooler 매핑, 스폰 허브<br>
│ ├─ EnemySpawner.cs // 스폰 타이밍/위치 제어<br>
│ ├─ EnemyType.cs // 적 타입 열거/정의<br>
│ ├─ HitFlashKnockback.cs // 피격 플래시/넉백/히트스톱 연출<br>
│ └─ SpawnProgressionData.cs // 시간·웨이브에 따른 스폰 곡선/테이블<br>
│<br>
├─ Item/<br>
│ └─ ExpGem.cs // 경험치 젬(드랍/흡수/가치)<br>
│<br>
├─ Managers/ // 전역 시스템/상태 머신<br>
│ ├─ AudioManager.cs // BGM/SFX 재생·풀링<br>
│ ├─ GameManager.cs // 게임 전역 상태/참조 허브<br>
│ ├─ GameOverState.cs // 상태 머신: 게임오버 상태<br>
│ ├─ GameStateManager.cs // 상태 전환 관리자(IGameState 기반)<br>
│ ├─ GameStateType.cs // 상태 타입 열거<br>
│ ├─ IGameState.cs // 상태 인터페이스<br>
│ ├─ InGameState.cs // 상태 머신: 인게임 상태<br>
│ ├─ MainMenuManager.cs // 메인 메뉴 흐름<br>
│ ├─ PermanentStatManager.cs // 메타 영구 스탯 저장/적용<br>
│ ├─ PlayerExpManager.cs // 플레이어 경험치/레벨 관리<br>
│ ├─ ResultSceneManager.cs // 결과 화면(보상/통계) 표시<br>
│ ├─ SettingsManager.cs // 옵션(볼륨/그래픽 등) 저장/로드<br>
│ ├─ StatUpgradeManager.cs // 레벨업 업그레이드 선택/적용<br>
│ ├─ UpgradeRoller.cs // 업그레이드 후보 롤링/가중치<br>
│ └─ WeaponManager.cs // 무기 등록/장착/업그레이드 파이프라인<br>
│<br>
├─ Player/<br>
│ ├─ PlayerAutoAttack.cs // 자동 공격 트리거/쿨다운<br>
│ ├─ PlayerCollision.cs // 충돌 처리(피해/획득)<br>
│ ├─ PlayerHealth.cs // 플레이어 체력/피격/사망<br>
│ └─ PlayerMovement.cs // 이동 입력/속도/경계 처리<br>
│<br>
├─ Projectiles/ // 투사체 시스템<br>
│ ├─ Projectile.cs // 투사체 수명/충돌/피해<br>
│ ├─ ProjectileData.cs // 투사체 SO(속도/관통/피해 등)<br>
│ ├─ ProjectileHitType.cs // 히트 타입 정의<br>
│ ├─ ProjectileMoveType.cs // 이동 타입 정의(직선/곡선 등)<br>
│ └─ WeaponFireType.cs // 발사 방식 열거(단발/연사/AoE 등)<br>
│<br>
├─ Stats/ // 스탯/업그레이드 데이터<br>
│ ├─ PermanentStatType.cs // 영구 스탯 종류<br>
│ ├─ PlayerStats.cs // 런타임 스탯 집계(공격력/공속/이속 등)<br>
│ ├─ StatType.cs // 일시 업그레이드 스탯 종류<br>
│ ├─ StatUpgradeData.cs // 업그레이드 수치/곡선 데이터<br>
│ ├─ StatUpgradeOptionSO.cs // 업그레이드 옵션 SO(선택지)<br>
│ └─ UpgradeOptionSO.cs // 업그레이드 공통 SO<br>
│<br>
├─ UI/ // HUD/메뉴/보상 화면<br>
│ ├─ CameraFollow.cs // 카메라 추적(목표 따라가기)<br>
│ ├─ DebugUI.cs // FPS/카운트 등 디버그 HUD<br>
│ ├─ EnemyKillsManager.cs // 처치 수 UI 바인딩<br>
│ ├─ GoldUI.cs // 골드 표시 UI<br>
│ ├─ GridSnapManager.cs // 2×2 청크 스냅 이동(무한 맵 배경)<br>
│ ├─ HealthBar.cs // 체력바 표시 로직<br>
│ ├─ LevelUpSystem.cs // 레벨업/업그레이드 선택 UI 플로우<br>
│ ├─ ParallaxLayer.cs // 배경 패럴럭스<br>
│ ├─ PlayTimeDisplay.cs // 플레이타임 표시<br>
│ ├─ Reposition.cs // 반복 배경·오브젝트 재배치 헬퍼<br>
│ ├─ SafeAreaFitter.cs // 모바일 세이프에어리아 대응<br>
│ ├─ SettingsUI.cs // 옵션 UI(볼륨/그래픽)<br>
│ ├─ UIExpDisplay.cs // 경험치 게이지 표시<br>
│ ├─ UIManager.cs // UI 화면 전환/참조<br>
│ └─ WeaponChoiceUI.cs // 업그레이드/무기 선택 패널<br>
│<br>
└─ Weapon/ // 무기·발사 로직<br>
├─ AoeFireBehaviour.cs // 광역형 발사 동작<br>
├─ BurstFireBehaviour.cs // 연사(버스트) 발사 동작<br>
├─ FlamethrowerBehaviour.cs // 화염방사 동작<br>
├─ GarlicWeapon.cs // 고유 근접/오라형 무기 구현<br>
├─ IWeaponFireBehaviour.cs // 발사 동작 인터페이스<br>
├─ SingleShotBehaviour.cs // 단발 발사 동작<br>
├─ Weapon.cs // 공통 무기 컴포넌트(상태/쿨다운)<br>
├─ WeaponData.cs // 무기 SO(스탯/이펙트/발사 방식)<br>
├─ WeaponDatabaseLoader.cs // 무기 데이터 로더/등록<br>
├─ WeaponInstance.cs // 장착 인스턴스/레벨/시너지<br>
└─ WeaponUpgradeOptionSO.cs // 무기 업그레이드 옵션 SO<br>

</details>
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

ChatGPT : 로고 그림
