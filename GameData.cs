using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GameData
/// </summary>
/// 
public interface ITeam { }
public class GameData
{
    public DateTime Date { get; set; }
    public HomeTeam Home { get; set; }
    public VisTeam Visitor { get; set; }
    public string VisSpread { get; set; }
    public string HomeSpread { get; set; }
    public decimal OverUnder { get; set; }
    public decimal TrueOdds { get; set; }
    public decimal TrueTotal { get; set; }

}
//Yes I know this isn't optimal class structure but the stupid csvparser I used sucks, it doesn't use propertyattributes like a normal machine. 
//See CustomClassMap...it's awesome.  I've already <redacted> with it multiple times now.  Frustrating is csvhelper.
public class HomeTeam : ITeam
{
    public string HomeName { get; set; }
    public int HRushingYards { get; set; }
    public int HRushingAttempts { get; set; }
    public int HPassingYards { get; set; }
    public int HPassingAttempts { get; set; }
    public int HPassingCompletions { get; set; }
    public int HPenalties { get; set; }
    public int HPenaltyYards { get; set; }
    public int HFumblesLost { get; set; }
    public int HInterceptionsThrown { get; set; }
    public int HFirstDowns { get; set; }
    public int HThirdDAtt { get; set; }
    public int HThirdDComp { get; set; }

    public int HFourthDAtt { get; set; }

    public int HFourthDComp { get; set; }

    public int HTop { get; set; }

    public int HScore { get; set; }
    public decimal HSpread { get; set; }
    public decimal OverUnder { get; set; }
    public decimal HTrueOdds { get; set; }
    public decimal TrueTotal { get; set; }
}

public class VisTeam : ITeam
{
    public string VisName { get; set; }
    public int VRushingYards { get; set; }
    public int VRushingAttempts { get; set; }
    public int VPassingYards { get; set; }
    public int VPassingAttempts { get; set; }
    public int VPassingCompletions { get; set; }
    public int VPenalties { get; set; }
    public int VPenaltyYards { get; set; }
    public int VFumblesLost { get; set; }
    public int VInterceptionsThrown { get; set; }
    public int VFirstDowns { get; set; }
    public int VThirdDAtt { get; set; }
    public int VThirdDComp { get; set; }
    public int VFourthDAtt { get; set; }
    public int VFourthDComp { get; set; }
    public int VTop { get; set; }
    public int VScore { get; set; }
    public decimal VSpread { get; set; }
    public decimal OverUnder { get; set; }
    public decimal VTrueOdds { get; set; }
    public decimal TrueTotal { get; set; }
}

public class Rank
{
    public string mTeamName { get; set; }
    public double mRYA { get; set; }
    public double mAYA { get; set; }
    public double mIPA { get; set; }
    public double mTPAA { get; set; }
    public double mTE { get; set; }
    public decimal mTrueScore { get; set; }
}