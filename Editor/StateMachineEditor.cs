using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using static UnityEditor.VersionControl.Asset;

namespace StateMachine
{
    [CustomEditor(typeof(StateMachine))]
    [CanEditMultipleObjects]
    public class StateMachineEditor : Editor
    {
        SerializedProperty container;
        SerializedProperty states;
        SerializedProperty choser;
        SerializedProperty isDefault;

        GUIStyle addStyle;
        Texture2D addTexture;
        Texture2D addHoverTexture;
        Texture2D addClickTexture;

        GUIStyle removeStyle;
        Texture2D removeTexture;
        Texture2D removeHoverTexture;
        Texture2D removeClickTexture;

        GUIStyle stateStyle;
        Texture2D stateHover;
        Texture2D stateSelected;
        Texture2D stateNormal;
        Texture2D stateActive;

        GUIStyle selectionBoxStyle;
        Texture2D selectionBoxTexture;

        bool isSelectedFrame = false;

        int selectedIndex = -1;

        void OnEnable()
        {
            Color statesColor;
            Color stateHoverColor;
            Color stateSelectedColor;
            Color stateActiveColor = Color.green;
            ColorUtility.TryParseHtmlString("#404040", out statesColor);
            ColorUtility.TryParseHtmlString("#545454", out stateHoverColor);
            ColorUtility.TryParseHtmlString("#363636", out stateSelectedColor);

            stateHover = new Texture2D(1, 1);
            stateSelected = new Texture2D(1, 1);
            stateNormal = new Texture2D(1, 1);
            stateActive = new Texture2D(1, 1);

            addTexture = Resources.Load<Texture2D>("NormalAdd");
            removeTexture = Resources.Load<Texture2D>("NormalRemove");

            addHoverTexture = Resources.Load<Texture2D>("HoverAdd");
            removeHoverTexture = Resources.Load<Texture2D>("HoverRemove");

            addClickTexture = Resources.Load<Texture2D>("ClickAdd");
            removeClickTexture = Resources.Load<Texture2D>("ClickRemove");

            selectionBoxTexture = Resources.Load<Texture2D>("SelectionBox");

            stateHover.SetPixels(new Color[] { stateHoverColor });
            stateHover.Apply();

            stateSelected.SetPixels(new Color[] { stateSelectedColor });
            stateSelected.Apply();

            stateNormal.SetPixels(new Color[] { statesColor });
            stateNormal.Apply();

            stateActive.SetPixels(new Color[] { stateActiveColor });
            stateActive.Apply();

            addStyle = new GUIStyle();
            addStyle.normal.background = addTexture;
            addStyle.hover.background = addHoverTexture;
            addStyle.active.background = addClickTexture;

            removeStyle = new GUIStyle();
            removeStyle.normal.background = removeTexture;
            removeStyle.hover.background = removeHoverTexture;
            removeStyle.active.background = removeClickTexture;

            stateStyle = new GUIStyle();
            stateStyle.hover.background = stateHover;

            selectionBoxStyle = new GUIStyle();
            selectionBoxStyle.normal.background = selectionBoxTexture;
        }

        public override void OnInspectorGUI()
        {
            isSelectedFrame = false;

            container = serializedObject.FindProperty("parameterContainer");
            states = serializedObject.FindProperty("states");
            choser = serializedObject.FindProperty("stateChoser");

            serializedObject.Update();
            EditorGUILayout.PropertyField(container);

            int arrayLength = states.arraySize;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("States"));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            float width = EditorGUIUtility.currentViewWidth - 60;
            float totalElementHeight = 22.5f * arrayLength;
            float selectionBoxWidth = 26;
            float selectionBoxHeight = 16;
            float isDefaultWidth = 32;
            float elementWidth = (width - selectionBoxWidth - isDefaultWidth) / 3.0f - 7.0f;

