namespace TheWorld.GameMechanics;

/// <summary>
/// Anything the players fight.  A goblin, a wolf, a dragon.
///
/// Made of parts, the same as <see cref="Player"/>.  The constructor is
/// private, and there are two doors in.  The public Create is Lesson 5's
/// factory, which checks the numbers it's handed.  The internal Create is for
/// CreatureBuilder, and it trusts the builder to have done the checking
/// already.
/// </summary>
public record Creature : Identifiable
{
    /// <summary>
    /// This is a unique identifier for the creature.  It is pure data, and is not displayed ever.
    /// It is used to identify the creature in the Game Engine.
    /// TODO:  Guid.NewGuid() is a placeholder!  Every Creature gets a brand-new identity each time it's built,
    /// so the same Creature comes back with a different Id every run.  Serialization will need to re-create
    /// a Creature from the identity it was saved with.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; }

    public HealthPoints HealthPoints { get; }

    public AbilityScore Abilities { get; }

    /// <summary>TODO: This is probably an object in its own rite. Experience the party earns for defeating it.</summary>
    public int ExperienceReward { get; }

    private Creature(string name,
        HealthPoints healthPoints,
        AbilityScore abilities,
        int experienceReward)
    {
        Name = name;
        HealthPoints = healthPoints;
        Abilities = abilities;
        ExperienceReward = experienceReward;
    }

    internal static Creature Create(string name, HealthPoints healthPoints,
        AbilityScore abilities, int experienceReward) => new(name, healthPoints, abilities, experienceReward);

    /// <summary>
    /// Builds a creature, or refuses to.  Every creature in the program comes
    /// through here.
    /// </summary>
    public static Creature Create(string name,
        int maxHealth,
        int strength, int dexterity, int constitution,
        int intelligence, int wisdom, int charisma,
        int experienceReward)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A creature needs a name.", nameof(name));

        var healthPoints = HealthPoints.Create(maxHealth);

        var abilities = AbilityScore.Create(
            strength, dexterity, constitution, intelligence, wisdom, charisma)
                    ?? AbilityScore.Create(
                        10, 10, 10,
                        10, 10, 10)!;

        if (experienceReward < 0)
            throw new ArgumentOutOfRangeException(
                nameof(experienceReward), experienceReward,
                "Experience reward can't be negative.");

        return new Creature(name, healthPoints, abilities, experienceReward);
    }
}
