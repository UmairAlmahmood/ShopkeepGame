using System.Collections.Generic;
using System;
using Godot;

public static class Dialogue {
    public static List<String> greetings = new List<String>{"Hello there", "I'm looking for powerful objects", "I've heard that you're the best seller for the budget", "Hi"};
    public static Dictionary<PlayerTypes, String> greetingsDict = new Dictionary<PlayerTypes, String>{ 
        [new PlayerTypes(Personality.Jolly, SpecialTrait.VsUndead)] = "Cloudy skies and mists rolling through the forest. Looks like a perfect day to purge some zombies! Hahaha. Do you have anything that may aid me in my endeavors?",
        [new PlayerTypes(Personality.Jolly, SpecialTrait.VsGoblins)] = "Good news! I stumbled upon the camp of the goblins that have been raiding the town. I was wondering if you had any tools that I could use to put an end to their pillaging.",
        [new PlayerTypes(Personality.Jolly, SpecialTrait.Dungeoneer)] = "Hello there! I’ve come to this peaceful town to recuperate after delving into several dungeons. I thought I’d see if you carried any gear I could use in the field.",
        [new PlayerTypes(Personality.Jolly, SpecialTrait.Diver)] = "Salutations, friend! It’s good to be back on land for a change! Alas, I’ll be back to fighting those damn mer-raiders soon enough, as per the King’s orders. Say, any chance you’ve got something I might find of use?",
        [new PlayerTypes(Personality.Jolly, SpecialTrait.CurseHunter)] = "Greetings honest shopkeep! I’ve heard rumors that you carry cursed items. Do you have any in stock?",
        [new PlayerTypes(Personality.Cheapskate, SpecialTrait.VsUndead)] = "Hey shopkeep. I’m looking for gear but the last shopkeep I went to tried to sell me a rusty old sword for $50. I don’t want to see any funny business.",
        [new PlayerTypes(Personality.Cheapskate, SpecialTrait.VsGoblins)] = "Hey shopkeep. I’m looking for gear but the last shopkeep I went to tried to sell me a rusty old sword for $50. I don’t want to see any funny business.",
        [new PlayerTypes(Personality.Cheapskate, SpecialTrait.Dungeoneer)] = "Hey shopkeep. I’m looking for gear but the last shopkeep I went to tried to sell me a rusty old sword for $50. I don’t want to see any funny business.",
        [new PlayerTypes(Personality.Cheapskate, SpecialTrait.Diver)] = "Hey shopkeep. I’m looking for gear but the last shopkeep I went to tried to sell me a rusty old sword for $50. I don’t want to see any funny business.",
        [new PlayerTypes(Personality.Cheapskate, SpecialTrait.VsDragon)] = "Hey shopkeep. I’m looking for gear but the last shopkeep I went to tried to sell me a rusty old sword for $50. I don’t want to see any funny business.",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.VsUndead)] = "I found a whole goblin tribe in the forest. I think I can handle it myself if I just find the right weapon. What’ve you got, shopkeep?",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.VsGoblins)] = "I found a whole goblin tribe in the forest. I think I can handle it myself if I just find the right weapon. What’ve you got, shopkeep?",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.Dungeoneer)] = "I found the entrance to an ancient temple. It’s littered with traps and danger I think I can handle it myself if I just find the right gear. What’ve you got, shopkeep?",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.VsDragon)] = "I found the lair of a dragon up in the mountains. I think I can handle it myself if I can just find the right weapon. What’ve you got shopkeep?",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.Dungeoneer)] = "I found a whole goblin tribe in the forest. I think I can handle it myself if I just find the right weapon. What’ve you got, shopkeep?",
        [new PlayerTypes(Personality.Foolhardy, SpecialTrait.Diver)] = "I found an underwater city that spans miles. I plan to comb through the ruins myself but I need a weapon. What’ve you got shopkeep?",
        [new PlayerTypes(Personality.Cowardly, SpecialTrait.VsUndead)] = "I was wandering in the dark spooky forest *shudder* and I found a hoard of zombies out in the forest… but I got scared and ran off. I’m sure if I had just the right weapon to boost my confidence, I could slay them.",
        [new PlayerTypes(Personality.Cowardly, SpecialTrait.Diver)] = "I was out diving when I found a creepy shrine on the sea floor. I heard something swimming around down there so I swam up to the surface as fast as I could. I too curious not to go back. Do you have anything I could use to defend myself?",
        [new PlayerTypes(Personality.Cowardly, SpecialTrait.VsDragon)] = "Hello there, shopkeep. Uh, it’s me… the famous dragon slayer. I was wondering if you had anything I could use to slay the dragon… and not be slain myself… *wipes sweat from forehead",
        [new PlayerTypes(Personality.Cowardly, SpecialTrait.Dungeoneer)] = "“I was hired to lead a team into an old castle. I’ve heard nobody has ever gone in and returned… *gulp* Do you have anything that might prove useful to me?”",
    };
    
    public static String getGreeting(Personality playerPersonality, SpecialTrait specialTrait) {
        try {
            PlayerTypes dialogeType = new PlayerTypes(playerPersonality, specialTrait);
            String playerGreetings = greetingsDict[dialogeType];
            return playerGreetings;
        } catch {
            return "Greetings shopkeep. What’ve you got for me today?";
        }
    }
    public static String getDialogueFromWillingess(float willingness) {
        string response = "";
        if(willingness < .1) {
            response = "Is this a joke?";
        } if(willingness < .3) {
            response = "I'm not impressed";
        } else if(willingness < .5) {
            response = "I see the value but it isn't for me";
        } else if(willingness < .7) {
            response = "I am intriguied by the item, tell me more";
        } else if(willingness < .95) {
            response = "I do like this item very much, it does seem useful to me";
        } else if (willingness < 1.0) {
            response = "Let it already be sold!";
        } else {
            response = "...\nI will literally die for this";
        }
        GD.Print(willingness);
        return response;
    }
}

public struct PlayerTypes {
    public PlayerTypes(Personality personality, SpecialTrait specialTrait) {
        this.personality = personality;
        this.specialTrait = specialTrait;
    }
    public Personality personality {get;set;}
    public SpecialTrait specialTrait {get;set;}
}