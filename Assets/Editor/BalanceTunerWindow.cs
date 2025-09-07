// File: Assets/Editor/BalanceTunerWindow.cs
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// 무기/투사체/적/스폰 데이터 밸런스를 편집하기 위한 에디터 윈도우
/// ScriptableObject 기반 데이터를 불러와 일괄 조정/저장/CSV 내보내기 가능
public class BalanceTunerWindow : EditorWindow
{
    // 데이터 타입명 (프로젝트 ScriptableObject 타입과 맞춰야 함)
    private const string WEAPON_DATA_TYPE = "WeaponData";
    private const string PROJECTILE_DATA_TYPE = "ProjectileData";
    private const string ENEMY_DATA_TYPE = "EnemyData";
    private const string SPAWN_DATA_TYPE = "SpawnProgressionData";

    // 대표 속성 배열 (Inspector와 CSV 내보내기용)
    private static readonly string[] WeaponProps = { "weaponName", "cooldown", "burstCount", "fireType", "projectileData" };
    private static readonly string[] ProjectileProps = { "damage", "speed", "pierceCount", "aoeRadius", "lifetime" };
    private static readonly string[] EnemyProps = { "enemyName", "maxHP", "moveSpeed", "contactDamage", "expDrop", "goldDrop", "spawnWeight" };
    private static readonly string[] SpawnProps = { "difficultyDuration", "groupIntervalSec", "groupSize", "inGroupStagger", "spawnDistance", "formationSpacing" };

    private enum Tab { Weapons, Projectiles, Enemies, Spawn }
    private Tab currentTab = Tab.Weapons;

    // 상태/캐시
    private Vector2 leftScroll, rightScroll;
    private string search = "";
    private bool applyWhilePlaying = true;
    private float batchMultiplier = 1.1f; // 일괄 배율

    private List<Object> allWeapons = new List<Object>();
    private List<Object> allProjectiles = new List<Object>();
    private List<Object> allEnemies = new List<Object>();
    private List<Object> allSpawns = new List<Object>();

    private HashSet<Object> selection = new HashSet<Object>(); // 좌측 다중 선택
    private SerializedObject serializedSelected; // 단일 선택 상세 편집
    private Object singleSelected;

    // 메뉴에서 열기
    [MenuItem("Tools/Balance Tuner")]
    public static void Open()
    {
        var win = GetWindow<BalanceTunerWindow>("Balance Tuner");
        win.minSize = new Vector2(830, 480);
        win.RefreshAll();
    }

    // 윈도우가 포커스를 얻을 때 자동 갱신
    private void OnFocus()
    {
        RefreshAll();
    }

    // 모든 ScriptableObject 로드
    private void RefreshAll()
    {
        allWeapons = LoadAllOfType(WEAPON_DATA_TYPE);
        allProjectiles = LoadAllOfType(PROJECTILE_DATA_TYPE);
        allEnemies = LoadAllOfType(ENEMY_DATA_TYPE);
        allSpawns = LoadAllOfType(SPAWN_DATA_TYPE);
        Repaint();
    }

