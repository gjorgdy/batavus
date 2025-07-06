namespace Logic.Roll;

public record struct RollResponse(string[] ResultStrings, string[] CalculationStrings, int Total);