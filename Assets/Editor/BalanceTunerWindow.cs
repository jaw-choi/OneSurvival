// File: Assets/Editor/BalanceTunerWindow.cs
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BalanceTunerWindow : EditorWindow
{
    // --------------------------
    // Types that your project uses
    // Adjust type names or property names here if your SOs differ.
    // --------------------------
    private const string WEAPON_DATA_TYPE = "WeaponData";
    private const string PROJECTILE_DATA_TYPE = "ProjectileData";
    private const string ENEMY_DATA_TYPE = "EnemyData";
    private const string SPAWN_DATA_TYPE = "SpawnProgressionData";

    // Common property paths (edit these if your fields differ)
    // WeaponData
    private static readonly string[] WeaponProps = {
        "weaponName",       // string
        "cooldown",         // float
        "burstCount",       // int
        "fireType",         // enum/int
        "projectileData"    // object ref
    };

    // ProjectileData
    private static readonly string[] ProjectileProps = {
        "damage",           // float
        "speed",            // float
        "pierceCount",      // int
        "aoeRadius",        // float
        "lifetime"          // float
    };

    // EnemyData
    private static readonly string[] EnemyProps = {
        "enemyName",        // string
        "maxHP",            // float/int
        "moveSpeed",        // float
        "contactDamage",    // float
        "expDrop",          // int/float
        "goldDrop",         // int/float
        "spawnWeight"       // int/float (optional)
    };
    private static readonly string[] SpawnProps = {
    "difficultyDuration",
    "groupIntervalSec",
    "groupSize",
    "inGroupStagger",
    "spawnDistance",
    "formationSpacing"
    };
    private enum Tab { Weapons, Projectiles, Enemies, Spawn }
    private Tab currentTab = Tab.Weapons;

    // Data caches
    private Vector2 leftScroll, rightScroll;
    private string search = "";
    private bool applyWhilePlaying = true;
    private float batchMultiplier = 1.1f; // default 10% up

    private List<Object> allWeapons = new List<Object>();
    private List<Object> allProjectiles = new List<Object>();
    private List<Object> allEnemies = new List<Object>();
    private List<Object> allSpawns = new List<Object>();

    private HashSet<Object> selection = new HashSet<Object>();
    private SerializedObject serializedSelected; // single selection drawer
    private Object singleSelected;

    [MenuItem("Tools/Balance Tuner")]
    public static void Open()
    {
        var win = GetWindow<BalanceTunerWindow>("Balance Tuner");
        win.minSize = new Vector2(830, 480);
        win.RefreshAll();
    }

    private void OnFocus()
    {
        // Auto refresh when window regains focus
        RefreshAll();
    }

    private void RefreshAll()
    {
        allWeapons = LoadAllOfType(WEAPON_DATA_TYPE);
        allProjectiles = LoadAllOfType(PROJECTILE_DATA_TYPE);
        allEnemies = LoadAllOfType(ENEMY_DATA_TYPE);
        allSpawns = LoadAllOfType(SPAWN_DATA_TYPE);
        Repaint();
    }

    private static List<Object> LoadAllOfType(string typeName)
    {
        // Finds all assets by type name (ScriptableObjects)
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

    private void OnGUI()
    {
        DrawToolbar();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginHorizontal();

        // Left: list & actions
        EditorGUILayout.BeginVertical(GUILayout.Width(320));
        DrawSearch();
        DrawList();
        DrawLeftActions();
        EditorGUILayout.EndVertical();

        // Right: inspector/editor
        EditorGUILayout.BeginVertical();
        DrawInspector();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

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
        {
            RefreshAll();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSearch()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        search = EditorGUILayout.TextField("검색", search);
        if (EditorGUI.EndChangeCheck())
        {
            // just trigger repaint
        }
        if (GUILayout.Button("Clear", GUILayout.Width(60)))
        {
            search = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
    }

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
                // Single selection for inspector
                singleSelected = obj;
                serializedSelected = new SerializedObject(obj);
                // If not already in multi-selection, keep it single
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

    private void DrawLeftActions()
    {
        EditorGUILayout.Space(6);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("일괄 조정(Batch)", EditorStyles.boldLabel);

        batchMultiplier = EditorGUILayout.FloatField("배율", batchMultiplier);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("선택 항목 수치 × 배율 적용"))
        {
            ApplyBatchMultiplier();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("선택 해제"))
        {
            selection.Clear();
            singleSelected = null;
            serializedSelected = null;
        }
        if (GUILayout.Button("모두 선택"))
        {
            selection = new HashSet<Object>(CurrentList());
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("저장/내보내기", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("변경 저장"))
        {
            SaveAllDirty();
        }
        if (GUILayout.Button("CSV 내보내기"))
        {
            ExportCsv();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("CSV는 대표 필드만 내보냅니다. 필드명 다르면 코드 상단 배열 수정", MessageType.Info);
        EditorGUILayout.EndVertical();
    }

    private void DrawInspector()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("상세 편집(단일 선택)", EditorStyles.boldLabel);

        if (singleSelected == null)
        {
            EditorGUILayout.HelpBox("좌측 리스트에서 하나를 클릭하면 상세 편집 가능합니다.", MessageType.Info);
            EditorGUILayout.EndVertical();
            return;
        }

        if (serializedSelected == null)
            serializedSelected = new SerializedObject(singleSelected);

        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

        EditorGUI.BeginChangeCheck();

        // Draw common header
        EditorGUILayout.ObjectField("Target", singleSelected, typeof(Object), false);

        // Draw known properties by tab
        string[] propNames = GetPropNamesForCurrentTab();
        foreach (var p in propNames)
        {
            var prop = serializedSelected.FindProperty(p);
            if (prop == null)
            {
                EditorGUILayout.LabelField($"(필드 없음) {p}");
                continue;
            }
            EditorGUILayout.PropertyField(prop, true);
        }

        // Draw rest of properties optionally
        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("추가 필드(자동 탐색)", EditorStyles.miniBoldLabel);
        DrawUnknownProperties(serializedSelected, propNames);

        if (EditorGUI.EndChangeCheck())
        {
            serializedSelected.ApplyModifiedProperties();
            MarkDirty(singleSelected);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Revert (파일 재로드)"))
        {
            string path = AssetDatabase.GetAssetPath(singleSelected);
            AssetDatabase.ImportAsset(path);
            serializedSelected = new SerializedObject(singleSelected);
        }

        if (GUILayout.Button("저장"))
        {
            SaveAllDirty();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

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

    private void DrawUnknownProperties(SerializedObject so, string[] known)
    {
        // Draw any visible root props that are not in known list
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
        so.ApplyModifiedProperties();
    }

    private void ApplyBatchMultiplier()
    {
        if (selection.Count == 0) return;

        Undo.RecordObjects(selection.ToArray(), "Batch Multiply");

        foreach (var obj in selection)
        {
            var so = new SerializedObject(obj);
            string[] props = GetPropNamesForCurrentTab();

            foreach (var p in props)
            {
                var sp = so.FindProperty(p);
                if (sp == null) continue;

                // Multiply numeric fields only
                switch (sp.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        sp.intValue = Mathf.RoundToInt(sp.intValue * batchMultiplier);
                        break;
                    case SerializedPropertyType.Float:
                        sp.floatValue *= batchMultiplier;
                        break;
                }
            }

            so.ApplyModifiedProperties();
            MarkDirty(obj);
        }

        ShowTempNotification("배치 적용 완료");
    }

    private void SaveAllDirty()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        ShowTempNotification("저장 완료");
    }

    private void MarkDirty(Object obj)
    {
        // Mark asset dirty, and allow runtime applying if desired
        EditorUtility.SetDirty(obj);
        if (applyWhilePlaying && Application.isPlaying)
        {
            // No extra steps required; values on ScriptableObject are read live by your systems
        }
    }

    private void ExportCsv()
    {
        string path = EditorUtility.SaveFilePanel("Export CSV", Application.dataPath, $"balance_{currentTab}.csv", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] headers = GetPropNamesForCurrentTab();
        var list = CurrentList().ToList();

        using (var sw = new StreamWriter(path))
        {
            // header
            sw.Write("assetName");
            foreach (var h in headers) sw.Write($",{h}");
            sw.WriteLine();

            // rows
            foreach (var obj in list)
            {
                var so = new SerializedObject(obj);
                sw.Write(obj.name);
                foreach (var h in headers)
                {
                    var sp = so.FindProperty(h);
                    sw.Write(",");
                    if (sp == null)
                    {
                        sw.Write("");
                        continue;
                    }
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

    private void ShowTempNotification(string msg)
    {
        this.ShowNotification(new GUIContent(msg));
        EditorApplication.delayCall += () => RemoveNotification();
    }
}
#endif
