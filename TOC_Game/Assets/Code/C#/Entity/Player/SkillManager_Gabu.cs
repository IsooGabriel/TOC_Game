﻿using System.Collections.Generic;
using UnityEngine;
using static DBManager_Gabu;

public class SkillManager_Gabu : MonoBehaviour
{
    private List<UpGrade_Gabu> skills = new List<UpGrade_Gabu>();

    public void SetSkills()
    {
        for(int i = 0; i < DB.baseUpGrageDBs.Length; i++)
        {
            if(DB.playerDBs[DB.AccountID].baseUpGrages[i])
            {
                AddSkill(DB.baseUpGrageDBs[i]);
            }
        }
    }

    public void UseAllSkills(Player_Gabu player)
    {
        foreach (var skill in skills)
        {
            skill.Execute(player);
        }
    }

    public void AddSkill(UpGrade_Gabu skill)
    {
        skills.Add(skill);
        skills.Sort((a, b) => a.priority.CompareTo(b.priority));
    }

    public void UseSkill(int skillID, Player_Gabu player)
    {
        if (skillID >= 0 && skillID < skills.Count)
        {
            skills[skillID].Execute(player);
        }
    }
}
