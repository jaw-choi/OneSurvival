//using UnityEngine;

//public class Projectile : MonoBehaviour
//{
//    public float speed;
//    public float damage;
//    private int pierceCount;
//    private Vector2 dir;
//    private Weapon ownerWeapon;

//    private Transform player;
//    private Transform homingTarget;
//    private float orbitAngle;

//    public ProjectileData projectileData;

//    private Vector3 startPos;

//    private float boomerangTime = 0f;
//    private float totalBoomerangDuration = 2f; // 총 이동 시간
//    private float arcRadius = 3f;
//    private float rotationSpeed = 720f; // 회전 속도 (시각 효과용)
//    public void SetData(ProjectileData baseData, Weapon weapon)
//    {
//        this.projectileData = baseData;
//        damage = baseData.damage * weapon.GetDamageMultiplier();
//        speed = baseData.speed * weapon.GetSpeedMultiplier();
//        pierceCount = baseData.pierceCount + Mathf.RoundToInt(weapon.GetPierceBonus());
//        ownerWeapon = weapon;
//        Destroy(gameObject, baseData.lifetime);
//    }

//    public void Init(Vector2 direction)
//    {
//        if (projectileData.moveType == ProjectileMoveType.RandomSpread)
//        {
//            float angle = Random.Range(-30f, 30f);
//            dir = Quaternion.Euler(0, 0, angle) * direction;
//        }
//        else
//        {
//            dir = direction.normalized;
//        }

//        if (projectileData.moveType == ProjectileMoveType.OrbitPlayer)
//        {
//            player = GameObject.FindGameObjectWithTag("Player").transform;
//            orbitAngle = Random.Range(0f, 360f);
//        }

//        if (projectileData.moveType == ProjectileMoveType.Boomerang)
//        {
//            player = GameObject.FindGameObjectWithTag("Player").transform; //  필수!
//            startPos = transform.position;
//            boomerangTime = 0f;
//        }
//    }

//    void Update()
//    {
//        switch (projectileData.moveType)
//        {
//            case ProjectileMoveType.Straight:
//                MoveStraight();
//                break;
//            case ProjectileMoveType.Homing:
//                MoveHoming();
//                break;
//            case ProjectileMoveType.OrbitPlayer:
//                MoveOrbitPlayer();
//                break;
//            case ProjectileMoveType.RandomSpread:
//                MoveStraight();
//                break;
//            case ProjectileMoveType.Flamethrower:
//                MoveFlame();
//                break;
//            case ProjectileMoveType.Boomerang:
//                MoveBoomerang();
//                break;
//        }
//    }
//    void MoveBoomerang()
//    {
//        if (player == null) return;

//        boomerangTime += Time.deltaTime;

//        // 시간에 따라 0 → 1 → 0으로 갔다가 돌아오는 Lerp값
//        float t = boomerangTime / totalBoomerangDuration;
//        float progress = Mathf.Sin(t * Mathf.PI); // 0 → 1 → 0

//        // 이동 방향 기준 좌우로 휘는 궤적 만들기
//        Vector2 forward = dir.normalized;
//        Vector2 right = new Vector2(forward.y, -forward.x); // 수직 벡터

//        Vector2 offset = forward * arcRadius * progress + right * Mathf.Sin(t * Mathf.PI * 2) * arcRadius * 0.5f;

//        transform.position = new Vector3(startPos.x + offset.x, startPos.y + offset.y, transform.position.z);

//        // 시각적인 회전
//        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

//        // 종료 조건
//        if (boomerangTime >= totalBoomerangDuration)
//        {
//            Destroy(gameObject);
//        }
//    }

//    void MoveStraight()
//    {
//        transform.Translate(dir * speed * Time.deltaTime);
//    }

//    void MoveHoming()
//    {
//        if (homingTarget == null)
//        {
//            float minDist = float.MaxValue;
//            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
//            {
//                float dist = Vector2.Distance(transform.position, enemy.transform.position);
//                if (dist < minDist)
//                {
//                    minDist = dist;
//                    homingTarget = enemy.transform;
//                }
//            }
//        }

//        if (homingTarget != null)
//        {
//            Vector2 direction = (homingTarget.position - transform.position).normalized;
//            transform.Translate(direction * speed * Time.deltaTime);
//        }
//    }

//    void MoveOrbitPlayer()
//    {
//        if (player == null) return;

//        orbitAngle += speed * Time.deltaTime;
//        float radius = 1.5f;
//        Vector2 offset = new Vector2(Mathf.Cos(orbitAngle), Mathf.Sin(orbitAngle)) * radius;
//        transform.position = player.position + (Vector3)offset;
//    }

//    void MoveFlame()
//    {
//        transform.Translate(dir * speed * Time.deltaTime * 0.5f);
//    }

//    void OnTriggerEnter2D(Collider2D col)
//    {
//        if (!col.CompareTag("Enemy")) return;

//        switch (projectileData.hitType)
//        {
//            case ProjectileHitType.SingleHit:
//                col.GetComponent<EnemyBase>().TakeDamage(damage);
//                ownerWeapon?.AddDamage(damage); // 누적 데미지 기록
//                Destroy(gameObject);
//                break;

//            case ProjectileHitType.Pierce:
//                col.GetComponent<EnemyBase>().TakeDamage(damage);
//                ownerWeapon?.AddDamage(damage); // 누적 데미지 기록
//                pierceCount--;
//                if (pierceCount <= 0)
//                    Destroy(gameObject);
//                break;