    // 특정 타입 ScriptableObject 모두 불러오기
    private static List<Object> LoadAllOfType(string typeName)
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeName}");
        var list = new List<Object>();
        foreach (var g in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(g);
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (obj != null) list.Add(obj);
        }
        return list.OrderBy(o => o.name).ToList();
    }

    // 메인 GUI 렌더링
    private void OnGUI()
    {
        DrawToolbar();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginHorizontal();

        // 좌측: 리스트 및 버튼
        EditorGUILayout.BeginVertical(GUILayout.Width(320));
        DrawSearch();
        DrawList();
        DrawLeftActions();
        EditorGUILayout.EndVertical();

        // 우측: 상세 편집
        EditorGUILayout.BeginVertical();
        DrawInspector();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    // 상단 툴바 (탭 전환, 옵션)
    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        var newTab = (Tab)GUILayout.Toolbar((int)currentTab, new[] { "Weapons", "Projectiles", "Enemies", "Spawn" }, EditorStyles.toolbarButton, GUILayout.Width(300));
        if (newTab != currentTab)
        {
            currentTab = newTab;
            selection.Clear();
            singleSelected = null;
            serializedSelected = null;
        }

        GUILayout.FlexibleSpace();
        applyWhilePlaying = GUILayout.Toggle(applyWhilePlaying, "Apply in Play Mode", EditorStyles.toolbarButton);

        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(80)))
            RefreshAll();

        EditorGUILayout.EndHorizontal();
    }

    // 검색창
    private void DrawSearch()
    {
        EditorGUILayout.BeginHorizontal();
        search = EditorGUILayout.TextField("검색", search);
        if (GUILayout.Button("Clear", GUILayout.Width(60)))
        {
            search = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
    }

    // 현재 탭에 맞는 리스트 반환
    private IEnumerable<Object> CurrentList()
    {
        switch (currentTab)
        {
            case Tab.Weapons: return allWeapons;
            case Tab.Projectiles: return allProjectiles;
            case Tab.Enemies: return allEnemies;
            case Tab.Spawn: return allSpawns;
        }
        return Enumerable.Empty<Object>();
    }

    // 좌측 리스트 출력
    private void DrawList()
    {
        var list = CurrentList();
        leftScroll = EditorGUILayout.BeginScrollView(leftScroll, "box");

        foreach (var obj in list)
        {
            if (!string.IsNullOrEmpty(search) && !obj.name.ToLower().Contains(search.ToLower()))
                continue;

            bool selected = selection.Contains(obj);

            EditorGUILayout.BeginHorizontal();
            bool clicked = GUILayout.Toggle(selected, GUIContent.none, GUILayout.Width(18));
            if (clicked != selected)
            {
                if (clicked) selection.Add(obj);
                else selection.Remove(obj);
            }

            if (GUILayout.Button(obj.name, EditorStyles.label))
            {
                // 단일 선택
                singleSelected = obj;
                serializedSelected = new SerializedObject(obj);
                if (!selection.Contains(obj))
                {
                    selection.Clear();
                    selection.Add(obj);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    // 좌측 하단 액션 버튼들
    private void DrawLeftActions()
    {
        EditorGUILayout.Space(6);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("일괄 조정(Batch)", EditorStyles.boldLabel);

        batchMultiplier = EditorGUILayout.FloatField("배율", batchMultiplier);

        if (GUILayout.Button("선택 항목 × 배율 적용"))
            ApplyBatchMultiplier();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("선택 해제"))
        {
            selection.Clear();
            singleSelected = null;
            serializedSelected = null;
        }
        if (GUILayout.Button("모두 선택"))
            selection = new HashSet<Object>(CurrentList());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("저장/내보내기", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("변경 저장")) SaveAllDirty();
        if (GUILayout.Button("CSV 내보내기")) ExportCsv();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("CSV는 대표 필드만 내보냅니다. 필요 시 코드 상단 배열 수정", MessageType.Info);
        EditorGUILayout.EndVertical();
    }

    // 우측 상세 편집 Inspector
    private void DrawInspector()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("상세 편집", EditorStyles.boldLabel);

        if (singleSelected == null)
        {
            EditorGUILayout.HelpBox("좌측에서 하나를 클릭하면 상세 편집 가능", MessageType.Info);
            EditorGUILayout.EndVertical();
            return;
        }

        if (serializedSelected == null)
            serializedSelected = new SerializedObject(singleSelected);

        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);
        EditorGUILayout.ObjectField("Target", singleSelected, typeof(Object), false);

        // 대표 속성 먼저 표시
        string[] propNames = GetPropNamesForCurrentTab();
        foreach (var p in propNames)
        {
            var prop = serializedSelected.FindProperty(p);
            if (prop != null) EditorGUILayout.PropertyField(prop, true);
            else EditorGUILayout.LabelField($"(필드 없음) {p}");
        }

        // 나머지 속성 자동 표시
        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("추가 필드", EditorStyles.miniBoldLabel);
        DrawUnknownProperties(serializedSelected, propNames);

        serializedSelected.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Revert")) // 파일 다시 불러오기
        {
            string path = AssetDatabase.GetAssetPath(singleSelected);
            AssetDatabase.ImportAsset(path);
            serializedSelected = new SerializedObject(singleSelected);
        }
        if (GUILayout.Button("저장")) SaveAllDirty();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    // 현재 탭 속성 배열 반환
    private string[] GetPropNamesForCurrentTab()
    {
        switch (currentTab)
        {
            case Tab.Weapons: return WeaponProps;
            case Tab.Projectiles: return ProjectileProps;
            case Tab.Enemies: return EnemyProps;
            case Tab.Spawn: return SpawnProps;
        }
        return System.Array.Empty<string>();
    }

    // 알려지지 않은 속성 자동 탐색 표시
    private void DrawUnknownProperties(SerializedObject so, string[] known)
    {
        var it = so.GetIterator();
        bool enterChildren = true;
        HashSet<string> knownSet = new HashSet<string>(known);

        while (it.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (it.propertyPath == "m_Script") continue;
            if (knownSet.Contains(it.propertyPath)) continue;
            EditorGUILayout.PropertyField(it, true);
        }
    }

    // 선택 항목 수치 일괄 배율 적용
    private void ApplyBatchMultiplier()
    {
        if (selection.Count == 0) return;
        Undo.RecordObjects(selection.ToArray(), "Batch Multiply");

        foreach (var obj in selection)
        {
            var so = new SerializedObject(obj);
            foreach (var p in GetPropNamesForCurrentTab())
            {
                var sp = so.FindProperty(p);
                if (sp == null) continue;
                if (sp.propertyType == SerializedPropertyType.Integer)
                    sp.intValue = Mathf.RoundToInt(sp.intValue * batchMultiplier);
                else if (sp.propertyType == SerializedPropertyType.Float)
                    sp.floatValue *= batchMultiplier;
            }
            so.ApplyModifiedProperties();
            MarkDirty(obj);
        }
        ShowTempNotification("배치 적용 완료");
    }

    // Dirty Flag 저장
    private void SaveAllDirty()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        ShowTempNotification("저장 완료");
    }

    // Dirty Flag표시 (플레이 모드에서도 즉시 반영)
    private void MarkDirty(Object obj)
    {
        EditorUtility.SetDirty(obj);
    }

    // CSV 내보내기
    private void ExportCsv()
    {
        string path = EditorUtility.SaveFilePanel("Export CSV", Application.dataPath, $"balance_{currentTab}.csv", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] headers = GetPropNamesForCurrentTab();
        var list = CurrentList().ToList();

        using (var sw = new StreamWriter(path))
        {
            sw.Write("assetName");
            foreach (var h in headers) sw.Write($",{h}");
            sw.WriteLine();

            foreach (var obj in list)
            {
                var so = new SerializedObject(obj);
                sw.Write(obj.name);
                foreach (var h in headers)
                {
                    var sp = so.FindProperty(h);
                    sw.Write(",");
                    if (sp == null) { sw.Write(""); continue; }
                    switch (sp.propertyType)
                    {
                        case SerializedPropertyType.Integer: sw.Write(sp.intValue); break;
                        case SerializedPropertyType.Float: sw.Write(sp.floatValue); break;
                        case SerializedPropertyType.String: sw.Write(sp.stringValue); break;
                        case SerializedPropertyType.Boolean: sw.Write(sp.boolValue ? 1 : 0); break;
                        case SerializedPropertyType.ObjectReference: sw.Write(sp.objectReferenceValue ? sp.objectReferenceValue.name : "null"); break;
                        default: sw.Write(""); break;
                    }
                }
                sw.WriteLine();
            }
        }
        EditorUtility.RevealInFinder(path);
        ShowTempNotification("CSV 내보내기 완료");
    }

    // 일시적 알림 메시지
    private void ShowTempNotification(string msg)
    {
        this.ShowNotification(new GUIContent(msg));
        EditorApplication.delayCall += () => RemoveNotification();
    }
}
#endif
