using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextData))]
public class TextDataEditor : Editor
{
    private string text1, text2;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //base.OnInspectorGUI();

        TextData script = (TextData)target;

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Start Texts", GUILayout.Width(100));
        script.firstTextCount = EditorGUILayout.IntField(script.firstTextCount, GUILayout.Width(100));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Person", GUILayout.Width(100));
        GUILayout.Label("Text");

        EditorGUILayout.EndHorizontal();

        while (script.preQuestionPerson.Count < script.firstTextCount)
            script.preQuestionPerson.Add("");

        while (script.preQuestionText.Count < script.firstTextCount)
            script.preQuestionText.Add("");

        for (int i = 0; i < script.firstTextCount; i++)
        {
            EditorGUILayout.BeginHorizontal();

            script.preQuestionPerson[i] = GUILayout.TextArea(script.preQuestionPerson[i], GUILayout.Width(100));
            script.preQuestionText[i] = GUILayout.TextArea(script.preQuestionText[i], GUILayout.ExpandHeight(true));
            
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Answers", GUILayout.Width(100));
        script.answerCount = EditorGUILayout.IntField(script.answerCount, GUILayout.Width(100));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        //GUILayout.Label("Id", GUILayout.Width(20));
        GUILayout.Label("Relationship", GUILayout.Width(100));
        GUILayout.Label("Text");
        GUILayout.Label("Result");

        EditorGUILayout.EndHorizontal();

        while (script.answers.Count < script.answerCount)
            script.answers.Add(new TextData.AnswerData() { afterQuestionPerson = new System.Collections.Generic.List<string>(), afterQuestionText = new System.Collections.Generic.List<string>() });

        for (int i = 0; i < script.answerCount; i++)
        {
            EditorGUILayout.BeginHorizontal();

            //GUILayout.Label($" ", GUILayout.Width(20));
            script.answers[i].relationShipEffect = EditorGUILayout.IntField(script.answers[i].relationShipEffect, GUILayout.Width(100));
            script.answers[i].answerText = GUILayout.TextArea(script.answers[i].answerText, GUILayout.ExpandHeight(true));

            // EditorGUILayout.EndHorizontal();
            
            // EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Results", GUILayout.Width(100));
            script.answers[i].resultCount = EditorGUILayout.IntField(script.answers[i].resultCount, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();

            if (script.answers[i].afterQuestionPerson == null)
                script.answers[i].afterQuestionPerson = new System.Collections.Generic.List<string>();

            while (script.answers[i].afterQuestionPerson.Count < script.answers[i].resultCount)
                script.answers[i].afterQuestionPerson.Add("");

            if (script.answers[i].afterQuestionText == null)
                script.answers[i].afterQuestionText = new System.Collections.Generic.List<string>();

            while (script.answers[i].afterQuestionText.Count < script.answers[i].resultCount)
                script.answers[i].afterQuestionText.Add("");

            for (int g = 0; g < script.answers[i].resultCount; g++)
            {
                EditorGUILayout.BeginHorizontal();

                //GUILayout.Label($"{g + 1}", GUILayout.Width(20));
                script.answers[i].afterQuestionPerson[g] = GUILayout.TextArea(script.answers[i].afterQuestionPerson[g], GUILayout.Width(100));
                script.answers[i].afterQuestionText[g] = GUILayout.TextArea(script.answers[i].afterQuestionText[g], GUILayout.ExpandHeight(true));

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
        }

        serializedObject.ApplyModifiedProperties();
    }
}