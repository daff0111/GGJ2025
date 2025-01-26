using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Bubblemon
{
    [SerializeField] BubblemonBase _base;
    [SerializeField] int level;

    public BubblemonBase Base { 
        get {
            return _base;
        }
    }
    public int Level { 
        get {
            return level;
        }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = MaxHp;

         // Asegúrate de que Moves no sea null antes de agregar los movimientos
        Moves = new List<Move>();

        // Generate Moves
        //Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
        // Si no se añadieron movimientos, se puede agregar un movimiento predeterminado, si lo deseas
        if (Moves.Count == 0)
        {
            Debug.LogWarning("Bubblemon has no moves available.");
        }
    }

    public int Attack {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defense {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }

    public int SpAttack {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }

    public int SpDefense {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }

    public int Speed {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }

    public int MaxHp {
        get {
            return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10;
        }
    }

    public DamageDetails TakeDamage(Move move, Bubblemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        if (Moves.Count == 0)
        {
            Debug.LogError("No moves available for this Bubblemon.");
            return null; // O un movimiento predeterminado si lo deseas
        }
        // Si hay movimientos disponibles, devuelve uno aleatorio
        //int randomIndex = Random.Range(0, Moves.Count);
        //return Moves[randomIndex];
        return Moves = Random.Range(0, Moves.Count);
        //return Moves[r];
    }
}

public class DamageDetails 
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
