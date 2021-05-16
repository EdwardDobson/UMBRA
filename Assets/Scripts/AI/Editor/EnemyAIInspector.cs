using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIInspector : Editor
{
    SerializedProperty currentPathType;
    SerializedProperty patrolPointObjects;   
    SerializedProperty enemyType;   

    void OnEnable()
    {
        //Get the variable info from the path
        currentPathType = serializedObject.FindProperty("currentPathType");
        patrolPointObjects = serializedObject.FindProperty("patrolPointObjects");
        enemyType = serializedObject.FindProperty("enemyType");
    
    }

    public override void OnInspectorGUI()
    {
        EnemyAI myScript = (EnemyAI)target;

        GUIContent pathTypeLabel = new GUIContent("Pathfinding Type");
        GUIContent deleteButtonLabel = new GUIContent("-", "delete");

    serializedObject.Update();
        
        GUILayout.Label("Object to designate center of pathfinding grid", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enemyType);

        //GUILayout.Label(" ", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(currentPathType, pathTypeLabel);

        EnemyAI.EPathType type = (EnemyAI.EPathType)currentPathType.enumValueIndex;
        switch (type)
        {
            case EnemyAI.EPathType.patrol:

                GUILayout.Label(" ");
                GUILayout.Label("Patrol Points", EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                if (patrolPointObjects.isExpanded)
                {
                    for (int i = 0; i < patrolPointObjects.arraySize; i++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(patrolPointObjects.GetArrayElementAtIndex(i), GUIContent.none);
                        //Delete
                        if (GUILayout.Button(deleteButtonLabel, EditorStyles.miniButton, GUILayout.Width(20f)))
                        {                            
                            myScript.DeletePatrolPoint(i);
                            
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel -= 1;

                if (GUILayout.Button("Add Patrol Point"))
                {
                    myScript.AddPatrolPoint();
                }

                break;
        }       
        
        //End of Inspector GUI
        serializedObject.ApplyModifiedProperties();
    }
}