//            case ProjectileHitType.AoE:
//                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, projectileData.aoeRadius);
//                foreach (var enemy in targets)
//                {
//                    if (enemy.CompareTag("Enemy"))
//                    {
//                        enemy.GetComponent<EnemyBase>().TakeDamage(damage);
//                        ownerWeapon?.AddDamage(damage); // 누적 데미지 기록
//                    }
//                }
//                Destroy(gameObject);
//                break;
//        }
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (projectileData != null && projectileData.hitType == ProjectileHitType.AoE)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(transform.position, projectileData.aoeRadius);
//        }
//    }
//}

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    private int pierceCount;
    private Vector2 dir;
    private Weapon ownerWeapon;

    private Transform player;
    private Transform homingTarget;
    private float orbitAngle;

    public ProjectileData projectileData;

    private Vector3 startPos;

    private float boomerangTime = 0f;
    private float totalBoomerangDuration = 2f;
    private float arcRadius = 3f;
    private float rotationSpeed = 720f;

    public void SetData(ProjectileData baseData, Weapon weapon)
    {
        projectileData = baseData;
        damage = baseData.damage * weapon.GetDamageMultiplier();
        speed = baseData.speed * weapon.GetSpeedMultiplier();
        pierceCount = baseData.pierceCount + Mathf.RoundToInt(weapon.GetPierceBonus());
        ownerWeapon = weapon;
        Destroy(gameObject, baseData.lifetime);
    }

    public void Init(Vector2 direction)
    {
        if (projectileData.moveType == ProjectileMoveType.RandomSpread)
        {
            float angle = Random.Range(-30f, 30f);
            dir = (Vector2)(Quaternion.Euler(0, 0, angle) * direction);
        }
        else
        {
            dir = direction.normalized;
        }

        if (projectileData.moveType == ProjectileMoveType.OrbitPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            orbitAngle = Random.Range(0f, 360f);
        }

        if (projectileData.moveType == ProjectileMoveType.Boomerang)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            startPos = transform.position;
            boomerangTime = 0f;
        }
    }

    void Update()
    {
        switch (projectileData.moveType)
        {
            case ProjectileMoveType.Straight: MoveStraight(); break;
            case ProjectileMoveType.Homing: MoveHoming(); break;
            case ProjectileMoveType.OrbitPlayer: MoveOrbitPlayer(); break;
            case ProjectileMoveType.RandomSpread: MoveStraight(); break;
            case ProjectileMoveType.Flamethrower: MoveFlame(); break;
            case ProjectileMoveType.Boomerang: MoveBoomerang(); break;
        }
    }

    void MoveBoomerang()
    {
        if (player == null) return;
        boomerangTime += Time.deltaTime;
        float t = boomerangTime / totalBoomerangDuration;
        float progress = Mathf.Sin(t * Mathf.PI);
        Vector2 forward = dir.normalized;
        Vector2 right = new Vector2(forward.y, -forward.x);
        Vector2 offset = forward * arcRadius * progress + right * Mathf.Sin(t * Mathf.PI * 2) * arcRadius * 0.5f;
        transform.position = new Vector3(startPos.x + offset.x, startPos.y + offset.y, transform.position.z);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (boomerangTime >= totalBoomerangDuration) Destroy(gameObject);
    }

    void MoveStraight()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void MoveHoming()
    {
        if (homingTarget == null)
        {
            float minDist = float.MaxValue;
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    homingTarget = enemy.transform;
                }
            }
        }
        if (homingTarget != null)
        {
            Vector2 direction = (homingTarget.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
            dir = direction;
        }
    }

    void MoveOrbitPlayer()
    {
        if (player == null) return;
        orbitAngle += speed * Time.deltaTime;
        float radius = 1.5f;
        Vector2 offset = new Vector2(Mathf.Cos(orbitAngle), Mathf.Sin(orbitAngle)) * radius;
        transform.position = player.position + (Vector3)offset;
    }

    void MoveFlame()
    {
        transform.Translate(dir * speed * Time.deltaTime * 0.5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        switch (projectileData.hitType)
        {
            case ProjectileHitType.SingleHit:
                {
                    Vector2 contact = col.ClosestPoint(transform.position);
                    SpawnHitFx(contact, dir);
                    col.GetComponent<EnemyBase>()?.TakeDamage(damage);
                    ownerWeapon?.AddDamage(damage);
                    Destroy(gameObject);
                    break;
                }
            case ProjectileHitType.Pierce:
                {
                    Vector2 contact = col.ClosestPoint(transform.position);
                    SpawnHitFx(contact, dir);
                    col.GetComponent<EnemyBase>()?.TakeDamage(damage);
                    ownerWeapon?.AddDamage(damage);
                    pierceCount--;
                    if (pierceCount <= 0) Destroy(gameObject);
                    break;
                }
            case ProjectileHitType.AoE:
                {
                    Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, projectileData.aoeRadius);
                    for (int i = 0; i < targets.Length; ++i)
                    {
                        var enemyCol = targets[i];
                        if (!enemyCol.CompareTag("Enemy")) continue;
                        Vector2 contact = enemyCol.ClosestPoint(transform.position);
                        SpawnHitFx(contact, (contact - (Vector2)transform.position).sqrMagnitude > 0.0001f ? (contact - (Vector2)transform.position).normalized : dir);
                        enemyCol.GetComponent<EnemyBase>()?.TakeDamage(damage);
                        ownerWeapon?.AddDamage(damage);
                    }
                    Destroy(gameObject);
                    break;
                }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (projectileData != null && projectileData.hitType == ProjectileHitType.AoE)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, projectileData.aoeRadius);
        }
    }

    void SpawnHitFx(Vector2 position, Vector2 forward)
    {
        if (projectileData == null || projectileData.hitEffectPrefab == null) return;
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        var go = Instantiate(projectileData.hitEffectPrefab, position, Quaternion.Euler(0, 0, angle));
        var ps = go.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            ps.Play();
        }
        else
        {
            Destroy(go, 2f);
        }
    }
}

