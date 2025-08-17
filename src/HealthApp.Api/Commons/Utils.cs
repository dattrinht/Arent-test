using System.Globalization;

namespace HealthApp.Api.Commons;

public static class Utils
{
    public static bool TryParseYearMonth(string ym, out DateOnly month)
    {
        return DateOnly.TryParseExact(
            ym + "-01",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out month
        );
    }
}
