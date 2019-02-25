using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionGroupSetter : MonoBehaviour
{
    public Toggle group1;
    public Toggle group2;
    public Toggle group3;

    public void Start()
    {
        //GAMBIARRA :D

        QuestionManager.SelectedGroup = QuestionManager.Group.Group_1;

        group1.onValueChanged.AddListener(x => SetGroup(x, QuestionManager.Group.Group_1));
        group2.onValueChanged.AddListener(x => SetGroup(x, QuestionManager.Group.Group_2));
        group3.onValueChanged.AddListener(x => SetGroup(x, QuestionManager.Group.Group_3));
    }

    private void SetGroup(bool isOn, QuestionManager.Group group)
    {
        if (!isOn)
            return;

        QuestionManager.SelectedGroup = group;
        print("group selected: " + QuestionManager.SelectedGroup.ToString());
    }
}
