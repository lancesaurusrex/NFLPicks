using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;

/// <summary>
/// Summary description for CustomClassMap
/// </summary>
public sealed class CustomClassMap : CsvClassMap<GameData>
{
    public CustomClassMap()
    {
        Map(m => m.Date).Name("Date");
        References<VisClassMap>(m => m.Visitor);
        References<HomeClassMap>(m => m.Home);
        Map(m => m.VisSpread).Ignore();
        Map(m => m.HomeSpread).Ignore();
        Map(m => m.OverUnder).Ignore();
        Map(m => m.TrueOdds).Ignore();
        Map(m => m.TrueTotal).Ignore();
    }
}
public sealed class VisClassMap : CsvClassMap<VisTeam>
{
    public VisClassMap()
    {
        Map(m => m.VisName).Name("VisTeam");
        Map(m => m.VRushingYards).Name("RushingYards");
        Map(m => m.VRushingAttempts).Name("RushingAttempts");
        Map(m => m.VPassingYards).Name("PassingYards");
        Map(m => m.VPassingAttempts).Name("PassingAttempts");
        Map(m => m.VPassingCompletions).Name("PassingCompletions");
        Map(m => m.VPenalties).Name("Penalties");
        Map(m => m.VPenaltyYards).Name("PenaltyYards");
        Map(m => m.VFumblesLost).Name("FumblesLost");
        Map(m => m.VInterceptionsThrown).Name("InterceptionsThrown");
        Map(m => m.VFirstDowns).Name("1stDowns");
        Map(m => m.VThirdDAtt).Name("3rdDownAttempts");
        Map(m => m.VThirdDComp).Name("3rdDownConversions");
        Map(m => m.VFourthDAtt).Name("4thDownAttempts");
        Map(m => m.VFourthDComp).Name("4thDownconversions");
        Map(m => m.VTop).Name("TimeofPossession");
        Map(m => m.VScore).Name("Score");
        Map(m => m.VSpread).Ignore();
        Map(m => m.OverUnder).Ignore();
        Map(m => m.VTrueOdds).Ignore();
        Map(m => m.TrueTotal).Ignore();
    }
}

public sealed class HomeClassMap : CsvClassMap<HomeTeam>
{
    public HomeClassMap()
    {
        Map(m => m.HomeName).Name("HomeTeam");
        Map(m => m.HRushingYards).Name("RushingYards").NameIndex(1);
        Map(m => m.HRushingAttempts).Name("RushingAttempts").NameIndex(1);
        Map(m => m.HPassingYards).Name("PassingYards").NameIndex(1);
        Map(m => m.HPassingAttempts).Name("PassingAttempts").NameIndex(1);
        Map(m => m.HPassingCompletions).Name("PassingCompletions").NameIndex(1);
        Map(m => m.HPenalties).Name("Penalties").NameIndex(1);
        Map(m => m.HPenaltyYards).Name("PenaltyYards").NameIndex(1);
        Map(m => m.HFumblesLost).Name("FumblesLost").NameIndex(1);
        Map(m => m.HInterceptionsThrown).Name("InterceptionsThrown").NameIndex(1);
        Map(m => m.HFirstDowns).Name("1stDowns").NameIndex(1);
        Map(m => m.HThirdDAtt).Name("3rdDownAttempts").NameIndex(1);
        Map(m => m.HThirdDComp).Name("3rdDownConversions").NameIndex(1);
        Map(m => m.HFourthDAtt).Name("4thDownAttempts").NameIndex(1);
        Map(m => m.HFourthDComp).Name("4thDownconversions").NameIndex(1);
        Map(m => m.HTop).Name("TimeofPossession").NameIndex(1);
        Map(m => m.HScore).Name("Score").NameIndex(1);
        Map(m => m.HSpread).Ignore();
        Map(m => m.OverUnder).Ignore();
        Map(m => m.HTrueOdds).Ignore();
        Map(m => m.TrueTotal).Ignore();
    }
}



