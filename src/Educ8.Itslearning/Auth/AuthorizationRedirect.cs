using System.Text.RegularExpressions;

namespace Educ8.Itslearning.Auth;

/// <summary>The parsed <c>itsl-itslearning://login/?state=…&amp;code=…</c> redirect at the end of a login.</summary>
public sealed partial record AuthorizationRedirect(string State, string Code)
{
    /// <summary>
    /// Parses the redirect URL (or any text containing it, e.g. pasted from a browser). Also reports an
    /// OAuth error carried in the URL. Throws <see cref="LoginException"/> if no code/state is found.
    /// </summary>
    public static AuthorizationRedirect Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        var query = text.Trim();
        var q = query.IndexOf('?');
        if (q >= 0) query = query[(q + 1)..];

        string? state = null, code = null, error = null;
        foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var eq = part.IndexOf('=');
            if (eq <= 0) continue;
            var key = part[..eq];
            var value = Uri.UnescapeDataString(part[(eq + 1)..].Trim().TrimEnd('\'', '"', ')', ';'));
            switch (key)
            {
                case "state": state = value; break;
                case "code": code = value; break;
                case "error": error = value; break;
            }
        }

        if (error is not null) throw new LoginException($"itslearning returned an error instead of a login code: {error}");
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            throw new LoginException("That doesn't look like an itslearning login redirect (expected a URL with 'state' and 'code').");
        if (!StateFormat().IsMatch(state))
            throw new LoginException("The login redirect has a malformed 'state' value.");
        return new AuthorizationRedirect(state, code);
    }

    /// <summary>States we generate are lowercase hex; anything else is rejected (also keeps file names safe).</summary>
    [GeneratedRegex("^[0-9a-f]{16,64}$")]
    internal static partial Regex StateFormat();
}
