using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TextData")]
public class TextData : ScriptableObject
{
    public int firstTextCount;
    public int answerCount;
    // Start phrases
    public List<string> preQuestionPerson;
    public List<string> preQuestionText;

    [System.Serializable]
    public class AnswerData
    {
        public int relationShipEffect;
        public string answerText;
        public int resultCount;

        public List<string> afterQuestionPerson;
        public List<string> afterQuestionText;
    }
    public List<AnswerData> answers;

}