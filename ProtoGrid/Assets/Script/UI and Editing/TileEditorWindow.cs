using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;



public class TileEditorWindow : EditorWindow
{
   /* GridGenerator gridG;
    bool tileSelected;

    bool walkable;
    bool invisible;
    bool ogPos;
    int Key;
    int Door;
    bool Open;
    bool Crumble;
    int LevelTransiIndex;
    int TempoTile;
    int Teleporter;
    int TeleporterTarget;
    int Raws;
    int Columns;




    [MenuItem("Window/Tiling")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<TileEditorWindow>("Tiling");
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        

        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 22;
        style.normal.textColor = Color.yellow;  
        GUILayout.Label("Grid Generation", style);

        GridGeneration();

        EditorGUILayout.Space(30);

        GUILayout.Label("Tile Management", style);
        
        MatRefresh();

        AssignVariableValue();
        DrawVariablesInWindow();
        UpdateValues();
        EditorUtility.SetDirty(gridG);
    }

    void GridGeneration()
    {
        
        gridG = FindObjectOfType<GridGenerator>();

        Raws = gridG.raws;
        Columns = gridG.columns;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Raws");
        Raws = EditorGUILayout.IntSlider(Raws, 1, 10);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Columns");
        Columns = EditorGUILayout.IntSlider(Columns, 1, 10);
        EditorGUILayout.EndHorizontal();

        gridG.raws = Raws;
        gridG.columns = Columns;

        if (GUILayout.Button("Generate Grid"))
        {
            gridG.instantiateGrid = true;
        }
    }

    void MatRefresh()
    {
        if (GUILayout.Button("Refresh Mats"))
        {
            foreach (GridTiling tile in FindObjectsOfType<GridTiling>())
            {
                tile.GetComponent<GridTiling>().refreshRend = true;
                tile.GetComponent<GridTiling>().refreshRendTempo = true;

            }
        }
    }

    void AssignVariableValue()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            GridTiles g = obj.GetComponent<GridTiles>();
            if (obj.GetComponent<GridTiles>())
            {

                walkable = g.walkable;
                ogPos = g.originalPosition;
                Key = g.key;
                Door = g.door;
                Open = g.open;
                Crumble = g.crumble;
                LevelTransiIndex = (int)(g.levelTransiIndex * 10);
                TempoTile = g.tempoTile;
                Teleporter = g.teleporter;
                TeleporterTarget = g.tpTargetIndex;
                invisible = g.invisible;


            }
        }
    }

    void DrawVariablesInWindow()
    {
        if (tileSelected)
        {
            EditorGUILayout.Space(15);
            GUILayout.Label("Can Walk Onto", EditorStyles.boldLabel);


            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Walkable             ");
            walkable = EditorGUILayout.Toggle(walkable);
            EditorGUILayout.EndHorizontal();

            if (!walkable)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Invisible               ");
                invisible = EditorGUILayout.Toggle(invisible);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Key Feature Parameters", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Key");
            Key = EditorGUILayout.IntSlider(Key, 0, 5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Door");
            Door = EditorGUILayout.IntSlider(Door, 0, 5);
            EditorGUILayout.EndHorizontal();

            if (Door > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Open");
                Open = EditorGUILayout.Toggle(Open);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Tempo Tiling", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("TempoTile");
            TempoTile = EditorGUILayout.IntSlider(TempoTile, 0, 3);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Crumble");
            Crumble = EditorGUILayout.Toggle(Crumble);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);
            GUILayout.Label("Teleportation Feature Parameters", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Teleporter");
            Teleporter = EditorGUILayout.IntSlider(Teleporter, 0, 10);
            EditorGUILayout.EndHorizontal();

            if (Teleporter != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Teleporter Target");
                TeleporterTarget = EditorGUILayout.IntSlider(TeleporterTarget, 0, 10);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Other", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Original Position");
            ogPos = EditorGUILayout.Toggle(ogPos);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Level Transi Index");
            LevelTransiIndex = EditorGUILayout.IntSlider(LevelTransiIndex, 1, 65);
            EditorGUILayout.EndHorizontal();
        }
        else if (!tileSelected)
        {
             GUI.enabled = false;
            EditorGUILayout.Space(15);
            GUILayout.Label("Can Walk Onto", EditorStyles.boldLabel);


            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Walkable             ");
            walkable = EditorGUILayout.Toggle(walkable);
            EditorGUILayout.EndHorizontal();

            if (!walkable)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Invisible               ");
                invisible = EditorGUILayout.Toggle(invisible);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Key Feature Parameters", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Key");
            Key = EditorGUILayout.IntSlider(Key, 0, 5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Door");
            Door = EditorGUILayout.IntSlider(Door, 0, 5);
            EditorGUILayout.EndHorizontal();

            if (Door > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Open");
                Open = EditorGUILayout.Toggle(Open);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Tempo Tiling", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("TempoTile");
            TempoTile = EditorGUILayout.IntSlider(TempoTile, 0, 3);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Crumble");
            Crumble = EditorGUILayout.Toggle(Crumble);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);
            GUILayout.Label("Teleportation Feature Parameters", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Teleporter");
            Teleporter = EditorGUILayout.IntSlider(Teleporter, 0, 10);
            EditorGUILayout.EndHorizontal();

            if (Teleporter != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Teleporter Target");
                TeleporterTarget = EditorGUILayout.IntSlider(TeleporterTarget, 0, 10);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(15);
            GUILayout.Label("Other", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Original Position");
            ogPos = EditorGUILayout.Toggle(ogPos);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Level Transi Index");
            LevelTransiIndex = EditorGUILayout.IntSlider(LevelTransiIndex, 1, 65);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
        }
    }

    void UpdateValues()
    {
        if (Selection.objects.Length == 0)
            tileSelected = false;

        foreach (GameObject obj in Selection.gameObjects)
        {
            GridTiles g = obj.GetComponent<GridTiles>();
            if (obj.GetComponent<GridTiles>())
            {
                tileSelected = true;

                if (g.walkableC != walkable)
                {
                    g.walkableC = walkable;
                    g.walkable = walkable;
                }

                if (g.ogPosC != ogPos)
                {
                    g.ogPosC = ogPos;
                    g.originalPosition = ogPos;
                }

                if (g.KeyC != Key)
                {
                    g.KeyC = Key;
                    g.KeyC = Key;
                }

                if (g.DoorC != Door )
                {
                    g.door = Door;
                    g.door = Door;
                }

                if (g.OpenC != Open )
                {
                    g.OpenC  = Open ;
                    g.open = Open;
                }

                if (g.CrumbleC != Crumble)
                {
                    g.CrumbleC = Crumble;
                    g.crumble = Crumble;
                }

               *//* if (g.LevelTransiIndexC != LevelTransiIndex)
                {
                    g.LevelTransiIndexC = LevelTransiIndex;
                    g.levelTransiIndex = LevelTransiIndex / 10;
                }*//*

                if (g.TempoTileC != TempoTile)
                {
                    g.TempoTileC = TempoTile;
                    g.tempoTile = TempoTile;
                }

                if (g.TeleporterC != Teleporter)
                {
                    g.TeleporterC = Teleporter;
                    g.teleporter = Teleporter;
                }

                if (g.TeleporterTargetC != TeleporterTarget )
                {
                    g.TeleporterTargetC = TeleporterTarget;
                    g.tpTargetIndex = TeleporterTarget;
                }
               

                if (!walkable)
                {
                    if (g.invisibleC != invisible)
                    {
                        g.invisibleC = invisible;
                        g.invisible = invisible;
                    }
                }

            }
            else
            {
                tileSelected = false;
            }
        }
    }*/
}
#endif


