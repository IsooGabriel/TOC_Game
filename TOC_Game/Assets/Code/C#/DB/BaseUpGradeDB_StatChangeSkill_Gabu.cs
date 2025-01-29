using UnityEngine;
using static DBManager_Gabu;

[CreateAssetMenu(menuName = "CreateData/BaseUpGrage")]
public class BaseUpGradeDB_StatChangeSkill_Gabu : UpGrade_Gabu
{

    #region 関数

    // 各関数の実装
    private void HpAdd(Player_Gabu player, long value) { player.maxHP += value; }
    private void HpMultiply(Player_Gabu player, long value) { player.maxHP *= value; }
    private void HpSubtract(Player_Gabu player, long value) { player.maxHP -= value; }
    private void HpDivide(Player_Gabu player, long value) { player.maxHP /= value; }

    private void AtkAdd(Player_Gabu player, long value) { player.atk += value; }
    private void AtkMultiply(Player_Gabu player, long value) { player.atk *= value; }
    private void AtkSubtract(Player_Gabu player, long value) { player.atk -= value; }
    private void AtkDivide(Player_Gabu player, long value) { player.atk /= value; }

    private void AtkSpeedAdd(Player_Gabu player, float value) { player.atkSpeed += value; }
    private void AtkSpeedMultiply(Player_Gabu player, float value) { player.atkSpeed *= value; }
    private void AtkSpeedSubtract(Player_Gabu player, float value) { player.atkSpeed -= value; }
    private void AtkSpeedDivide(Player_Gabu player, float value) { player.atkSpeed /= value; }

    private void SpeedAdd(Player_Gabu player, float value) { player.speed += value; }
    private void SpeedMultiply(Player_Gabu player, float value) { player.speed *= value; }
    private void SpeedSubtract(Player_Gabu player, float value) { player.speed -= value; }
    private void SpeedDivide(Player_Gabu player, float value) { player.speed /= value; }

    private void DefenseAdd(Player_Gabu player, float value) { player.defense += value; }
    private void DefenseMultiply(Player_Gabu player, float value) { player.defense *= value; }
    private void DefenseSubtract(Player_Gabu player, float value) { player.defense -= value; }
    private void DefenseDivide(Player_Gabu player, float value) { player.defense /= value; }

    private void RerollSpeedAdd(Player_Gabu player, float value) { player.rerollSpeed += value; }
    private void RerollSpeedMultiply(Player_Gabu player, float value) { player.rerollSpeed *= value; }
    private void RerollSpeedSubtract(Player_Gabu player, float value) { player.rerollSpeed -= value; }
    private void RerollSpeedDivide(Player_Gabu player, float value) { player.rerollSpeed /= value; }

    private void LevelAdd(Player_Gabu player, int value) { player.level += value; }
    private void LevelMultiply(Player_Gabu player, int value) { player.level *= value; }
    private void LevelSubtract(Player_Gabu player, int value) { player.level -= value; }
    private void LevelDivide(Player_Gabu player, int value) { player.level /= value; }

    private void BuffAdd(Player_Gabu player, float value) { player.Buff += value; }
    private void BuffMultiply(Player_Gabu player, float value) { player.Buff *= value; }
    private void BuffSubtract(Player_Gabu player, float value) { player.Buff -= value; }
    private void BuffDivide(Player_Gabu player, float value) { player.Buff /= value; }

    private void CriticalChanceAdd(Player_Gabu player, float value) { player.criticalChance += value; }
    private void CriticalChanceMultiply(Player_Gabu player, float value) { player.criticalChance *= value; }

    private void CriticalDamageAdd(Player_Gabu player, float value) { player.criticalDamage += value; }
    private void CriticalDamageMultiply(Player_Gabu player, float value) { player.criticalDamage *= value; }

    #endregion

    public override void Execute(Player_Gabu player)
    {
        // ステータス変動を優先度順にソート
        System.Array.Sort(fluctuateStats, (a, b) => a.priority.CompareTo(b.priority));

        foreach (var fluctuateStat in fluctuateStats)
        {
            switch (fluctuateStat.fluctuateStats)
            {
                case E_FLUCTUATE_STATS.HP_ADD:
                    HpAdd(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.HP_MULTIPLY:
                    HpMultiply(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.HP_SUBTRACT:
                    HpSubtract(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.HP_DIVIDE:
                    HpDivide(player, (long)fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.ATK_ADD:
                    AtkAdd(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_MULTIPLY:
                    AtkMultiply(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_SUBTRACT:
                    AtkSubtract(player, (long)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_DIVIDE:
                    AtkDivide(player, (long)fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.ATK_SPEED_ADD:
                    AtkSpeedAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_SPEED_MULTIPLY:
                    AtkSpeedMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_SPEED_SUBTRACT:
                    AtkSpeedSubtract(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.ATK_SPEED_DIVIDE:
                    AtkSpeedDivide(player, fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.SPEED_ADD:
                    SpeedAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.SPEED_MULTIPLY:
                    SpeedMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.SPEED_SUBTRACT:
                    SpeedSubtract(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.SPEED_DIVIDE:
                    SpeedDivide(player, fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.DEFENSE_ADD:
                    DefenseAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.DEFENSE_MULTIPLY:
                    DefenseMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.DEFENSE_SUBTRACT:
                    DefenseSubtract(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.DEFENSE_DIVIDE:
                    DefenseDivide(player, fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.REROLL_SPEED_ADD:
                    RerollSpeedAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.REROLL_SPEED_MULTIPLY:
                    RerollSpeedMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.REROLL_SPEED_SUBTRACT:
                    RerollSpeedSubtract(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.REROLL_SPEED_DIVIDE:
                    RerollSpeedDivide(player, fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.LEVEL_ADD:
                    LevelAdd(player, (int)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.LEVEL_MULTIPLY:
                    LevelMultiply(player, (int)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.LEVEL_SUBTRACT:
                    LevelSubtract(player, (int)fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.LEVEL_DIVIDE:
                    LevelDivide(player, (int)fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.BUFF_ADD:
                    BuffAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.BUFF_MULTIPLY:
                    BuffMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.BUFF_SUBTRACT:
                    BuffSubtract(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.BUFF_DIVIDE:
                    BuffDivide(player, fluctuateStat.value);
                    break;

                case E_FLUCTUATE_STATS.CRITICAL_CHANCE_ADD:
                    CriticalChanceAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.CRITICAL_CHANCE_MULTIPLY:
                    CriticalChanceMultiply(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.CRITICAL_DAMAGE_ADD:
                    CriticalDamageAdd(player, fluctuateStat.value);
                    break;
                case E_FLUCTUATE_STATS.CRITICAL_DAMAGE_MULTIPLY:
                    CriticalDamageMultiply(player, fluctuateStat.value);
                    break;

                default:
                    Debug.LogWarning($"未対応のフラグ: {fluctuateStat.fluctuateStats}");
                    break;
            }
        }
    }

}