using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFireBehaviour : IWeaponFireBehaviour
{
    private readonly MonoBehaviour coroutineOwner;
    private readonly Transform ownerTransform;
    private readonly Weapon weapon;

    private readonly List<GameObject> orbitProjectiles = new List<GameObject>();
    private readonly List<float> createdTimes = new List<float>();
    private Coroutine orbitRoutine;
    private float elapsed;
    private float phaseRad;

    private int appliedLevel = -1;
    private int appliedCount = 0;
    private float appliedSpeedDeg = 0f;

    public OrbitFireBehaviour(Weapon weapon)
    {
        this.weapon = weapon;
        coroutineOwner = weapon;
        ownerTransform = weapon.transform;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        // 이미 돌고 있으면 아무 것도 하지 않음(영구 오라)
        if (orbitRoutine != null)
        {
            // 레벨/파라미터만 갱신
            ApplyLevelIfChanged(data, keepPhase: true);
            return;
        }

        // 시작 위상 설정
        if (data.orbitRandomStart) phaseRad = Random.Range(0f, 2f * Mathf.PI);
        else
        {
            float omega = GetSpeedDegForLevel(data) * Mathf.Deg2Rad;
            phaseRad = Mathf.Repeat(Time.time * omega, 2f * Mathf.PI);
        }

        orbitRoutine = coroutineOwner.StartCoroutine(OrbitLoop(data));
    }

    public void OnLevelChanged(int newLevel, WeaponData data)
    {
        data.currentLevel = newLevel;
        ApplyLevelIfChanged(data, keepPhase: true);
    }
    // OrbitFireBehaviour 내부에 추가
    private void ValidateAndRefillOrbit(WeaponData data)
    {
        // 1) 파괴/풀 반환 등으로 null이 된 항목 제거 (뒤에서 앞으로)
        for (int i = orbitProjectiles.Count - 1; i >= 0; --i)
        {
            if (!orbitProjectiles[i]) // Unity의 == null 오버로드 활용
            {
                orbitProjectiles.RemoveAt(i);
                createdTimes.RemoveAt(i);
            }
        }

        // 2) 영구 오라면 부족분 즉시 보충
        int targetCount = appliedCount;
        Vector3 center = ownerTransform ? ownerTransform.position : Vector3.zero;
        float rot0 = phaseRad;

        while (orbitProjectiles.Count < targetCount)
        {
            int i = orbitProjectiles.Count;
            float baseAngle = 2f * Mathf.PI * i / Mathf.Max(1, targetCount);
            float angle = baseAngle + rot0;

            Vector3 spawnPos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f)
                               * (weapon.weaponData.orbitSpawnEase ? 0f : weapon.weaponData.orbitRadius);

            GameObject go = Object.Instantiate(weapon.weaponData.projectilePrefab, spawnPos, Quaternion.identity);

            var projectile = go.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetData(weapon.weaponData.projectileData, weapon);
                projectile.Init(Vector2.zero);
                //if (projectile is IOrbitAware orbitAware) orbitAware.SetOrbitMode(true);
            }

            if (weapon.weaponData.orbitFaceOutward)
            {
                float deg = angle * Mathf.Rad2Deg;
                go.transform.rotation = Quaternion.Euler(0f, 0f, deg + 90f);
            }

            orbitProjectiles.Add(go);
            createdTimes.Add(Time.time);
        }
    }

    private IEnumerator OrbitLoop(WeaponData data)
    {
        ApplyLevelIfChanged(data, keepPhase: false);

        if (data.projectileData.hitEffectPrefab != null)
            Object.Instantiate(data.projectileData.hitEffectPrefab, ownerTransform.position, Quaternion.identity);

        while (true)
        {
            // 오너가 파괴/비활성되면 종료
            if (!ownerTransform || !coroutineOwner || !weapon)
                yield break;

            // 영구 오라 아니고, 제한시간 사용 시에만 만료 검사
            if (!data.orbitPersistent && data.orbitDuration > 0f && elapsed >= data.orbitDuration)
            {
                BreakOrbit();
                yield break;
            }

            // 레벨 변화 반영
            ApplyLevelIfChanged(data, keepPhase: true);

            // 리스트 정합성 보수: null 제거 및 부족분 보충
            ValidateAndRefillOrbit(data);

            float omega = appliedSpeedDeg * Mathf.Deg2Rad;
            float rot = phaseRad + omega * elapsed;

            int n = orbitProjectiles.Count;
            Vector3 center = ownerTransform.position;

            for (int i = 0; i < n; ++i)
            {
                var go = orbitProjectiles[i];
                if (!go) continue; // 파괴된 경우 transform 접근 금지

                float baseAngle = 2f * Mathf.PI * i / Mathf.Max(1, n);
                float angle = baseAngle + rot;

                float t = 1f;
                if (data.orbitSpawnEase)
                    t = Mathf.Clamp01((Time.time - createdTimes[i]) / Mathf.Max(0.0001f, data.orbitSpawnEaseTime));

                float r = data.orbitRadius * t;
                Vector3 targetPos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * r;

                // 안전 접근: Rigidbody2D가 있으면 MovePosition, 없으면 transform.position
                var rb = go.GetComponent<Rigidbody2D>();
                if (rb != null) rb.MovePosition(targetPos);
                else go.transform.position = targetPos;

                if (data.orbitFaceOutward)
                {
                    float deg = angle * Mathf.Rad2Deg;
                    go.transform.rotation = Quaternion.Euler(0f, 0f, deg + 90f);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void ApplyLevelIfChanged(WeaponData data, bool keepPhase)
    {
        int level = Mathf.Max(1, data.currentLevel);
        int count = GetCountForLevel(data);
        float speedDeg = GetSpeedDegForLevel(data);

        bool needRebuild = (level != appliedLevel) || (count != appliedCount);
        appliedLevel = level;
        appliedCount = count;

        bool speedChanged = !Mathf.Approximately(speedDeg, appliedSpeedDeg);
        appliedSpeedDeg = speedDeg;

        if (!keepPhase)
        {
            if (data.orbitRandomStart) phaseRad = Random.Range(0f, 2f * Mathf.PI);
            else
            {
                float omega = speedDeg * Mathf.Deg2Rad;
                phaseRad = Mathf.Repeat(Time.time * omega, 2f * Mathf.PI);
            }
        }

        if (needRebuild)
        {
            RebuildOrbit(data);
            // elapsed 유지하면 회전이 끊기지 않음(원하면 0으로 리셋)
        }
        // 속도만 바뀐 경우는 루프에서 자동 반영
    }

    private int GetCountForLevel(WeaponData data)
    {
        int idx = Mathf.Max(0, data.currentLevel - 1);
        if (data.orbitCountPerLevel != null && data.orbitCountPerLevel.Length > 0)
        {
            idx = Mathf.Min(idx, data.orbitCountPerLevel.Length - 1);
            return Mathf.Max(1, data.orbitCountPerLevel[idx]);
        }
        return Mathf.Max(1, data.currentLevel);
    }

    private float GetSpeedDegForLevel(WeaponData data)
    {
        int idx = Mathf.Max(0, data.currentLevel - 1);
        if (data.orbitSpeedDegPerLevel != null && data.orbitSpeedDegPerLevel.Length > 0)
        {
            idx = Mathf.Min(idx, data.orbitSpeedDegPerLevel.Length - 1);
            return data.orbitSpeedDegPerLevel[idx];
        }
        float baseDeg = data.orbitSpeedBaseDeg;
        if (data.speedMultiplierPerLevel != null && data.speedMultiplierPerLevel.Length > 0)
        {
            idx = Mathf.Min(idx, data.speedMultiplierPerLevel.Length - 1);
            return baseDeg * Mathf.Max(0.01f, data.speedMultiplierPerLevel[idx]);
        }
        return baseDeg;
    }

    private void RebuildOrbit(WeaponData data)
    {
        for (int i = 0; i < orbitProjectiles.Count; ++i)
            if (orbitProjectiles[i] != null) Object.Destroy(orbitProjectiles[i]);
        orbitProjectiles.Clear();
        createdTimes.Clear();

        int count = appliedCount;
        Vector3 center = ownerTransform.position;
        float rot0 = phaseRad;

        for (int i = 0; i < count; ++i)
        {
            float baseAngle = 2f * Mathf.PI * i / count;
            float angle = baseAngle + rot0;

            Vector3 spawnPos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * (data.orbitSpawnEase ? 0f : data.orbitRadius);

            GameObject go = Object.Instantiate(data.projectilePrefab, spawnPos, Quaternion.identity);
            var projectile = go.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetData(data.projectileData, weapon);
                projectile.Init(Vector2.zero);
                // 영구 오라 모드 전달: 자동 파괴 로직 끔
                //if (projectile is IOrbitAware orbitAware) orbitAware.SetOrbitMode(true);
            }

            if (data.orbitFaceOutward)
            {
                float deg = angle * Mathf.Rad2Deg;
                go.transform.rotation = Quaternion.Euler(0f, 0f, deg + 90f);
            }

            orbitProjectiles.Add(go);
            createdTimes.Add(Time.time);
        }
    }

    private void BreakOrbit()
    {
        for (int i = 0; i < orbitProjectiles.Count; ++i)
            if (orbitProjectiles[i] != null) Object.Destroy(orbitProjectiles[i]);

        orbitProjectiles.Clear();
        createdTimes.Clear();

        if (orbitRoutine != null)
        {
            coroutineOwner.StopCoroutine(orbitRoutine);
            orbitRoutine = null;
        }
    }

    public void Stop() => BreakOrbit();
}
