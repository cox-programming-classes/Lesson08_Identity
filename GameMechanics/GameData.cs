namespace TheWorld.GameMechanics;

/// <summary>
/// All the data in the game!  
/// This is the one place where all the Areas, Items, and Creatures are stored.  
/// It is injected into every builder, so that they can add their new creations to the game data.
/// 
/// TODO: we need to add Players here too, so that we can save and load the game state eventually.
/// </summary>
public record GameData
{
    /// <summary>
    /// All the Area's in the game!
    /// </summary>
    private Dictionary<Guid,Area> Areas { get; init; } = [];

    /// <summary>
    /// All the Item's in the game!
    /// </summary>
    private Dictionary<Guid, Item> Items { get; init; } = [];

    /// <summary>
    /// All the Creature's in the game!
    /// </summary>
    private Dictionary<Guid, Creature> Creatures { get; init; } = [];

    //--------------------------------------------------------------
    //  Methods for adding and retrieving Areas, Items, and Creatures.
    // TODO:  This is a very simple implementation.  It does not handle duplicates, and it does not handle errors.  It is just a starting point.
    //--------------------------------------------------------------

    public bool TryAddArea(Area area) => Areas.TryAdd(area.Id, area);
    public bool TryAddItem(Item item) => Items.TryAdd(item.Id, item);
    public bool TryAddCreature(Creature creature) => Creatures.TryAdd(creature.Id, creature);

    //--------------------------------------------------------------
    //  Methods for retrieving Areas, Items, and Creatures by their Id.
    // TODO:  This is a very simple implementation.  It does not handle errors.  It is just a starting point.
    //--------------------------------------------------------------
    public Area? GetArea(Guid id) => Areas.GetValueOrDefault(id);
    public Item? GetItem(Guid id) => Items.GetValueOrDefault(id);
    public Creature? GetCreature(Guid id) => Creatures.GetValueOrDefault(id);
}

/// <summary>
/// An Identifiable is a thing that has a unique identifier.  It is pure data, and is not displayed ever.
/// </summary>
public interface Identifiable
{
    public Guid Id { get; }
}

/// <summary>
/// A ReferenceLabel is a thing that has a label and a unique identifier.  It is pure data, and is not displayed ever.
/// It is a reference to an Identifiable, and can be used to resolve the Identifiable from the GameData.
/// 
/// This structure is useful for saving and loading game data, as it allows us to store a reference to an Identifiable without storing the entire object.
/// using an Abstract Record in this way allows us to have a single structure that can be used for any Identifiable, 
/// and we can create specific ReferenceLabel's for each type of Identifiable.
/// 
/// This is called a Union Type, and is a common pattern in functional programming.  
/// It allows us to have a single type that can be one of many types, and we can use pattern matching to determine which type it is.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record ReferenceLabel<T> where T : class, Identifiable
{
    public required string Label { get; init; }

    public required Guid Id { get; init; }

    public abstract T? Resolve(GameData gameData);
}

/// <summary>
/// A ReferenceLabel for an Area.  It uses the human readable label to map to a particular Area in the GameData.
/// </summary>
public record AreaReferenceLabel : ReferenceLabel<Area>
{
    public override Area? Resolve(GameData gameData) => gameData.GetArea(Id);
}

/// <summary>
/// A ReferenceLabel for an Item.  It uses the human readable label to map to a particular Item in the GameData.
/// </summary>
public record ItemReferenceLabel : ReferenceLabel<Item>
{
    public override Item? Resolve(GameData gameData) => gameData.GetItem(Id);
}

/// <summary>
/// A ReferenceLabel for a Creature.  It uses the human readable label to map to a particular Creature in the GameData.
/// </summary>
public record CreatureReferenceLabel : ReferenceLabel<Creature>
{
    public override Creature? Resolve(GameData gameData) => gameData.GetCreature(Id);
}