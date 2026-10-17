using TheWorld.GameMechanics;
using TheWorld.Display;
using TheWorld.GameMechanics.StockEntities;
using TheWorld.Rules;

using static TheWorld.Rules.DiceRollStrategies;

// This is the one Random Number Generator for the whole program.  We inject it everywhere a roll happens.
Random rng = new();
// This is the one GameData for the whole program.  We inject it everywhere a new entity is created, so that it can be added to the game data.
GameData gameData = new();


// The party.  Bree is the wizard;  Tamsin is the fighter.
var bree = StockPlayers.Bree;
var tamsin = StockPlayers.Tamsin;

var player = PlayerBuilder
    .FromName("Kyroki")
    .WithHealthPoints(rng, Dice.D6 with { Count = 2, Modifier = 1 })
    .WithAbilities(rng, Dice.D20, ClampRoll(strategy: Advantage))
    .WithWallet(Money.Roll(rng, Dice.D100, Default))
    .Build();

// What they're up against.
var goblin = StockCreatures.Goblin;

var tree = ItemBuilder
    .FromName("Tree")
    .WithDescription("A big tree.  It has a lot of leaves.")
    .Build(gameData);

Console.WriteLine(tree?.Resolve(gameData)?.Examine() ?? "nothing to look at");

var forest = AreaBuilder
    .FromName("Forest")
    .WithDescription("A dark forest.  It has a lot of trees.")
    .WithItem(tree!) // TODO: That ! is a hack.  We need to do something better someday...
    .Build(gameData);

Console.WriteLine(forest?.Resolve(gameData)?.LookAround() ?? "nothing to look at");

player.ShowSheet();

bree.ShowSheet();
tamsin.ShowSheet();
goblin.ShowSheet();

Console.WriteLine($"Tamsin swings at the goblin:  {tamsin.AttackBonus:+0;-0;+0} to hit, against AC {goblin.ArmorClass}.");
Console.WriteLine($"The goblin swings back:  {goblin.AttackBonus:+0;-0;+0} to hit, against AC {tamsin.ArmorClass}.");
