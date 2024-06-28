using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"ApplyTaxWithIfs(50, true) = {ApplyTaxWithIfs(50, true)}");
        Console.WriteLine($"ApplyTaxWithIfs(50, false) = {ApplyTaxWithIfs(50, false)}");
        Console.WriteLine($"ApplyTaxWithInlineIfs(50, true) = {ApplyTaxWithInlineIfs(50, true)}");
        Console.WriteLine($"ApplyTaxWithInlineIfs(50, false) = {ApplyTaxWithInlineIfs(50, false)}");
        Console.WriteLine($"ApplyTaxWithoutIfsEnum(50, true) = {ApplyTaxWithoutIfsEnum(50, Bool.TRUE)}");
        Console.WriteLine($"ApplyTaxWithoutIfsEnum(50, false) = {ApplyTaxWithoutIfsEnum(50, Bool.FALSE)}");
        Console.WriteLine($"ApplyTaxWithoutIfsInt(50, true) = {ApplyTaxWithoutIfsInt(50, 1)}");
        Console.WriteLine($"ApplyTaxWithoutIfsInt(50, false) = {ApplyTaxWithoutIfsInt(50, 0)}");

        int numCases = 10000000;
        int numRuns = 100;
        IDictionary<string, long> functionRuntimes = new Dictionary<string, long>
        {
            { nameof(ApplyTaxWithIfs), 0 },
            { nameof(ApplyTaxWithInlineIfs), 0 },
            { nameof(ApplyTaxWithoutIfsEnum), 0 },
            { nameof(ApplyTaxWithoutIfsInt), 0 }
        };

        for (int i = 0; i < numRuns; i++)
        {
            functionRuntimes[nameof(ApplyTaxWithIfs)] += RunTestCase(numCases, () => ApplyTaxWithIfs(50, true));
            functionRuntimes[nameof(ApplyTaxWithIfs)] += RunTestCase(numCases, () => ApplyTaxWithIfs(50, false));
            functionRuntimes[nameof(ApplyTaxWithInlineIfs)] += RunTestCase(numCases, () => ApplyTaxWithInlineIfs(50, true));
            functionRuntimes[nameof(ApplyTaxWithInlineIfs)] += RunTestCase(numCases, () => ApplyTaxWithInlineIfs(50, false));
            functionRuntimes[nameof(ApplyTaxWithoutIfsEnum)] += RunTestCase(numCases, () => ApplyTaxWithoutIfsEnum(50, Bool.TRUE));
            functionRuntimes[nameof(ApplyTaxWithoutIfsEnum)] += RunTestCase(numCases, () => ApplyTaxWithoutIfsEnum(50, Bool.FALSE));
            functionRuntimes[nameof(ApplyTaxWithoutIfsInt)] += RunTestCase(numCases, () => ApplyTaxWithoutIfsInt(50, 1));
            functionRuntimes[nameof(ApplyTaxWithoutIfsInt)] += RunTestCase(numCases, () => ApplyTaxWithoutIfsInt(50, 0));
        }

        foreach (var key in functionRuntimes.Keys)
        {
            Console.WriteLine($"{key}: Average runtime: {(float)functionRuntimes[key] / (numRuns*numCases)}ms");
        }

        /*
         * Results:
ApplyTaxWithIfs(50, true) = 57.500000298023224
ApplyTaxWithIfs(50, false) = 75
ApplyTaxWithInlineIfs(50, true) = 57.500000298023224
ApplyTaxWithInlineIfs(50, false) = 75
ApplyTaxWithoutIfsEnum(50, true) = 57.500000298023224
ApplyTaxWithoutIfsEnum(50, false) = 75
ApplyTaxWithoutIfsInt(50, true) = 57.500000298023224
ApplyTaxWithoutIfsInt(50, false) = 75
ApplyTaxWithIfs: Average runtime: 1.752E-05ms
ApplyTaxWithInlineIfs: Average runtime: 2.3185E-05ms
ApplyTaxWithoutIfsEnum: Average runtime: 1.8497E-05ms
ApplyTaxWithoutIfsInt: Average runtime: 1.8457E-05ms
         */
    }

    private static long RunTestCase(int numCases, Action toRun)
    {
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < numCases; i++)
        {
            toRun();
        }

        sw.Stop();

        return sw.ElapsedMilliseconds;
    }

    private static double ApplyTaxWithIfs(double amt, bool apply)
    {
        if (apply)
        {
            return amt + (amt * 0.15f);
        }
        else
        {
            return amt + (amt * 0.5f);
        }
    }

    private static double ApplyTaxWithInlineIfs(double amt, bool apply)
    {
        return amt + (amt * (apply ? 0.15f : 0.5));
    }

    private static double ApplyTaxWithoutIfsEnum(double amt, Bool apply)
    {
        return amt + (amt * (0.15f * (int)apply)) + (amt * (0.5f * (1 - (int)apply)));
    }

    private static double ApplyTaxWithoutIfsInt(double amt, int apply)
    {
        return amt + (amt * (0.15f * apply)) + (amt * (0.5f * (1 - apply)));
    }
}

public enum Bool
{
    TRUE = 1,
    FALSE = 0,
}