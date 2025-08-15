using IdGen;

namespace HealthApp.Application.Helpers;

public static class IdGenHelper
{
    private static readonly IdGenerator _generator;

    static IdGenHelper()
    {
        var epoch = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var structure = new IdStructure(timestampBits: 41, generatorIdBits: 10, sequenceBits: 12);
        var timeSource = new DefaultTimeSource(epoch, TimeSpan.FromMilliseconds(1));
        var options = new IdGeneratorOptions(structure, timeSource);
        _generator = new IdGenerator(generatorId: 1, options);
    }

    public static long CreateId()
    {
        return _generator.CreateId();
    }
}
