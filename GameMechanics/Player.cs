namespace TheWorld.GameMechanics;

/// <summary>
/// A player character.  Everything a character sheet says, one property per
/// line.
///
/// The constructor is private, and there are two doors in.  The public Create
/// is Lesson 5's factory, which checks the numbers it's handed.  The internal
/// Create is for PlayerBuilder, and it trusts the builder to have done the
/// checking already.
/// </summary>
public record Player
{
    // TODO: Make Player Identifiable, and give it a Guid Id, just like Creature does.  Then we can make PlayerBuilder, and add players to the GameData.

    public string Name { get; }
    
    public PlayerLevel Level { get; }

    public HealthPoints HealthPoints { get; }

    public AbilityScore Abilities { get; }

    public Money MyWallet { get; }

    private Player(string name, 
        PlayerLevel level,
        HealthPoints healthPoints,
        AbilityScore abilities,
        Money myWallet)
    {
        Name = name;
        Level = level;
        HealthPoints = healthPoints;
        Abilities = abilities;
        MyWallet = myWallet;
    }
    
    internal static Player Create(string name, PlayerLevel level, HealthPoints healthPoints,
        AbilityScore abilities, Money myWallet) => new(name, level, healthPoints, abilities, myWallet);
    
    
    /// <summary>
    /// Builds a player, or refuses to.  Every player in the program comes
    /// through here.
    /// </summary>
    public static Player Create(string name,
        int strength, int dexterity, int constitution,
        int intelligence, int wisdom, int charisma)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A player needs a name.", nameof(name));

        var level = PlayerLevel.Level1;

        var healthPoints = HealthPoints.Create(20);

        var stats = AbilityScore.Create(
            strength, dexterity, constitution, intelligence, wisdom, charisma)
                    ?? AbilityScore.Create(
                        10,10, 10, 
                        10, 10, 10)!;

        var gold = Money.Default;

        return new Player(name, level, healthPoints, stats, gold);
    }
}