            GUILayout.BeginArea(new Rect(30, 45, width, 15.0f + totalElementHeight + 15.0f), EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("", GUILayout.Width(selectionBoxWidth));
            GUILayout.Label("Name", GUILayout.Width(elementWidth));
            GUILayout.Label("Conditioner", GUILayout.Width(elementWidth));
            GUILayout.Label("Executer", GUILayout.Width(elementWidth));
            GUILayout.Label("D", GUILayout.Width(isDefaultWidth));

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < arrayLength; i++)
            {
                SerializedProperty p = states.GetArrayElementAtIndex(i);

                if (p.objectReferenceValue != null)
                {
                    SerializedObject o = new SerializedObject(p.objectReferenceValue);

                    o.Update();

                    SerializedProperty conditioner = o.FindProperty("conditioner");
                    SerializedProperty executer = o.FindProperty("executer");
                    SerializedProperty name = o.FindProperty("name");
                    SerializedProperty isDefault = o.FindProperty("isDefault");
                    SerializedProperty isActive = o.FindProperty("isActive");



                    if (selectedIndex == i)
                    {
                        stateStyle.normal.background = stateSelected;
                        stateStyle.hover.background = stateSelected;
                    }
                    else
                    {
                        if (isActive.boolValue == true)
                            stateStyle.normal.background = stateActive;
                        else
                            stateStyle.normal.background = stateNormal;

                        stateStyle.hover.background = stateHover;
                    }

                    EditorGUILayout.BeginHorizontal(stateStyle, GUILayout.Height(20));

                    bool dValue = isDefault.boolValue;

                    EditorGUILayout.LabelField("", selectionBoxStyle, GUILayout.Width(selectionBoxWidth), GUILayout.Height(selectionBoxHeight));
                    EditorGUILayout.PropertyField(name, GUIContent.none, GUILayout.Width(elementWidth));
                    EditorGUILayout.PropertyField(conditioner, GUIContent.none, GUILayout.Width(elementWidth));
                    EditorGUILayout.PropertyField(executer, GUIContent.none, GUILayout.Width(elementWidth));
                    EditorGUILayout.PropertyField(isDefault, GUIContent.none, GUILayout.Width(isDefaultWidth));

                    if (dValue != isDefault.boolValue)
                    {
                        for (int j = 0; j < arrayLength; j++)
                        {
                            if (j != i)
                            {
                                SerializedObject oD = new SerializedObject(states.GetArrayElementAtIndex(j).objectReferenceValue);

                                oD.Update();

                                SerializedProperty d = oD.FindProperty("isDefault");
                                d.boolValue = false;

                                oD.ApplyModifiedProperties();
                            }     
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    OnClick(i);

                    o.ApplyModifiedProperties();
                }
            }

            GUILayout.EndArea();

            EditorGUILayout.Space(10.0f + totalElementHeight);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(width - 64));

            bool isAdd = GUILayout.Button(new GUIContent(""), addStyle, GUILayout.Width(32));
            bool isRemove = GUILayout.Button(new GUIContent(""), removeStyle, GUILayout.Width(32));

            EditorGUILayout.EndHorizontal();

            if (isAdd)
            {
                states.InsertArrayElementAtIndex(arrayLength);
                states.GetArrayElementAtIndex(arrayLength).objectReferenceValue = CreateInstance<State>();
            }

            if (isRemove)
            {
                if (selectedIndex != -1)
                {
                    states.DeleteArrayElementAtIndex(selectedIndex);
                    selectedIndex = -1;
                }
                else if (arrayLength > 0)
                    states.DeleteArrayElementAtIndex(arrayLength - 1);
            }

            EditorGUILayout.PropertyField(choser);

            serializedObject.ApplyModifiedProperties();
            CheckIfResetSelected();
        }

        void OnClick(int index)
        {
            EventType eventType = Event.current.type;
            
            if (eventType == EventType.MouseDown) 
            {
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    isSelectedFrame = true;
                    selectedIndex = index;
                }
            }
        }

        void CheckIfResetSelected()
        {
            EventType eventType = Event.current.type;

            if (eventType == EventType.MouseDown)
            {
                if (isSelectedFrame == false) { selectedIndex = -1; }
            }
        }

        public override bool RequiresConstantRepaint() => true;
    }
}
