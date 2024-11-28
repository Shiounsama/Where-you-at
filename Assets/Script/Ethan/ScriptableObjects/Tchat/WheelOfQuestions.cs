using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/WheelGeneralQuestion")]
public class WheelOfQuestions : ScriptableObject    
{
    public List<QuestionWheel> listOfQuestion;
    public List<int> listOfQuestionUsed;

    public string GetQuestion()
    {
        if (listOfQuestion.Count > 0)
        {
            int x = Random.Range(0, listOfQuestion.Count);
            while (listOfQuestionUsed.Contains(x))
            {
                x = Random.Range(0, listOfQuestion.Count);
                if (listOfQuestionUsed.Count >= listOfQuestion.Count)
                {
                    return listOfQuestion[0].QuestionText;
                }
            }
            listOfQuestionUsed.Add(x);
            if (listOfQuestionUsed.Count <= listOfQuestion.Count)
            {
                return listOfQuestion[x].QuestionText;
            }
        }
        return listOfQuestion[0].QuestionText;
    }
}
