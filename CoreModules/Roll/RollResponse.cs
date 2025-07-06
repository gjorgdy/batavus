namespace CoreModules.Roll;

public record struct RollResponse(string[] ResultStrings, string[] CalculationStrings, int Total);